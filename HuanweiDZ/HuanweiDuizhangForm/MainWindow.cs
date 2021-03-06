﻿using HuanweiDZ.Components;
using HuanweiDZ.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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
                DialogResult answer = MessageBox.Show("两表准备完毕，是否开始执行平帐？", "准备完毕", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.No) return;
                Debug.Print("双侧同步完成，正在执行平账逻辑");
                LedgerBalancer b = new LedgerBalancer();
                Ledger BalancedLedger;
                //TODO: 需要更好的对账机制


                //TODO: 循环每一条账目，并且生成相应的网页
                /*
                 * <table class="result balanced" width="auto">
                 * <tr>
                 * <th/>
                 * </tr>
                 * <tr>
                 * <td/>
                 * </tr>
                 * ...
                 * </table>
                 * 
                 */


                /*
                string message = string.Format("##### 以下为平账款项，共{0}条 #####\r\n"
                    , b.StartBalanceWork(comLedger, bankLedger, out BalancedLedger));
                for (int i = 0; i < BalancedLedger.Count; i++)
                {
                    message += BalancedLedger[i].ToString() + "\r\n";
                }
                message += "##### 以下为公司侧未平账目 #####\r\n";
                for (int i = 0; i < comLedger.Count; i++)
                {
                    message += comLedger[i].ToString() + "\r\n";
                }
                message += "##### 以下为银行侧未平账目 #####\r\n";
                for (int i = 0; i < bankLedger.Count; i++)
                {
                    message += bankLedger[i].ToString() + "\r\n";
                }
                */
                string message = string.Format("<p>##### 以下为平账款项，共{0}条 #####</p>\r\n", b.StartBalanceWork(comLedger, bankLedger, out BalancedLedger));
                message += "\t<table class=\"result balanced\" style=\"width:100%\">\r\n";
                message += "\t\t<tr>\r\n";
                message += "\t\t\t<th>侧</th>\r\n";
                message += "\t\t\t<th>款项信息</th>\r\n";
                message += "\t\t\t<th>借方</th>\r\n";
                message += "\t\t\t<th>贷方</th>\r\n";
                message += "\t\t\t<th>方向</th>\r\n";
                message += "\t\t\t<th>余额</th>\r\n";
                message += "\t\t</tr>\r\n";

                for (int i = 0; i < BalancedLedger.Count; i++)
                {
                    //message += BalancedLedger[i].ToString() + "\r\n";
                    message += "\t\t<tr>\r\n";
                    message += string.Format("\t\t\t<td>{0}</td>\r\n"
                        , BalancedLedger[i].Side);
                    message += string.Format("\t\t\t<td>{0}</td>\r\n"
                        , BalancedLedger[i].Info); 
                    message += string.Format("\t\t\t<td id=\"money\">￥{0:N2}</td>\r\n"
                        , BalancedLedger[i].Credit);
                    message += string.Format("\t\t\t<td id=\"money\">￥{0:N2}</td>\r\n"
                        , BalancedLedger[i].Debit);
                    message += string.Format("\t\t\t<td>{0}</td>\r\n"
                        , BalancedLedger[i].Direction);
                    message += string.Format("\t\t\t<td id=\"money\">￥{0:N2}</td>\r\n"
                        , BalancedLedger[i].RemainingFund);
                    message += "\t\t</tr>\r\n";

                }
                message += "\t</table>\r\n";
                textBox3.Text = message;
                MasterWindow window = new MasterWindow();
                window.ShowDialog();
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
                //Read(tbx.Text, "公司");
                //ExcelReader reader = new ExcelReader();
                ExcelReaderXLSReader xlsReader = new ExcelReaderXLSReader();
                comLedger = xlsReader.Read(tbx.Text, "公司");
                //reader.ProgressChanged += reportReaderProgress;
                //comLedger = reader.ReadFromFile(tbx.Text, "公司");
                //OnSideLedgerFulfilled(Fulfilled.Company);

                //MessageBox.Show(string.Format("The Ledger size is {0}", comLedger.Count),"Starting Showing in TextBox");
                string message = textBox3.Text;
                for (int i = 0; i < comLedger.Count; i++)
                {
                    if (comLedger[i] == null)
                    {
                        message += "null item!\r\n";
                    }
                    else
                    {
                        message += comLedger[i].ToString() + "\r\n";
                    }

                }
                textBox3.Text = message;
                OnSideLedgerFulfilled(Fulfilled.Company);
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
                bankLedger = xlsReader.Read(tbx.Text, "银行");
                //生成对账条目对象实例
                //Read(tbx.Text, "银行");
                //ExcelReader reader = new ExcelReader();
                //bankLedger = reader.ReadFromFile(tbx.Text, "银行");
                //OnSideLedgerFulfilled(Fulfilled.Bank);
                string message = textBox3.Text;
                for (int i = 0; i < bankLedger.Count; i++)
                {
                    if (bankLedger[i] == null)
                    {
                        message += "null item!\r\n";
                    }
                    else
                    {
                        message += bankLedger[i].ToString() + "\r\n";
                    }
                }
                textBox3.Text = message;
                OnSideLedgerFulfilled(Fulfilled.Bank);
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
    }
}
