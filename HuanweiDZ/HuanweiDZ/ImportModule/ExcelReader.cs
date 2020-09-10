using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HuanweiDZ.Models;
using EXCEL = Microsoft.Office.Interop.Excel;

namespace HuanweiDZ.ImportModule
{
    class ExcelReader
    {
        public static TransactionItem ReadFromFile(string inputFileName, Side side)
        {
            TransactionItem item = new TransactionItem();
            item.TransactionSide = side;
            EXCEL.Application app = new EXCEL.Application();
            EXCEL.Workbook book = app.Workbooks.Open(inputFileName);
            EXCEL.Worksheet sheet = (EXCEL.Worksheet)book.ActiveSheet;

            //填充item对象

            #region 银行端同步
            //在A4中寻找年份
            int year = int.Parse(sheet.Cells[3, 0].Value);
            //从A6列开始，每行循环直到没有内容为止

            //单列测试：A7 B7为月、日
            int month = int.Parse(sheet.Cells[6, 0].Value);
            int day = int.Parse(sheet.Cells[6, 1].Value);

            DateTime transDate = new DateTime(year, month, day);
            StringBuilder MessageBuilder = new StringBuilder();
            MessageBuilder.Append("Opened File " + book.Name);
            MessageBuilder.Append("Date of Item at Row 7 is: " + transDate.ToString());
            MessageBuilder.AppendLine();
            Debug.Print(MessageBuilder.ToString());



            #endregion



            book.Close(false, null, null);
            app.Quit();
            Marshal.ReleaseComObject(app);
            return item;
        }
    }
}
