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
using Microsoft.Win32;

namespace EquipmentRent3ISP9_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddSale.xaml
    /// </summary>
    public partial class AddSale : Window
    {
        private bool isEdit;
        Sale editSale = new Sale();

        public AddSale()
        {
            InitializeComponent();
            // Заполнение полей свойствами аргумента sale 
            cmbClient.ItemsSource = HelperCl.Context.Client.ToList();
            cmbClient.DisplayMemberPath = "CMBProperty";

            cmbProduct.ItemsSource = HelperCl.Context.Product.ToList();
            cmbProduct.DisplayMemberPath = "ProductName";

            cmbEmployee.ItemsSource = HelperCl.Context.Employee.ToList();
            cmbEmployee.DisplayMemberPath = "CMBProperty";

            isEdit = false;
        }

        public AddSale(Sale sale)
        {
            InitializeComponent();

            // Заполнение полей свойствами аргумента sale 
            cmbClient.ItemsSource = HelperCl.Context.Client.ToList();
            cmbClient.DisplayMemberPath = "CMBProperty";

            cmbProduct.ItemsSource = HelperCl.Context.Product.ToList();
            cmbProduct.DisplayMemberPath = "ProductName";

            cmbEmployee.ItemsSource = HelperCl.Context.Employee.ToList();
            cmbEmployee.DisplayMemberPath = "CMBProperty";

            cmbClient.SelectedIndex = sale.IdClient - 1;
            cmbProduct.SelectedIndex = sale.IdProduct - 1;
            cmbEmployee.SelectedIndex = sale.IdEmployee - 1;
            SaleDate.Text = Convert.ToString(sale.SaleDate);
            ReturnDate.Text = Convert.ToString(sale.ReturnDate);


            tbTitle.Text = "Изменение данных работника";
            btnAddNewSale.Content = "Сохранить";

            isEdit = true;
            // Сохраняем sale для доступа вне конструктора
            editSale = sale;
        }

        private void btnAddNewSale_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                // Обработка случайного нажатия
                MessageBoxResult resClick = MessageBox.Show("Изменить запись?", "Подтверждение редактирования", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    editSale.IdProduct = (cmbProduct.SelectedItem as Product).IdProduct;
                    editSale.IdClient = (cmbClient.SelectedItem as Client).IdClient;
                    editSale.IdEmployee = (cmbEmployee.SelectedItem as Employee).IdEmployee;

                    editSale.SaleDate = DateTime.Parse(SaleDate.Text);
                    editSale.ReturnDate = DateTime.Parse(ReturnDate.Text);

                    HelperCl.Context.SaveChanges();
                    MessageBox.Show("Запись изменена!", "Редактирование");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!");
                }
            }
            else
            {
                // Обработка случайного нажатия
                MessageBoxResult resClick = MessageBox.Show("Добавить запись?", "Подтверждение добавления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    Sale newSale = new Sale
                    {
                        IdProduct = (cmbProduct.SelectedItem as Product).IdProduct,
                        IdClient = (cmbClient.SelectedItem as Client).IdClient,
                        IdEmployee = (cmbEmployee.SelectedItem as Employee).IdEmployee,
                        SaleDate = DateTime.Parse(SaleDate.Text),
                        ReturnDate = DateTime.Parse(ReturnDate.Text)
                    };
                    HelperCl.Context.Sale.Add(newSale);

                    HelperCl.Context.SaveChanges();

                    MessageBox.Show("Запсиь добавлена!", "Добавление");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!");
                }
            }
        }
    }
}