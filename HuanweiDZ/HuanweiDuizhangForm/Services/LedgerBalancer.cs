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
        
        public int StartBalanceWork(Ledger company, Ledger bank, out Ledger balanced)
        {
            int FoundBalanced = 0;
            if (company == null || bank == null)
            {
                balanced = null;
                //unmatched = null;
                return -1;
            }
            //开始对账活动
            Debug.Print("开始对账工作：");
            balanced = new Ledger();
            //unmatched = new Ledger();

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
                    Debug.Print("公司项: {0}", CompanyItem.ToString());
                    Debug.Print("银行项: {0}", bankItem.ToString());
                    if (IsBalanced)
                    {
                        Debug.Print("匹配成功！");
                        //coItem.BalanceItem.Add(baItem);
                        balanced.Add(company[i]);
                        balanced.Add(bank[j]);
                        company.Remove(company[i]);
                        bank.Remove(bank[j]);
                        i--; j--; //移除一位i和j之后，需要重新判定新的同一位置，不然会有跳过的项目
                        FoundBalanced++;

                    }
                }
            }
            /* 保持原有的两侧对象
            for (int i = 0; i < company.Count; i++)
            {
                unmatched.Add(company[i]);
            }
            for (int i = 0; i < bank.Count; i++)
            {
                unmatched.Add(bank[i]);
            }
            */

            //TODO: 当前一次对账完成，需要进行【一对多】项目的对账
            //首先是左对右的【一对多】
            //MultiBalancer(company, bank);


            return FoundBalanced;
        }

        private void MultiBalancer(Ledger left, Ledger right)
        {
            left.Sort();
            right.Sort();
            for (int i = 0; i < left.Count; i++)
            {
                int j = 0;
                double rightSum = 0;
                do
                {
                    if (true)   //如果左侧比右侧大，则开始判定循环
                    {
                        do
                        {
                            rightSum += right[j].Debit - right[j].Credit;       //对 rightSum 进行增长

                        } while (rightSum < (left[i].Debit - left[i].Credit));  //当右侧的和超过左侧时， 跳出循环，重置 rightSum,
                    }
                    else
                    {

                    }

                    j++;
                } while (j < right.Count);
            }
        }

    }
}
