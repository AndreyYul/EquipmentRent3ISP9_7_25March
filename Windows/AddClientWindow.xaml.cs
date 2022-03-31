using EquipmentRent3ISP9_7.EF;
using Microsoft.Win32;
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

namespace EquipmentRent3ISP9_7.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private bool isEdit;
        Client editClient = new Client();
        private string pathPhoto = null;

        public AddClientWindow()
        {
            InitializeComponent();
            cmbGender.ItemsSource = HelperClass.HelperCl.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";
            cmbGender.SelectedIndex = 0;
        }

        public AddClientWindow(Client client)
        {
            InitializeComponent();

            // Заполнение полей свойствами аргумента client 
            cmbGender.ItemsSource = HelperClass.HelperCl.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";

            txtLname.Text = client.LastName;
            txtFname.Text = client.FirstName;
            txtMname.Text = client.MiddleName;
            txtEmail.Text = client.Email;
            txtPhone.Text = client.Phone;
            
            dpBirthday.SelectedDate = client.Birthday;
            //txtPassport.Text = client.Passport.FullPassport;
            txtPassportSeries.Text = client.Passport.PassportSeries;
            txtPassportNumber.Text = client.Passport.PassportNumber;

            cmbGender.SelectedIndex = client.IdGender - 1;

            if (client.ClientPhoto != null)
            {
                using (MemoryStream stream = new MemoryStream(client.ClientPhoto))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    photoUser.Source = bitmapImage;
                }
            }

            tbTitle.Text = "Изменение данных клиента";
            btnAddNew.Content = "Сохранить";

            isEdit = true;
            
            // Сохраняем client для доступа вне конструктора
            editClient = client;
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            #region Validation
            // Null or White Space
            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Поле ФАМИЛИЯ пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Поле ИМЯ пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Поле EMAIL пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Поле ТЕЛЕФОН пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(dpBirthday.Text))
            {
                MessageBox.Show("Поле ДАТА РОЖДЕНИЯ пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassportSeries.Text))
            {
                MessageBox.Show("Поле ПАСПОРТ пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassportNumber.Text))
            {
                MessageBox.Show("Поле ПАСПОРТ пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!long.TryParse(txtPhone.Text, out long res))
            {
                MessageBox.Show("Поле ТЕЛЕФОН введено некорректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool IsValidEmail(string email)
            {
                string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
                Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
                return isMatch.Success;
            }

            if (IsValidEmail(txtEmail.Text) == false)
            {
                MessageBox.Show("Введен некорректный Email", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion

            if (isEdit)
            {
                // Обработка случайного нажатия
                MessageBoxResult resClick = MessageBox.Show("Изменить пользователя?", "Подтверждение редактирования", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return; 
                }

                try
                {
                    //Passport newPassport = new Passport();
                    //string series = txtPassport.Text.Substring(0, 3);
                    //string number = txtPassport.Text.Substring(4, 9);

                    editClient.LastName = txtLname.Text;
                    editClient.FirstName = txtFname.Text;
                    editClient.MiddleName = txtMname.Text;
                    editClient.IdGender = (cmbGender.SelectedItem as Gender).IdGender;
                    editClient.Email = txtEmail.Text;
                    editClient.Phone = txtPhone.Text;
                    editClient.Birthday = DateTime.Parse(dpBirthday.Text);
                    editClient.Passport.PassportSeries = txtPassportSeries.Text;
                    editClient.Passport.PassportNumber = txtPassportNumber.Text;

                    //newPassport.PassportSeries = series;
                    //newPassport.PassportNumber = number;
                    //HelperClass.HelperCl.Context.Passport.Add(newPassport);

                    if (pathPhoto != null)
                    {
                        editClient.ClientPhoto = File.ReadAllBytes(pathPhoto);
                    }

                    HelperClass.HelperCl.Context.SaveChanges();
                    MessageBox.Show("Пользователь изменён!", "Редактирование");
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
                    Client newClient = new Client
                    {
                        LastName = txtLname.Text,
                        FirstName = txtFname.Text,
                        MiddleName = txtMname.Text,
                        IdGender = (cmbGender.SelectedItem as Gender).IdGender,
                        Email = txtEmail.Text,
                        Phone = txtPhone.Text,
                        Birthday = DateTime.Parse(dpBirthday.Text)
                        //Passport.PassportSeries = txtPassportSeries,

                    };
                    string series = txtPassportSeries.Text;
                    string number = txtPassportNumber.Text;
                    Passport passport = new Passport
                    {
                        PassportNumber = number,
                        PassportSeries = series
                    };
                    HelperClass.HelperCl.Context.Passport.Add(passport);

                    if (pathPhoto != null)
                    {
                        newClient.ClientPhoto = File.ReadAllBytes(pathPhoto);
                    }

                    HelperClass.HelperCl.Context.Client.Add(newClient);
                    
                    HelperClass.HelperCl.Context.SaveChanges();
                    MessageBox.Show("Пользователь добавлен!", "Добавление");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!");
                }
            }
        }

        private void btnChooseClientPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                photoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }
    }
}
