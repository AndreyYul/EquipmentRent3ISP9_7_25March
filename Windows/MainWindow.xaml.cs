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
using System.Windows.Navigation;
using System.Windows.Shapes;

using EquipmentRent3ISP9_7.EF;
using EquipmentRent3ISP9_7.HelperClass;
using EquipmentRent3ISP9_7.Windows;

namespace EquipmentRent3ISP9_7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region nav
        private void btnEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeList employeeList = new EmployeeList();
            Hide();
            employeeList.ShowDialog();
            Show();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            ClientList clientList = new ClientList();
            Hide();
            clientList.ShowDialog();
            Show();
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            EquipmentList equipmentList = new EquipmentList();
            Hide();
            equipmentList.ShowDialog();
            Show();
        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            SaleList saleList = new SaleList();
            Hide();
            saleList.ShowDialog();
            Show();
        }
        #endregion
    }
}
