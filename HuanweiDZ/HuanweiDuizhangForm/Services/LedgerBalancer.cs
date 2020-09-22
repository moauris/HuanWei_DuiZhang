using HuanweiDuizhangForm.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuanweiDuizhangForm.Services
{
    class LedgerBalancer
    {
        public Ledger ledger1 { get; set; }
        public Ledger ledger2 { get; set; }

        public LedgerBalancer(Ledger company, Ledger bank)
        {
            ledger1 = company;
            ledger2 = bank;
        }

        public int StartBalanceWork(out Ledger balanced, out Ledger unmatched)
        {
            int FoundBalanced = 0;
            if (ledger1 == null || ledger2 == null)
            {
                balanced = null;
                unmatched = null;
                return -1;
            }
            //开始对账活动
            balanced = new Ledger();
            unmatched = new Ledger();

            //由公司侧开始循环，寻找所有匹配的银行账目， 生成一次匹配成功的账本
            foreach (LedgerItem coItem in ledger1)
            {
                foreach (LedgerItem baItem in ledger2)
                {
                    if (coItem == baItem)
                    {
                        if (baItem != null)
                        {
                            coItem.BalanceItem.Add(baItem);
                            balanced.Add(coItem);
                            ledger1.Remove(coItem);
                            ledger2.Remove(baItem);
                            FoundBalanced++;
                        }
                        
                    }
                }
            }

            foreach (LedgerItem item in ledger1)
            {
                unmatched.Add(item);
            }
            foreach (LedgerItem item in ledger2)
            {
                unmatched.Add(item);
            }

            return FoundBalanced;
        }

    }
}
