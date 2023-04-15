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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Novoapp.Forms
{
    /// <summary>
    /// Логика взаимодействия для UsersAdd.xaml
    /// </summary>
    public partial class UsersAdd : Window
    {
        public UsersAdd()
        {
            InitializeComponent();
            Obud.Visibility = Visibility.Collapsed;
            Rect_Move();
            using (var db = new KP_Mihailov_InternetEntities())
            {
                RoleBox.ItemsSource = db.Role.ToList();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (Name.Text == "" || Surname.Text == "" || Password.Text == "" || Phone.Text == "" || Login.Text == "")
            {
                ShowAnim("Есть не заполненные обязательные поля");
                Name.BorderBrush = Brushes.Red;
                Surname.BorderBrush = Brushes.Red;
                Password.BorderBrush = Brushes.Red;
                Phone.BorderBrush = Brushes.Red;
            }
            else
            {
                try  //                        tar.IdTariffType = (Category.SelectedItem as TariffType).Id;
                {
                    using (var db = new KP_Mihailov_InternetEntities())
                    {
                        var Empl = new Employee();
                        Empl.Name = Name.Text;
                        Empl.Surname = Surname.Text;
                        Empl.Patron = Patron.Text;
                        Empl.Login = Login.Text;
                        Empl.RoleId = (RoleBox.SelectedItem as Role).RoleId;
                        Empl.Password = Password.Text;
                        Empl.Phone = Phone.Text;
                        db.Employee.Add(Empl);
                        //if (ImageBox.Source == null) tar.Image = null;
                        //else if (!string.IsNullOrWhiteSpace(FileNamePath)) tar.Image = File.ReadAllBytes(FileNamePath);
                        db.SaveChanges();

                        ShowAnim("Успешно добавлен");
                    }

                }
                catch (Exception)
                {
                    ShowAnim("Произошла ошибка, проверьте поля");
                }

            }
        }
        private async void ShowAnim(string error)
        {
            ErrorField.Text = error;
            Obud.Visibility = Visibility.Visible;
            var doubleAnim = new DoubleAnimation();
            if (!_isActive)
            {
                _isActive = true;
                doubleAnim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                doubleAnim.From = Obud.ActualWidth;
                doubleAnim.To = 400;
                doubleAnim.Duration = TimeSpan.FromSeconds(1);
                Obud.BeginAnimation(WidthProperty, doubleAnim);
            }

            var tempCount = ++_isCount;

            await Task.Delay(5000);

            if (_isCount == tempCount && _isActive)
            {
                _isActive = false;
                doubleAnim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                doubleAnim.From = Obud.ActualWidth;
                doubleAnim.To = 0;
                doubleAnim.Duration = TimeSpan.FromSeconds(1);
                Obud.BeginAnimation(WidthProperty, doubleAnim);
            }
        }

        private int _isCount = 0;
        private bool _isActive;

        private void MainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
                QYYY.Text = this.WindowState == WindowState.Maximized ? "2" : "1";
            }

            if (this.WindowState == WindowState.Normal) this.DragMove();
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void SmallBut_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SwitchBut_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
            QYYY.Text = this.WindowState == WindowState.Maximized ? "2" : "1";
        }
        private void CloseBut_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationGrid.Visibility = Visibility.Visible;
            SmallBut.IsTabStop = false;
            SwitchBut.IsTabStop = false;
            CloseBut.IsTabStop = false;
            ConfirmClick.IsTabStop = true;
            CancelExit.IsTabStop = true;
        }
        private void CancelExit_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationGrid.Visibility = Visibility.Collapsed;
            SmallBut.IsTabStop = true;
            SwitchBut.IsTabStop = true;
            CloseBut.IsTabStop = true;
            ConfirmClick.IsTabStop = false;
            CancelExit.IsTabStop = false;
        }

        private void ConfirmClick_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(13);
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            var cat = new Users();
            this.Close();
            cat.Show();
        }

        public async void Rect_Move()
        {
            var right = true;
            while (true)
            {
                var doubleAnim = new DoubleAnimation();
                doubleAnim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
                doubleAnim.From = obud.Width;
                doubleAnim.To = right ? 150 : 450;
                doubleAnim.Duration = TimeSpan.FromSeconds(2);
                obud.BeginAnimation(WidthProperty, doubleAnim);
                right = !right;
                await Task.Delay(3000);
            }
        }
    }
}
