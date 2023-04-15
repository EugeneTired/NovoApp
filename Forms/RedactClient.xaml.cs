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
    /// Логика взаимодействия для RedactClient.xaml
    /// </summary>
    public partial class RedactClient : Window
    {
        public RedactClient(Clients clients)
        {
            _client = clients;
            InitializeComponent();
            using (var db = new KP_Mihailov_InternetEntities())
            {
                address.ItemsSource = db.AvaAddress.ToList();
                address.SelectedItem = address.ItemsSource.Cast<AvaAddress>().FirstOrDefault(s => s.AvaAddName.StartsWith(clients.ClientsExtra?.Address.Substring(0, clients.ClientsExtra.Address.Length - 5)));
            }
            DomAddress.Text = _client.ClientsExtra.Address.Split(' ').Last();
            Name.Text = clients.Name;
            Surname.Text = clients.Surname;
            Patron.Text = clients.Patron;
            Mail.Text = clients.ClientsExtra?.Mail;
            Phone.Text = clients.ClientsExtra?.Phone;
            address.Text = clients.ClientsExtra?.Address;

            Obud.Visibility = Visibility.Collapsed;
            Rect_Move();
        }

        private string FileNamePath { get; set; }

        private readonly Clients _client;
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Surname.Text == "" || address.Text == "" || Phone.Text == "")
            {
                ShowAnim("Есть не заполненные обязательные поля");
                Name.BorderBrush = Brushes.Red;
                Surname.BorderBrush = Brushes.Red;
                address.BorderBrush = Brushes.Red;
                Phone.BorderBrush = Brushes.Red;
            }
            else
            {
                try
                {
                    using (var db = new KP_Mihailov_InternetEntities())
                    {
                        if(address.SelectedItem == null)
                        {
                            //Ошибка
                            return;
                        }
                        var adres = (address.SelectedItem as AvaAddress).AvaAddName + " " + DomAddress.Text;
                        var Client = db.Clients.Single(s => s.ClientId == _client.ClientId);
                        Client.Name = Name.Text;
                        Client.Surname = Surname.Text;
                        Client.Patron = Patron.Text;
                        var clientExtra = db.ClientsExtra.First(s => s.ClientId == Client.ClientId);
                            clientExtra.ClientId = Client.ClientId;
                            clientExtra.Mail = Mail.Text;
                        clientExtra.Phone = Phone.Text;
                        clientExtra.Address = adres;
                        //if (ImageBox.Source == null) tar.Image = null;
                        //else if (!string.IsNullOrWhiteSpace(FileNamePath)) tar.Image = File.ReadAllBytes(FileNamePath);
                        db.SaveChanges();

                        ShowAnim("Успешно изменен");
                    }

                }
                catch (Exception)
                {
                    ShowAnim("Произошла ошибка, проверьте поля");
                }
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

        //private void BtnClear_Click(object sender, RoutedEventArgs e)
        //{
        //    ImageBox.Source = null;
        //}
        //private void Button_click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var openFile = new OpenFileDialog()
        //        {
        //            Filter = "Image (png, jpg, jpeg, bmp) | *.png; *.jpg; *.jpeg; *.bmp"
        //        };
        //        if (openFile.ShowDialog() == false) return;
        //        var image = openFile.FileName;
        //        if (image == null) return;

        //        ImageBox.Source = File.ReadAllBytes(image).ToImageSource();
        //        FileNamePath = image;
        //        ImageBox.Visibility = Visibility.Visible;
        //        BtnClear.IsEnabled = true;
        //    }
        //    catch
        //    {
        //        ShowAnim("Ошибка при загрузке фото.");
        //    }
        //}

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
            var cat = new CatalogClients();
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
