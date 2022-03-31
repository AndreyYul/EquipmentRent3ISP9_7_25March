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
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private bool isEdit;
        Employee editEmployee = new Employee();
        private string pathPhoto = null;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            cmbGender.ItemsSource = HelperClass.HelperCl.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";
            cmbGender.SelectedIndex = 0;

            cmbRole.ItemsSource = HelperClass.HelperCl.Context.Role.ToList();
            cmbRole.DisplayMemberPath = "RoleName";
            cmbRole.SelectedIndex = 0;

            isEdit = false;
        }

        public AddEmployeeWindow(Employee employee)
        {
            InitializeComponent();

            // Заполнение полей свойствами аргумента employee 
            cmbGender.ItemsSource = HelperClass.HelperCl.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";

            cmbRole.ItemsSource = HelperClass.HelperCl.Context.Role.ToList();
            cmbRole.DisplayMemberPath = "RoleName";

            txtLname.Text = employee.LastName;
            txtFname.Text = employee.FirstName;
            txtMname.Text = employee.MiddleName;
            txtEmail.Text = employee.Email;
            txtPhone.Text = employee.Phone;
            txtLogin.Text = employee.Login;
            txtPassword.Password = employee.Password;
            txtPasswordRepeat.Password = employee.Password;

            cmbGender.SelectedIndex = employee.IdGender - 1;
            cmbRole.SelectedIndex = employee.IdRole - 1;

            if (employee.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(employee.Photo) )
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

            tbTitle.Text = "Изменение данных работника";
            btnAddNew.Content = "Сохранить";

            isEdit = true;
            // Сохраняем employee для доступа вне конструктора
            editEmployee = employee;
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

            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Поле ЛОГИН пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Поле ПАРОЛЬ пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLname.Text))
            {
                MessageBox.Show("Поле ПОВТОР ПАРОЛЯ пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Incorrect Data
            //var authUser = HelperClass.HelperCl.Context.Employee.ToList().
            //    Where(i => i.Login == txtLogin.Text).FirstOrDefault();

            //if (authUser != null && isEdit == false)
            //{
            //    MessageBox.Show("Данный логин уже занят!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            if (!long.TryParse(txtPhone.Text, out long res))
            {
                MessageBox.Show("Поле ТЕЛЕФОН введено некорректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPassword.Password != txtPasswordRepeat.Password)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                var resClick = MessageBox.Show("Изменить пользователя?", "Подтверждение редактирования", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                    return;
                try
                {
                    editEmployee.LastName =   txtLname.Text;
                    editEmployee.FirstName =  txtFname.Text;
                    editEmployee.MiddleName = txtMname.Text;
                    editEmployee.IdGender =   (cmbGender.SelectedItem as Gender).IdGender;
                    editEmployee.Email =      txtEmail.Text;
                    editEmployee.Phone =      txtPhone.Text;
                    editEmployee.Login =      txtLogin.Text;
                    editEmployee.Password =   txtPassword.Password;
                    editEmployee.IdRole =     (cmbRole.SelectedItem as Role).IdRole;

                    if (pathPhoto != null)
                    {
                        editEmployee.Photo = File.ReadAllBytes(pathPhoto);
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
                var resClick = MessageBox.Show("Добавить пользователя?", "Подтверждение добавления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                    return;

                try
                {
                    Employee newEmployee = new Employee
                    {
                        LastName = txtLname.Text,
                        FirstName = txtFname.Text,
                        MiddleName = txtMname.Text,
                        IdGender = (cmbGender.SelectedItem as Gender).IdGender,
                        Email = txtEmail.Text,
                        Phone = txtPhone.Text,
                        Login = txtLogin.Text,
                        Password = txtPassword.Password,
                        IdRole = (cmbRole.SelectedItem as Role).IdRole
                    };

                    if (pathPhoto != null)
                    {
                        newEmployee.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    HelperClass.HelperCl.Context.Employee.Add(newEmployee);

                    HelperClass.HelperCl.Context.SaveChanges();

                    MessageBox.Show("Пользователь добавлен!", "Добавление");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!");
                }
            }
            // Comment
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
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
