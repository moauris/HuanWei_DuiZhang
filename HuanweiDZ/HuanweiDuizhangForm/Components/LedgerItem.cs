using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HuanweiDZ.Components
{
    public class LedgerItem : IEquatable<LedgerItem>
    {

        public string Side { get; set; } //公司或者银行
        //public DateTime EntryDate { get; set; } //发生日期
        //public string Identifier { get; set; } //凭证号
        public string Info { get; set; } //摘要
        //public double StartFund { get; set; } //初始金额
        public double Credit { get; set; } //贷方
        public double Debit { get; set; } //借方
        public string Direction { get; set; } //方向（贷或者借）
        public double RemainingFund { get; set; } //余额

        //public bool isBalanced { get; set; } = false; //是否有齐平账目, 需要变为只读属性，返回this.BalanceItem.Sum = Math.Abs(this.Credit - this.Debit)
        //public Ledger BalanceItem { get; set; } //齐平对象账目

        public bool Equals(LedgerItem other)
        {
            bool isCreditMatch = Credit == other.Credit;
            bool isDebitMatch = Debit == other.Debit;
            bool isBothMatch = isCreditMatch && isDebitMatch;
            return isBothMatch;
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}",
                Side, Info, Credit, Debit, Direction,
                RemainingFund);
        }

        public static bool operator >(LedgerItem left, LedgerItem right)
        {
            double leftSum = left.Debit - left.Credit;
            double rightSum = right.Debit - right.Credit;
            return leftSum > rightSum;

        }
        public static bool operator <(LedgerItem left, LedgerItem right)
        {
            double leftSum = left.Debit - left.Credit;
            double rightSum = right.Debit - right.Credit;
            return leftSum < rightSum;
        }

        public static LedgerItem Parse(string[] Line, string side)
        {
            //判定输入字符串是否符合标准 例
            //01,07,记账    22,员工工资,猫猫公司,,103255.33,借,496744.67,
            //以下代码生成 LedgerItem 对象
            //两侧符合要求的对象：第0-4不为空，5或6至少有一位不为空，不为空时可以被转换为double。7为平、借、或者贷，余额为double

            //是否大于8项，否则返回null
            if (Line.Length < 7) return null;
            double[] CreditDebitRemain = new double[] { 0, 0, 0 };
            string LedgerInfo = "";
            for (int i = 0; i < 5; i++)
            { //判定1： 0-4项是否为空字符串
                if(Line[i] == string.Empty)
                {
                    Console.WriteLine("The {0} item is empty", i);
                    return null;
                }
                else
                {
                    if (i < 4)
                    {
                        LedgerInfo = LedgerInfo + Line[i] + "，";
                    }
                    else
                    {
                        LedgerInfo = LedgerInfo + Line[i];
                    }
                }
            }
            if (Line[7].Length != 1)
            { //判定3： 7项长度不为1
                Console.WriteLine("The 7th item length is no 1");
                return null;
            }
            //判定2： 试图转换5, 6, 8项不为空的值为double
            int EmptyTimes = 0;
            int Cycler = 0;
            foreach (int i in new int[] { 5, 6, 8 })
            {
                if (Line[i] == string.Empty)
                {
                    if (EmptyTimes++ > 2)
                    {
                        Console.WriteLine("Too many times has Credit Debit Remain set to empty");
                        return null;
                    }
                }
                else
                {
                    if (!double.TryParse(Line[i], out CreditDebitRemain[Cycler]))
                    {
                        Console.WriteLine("String {0} cannot be parsed as double.", Line[i]);
                        return null;
                    }
                }
                Cycler++;
            }

            
            return new LedgerItem()
            {
                Side = side, Info = LedgerInfo
                , Credit = CreditDebitRemain[0]
                , Debit = CreditDebitRemain[1]
                , RemainingFund = CreditDebitRemain[2]
                , Direction = Line[7]
            };
            //Console.WriteLine("输入的字符串集合有{0}项", Line.Length);

            //生成LedgerItem


        }
    }

}
