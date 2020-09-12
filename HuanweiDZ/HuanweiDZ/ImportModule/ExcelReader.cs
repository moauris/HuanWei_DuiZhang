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
    
    class ExcelReader
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
        public async Task<ObservableCollection<TransactionItem>> ReadFromFile(string inputFileName, Side side)
        {
            OnProgressChanged(0, "读取开始");

            //Task.Delay(1000);
            OnProgressChanged(1, "正在检查输入文件名有效性");

            //Task.Delay(1000);
            FileInfo File = new FileInfo(inputFileName);
            if (File.Exists == false)
            {
                OnProgressChanged(100, "无效文件名");
                return new ObservableCollection<TransactionItem>();
            }

            OnProgressChanged(5, "正在加载Excel应用实例");

            //Task.Delay(1000);
            EXCEL.Application app = new EXCEL.Application();
            OnProgressChanged(10, $"正在打开工作簿 {inputFileName}");

            //Task.Delay(1000);
            EXCEL.Workbook book = app.Workbooks.Open(inputFileName);

            OnProgressChanged(15, "正在打开工作表");

            //Task.Delay(1000);
            EXCEL.Worksheet sheet = (EXCEL.Worksheet)book.ActiveSheet;
            OnProgressChanged(20, $"工作表打开完成：{sheet.Name}");

            //Task.Delay(1000);
            //填充item对象

            #region 银行端同步
            //值为“2020年”，只取前4位
            //从A6列开始，每行循环直到没有内容为止

            //单列测试：A7 B7为月、日
            //int month = int.Parse(((string)sheet.Range["A7"].Value));
            //int day = int.Parse(((string)sheet.Range["B7"].Value));

            //Debug.Print("Date is {0}-{1}-{2}", year, month, day);
            //DateTime transDate = new DateTime(year, month, day);
            //从A6开始，直到Arow的值为空，开始循环：
            /// 计算工作总量 progress 20 ~ 90, 70 percent
            OnProgressChanged(20, $"正在生成条目循环");
            var itemCollection = new ObservableCollection<TransactionItem>();
            itemCollection = await IterateRowItems(sheet, "A6", side);
            
            OnProgressChanged(90, $"同步台账条目完成");
            #endregion
            OnProgressChanged(95, $"正在清理资源");
            book.Close(false, null, null);
            app.Quit();
            Marshal.ReleaseComObject(app);
            OnProgressChanged(100, $"读取完成");
            return itemCollection;
        }

        private async Task<ObservableCollection<TransactionItem>> IterateRowItems(EXCEL.Worksheet worksheet
            , string startingRangeAddress, Side side)
        {

            var itemCollection = new ObservableCollection<TransactionItem>();
            string year = (string)worksheet.Range["A4"].Value;
            EXCEL.Range startRange = worksheet.Range[startingRangeAddress];
            int TotalRow = startRange.CurrentRegion.Rows.Count - 5;
            int EndRow = startRange.Row + TotalRow;
            Debug.Print($"Total Row is: {TotalRow}");
            Debug.Print($"End Row is: {EndRow}");
            //await Task.Delay(5000);
            int CycleCounter = 0;
            await Task.Run(async () =>
            {
                for (EXCEL.Range r = startRange; r.Row != EndRow; r = r.Offset[1, 0])
                {
                    //Debug.Print($"{r.Value} is empty?");

                    decimal Increment = await Task.Run(() => Convert.ToDecimal(CycleCounter++) / Convert.ToDecimal(TotalRow) * 70M);
                    int IncrementProgress = await Task.Run(() => Convert.ToInt32(Increment));
                    Debug.Print($"计算增加进度:{CycleCounter}/{TotalRow} = {IncrementProgress}");
                    Debug.Print($"同步台账条目:{IncrementProgress}/{TotalRow}");
                    await Task.Run(() => OnProgressChanged(20 + IncrementProgress, $"同步台账条目:{IncrementProgress}/{TotalRow}"));
                    await Task.Delay(100);
                    List<string> ParmCollection = new List<string>();

                    Debug.Print("Currently Executing: " + r.Address);
                    TransactionItem item = new TransactionItem
                    {
                        TransDate = DateTime.Now, //测试用
                        Identifier = r.Offset[0, 2].Value,
                        Summary = r.Offset[0, 3].Value,
                        Debit = ConvertDecimal(r.Offset[0, 5].Value),
                        Credit = ConvertDecimal(r.Offset[0, 6].Value),
                        Flow = r.Offset[0, 7].Value,
                        RemainingBalance = ConvertDecimal(r.Offset[0, 8].Value),
                        TransactionSide = side
                    };
                    itemCollection.Add(item);
                }
            });

            return itemCollection;

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
