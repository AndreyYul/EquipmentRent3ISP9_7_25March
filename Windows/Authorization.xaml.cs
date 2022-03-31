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

/*Используем библиотеку HelperClass*/
using EquipmentRent3ISP9_7.HelperClass;

namespace EquipmentRent3ISP9_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            Employee authUser = HelperCl.Context.Employee.ToList().
                Where(i => i.Login == authFieldLog.Text 
                        && i.Password == authFieldPsw.Password).
                        FirstOrDefault();
            if (authUser != null)
            {
                MainWindow mainWindow = new MainWindow();
                Close();
                mainWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пользователь не найден", "Ошибка!");
            }
        }
    }
}
