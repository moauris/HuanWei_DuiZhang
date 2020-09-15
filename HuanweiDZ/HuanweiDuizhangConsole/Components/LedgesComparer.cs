using HuanweiDuizhangConsole.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HuanweiDuizhangConsole.Components
{
    class LedgesComparer
    {
        public Ledger CompanyLedger { get; set; }
        public Ledger BankLedger { get; set; }
        public bool IsCompareReady
        {
            get
            {
                return CompanyLedger.Count > 0 && BankLedger.Count > 0;
            }
        }

        public void Consolidate(out Ledger consolidatedLedger, out Ledger unmatchedLedger)
        {

            consolidatedLedger = new Ledger();
            unmatchedLedger = new Ledger();
            if (!IsCompareReady) return;

            foreach (LedgerItem baseItem in CompanyLedger)
            {
                foreach (LedgerItem compareItem in BankLedger)
                {
                    if (baseItem == compareItem)
                    {
                        consolidatedLedger.Add(baseItem);
                        consolidatedLedger.Add(compareItem);
                    }
                    else
                    {
                        unmatchedLedger.Add(baseItem);
                        unmatchedLedger.Add(compareItem);
                    }
                }
            }
        }
    }
}
