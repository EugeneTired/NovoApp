using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Novoapp
{
 
    internal static class AuthUser
    {
        public static Employee AuthPerson { get; set; }
    }
    static class SomeData
    {
        public static int guestOrNot { get; set; }
        public static int PasswordVisibility { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;     // Присвоение максимальных размеров окна
            Obud.Visibility = Visibility.Collapsed;                             // Скрытие уведомления об ошибке
        }


        // Перетаскивание и раскрытие окна
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

            if(this.WindowState == WindowState.Normal) this.DragMove();
        }

        //Кнопка показать пароль
        private void GigaChad_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SomeData.PasswordVisibility == 1)
            {
                MyTextBox.Visibility = Visibility.Collapsed;
                PassBox.Visibility = Visibility.Visible;
                SomeData.PasswordVisibility = 0;
                PassBox.Password = MyTextBox.Text;
                GigaChad.Source = new BitmapImage(new Uri(@"/images/EyeOpen.png", UriKind.Relative));
            }
            else
            {
                MyTextBox.Visibility = Visibility.Visible;
                PassBox.Visibility = Visibility.Collapsed;
                SomeData.PasswordVisibility = 1;
                MyTextBox.Text = PassBox.Password;
                GigaChad.Source = new BitmapImage(new Uri(@"/images/EyeClosed.png", UriKind.Relative));
            }
            
        }

        // Кнопка входа
        private void SubmitLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) | (SomeData.PasswordVisibility == 0 ? string.IsNullOrWhiteSpace(PassBox.Password) : string.IsNullOrWhiteSpace(MyTextBox.Text)))
            {
                ShowAnim("Поля не могут быть пустыми");
                return;

            }

            var log = LoginBox.Text;
            var pass = SomeData.PasswordVisibility == 0 ? PassBox.Password : MyTextBox.Text;
            using (var db = new KP_Mihailov_InternetEntities())
            {
                var doubleAnim = new DoubleAnimation();
                var Log = db.Employee.FirstOrDefault(s => s.Login == log);
                if (Log != null)
                {
                    var Pass = db.Employee.FirstOrDefault(s=> s.Password == pass);
                    if (Pass != null)
                    {
                        AuthUser.AuthPerson = Log;

                        Window wnd = null;
                        switch (Log.RoleId)                                                     // Распределение по ролям
                        {
                            case 1: SomeData.guestOrNot = 1; wnd = new SelectWindow(); break;
                            case 2: SomeData.guestOrNot = 2; wnd = new SelectWindow(); break;
                            case 3: SomeData.guestOrNot = 3; wnd = new SelectWindow(); break;
                        }
                        wnd.Show();
                        this.Close();
                        return;
                    }
                    else
                    {
                        ShowAnim("Пароль введен неверно");

                    }
                }
                else ShowAnim("Такого логина нет в системе");
            }
        }

        // Предупреждение об ошибке
        private async void ShowAnim(string error)
        {
            ErrorField.Text = error;
            Obud.Visibility = Visibility.Visible;
            var doubleAnim = new DoubleAnimation();
            if (!_isActive)
            {
                _isActive = true;
                doubleAnim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut }; // Сглаживание анимации
                doubleAnim.From = Obud.ActualWidth;
                doubleAnim.To = 300;                                                        // Присвоение размера по ширине для окна
                doubleAnim.Duration = TimeSpan.FromSeconds(1);                              // Длительность анимации
                Obud.BeginAnimation(WidthProperty, doubleAnim);
            }

            var tempCount = ++_isCount;

            await Task.Delay(3500);                                                         // Время существования уведомления

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


        // Хранят данные для правильной работы уведомления об ошибке
        private int _isCount = 0;
        private bool _isActive;
        private void SmallBut_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        // Кнопка панели управления для разворачивания, восстановления размеров окна
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


        // Кнопка панели управления для закрытия окна
        private void CloseBut_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationGrid.Visibility = Visibility.Visible;
            SmallBut.IsTabStop = false;
            SwitchBut.IsTabStop = false;
            CloseBut.IsTabStop = false;
            SubmitLogin.IsTabStop = false;
            LoginBox.IsTabStop = false;
            PassBox.IsTabStop = false;
            ConfirmClick.IsTabStop = true;
            CancelExit.IsTabStop = true;
        }

        // Кнопка отмены выхода в окне подтверждения
        private void CancelExit_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationGrid.Visibility = Visibility.Collapsed;
            SmallBut.IsTabStop = true;
            SwitchBut.IsTabStop = true;
            CloseBut.IsTabStop = true;
            SubmitLogin.IsTabStop = true;
            LoginBox.IsTabStop = true;
            PassBox.IsTabStop = true;
            ConfirmClick.IsTabStop = false;
            CancelExit.IsTabStop = false;
        }

        // Кнопка подтверждения выхода из ПО
        private void ConfirmClick_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(13);
        }
    }
}
