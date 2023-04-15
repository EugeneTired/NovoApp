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
using System.Data.Entity;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System.IO;

namespace Novoapp.Forms
{
    /// <summary>
    /// Логика взаимодействия для Contract.xaml
    /// </summary>
    public partial class ContractFoorm : Window
    {
        public ContractFoorm(Clients client)
        {
            this.client = client;
            InitializeComponent();
            Obud.Visibility = Visibility.Collapsed;
            ClientNameBlock.Text = $"{client.Surname} {client.Name} {client.Patron}";

            AddressBlock.Text = $"Адрес проживания: {client.ClientsExtra.Address}";
            using (var db = new KP_Mihailov_InternetEntities())
            {
                CBequipBlock.ItemsSource = db.Equipment.ToList();
                CBtariffBlock.ItemsSource = db.Tariff.Where(x => !x.Archived.Value).ToList();
                if (client.Contract != null)
                {
                    CBtariffBlock.SelectedItem = CBtariffBlock.ItemsSource.Cast<Tariff>().First(s => s.TariffId == client.Contract.TariffId);
                    CBequipBlock.SelectedItem = CBequipBlock.ItemsSource.Cast<Equipment>().First(s => s.EquipmentId == client.Contract.EquipmentId);
                    TextBlockDate.Text = client.Contract.CreationDate.ToShortDateString();
                    TarifEmp.Text = $"{client.Contract.Employee.Surname} {client.Contract.Employee.Name} {client.Contract.Employee.Patron}";
                    ConclBox.DisplayDateStart = client.Contract.CreationDate;
                    ConclBox.SelectedDate = client.Contract.TermOfConcl;
                }
                else
                {
                    ConclBox.DisplayDateStart = DateTime.Today;
                    TextBlockDate.Text = "Еще не заключен";
                    TarifEmp.Text = "Еще не заключен";
                }
            }
           
        }

        private Clients client;
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
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ClientNameBlock.Text == "" || AddressBlock.Text == "" || CBequipBlock.SelectedItem.ToString() == "" || TextBlockDate.Text == "" || TarifEmp.Text == "" ||  CBtariffBlock.SelectedItem.ToString() == "")
            {
                ShowAnim("Есть не заполненные обязательные поля");
            }
            else
            {
                try
                {
                    using (var db = new KP_Mihailov_InternetEntities())
                    {
                        Novoapp.Contract contract;
                        if (client.Contract != null)
                        {
                            //change
                            contract = db.Contract.Include(s => s.Employee).First(s => s.ContractId == client.ContractId);
                            contract.EquipmentId = (CBequipBlock.SelectedItem as Equipment).EquipmentId;
                            contract.TariffId = (CBtariffBlock.SelectedItem as Tariff).TariffId;
                            contract.TermOfConcl = ConclBox.SelectedDate;
                        }
                        else
                        {
                            //add
                            contract = new Novoapp.Contract
                            {
                                CreationDate = DateTime.Now,
                                EmployeeId = AuthUser.AuthPerson.EmployeeId,
                                EquipmentId = (CBequipBlock.SelectedItem as Equipment).EquipmentId,
                                TariffId = (CBtariffBlock.SelectedItem as Tariff).TariffId,
                                Employee = AuthUser.AuthPerson,
                              //TermOfConcl
                            };
                            db.Contract.Add(contract);
                            var client = db.Clients.First(s => s.ClientId == this.client.ClientId);
                            client.ContractId = contract.ContractId;
                        }
                        //if (ImageBox.Source == null) tar.Image = null;
                        //else if (!string.IsNullOrWhiteSpace(FileNamePath)) tar.Image = File.ReadAllBytes(FileNamePath);
                        db.SaveChanges();

                        this.client.ContractId = contract.ContractId;
                        this.client.Contract = contract;

                        TextBlockDate.Text = contract.CreationDate.ToShortDateString();
                        TarifEmp.Text = $"{contract.Employee.Surname} {contract.Employee.Name} {contract.Employee.Patron}";

                        ShowAnim("Успешно сохранено");
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
        private bool _isActive;
        private int _isCount = 0;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        // Источник шаблона
        private readonly string _templatePath = @"\Extra\ContractExample.docx";

        // Печать фактуры по шаблону
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new KP_Mihailov_InternetEntities())
                {
                    var saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Documents | *.docx | Documents | *.doc";
                    if (saveDialog.ShowDialog() != true) return;

                    var wordApp = new Word.Application();

                    var date = client.Contract.CreationDate.ToShortDateString();
                    var madeBy = TarifEmp.Text;

                    var FinalPrice = client.Contract.Equipment.Cost + client.Contract.Tariff.MonthlyCost;

                    var wordDoc = wordApp.Documents.Open(Directory.GetCurrentDirectory() + _templatePath);

                    ReplaceWord("{ContractId}", client.ContractId.ToString(), wordDoc);
                    ReplaceWord("{Employee}", client.Contract.Employee.ToString(), wordDoc);
                    ReplaceWord("{CreateDate}", client.Contract.CreationDate.ToShortDateString(), wordDoc);
                    ReplaceWord("{Concl}", client.Contract.TermOfConcl?.ToShortDateString(), wordDoc);
                    ReplaceWord("{Price}", FinalPrice.ToString(), wordDoc);
                    ReplaceWord("{Tariff}", client.Contract.Tariff.Name, wordDoc);
                    ReplaceWord("{Client}", client.ToString(), wordDoc);

                    ReplaceWord("{date}", date.ToString(), wordDoc);


                    wordDoc.SaveAs2(saveDialog.FileName);

                    wordDoc.Close();
                    wordApp.Quit();


                    //ReplaceWordStub("{dateNow}", date.ToShortDateString(), wordDocument);
                    //ReplaceWordStub("{nameWorker}", name, wordDocument);
                    //ReplaceWordStub("{allWorkers}", amountEmployee.Text.ToString(), wordDocument);
                    //ReplaceWordStub("{tellers}", amountTeller.Text.ToString(), wordDocument);
                    //ReplaceWordStub("{gids}", amountGuide.Text.ToString(), wordDocument);
                    //ReplaceWordStub("{careTaker}", amountCareTaker.Text.ToString(), wordDocument);
                    //ReplaceWordStub("{Keepers}", amountKeeper.Text.ToString(), wordDocument);
                    //ReplaceWordStub("{HRDS}", amountHRD.Text.ToString(), wordDocument);

                    //wordDocument.SaveAs2(saveDialog.FileName);

                    //wordDocument.Close();
                    //wordApp.Quit();

                    //errorInfo.Text = string.Empty;
                }
            }
            catch
            {
                ShowAnim("Закройте файл перед сохранением");
            }
        }
        private void ReplaceWord(string stub, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stub, ReplaceWith: text,Replace: Word.WdReplace.wdReplaceAll);
        }
    }
}
