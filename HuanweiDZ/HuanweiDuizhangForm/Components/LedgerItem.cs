using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HuanweiDuizhangForm.Components
{
    public class LedgerItem : IEquatable<LedgerItem>
    {

        public string Side { get; set; }
        public DateTime EntryDate { get; set; }
        public string Identifier { get; set; }
        public string Summary { get; set; }
        public decimal StartFund { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string Direction { get; set; }
        public decimal RemainingFund { get; set; }

        public bool isBalanced { get; set; } = false;
        public ObservableCollection<LedgerItem> BalanceItem { get; set; }

        public bool Equals(LedgerItem other)
        {
            bool isCreditMatch = Credit == other.Credit;
            bool isDebitMatch = Debit == other.Debit;
            bool isBothMatch = isCreditMatch && isDebitMatch;
            return isBothMatch;
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}",
                Side, EntryDate, Identifier, Summary,
                StartFund, Credit, Debit, Direction,
                RemainingFund);
        }
    }

}
