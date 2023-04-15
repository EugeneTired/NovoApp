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
using Novoapp.Forms;

namespace Novoapp
{
    /// <summary>
    /// Логика взаимодействия для SelectWindow.xaml
    /// </summary>
    public partial class SelectWindow : Window
    {
        public SelectWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            Convert();
            if(SomeData.guestOrNot == 2)
            {
                OpenUsers.Visibility = Visibility.Collapsed;
                TxtBlock.Visibility = Visibility.Collapsed;
            }

        }
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
        private void Tarif_Click(object sender, RoutedEventArgs e)
        {
            var cat = new Catalog();
            cat.Show();
            this.Close();

        }

        private void Factur_Click(object sender, RoutedEventArgs e)
        {
            var fact = new Forms.CatalogClients();
            fact.Show();
            this.Close();
        }

        //17, -9        0   1   0.283
        // Начало: 0,0   1,-9
        // Конеч: 8,17 1,-9
        private async void Convert()
        {
            var start = new Point(1, -9);
            var endX = 8d;
            var endY = 17d;
            var end = new Point(0, 0);

            while (true)
            {
                var gradientBrush = new LinearGradientBrush() { EndPoint = end, StartPoint = start };
                gradientBrush.GradientStops.Add(new GradientStop() { Color = Color.FromRgb(110, 193, 249), Offset = 0 });
                gradientBrush.GradientStops.Add(new GradientStop() { Color = Color.FromRgb(110, 193, 249), Offset = 0.874 });
                gradientBrush.GradientStops.Add(new GradientStop() { Color = Color.FromRgb(255, 255, 255), Offset = 0.577 });
                gradientBrush.GradientStops.Add(new GradientStop() { Color = Color.FromRgb(122, 202, 232), Offset = 0.601 });
                gradientBrush.GradientStops.Add(new GradientStop() { Color = Color.FromRgb(255, 255, 255), Offset = 0.65 });

                OpenUsers.Background = gradientBrush;
                TarifBut.Background = gradientBrush;
                Factur.Background = gradientBrush;
                await Task.Delay(75);
                if (++end.Y > endY && ++end.X > endX) end = new Point(0, 0);
            }
        }

        private void OpenUsers_Click(object sender, RoutedEventArgs e)
        {
            var NewUserWin = new Users();
            NewUserWin.Show();
            this.Close();
        }
    }
}
