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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Novoapp.Extra;
using System.IO;

namespace Novoapp.Forms
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public Catalog()
        {
            InitializeComponent();
            //BaskAmountTXT.Visibility = Visibility.Collapsed;
            //BaskAmountTXT2.Visibility = Visibility.Collapsed;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //Задание максимального размера окна
            LV.Items.Clear();                                               //Очистка списка тарифов для удаления коллекции
            SearchBox.Text = "";                                            //Очистка поисковика
            using (var db = new KP_Mihailov_InternetEntities())
            {
                LV.ItemsSource = db.Tariff.Include(s => s.TariffType).Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower()) && !x.Archived.Value).ToList();
                var list = new List<TariffType>();
                list.Add(new TariffType { Name = "Очистить" });
                list.AddRange(db.TariffType.ToList());
                ComboFilter.ItemsSource = list;
            }

        }

        public static MegaGlist<Tariff> BusketOrder = new MegaGlist<Tariff>();

        private int BInt = 0;

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
            LV.IsTabStop = false;
            ShowMinObud.IsTabStop = false;
            SearchBox.IsTabStop = false;
            ComboFilter.IsTabStop = false;
            PriceSort.IsTabStop = false;
            ConfirmClick.IsTabStop = true;
            CancelExit.IsTabStop = true;
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
            if (!_isShowed) {
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
                    LV.Effect = new System.Windows.Media.Effects.BlurEffect()
                    {
                        Radius = 1,
                        RenderingBias = RenderingBias.Quality
                    };
                }
                else
                {
                    LV.Effect = new System.Windows.Media.Effects.BlurEffect()
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
            else {
                //StackPenMega.Visibility = Visibility.Collapsed;
                RectOne.Fill = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0) };
                RectTwo.Fill = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0) };
                RectThree.Fill = new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0) };
                BorderColorChange.Background = new SolidColorBrush() { Color = Color.FromArgb(0, 0, 0, 0) };
                doubleAnim.To = 15;
                RectOne.Width = 40;
                RectTwo.Width = 40;

                LV.Effect = new System.Windows.Media.Effects.BlurEffect()
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

        private void CancelExit_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationGrid.Visibility = Visibility.Collapsed;
            SmallBut.IsTabStop = true;
            SwitchBut.IsTabStop = true;
            CloseBut.IsTabStop = true;
            LV.IsTabStop = true;
            ShowMinObud.IsTabStop = true;
            SearchBox.IsTabStop = true;
            ComboFilter.IsTabStop = true;
            PriceSort.IsTabStop = true;
            ConfirmClick.IsTabStop = false;
            CancelExit.IsTabStop = false;
        }

        private void ConfirmClick_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(13);
        }

        //REDACT
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tariff = LV.SelectedItem as Tariff;
            var redact = new RedactTariff(tariff);
            redact.Show();
            this.Close();
        }

        //ADD
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var add = new AddTariff();
            this.Close();
            add.Show();
        }

        //DELETE
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            using (var db = new KP_Mihailov_InternetEntities())
            {


                var mes = MessageBox.Show("Вы уверены в удалении этого поля?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mes == MessageBoxResult.Yes)
                {
                    db.Tariff.Attach(LV.SelectedItem as Tariff);
                    db.Tariff.Remove(LV.SelectedItem as Tariff);
                    db.SaveChanges();
                }

                
            }
            CategFilt();
            //var tariff = LV.SelectedItem as tariff;

        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            SomeData.guestOrNot = 0;
            var Aut = new MainWindow();
            Aut.Show();
            this.Close();
        }

        public async Task CategFilt()
        {
            using (var db = await Task.Run(() => new KP_Mihailov_InternetEntities()))
            {
                LV.ItemsSource = await Task.Run(() => db.Tariff.Include(s => s.TariffType).ToList().Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower()) && x.Archived == CheckArchive.IsChecked));
                var cat = ComboFilter.SelectedItem as TariffType;
                if (cat != null && ComboFilter.SelectedIndex > 0) LV.ItemsSource = LV.ItemsSource.Cast<Tariff>().ToList().Where(s => s.IdTariffType == cat.Id);
            }
            switch (PriceSort.SelectedIndex)
            {
                case 0:
                    LV.ItemsSource = LV.ItemsSource.Cast<Tariff>().OrderBy(s => s.MonthlyCost);
                    break;
                case 1:
                    LV.ItemsSource = LV.ItemsSource.Cast<Tariff>().OrderByDescending(s => s.MonthlyCost);
                    break;
            }
        }
        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (LV != null) CategFilt();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LV != null) CategFilt();
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

        private void CheckBlur_Checked(object sender, RoutedEventArgs e)
        {
            LV.Effect = new System.Windows.Media.Effects.BlurEffect()
            {
                Radius = 1,
                RenderingBias = RenderingBias.Quality
            };
        }

        private void CheckBlur_Unchecked(object sender, RoutedEventArgs e)
        {
            LV.Effect = new System.Windows.Media.Effects.BlurEffect()
            {
                Radius = 10,
                RenderingBias = RenderingBias.Quality
            };
        }

        private void CheckArchive_Checked(object sender, RoutedEventArgs e)
        {
            CategFilt();
        }

        private void CheckArchive_Unchecked(object sender, RoutedEventArgs e)
        {
            CategFilt();
        }



        //private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        //{
        //BaskAmountTXT.Visibility = Visibility.Visible;
        //BaskAmountTXT.Clear();
        //BaskAmountTXT2.Clear();
        //BaskAmountTXT2.Visibility = Visibility.Visible;
        //var prod = (sender as Button).DataContext as Tariff;
        //AddBask(prod);

        //BaskAmountTXT.Text = BusketOrder.Sum(s => s.Quantity).ToString();
        //BaskAmountTXT2.Text = BusketOrder.Sum(s => s.Quantity).ToString();
    }
    //public void AddBask(Tariff tariff)
    //{
    //    if (BusketOrder.SingleOrDefault(s => s.TariffId == tariff.TariffId) == null)
    //        BusketOrder.Add(tariff);
    //    var bask = BusketOrder.SingleOrDefault(s => s.TariffId == tariff.TariffId);
    //    bask.Quantity++;
    //}

    //public void ImportImages(object sender, RoutedEventArgs e)
    //{
    //    using (var db = new KP_internet())
    //    {
    //        var ImageImport = Directory.GetFiles(@"D:\Все\Всё\Курсовая 4 курс\ВПФки\ВПФки\images");
    //        foreach (var tariff in db.Tariff)
    //        {
    //            var img = ImageImport.FirstOrDefault(imgs => imgs.ToLower().Contains(tariff.Name.ToLower().Trim()));
    //            if (img != null)
    //                tariff.Image = File.ReadAllBytes(img);
    //        }
    //        db.SaveChanges();
    //    }
    //}

    // Закрытие менюшки тыком на ListView

}

