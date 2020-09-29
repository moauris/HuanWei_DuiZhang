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
    public class LedgerItemTests
    {
        [TestMethod()]
        public void EqualsTest()
        {
            //LedgerItem item1 = LedgerItem.Parse(new string[] { "2020","01","01","记账 1*", "年初恢复零余额账户用款额度", "600000", "0", "借", "600000" }, "Company");
            //LedgerItem item2 = LedgerItem.Parse(new string[] { "2020","01","07","记账 8*","淮海路工程款", "0", "32000.7", "借", "397453.23" }, "Company");
            LedgerItem item1 = new LedgerItem
            {
                Info = "2020，01，01，记账 1*，买猫粮",
                Debit = 6753.43,
                Direction = "借",
                RemainingFund = 3365774
            };
            LedgerItem item2 = new LedgerItem
            {
                Info = "2020，01，28，记账 337*，买狗",
                Debit = 73,
                Direction = "借",
                RemainingFund = 553
            };


            Console.WriteLine("item1.Debit = {0}, item2.Debit = {1}, result of item1 > item2 is: {2}"
                ,item1.Debit, item2.Debit, item1 > item2);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            LedgerItem item1 = new LedgerItem
            {
                Info = "2020，01，01，记账 1*，买猫粮",
                Debit = 6753.43,
                Direction = "借",
                RemainingFund = 3365774
            };
            Console.WriteLine(item1.ToString());
        }

        [TestMethod()]
        public void ParseTest()
        {
            LedgerItem item1 = LedgerItem.Parse(new string[] { "2020", "01", "01", "记账 1*", "年初恢复零余额账户用款额度", "600000", "0", "借", "600000" }, "Company");

            Console.WriteLine(item1.ToString());
        }
    }
}