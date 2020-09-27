using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuanweiDZ.Components
{
    class BalancedPair
    {
        public Ledger LedgerCo { get; set; }
        public Ledger LedgerBa { get; set; }
        public bool IsBalanced
        {
            get
            {
                return LedgerCo.Sum() == LedgerBa.Sum();
            }
        }

        public BalancedPair()
        {
            LedgerCo = new Ledger();
            LedgerBa = new Ledger();
        }
    }
}
