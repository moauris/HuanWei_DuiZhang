using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuanweiDZ.Models
{
    public class TransactionItem
    {
        public TransactionItem() { }
        public TransactionItem(List<string> parms)
        {
            //同步日期
            int year = int.Parse(parms[0]);
            int month = int.Parse(parms[1]);
            int day;
            
            if (int.TryParse(parms[2], out _))
            {
                day = int.Parse(parms[2]);
            }
            else
            {
                day = 1;
            }
            TransDate = new DateTime(year, month, day);

            //同步其他
            Identifier = parms[3];
            Summary = parms[4];
            decimal tempCr = 0;
            decimal.TryParse(parms[5], out tempCr);
            Credit = tempCr;
            decimal tempDr = 0;
            decimal.TryParse(parms[6], out tempDr);
            Credit = tempDr;
            Flow = parms[7];
            decimal tempRm = 0;
            decimal.TryParse(parms[8], out tempRm);
            RemainingBalance = tempRm;
            //共同步9项
        }
        private DateTime transdate;

        public DateTime TransDate
        {
            get { return transdate; }
            set { transdate = value; }
        }

        private string identifier;

        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        private string summary;

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        private decimal credit;

        public decimal Credit
        {
            get { return credit; }
            set { credit = value; }
        }

        private decimal debit;

        public decimal Debit
        {
            get { return debit; }
            set { debit = value; }
        }

        private string flow;

        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }

        private decimal remain;

        public decimal RemainingBalance
        {
            get { return remain; }
            set { remain = value; }
        }

        private Side transactionside;

        public Side TransactionSide
        {
            get { return transactionside; }
            set { transactionside = value; }
        }


    }

    public enum Side
    {
        Company, Bank
    }
}
