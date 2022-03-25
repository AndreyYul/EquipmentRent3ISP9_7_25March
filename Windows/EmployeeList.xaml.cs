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
using EquipmentRent3ISP9_7.HelperClass;


namespace EquipmentRent3ISP9_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для EmployeeList.xaml
    /// </summary>
    public partial class EmployeeList : Window
    {
        public EmployeeList()
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
            "По имени",
            "По фамилии",
            "По должности",
            "По Email"
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
            AddEmployeeWindow addemployeewindow = new AddEmployeeWindow();
            addemployeewindow.ShowDialog();

            // Обновление таблицы с помощью метода Filter с параметром 0
            Filter(0);
        }


        // Удаление пользователя
        private void LV_Employee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var resClick = MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                    return;

                try
                {
                    if (LV_Employee.SelectedItem is Employee)
                    {
                        var employee = LV_Employee.SelectedItem as Employee;
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
        private void LV_Employee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Employee.SelectedItem is Employee)
            {
                Employee empl = LV_Employee.SelectedItem as Employee;
                AddEmployeeWindow editEmployee = new AddEmployeeWindow(empl);
                editEmployee.ShowDialog();
                Filter(0);
            }
        }

        private void Filter(int chooseItem)
        {
            // Код, выполняющийся обязательно
            List<Employee> listEmployee = HelperCl.Context.Employee.Where(i => i.IsDeleted == false).ToList();

            switch (chooseItem)
            {
                default:
                    break;

                // Поиск по нужным параметрам и соритровка по нужным параметрам
                case 1:
                    listEmployee = listEmployee
                .Where(i => i.LastName.ToLower().Contains(txtSearch.Text.ToLower())
                        || i.FirstName.ToLower().Contains(txtSearch.Text.ToLower())
                        || i.Role.RoleName.ToLower().Contains(txtSearch.Text.ToLower())
                        || i.Email.ToLower().Contains(txtSearch.Text.ToLower())
                        || i.FIO.ToLower().Contains(txtSearch.Text.ToLower()))
                .ToList();

                    switch (cmbSort.SelectedIndex)
                    {
                        default:
                            break;

                        case 0: // По умолчанию
                            listEmployee = listEmployee.OrderBy(i => i.IdEmployee).ToList();
                            break;

                        case 1: // По имени
                            listEmployee = listEmployee.OrderBy(i => i.FirstName).ToList();
                            break;

                        case 2: // По фамилии
                            listEmployee = listEmployee.OrderBy(i => i.LastName).ToList();
                            break;

                        case 3: // По должности
                            listEmployee = listEmployee.OrderBy(i => i.IdRole).ToList();
                            break;

                        case 4: // По Email
                            listEmployee = listEmployee.OrderBy(i => i.Email).ToList();
                            break;
                    }
                    break;
            }
            LV_Employee.ItemsSource = listEmployee;
        }
    }
}
