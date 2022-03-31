using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;

namespace EquipmentRent3ISP9_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEquipmentWindow.xaml
    /// </summary>
    public partial class AddEquipmentWindow : Window
    {
        private bool isEdit;
        Product editProduct = new Product();
        private string pathPhoto = null;

        public AddEquipmentWindow()
        {
            InitializeComponent();
            cmbProductName.ItemsSource = HelperClass.HelperCl.Context.Product.ToList();
            cmbProductName.DisplayMemberPath = "ProductName";

            cmbManufacturerName.ItemsSource = HelperClass.HelperCl.Context.Manufacturer.ToList();
            cmbManufacturerName.DisplayMemberPath = "ManufacturerName";

            isEdit = false;
        }

        public AddEquipmentWindow(Product product)
        {
            InitializeComponent();

            // Заполнение полей свойствами аргумента employee 
            cmbProductName.ItemsSource = HelperClass.HelperCl.Context.Product.ToList();
            cmbProductName.DisplayMemberPath = "ProductName";

            cmbManufacturerName.ItemsSource = HelperClass.HelperCl.Context.Manufacturer.ToList();
            cmbManufacturerName.DisplayMemberPath = "ManufacturerName";

            cmbProductName.SelectedIndex = product.IdProduct - 1;
            cmbManufacturerName.SelectedIndex = product.IdManufacturer - 1;

            txtPrice.Text = product.Price.ToString();
            dpWarranty.SelectedDate = product.Warranty;

            if (product.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(product.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    productPhoto.Source = bitmapImage;
                }
            }

            tbTitle.Text = "Изменение данных оборудования";
            btnAddNew.Content = "Сохранить";

            isEdit = true;
            // Сохраняем employee для доступа вне конструктора
            editProduct = product;
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            #region Validation
            // Null or White Space
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Поле стоимость пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(dpWarranty.SelectedDate.ToString()))
            {
                MessageBox.Show("Поле гарантия пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            decimal savePrice = 0;
            try
            {
                savePrice = Convert.ToDecimal(txtPrice.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Стоимость введена некорректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (savePrice <= 0)
            {
                MessageBox.Show("Стоимость введена некорректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion

            if (isEdit)
            {
                // Обработка случайного нажатия
                MessageBoxResult resClick = MessageBox.Show("Изменить параметры оборудования?", "Подтверждение редактирования", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }
                try
                {
                    editProduct.IdManufacturer = (cmbManufacturerName.SelectedItem as Manufacturer).IdManufacturer;
                    editProduct.Price = savePrice;
                    editProduct.Warranty = Convert.ToDateTime(dpWarranty.SelectedDate);

                    if (pathPhoto != null)
                    {
                        editProduct.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    HelperClass.HelperCl.Context.SaveChanges();
                    MessageBox.Show("Параметры оборудования изменены!", "Редактирование");
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
                MessageBoxResult resClick = MessageBox.Show("Добавить пользователя?", "Подтверждение добавления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    Product newProduct = new Product
                    {
                        IdManufacturer = (cmbManufacturerName.SelectedItem as Manufacturer).IdManufacturer,
                        Price = savePrice,
                        Warranty = Convert.ToDateTime(dpWarranty.SelectedDate)
                    };

                    if (pathPhoto != null)
                    {
                        newProduct.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    HelperClass.HelperCl.Context.Product.Add(newProduct);

                    HelperClass.HelperCl.Context.SaveChanges();

                    MessageBox.Show("Новое оборудование добавлено!", "Добавление");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!");
                }
            }
        }
        // Comment

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                productPhoto.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }
    }
}
