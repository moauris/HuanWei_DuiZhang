using HuanweiDZ.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HuanweiDZ.Services
{
    class LedgerBalancer
    {
        
        public int StartBalanceWork(Ledger company, Ledger bank, out Ledger balanced, out Ledger unmatched)
        {
            int FoundBalanced = 0;
            if (company == null || bank == null)
            {
                balanced = null;
                unmatched = null;
                return -1;
            }
            //开始对账活动
            Debug.Print("开始对账工作：");
            balanced = new Ledger();
            unmatched = new Ledger();

            //由公司侧开始循环，寻找所有匹配的银行账目， 生成一次匹配成功的账本
            Debug.Print("由公司侧开始循环，寻找所有匹配的银行账目， 生成一次匹配成功的账本");
            for (int i = 0; i < company.Count; i++)
            {
                for (int j = 0; j < bank.Count; j++)
                {
                    Debug.Print("正在对比公司项第{0}与银行项第{1}", i, j);
                    LedgerItem CompanyItem = company[i];
                    LedgerItem bankItem = bank[j];

                    bool IsBalanced = CompanyItem.Equals(bankItem);
                    Debug.Print("公司项: ", CompanyItem.ToString());
                    Debug.Print("银行项: ", bankItem.ToString());
                    if (IsBalanced)
                    {
                        Debug.Print("匹配成功！");
                        //coItem.BalanceItem.Add(baItem);
                        balanced.Add(company[i]);
                        balanced.Add(bank[j]);
                        company.Remove(company[i]);
                        bank.Remove(bank[j]);
                        FoundBalanced++;

                    }
                }
            }
            for (int i = 0; i < company.Count; i++)
            {
                unmatched.Add(company[i]);
            }
            for (int i = 0; i < bank.Count; i++)
            {
                unmatched.Add(bank[i]);
            }

            return FoundBalanced;
        }

    }
}
