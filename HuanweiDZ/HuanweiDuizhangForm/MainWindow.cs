using HuanweiDZ.Components;
using HuanweiDZ.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HuanweiDZ
{
    [Flags]
    public enum Fulfilled
    {
        None = 0, Bank = 1, Company = 2, All = 3
    }
    public class SidesFulfilledEventArgs : EventArgs
    {
        public Fulfilled fullFilled { get; set; }
        public SidesFulfilledEventArgs(Fulfilled filled)
        {
            fullFilled = filled;
        }
    }
    public partial class MainWindow : Form
    {
        public Fulfilled fulfillmentStatus { get; set; }
        public Ledger bankLedger { get; set; }
        public Ledger comLedger { get; set; }
        public event EventHandler<SidesFulfilledEventArgs> SideLedgerFulfilled;
        protected virtual void OnSideLedgerFulfilled(Fulfilled filled)
        {
            SideLedgerFulfilled?.Invoke(this, new SidesFulfilledEventArgs(filled));
        }
        public MainWindow()
        {
            fulfillmentStatus = Fulfilled.None;
            InitializeComponent();
            SideLedgerFulfilled += Form1_SideLedgerFulfilled;
        }

        private void Form1_SideLedgerFulfilled(object sender, SidesFulfilledEventArgs e)
        {
            Debug.Print("单侧同步完成" + e.fullFilled);
            fulfillmentStatus = e.fullFilled | fulfillmentStatus;
            Debug.Print("目前窗体同步状态：{0}", fulfillmentStatus);
            if (fulfillmentStatus == Fulfilled.All)
            {
                Debug.Print("双侧同步完成，正在执行平账逻辑");
                LedgerBalancer b = new LedgerBalancer(comLedger, bankLedger);
                Ledger BalancedLedger, UnMatchedLedger;
                b.StartBalanceWork(out BalancedLedger, out UnMatchedLedger);
                foreach (LedgerItem item in BalancedLedger)
                {
                    //Debug.Print("{0} BALANCE {1}", item, item.BalanceItem[0]);
                    throw new NotImplementedException();
                }
                foreach (LedgerItem item in UnMatchedLedger)
                {
                    Debug.Print("{0} UNMATCH", item);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbx_DragOver(object sender, DragEventArgs e)
        {
            //拖拽覆盖动作
            //如果被拖拽的是文件那么显示为链接
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void txb_DragDrop_Company(object sender, DragEventArgs e)
        {
            //拖拽释放动作
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Any())
            {
                TextBox tbx = (TextBox)sender;
                tbx.Text = files.First();
                //生成对账条目对象实例
                //Read(tbx.Text, "company");
                //ExcelReader reader = new ExcelReader();
                ExcelReaderXLSReader xlsReader = new ExcelReaderXLSReader();
                comLedger = xlsReader.Read(tbx.Text, "Company");
                //reader.ProgressChanged += reportReaderProgress;
                //comLedger = reader.ReadFromFile(tbx.Text, "company");
                //OnSideLedgerFulfilled(Fulfilled.Company);
                foreach (LedgerItem item in comLedger)
                {
                    textBox3.Text += item.ToString() + "\r\n";
                }
            }
        }
        private void txb_DragDrop_Bank(object sender, DragEventArgs e)
        {
            //拖拽释放动作
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Any())
            {
                TextBox tbx = (TextBox)sender;
                tbx.Text = files.First();
                ExcelReaderXLSReader xlsReader = new ExcelReaderXLSReader();
                bankLedger = xlsReader.Read(tbx.Text, "Bank");
                //生成对账条目对象实例
                //Read(tbx.Text, "bank");
                //ExcelReader reader = new ExcelReader();
                //bankLedger = reader.ReadFromFile(tbx.Text, "bank");
                //OnSideLedgerFulfilled(Fulfilled.Bank);
                foreach (LedgerItem item in bankLedger)
                {
                    textBox3.Text += item.ToString() + "\r\n";
                }
            }
        }
        /*
        private void Read(string filePath, string side)
        {

            ExcelReader reader = new ExcelReader();
            reader.ProgressChanged += reportReaderProgress;
            Ledger ldg = reader.ReadFromFile(filePath, side);
            foreach (LedgerItem item in ldg)
            {
                if (item != null)
                {
                    textBox3.Text += item.ToString() + "\r\n";
                }
                
            }
        }
        */
        private void reportReaderProgress(object sender, ProgressChangedEventArgs e)
        {
            Debug.Print("Progress: {0}, Message: {1}", e.ProgressPercentage, e.UserState);
        }

        

        private void txb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Excel 工作簿|*.xls|Excel 工作簿|*.xlsx";

            if (fileDlg.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("没有选择文件", "没有文件");
            }
            TextBox tbx = (TextBox)sender;
            tbx.Text = fileDlg.FileName;
            ReadFromFile(tbx.Text);
            */
            throw new NotImplementedException();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
