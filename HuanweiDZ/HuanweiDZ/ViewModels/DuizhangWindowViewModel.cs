using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using HuanweiDZ.Models;
using System.Windows.Controls;

namespace HuanweiDZ.ViewModels
{
    public class DuizhangWindowViewModel : INotifyPropertyChanged
    {
        public DuizhangWindowViewModel()
        { }
        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private TransactionItem selectedItem;

        public TransactionItem SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        private ObservableCollection<TransactionItem> bankledger;

        public ObservableCollection<TransactionItem> BankLedger
        {
            get { return bankledger; }
            set { bankledger = value; OnPropertyChanged("BankLedger"); }
        }

        private ObservableCollection<TransactionItem> companyledger;

        public ObservableCollection<TransactionItem> CompanyLedger
        {
            get { return companyledger; }
            set { companyledger = value; OnPropertyChanged("CompanyLedger"); }
        }
    }
}
