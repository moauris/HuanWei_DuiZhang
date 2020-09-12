using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using HuanweiDZ.ImportModule;
using HuanweiDZ.Models;
using HuanweiDZ.ViewModels;
using Microsoft.Win32;

namespace HuanweiDZ.Views
{
    /// <summary>
    /// Interaction logic for DuiZhangWindow.xaml
    /// </summary>
    public partial class DuiZhangWindow : Window, INotifyPropertyChanged
    {
        public DuiZhangWindow()
        {

            ActiveVM = new DuizhangWindowViewModel();
            
            InitializeComponent();
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private DuizhangWindowViewModel activevm;

        public DuizhangWindowViewModel ActiveVM
        {
            get { return activevm; }
            set { activevm = value; OnPropertyChanged("ActiveVM"); }
        }

        private int currentProgress;

        public int CurrentProgress
        {
            get { return currentProgress; }
            set { currentProgress = value; OnPropertyChanged("CurrentProgress"); }
        }

        private string progressMessage;

        public string ProgressMessage
        {
            get { return progressMessage; }
            set { progressMessage = value; OnPropertyChanged("ProgressMessage"); }
        }

        private void OnWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception exp)
            {

                Debug.Print("There is an Exception: {0}", exp.Message);
            }
        }

        private async void OnImportBankDataClicked(object sender, RoutedEventArgs e)
        {

            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.DefaultExt = "*.xls";
            FileDialog.ShowDialog();
            ExcelReader reader = new ExcelReader();
            reader.ProgressChanged += OnBankReaderProgressChanged;
            ActiveVM.BankLedger = await reader.ReadFromFile(FileDialog.FileName, Side.Bank);
        }

        private void OnBankReaderProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
            Debug.Print($"Changed Progress:{CurrentProgress}");
            ProgressMessage = e.UserState.ToString();
        }
    }
}
