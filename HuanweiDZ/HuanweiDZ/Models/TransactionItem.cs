using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuanweiDZ.Models
{
    class TransactionItem
    {
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

        private Direction flow;

        public Direction Flow
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

    class CompanyTransaction : TransactionItem
    {
        public CompanyTransaction()
        { TransactionSide = Side.Company; }
    }

    class BankTransaction : TransactionItem
    {
        public BankTransaction()
        { TransactionSide = Side.Bank; }
    }
    public enum Direction
    {
        Dr,Cr
    }

    public enum Side
    {
        Company, Bank
    }
}
