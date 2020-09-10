using System;
using System.Collections.Generic;
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
using Microsoft.Win32;

namespace HuanweiDZ.Views
{
    /// <summary>
    /// Interaction logic for DuiZhangWindow.xaml
    /// </summary>
    public partial class DuiZhangWindow : Window
    {
        public DuiZhangWindow()
        {
            InitializeComponent();
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

        private void OnImportBankDataClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.DefaultExt = "*.xls";
            FileDialog.ShowDialog();
            TransactionItem item = ExcelReader.ReadFromFile(FileDialog.FileName, Side.Bank);
        }
    }
}
