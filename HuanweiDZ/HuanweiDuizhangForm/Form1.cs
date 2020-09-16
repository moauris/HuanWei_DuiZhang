using HuanweiDuizhangForm.Components;
using HuanweiDuizhangForm.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HuanweiDuizhangForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

        private void txb_DragDrop(object sender, DragEventArgs e)
        {
            //拖拽释放动作
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Any())
            {
                TextBox tbx = (TextBox)sender;
                tbx.Text = files.First();
                //生成对账条目对象实例
                ReadFromFile(tbx.Text);

            }
        }

        private void ReadFromFile(string filePath)
        {

            ExcelReader reader = new ExcelReader();
            reader.ProgressChanged += reportReaderProgress;
            Ledger ldg = reader.ReadFromFile(filePath, "Company");
            ldg.PrintContents();
            PopulateDataGrid(ldg);
        }

        private void reportReaderProgress(object sender, ProgressChangedEventArgs e)
        {
            Debug.Print("Progress: {0}, Message: {1}", e.ProgressPercentage, e.UserState);
        }

        private void PopulateDataGrid(Ledger ldg)
        {
            dataGridView1.DataSource = ldg;
        }

        private void txb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Excel 工作簿|*.xls|Excel 工作簿|*.xlsx";

            if (fileDlg.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("没有选择文件", "没有文件");
            }
            TextBox tbx = (TextBox)sender;
            tbx.Text = fileDlg.FileName;
            ReadFromFile(tbx.Text);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
