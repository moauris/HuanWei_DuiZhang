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

namespace HuanweiDZ.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        public string CurrentVersion { get; } = System.Reflection
            .Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnImportFileClicked(object sender, RoutedEventArgs e)
        {
            Window importFileWindow = new DuiZhangWindow();
            importFileWindow.ShowDialog();
        }
    }
}
