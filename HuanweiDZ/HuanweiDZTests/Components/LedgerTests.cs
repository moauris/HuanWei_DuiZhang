using Microsoft.VisualStudio.TestTools.UnitTesting;
using HuanweiDZ.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuanweiDZ.Components.Tests
{
    [TestClass()]
    public class LedgerTests
    {
        [TestMethod()]
        public void SortTest()
        {
            Ledger led = new Ledger();
            led.Add(new LedgerItem
            {
                Info = "2020，01，01，记账 1*，买猫粮",
                Debit = 6753.43,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，02，记账 22*，买猫粮",
                Debit = 25.39,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，03，记账 25*，买猫粮",
                Debit = 3947.22,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，03，记账 29*，买猫粮",
                Debit = 7787,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，05，记账 32*，买猫粮",
                Debit = 663,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，15，记账 33*，买猫粮",
                Debit = 69,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，18，记账 38*，买猫粮",
                Debit = 669,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，22，记账 40*，买猫粮",
                Debit = 996.96,
                Direction = "借",
                RemainingFund = 0
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，25，记账 51*，买猫粮",
                Debit = 6883.7,
                Direction = "借",
                RemainingFund = 3365774
            });
            led.Add(new LedgerItem
            {
                Info = "2020，01，29，记账 63*，买猫粮",
                Debit = 175,
                Direction = "借",
                RemainingFund = 3365774
            });
            Console.WriteLine("### 排序之前 ###");
            for (int i = 0; i < led.Count; i++)
            {
                Console.WriteLine(led[i].ToString());
            }

            led.Sort();

            Console.WriteLine("### 排序之后 ###");
            for (int i = 0; i < led.Count; i++)
            {
                Console.WriteLine(led[i].ToString());
            }
        }
    }
}