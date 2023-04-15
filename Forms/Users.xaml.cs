using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Novoapp.Forms
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        public Users()
        {
            InitializeComponent();
            LVusers.Items.Clear();                                               //Очистка списка тарифов для удаления коллекции
            SearchBox.Text = "";                                            //Очистка поисковика

            using (var db = new KP_Mihailov_InternetEntities())
            {
                LVusers.ItemsSource = db.Employee.Include(s => s.Role).Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
                var list = new List<Role>();
                list.Add(new Role { Name = "Очистить" });
                list.AddRange(db.Role.ToList());
                ComboFilter.ItemsSource = list;
            }
        }

        private bool _isShowed;

        private void ShowMinObud_Click(object sender, RoutedEventArgs e)
        {
            //StackPenMega.Visibility = Visibility.Visible;
            //SearchBox.Visibility = Visibility.Collapsed;
            //LabelCat.Visibility = Visibility.Collapsed;
            //LabelFilt.Visibility = Visibility.Collapsed;
            //ComboFilter.Visibility = Visibility.Collapsed;
            //LabelSort.Visibility = Visibility.Collapsed;
            //PriceSort.Visibility = Visibility.Collapsed;
            var doubleAnim = new DoubleAnimation();
            doubleAnim.EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };
            doubleAnim.From = Obud.ActualWidth;
            if (!_isShowed)
            {
                RectOne.Fill = new SolidColorBrush() { Color = Color.FromRgb(255, 255, 255) };
                RectTwo.Fill = new SolidColorBrush() { Color = Color.FromRgb(255, 255, 255) };
                RectThree.Fill = new SolidColorBrush() { Color = Color.FromRgb(255, 255, 255) };
                BorderColorChange.Background = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0) };
                doubleAnim.To = 300;
                RectOne.Width = 10;
                RectTwo.Width = 20;

                //ListView lv = sender as ListView;
                if (CheckBlur.IsChecked == true)
                {
                    LVusers.Effect = new System.Windows.Media.Effects.BlurEffect()
                    {
                        Radius = 1,
                        RenderingBias = RenderingBias.Quality
                    };
                }
                else
                {
                    LVusers.Effect = new System.Windows.Media.Effects.BlurEffect()
                    {
                        Radius = 10,
                        RenderingBias = RenderingBias.Quality
                    };
                }
                //GigaBar.Effect = new System.Windows.Media.Effects.BlurEffect()
                //{
                //    Radius = 10,
                //    RenderingBias = RenderingBias.Quality
                //};

            }
            else
            {
                //StackPenMega.Visibility = Visibility.Collapsed;
                RectOne.Fill = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0) };
                RectTwo.Fill = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0) };
                RectThree.Fill = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0) };
                BorderColorChange.Background = new SolidColorBrush() { Color = Color.FromArgb(0, 0, 0, 0) };
                doubleAnim.To = 15;
                RectOne.Width = 40;
                RectTwo.Width = 40;

                LVusers.Effect = new System.Windows.Media.Effects.BlurEffect()
                {
                    Radius = 1,
                    RenderingBias = RenderingBias.Quality
                };
                //GigaBar.Effect = new System.Windows.Media.Effects.BlurEffect()
                //{
                //    Radius = 1,
                //    RenderingBias = RenderingBias.Quality
                //};
            }
            doubleAnim.Duration = TimeSpan.FromSeconds(1);
            Obud.BeginAnimation(WidthProperty, doubleAnim);
            _isShowed = !_isShowed;

        }

        public async Task CategFilt()
        {
            using (var db = await Task.Run(() => new KP_Mihailov_InternetEntities()))
            {
                LVusers.ItemsSource = db.Employee.Include(s => s.Role).ToList().Where(x => x.Name.ToLower().Trim().Contains(SearchBox.Text.ToLower().Trim())
                                                             || x.Surname.ToLower().Trim().Contains(SearchBox.Text.ToLower().Trim())
                                                             || x.EmployeeId.ToString().Contains(SearchBox.Text.ToLower().Trim())
                                                             || x.Login.ToLower().Trim().Contains(SearchBox.Text.ToLower().Trim())
                                                             || x.Patron.ToLower().Trim().Contains(SearchBox.Text.ToLower().Trim()));
            }
            switch (PriceSort.SelectedIndex)
            {
                case 0:
                    LVusers.ItemsSource = LVusers.ItemsSource.Cast<Employee>().OrderBy(s => s.Surname);
                    break;
                case 1:
                    LVusers.ItemsSource = LVusers.ItemsSource.Cast<Employee>().OrderBy(s => s.Name);
                    break;
                case 2:
                    LVusers.ItemsSource = LVusers.ItemsSource.Cast<Employee>().OrderBy(s => s.Patron);
                    break;
                case 3:
                    LVusers.ItemsSource = LVusers.ItemsSource.Cast<Employee>().OrderBy(s => s.EmployeeId);
                    break;
            }
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (LVusers != null) CategFilt();
        }



        private void LV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isShowed) ShowMinObud_Click(sender, e);
        }

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isShowed) ShowMinObud_Click(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectWin = new SelectWindow();
            this.Close();
            selectWin.Show();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVusers != null) CategFilt();
        }

        private void CheckBlur_Checked(object sender, RoutedEventArgs e)
        {
            LVusers.Effect = new System.Windows.Media.Effects.BlurEffect()
            {
                Radius = 1,
                RenderingBias = RenderingBias.Quality
            };
        }

        private void CheckBlur_Unchecked(object sender, RoutedEventArgs e)
        {
            LVusers.Effect = new System.Windows.Media.Effects.BlurEffect()
            {
                Radius = 10,
                RenderingBias = RenderingBias.Quality
            };
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

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            var cat = new CatalogClients();
            this.Close();
            cat.Show();
        }

        // ADD

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var UserAddForm = new UsersAdd();
            UserAddForm.Show();
            this.Close();
        }

        //DELETE
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            using (var db = new KP_Mihailov_InternetEntities())
            {
                var mes = MessageBox.Show("Вы уверены в удалении этого поля?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mes == MessageBoxResult.Yes)
                {
                    db.Employee.Attach(LVusers.SelectedItem as Employee);
                    db.Employee.Remove(LVusers.SelectedItem as Employee);
                    db.SaveChanges();
                }
               
            }
            CategFilt();
        }

        //REDACT

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var employee = LVusers.SelectedItem as Employee;
            var userRedactForm = new RedactEmployee(employee);
            userRedactForm.Show();
            this.Close();
        }

        //EXIT
        private void UserMenu_Click(object sender, RoutedEventArgs e)
        {
            SomeData.guestOrNot = 0;
            var Aut = new MainWindow();
            Aut.Show();
            this.Close();
        }
    }
}
