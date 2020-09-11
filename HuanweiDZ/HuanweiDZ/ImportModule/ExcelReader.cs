using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    
    class ExcelReader : IDisposable
    {
        public event ProgressChangedEventHandler ProgressChanged;
        protected virtual void OnProgressChanged(int progressPercentage)
        {
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(progressPercentage, null));
        }
        protected virtual void OnProgressChanged(int progressPercentage, string state)
        {
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(progressPercentage, state));
        }
        public ObservableCollection<TransactionItem> ReadFromFile(string inputFileName, Side side)
        {
            OnProgressChanged(0, "读取开始");

            var itemCollection = new ObservableCollection<TransactionItem>();

            OnProgressChanged(5, "正在加载Excel应用实例");
            EXCEL.Application app = new EXCEL.Application();
            OnProgressChanged(10, $"正在打开工作簿 {inputFileName}");
            EXCEL.Workbook book = app.Workbooks.Open(inputFileName);

            OnProgressChanged(15, "正在打开工作表");
            EXCEL.Worksheet sheet = (EXCEL.Worksheet)book.ActiveSheet;
            OnProgressChanged(20, $"工作表打开完成：{sheet.Name}");
            //填充item对象

            #region 银行端同步
            string year = (string)sheet.Range["A4"].Value;
            //值为“2020年”，只取前4位
            //从A6列开始，每行循环直到没有内容为止

            //单列测试：A7 B7为月、日
            //int month = int.Parse(((string)sheet.Range["A7"].Value));
            //int day = int.Parse(((string)sheet.Range["B7"].Value));

            //Debug.Print("Date is {0}-{1}-{2}", year, month, day);
            //DateTime transDate = new DateTime(year, month, day);
            //从A6开始，直到Arow的值为空，开始循环：
            /// 计算工作总量 progress 20 ~ 90, 70 percent
                
            int TotalRow = sheet.Range["A6"].CurrentRegion.Rows.Count - 5;
            Debug.Print($"Total Row is: {TotalRow}");
            int EndRow = sheet.Range["A6"].Row + TotalRow;
            Debug.Print($"End Row is: {EndRow}");
            int CycleCounter = 0;
            for (EXCEL.Range r = sheet.Range["A6"]; r.Row != EndRow; r = r.Offset[1, 0])
            {
                Debug.Print($"{r.Value} is empty?");
                int IncrementProgress = Convert.ToInt32(CycleCounter++ / 0.7);

                OnProgressChanged(20 + IncrementProgress, $"同步台账条目:{CycleCounter}/{TotalRow}");
                List<string> ParmCollection = new List<string>();
                ParmCollection.Add(year);
                Debug.Print("Currently Executing: " + r.Address);
                TransactionItem item = new TransactionItem
                {
                    TransDate = DateTime.Now, //测试用
                    Identifier = r.Offset[0, 2].Value,
                    Summary = r.Offset[0, 3].Value,
                    Debit = ConvertDecimal(r.Offset[0, 5].Value),
                    Credit = ConvertDecimal(r.Offset[0, 6].Value),
                    RemainingBalance = ConvertDecimal(r.Offset[0, 7].Value),
                    TransactionSide = side
                };
                itemCollection.Add(item);
                Task.Delay(100);
            }
            OnProgressChanged(90, $"同步台账条目完成");
            #endregion
            OnProgressChanged(95, $"正在清理资源");
            book.Close(false, null, null);
            app.Quit();
            Marshal.ReleaseComObject(app);
            OnProgressChanged(100, $"读取完成");
            return itemCollection;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Converts a string to decimal, if cannot, return 0
        /// </summary>
        /// <param name="input"> Potential Decimal String </param>
        /// <returns></returns>
        private decimal ConvertDecimal(object input)
        {

            decimal result = 0;
            if (input is string && decimal.TryParse(input as string, out _))
            {
                result = decimal.Parse(input as string);
            }
            if (input is decimal) result = (decimal)input;
            return result;
        }
    }
}
