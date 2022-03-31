using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EquipmentRent3ISP9_7.HelperClass;
using EquipmentRent3ISP9_7.EF;

namespace EquipmentRent3ISP9_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для EquipmentList.xaml
    /// </summary>
    public partial class EquipmentList : Window
    {
        public EquipmentList()
        {
            InitializeComponent();
            // Обновление таблицы с помощью метода Filter с параметром 0
            Filter(0);

            // Сортировка по списку listSort
            // и установка значения по умолчанию = 0
            cmbSort.ItemsSource = listSort;
            cmbSort.SelectedIndex = 0;
        }
        // Поиск и список его видов
        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По названию оборудования",
            "По наименованию поставщика",
            "По стоимости"
        };

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Поиск с помощью метода Filter с параметром 1
            Filter(1);
        }


        // Соритровка
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Сортировка с помощью метода Filter с параметром 1
            Filter(1);
        }


        // Добавление нового пользователя
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            AddEquipmentWindow addEquipmentWindow = new AddEquipmentWindow();
            addEquipmentWindow.ShowDialog();

            // Обновление таблицы с помощью метода Filter с параметром 0
            Filter(0);
        }


        // Удаление пользователя
        private void LV_Equipment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var resClick = MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                    return;

                try
                {
                    if (LV_Equipment.SelectedItem is Product)
                    {
                        var employee = LV_Equipment.SelectedItem as Employee;
                        employee.IsDeleted = true;
                        HelperCl.Context.SaveChanges();
                        Filter(0);
                        MessageBox.Show("Пользователь успешно удалён", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        // Изменение пользователя
        private void LV_Equipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Equipment.SelectedItem is Product)
            {
                Product product = LV_Equipment.SelectedItem as Product;
                AddEquipmentWindow editEquipment = new AddEquipmentWindow(product);
                editEquipment.ShowDialog();

                Filter(0);
            }
        }

        private void Filter(int chooseItem)
        {
            // Код, выполняющийся обязательно
            List<Product> productList = HelperCl.Context.Product.Where(i => i.IsDeleted == false).ToList();

            switch (chooseItem)
            {
                default:
                    break;

                // Поиск по нужным параметрам и соритровка по нужным параметрам
                case 1:
                    productList = productList
                .Where(i => i.ProductName.ToLower().Contains(txtSearch.Text.ToLower())
                         || i.Manufacturer.ManufacturerName.ToLower().Contains(txtSearch.Text.ToLower())
                         || i.Price.ToString().Contains(txtSearch.Text))
                .ToList();

                    switch (cmbSort.SelectedIndex)
                    {
                        default:
                            break;

                        case 0: // По умолчанию
                            productList = productList.OrderBy(i => i.IdProduct).ToList();
                            break;

                        case 1: // По названию оборудования
                            productList = productList.OrderBy(i => i.ProductName).ToList();
                            break;

                        case 2: // По наименованию поставщика
                            productList = productList.OrderBy(i => i.Manufacturer.ManufacturerName).ToList();
                            break;

                        case 3: // По стоимости
                            productList = productList.OrderBy(i => i.Price).ToList();
                            break;
                    }
                    break;
            }
            LV_Equipment.ItemsSource = productList;
        }
    }
}
