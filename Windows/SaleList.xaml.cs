using EquipmentRent3ISP9_7.HelperClass;
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
using EquipmentRent3ISP9_7.EF;

namespace EquipmentRent3ISP9_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для SaleList.xaml
    /// </summary>
    public partial class SaleList : Window
    {
        public SaleList()
        {
            InitializeComponent();
            Filter(0);

            // Сортировка по списку listSort
            // и установка значения по умолчанию = 0
            cmbSort.ItemsSource = listSort;
            cmbSort.SelectedIndex = 0;

            LV_Sale.ItemsSource = HelperCl.Context.Sale.ToList();
        }

        // Поиск и список его видов
        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По фамилии клиента",
            "По фамилии работника",
            "По названию продукта"
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

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            AddSale addSale = new AddSale();
            addSale.ShowDialog();

            Filter(0);
        }

        private void LV_Sale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                MessageBoxResult resClick = MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    if (LV_Sale.SelectedItem is Sale)
                    {
                        var sale = LV_Sale.SelectedItem as Sale;
                        sale.IsDeleted = true;
                        HelperCl.Context.SaveChanges();
                        Filter(0);
                        MessageBox.Show("Запись успешно удалена", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void LV_Sale_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Sale.SelectedItem is Sale)
            {
                var sale = LV_Sale.SelectedItem as Sale;
                AddSale editSale = new AddSale(sale);
                editSale.ShowDialog();
                Filter(0);
            }
        }

        private void Filter(int chooseItem)
        {
            // Код, выполняющийся обязательно
            List<Sale> listSale = HelperCl.Context.Sale.Where(i => i.IsDeleted == false).ToList();

            switch (chooseItem)
            {
                default:
                    break;

                // Поиск по нужным параметрам и соритровка по нужным параметрам
                case 1:
                    listSale = listSale
                .Where(i => i.Client.LastName.ToLower().Contains(txtSearch.Text.ToLower())
                        || i.Employee.LastName.ToLower().Contains(txtSearch.Text.ToLower())
                        || i.Product.ProductName.ToLower().Contains(txtSearch.Text.ToLower()))
                .ToList();

                    switch (cmbSort.SelectedIndex)
                    {
                        default:
                            break;

                        case 0: // По умолчанию
                            listSale = listSale.OrderBy(i => i.IdSale).ToList();
                            break;

                        case 1:
                            listSale = listSale.OrderBy(i => i.Client.LastName).ToList();
                            break;

                        case 2: 
                            listSale = listSale.OrderBy(i => i.Employee.LastName).ToList();
                            break;

                        case 3:
                            listSale = listSale.OrderBy(i => i.Product.ProductName).ToList();
                            break;
                    }
                    break;
            }
            LV_Sale.ItemsSource = listSale;
        }
    }
}
