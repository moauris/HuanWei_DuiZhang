using ExcelDataReader;
using HuanweiDZ.Components;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using XLSReader = ExcelDataReader;

namespace HuanweiDZ.Services
{
    public class ExcelReaderXLSReader
    {
        public void TestRead(string filePath)
        {
            Trace.Listeners.Clear();
            string LogFileName = string.Format(".\\{0}_Trace.log"
                , DateTime.Now.ToString("yyyy_MMdd_HHmmss")) ;
            TextWriterTraceListener traceListener = new TextWriterTraceListener(LogFileName);
            Trace.Listeners.Add(traceListener);
            TraceWrapper("正在开始读取文件" + filePath);

            FileInfo file = new FileInfo(filePath);
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))//当文件被打开时会报错。
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream)) //需要加入文件后缀和类型的判定
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                string[] RowContent = new string[9];
                                int NonEmptyLength = 0;
                                for (int r = 0; r < 9; r++)
                                {
                                    object raw = reader.GetValue(r);
                                    if (raw == null)
                                    {
                                        RowContent[r] = string.Empty;
                                    }
                                    else
                                    {
                                        RowContent[r] = reader.GetValue(r).ToString();
                                        NonEmptyLength++;
                                    }
                                }
                                TraceWrapper("非空单行元素判定: " + NonEmptyLength);
                                //string RowContents = string.Join(",", RowContent);
                                
                                //TraceWrapper(RowContents);
                                //以下代码生成 LedgerItem 对象
                                //两侧符合要求的对象：第0-4不为空，5或6至少有一位不为空，不为空时可以被转换为double。7为平、借、或者贷，余额为double
                                LedgerItem ledgerItem = LedgerItem.Parse(RowContent, "Company");
                                if (ledgerItem != null)
                                {
                                    TraceWrapper(ledgerItem.ToString());
                                }
                                
                            }
                        } while (reader.NextResult());
                    }
                }
            }
            catch (IOException exp)
            {

                TraceWrapper("遇到了文件读写错误：");
                TraceWrapper(exp.Message);

                MessageBox.Show(
                    "文件读写遇到了错误。\r\n请检查目标工作簿是否已经打开或者被其他程序占用。\r\n请释放工作簿后再次尝试。",
                    "发生了错误：目标文件被程序占用。",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );

            }
            finally
            {
                MessageBox.Show("读取文件完成。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Trace.Close();

            }
            
        }

        [Conditional ("DEBUG")]
        private void TraceWrapper(string message)
        {
            string DebugMessage = string.Format("[{0}] @ <{1}>: {2}, EOL"
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                , "ExcelReaderXLSReader"
                , message);
            Trace.WriteLine(DebugMessage);
        }
    }

#if USE_MS_INTEROP
    public class ExcelReader
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

        public ExcelReader() { }
        public Ledger ResultLedger { get; set; }

        public Ledger ReadFromFile(string filePath, string side)
        {
            OnProgressChanged(0, "读取开始");

            //Task.Delay(1000);
            OnProgressChanged(1, "正在检查输入文件名有效性");
            Debug.Print(filePath);
            FileInfo inputFile = new FileInfo(filePath);
            if (!inputFile.Exists)
            {
                throw new FileNotFoundException($"没有找到文件，路径： {filePath}");
            }
            Console.WriteLine("生成reader实例: " + inputFile.FullName);
            //Task.Delay(1000);
            if (inputFile.Exists == false)
            {
                OnProgressChanged(100, "无效文件名");
                return new Ledger();
            }


            OnProgressChanged(5, "正在加载Excel应用实例");

            //Task.Delay(1000);
            EXCEL.Application app = new EXCEL.Application();
            OnProgressChanged(10, $"正在打开工作簿 {inputFile.FullName}");

            //Task.Delay(1000);
            EXCEL.Workbook book = app.Workbooks.Open(inputFile.FullName);

            OnProgressChanged(15, "正在打开工作表");

            //Task.Delay(1000);
            EXCEL.Worksheet sheet = (EXCEL.Worksheet)book.ActiveSheet;
            OnProgressChanged(20, $"工作表打开完成：{sheet.Name}");

            //Task.Delay(1000);
            //填充item对象

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
            Ledger OutputLedger = new Ledger();

            OutputLedger = IterateRowItems(sheet, "A6", side);

            OnProgressChanged(90, $"同步台账条目完成");
            OnProgressChanged(95, $"正在清理资源");
            book.Close(false, null, null);
            app.Quit();
            Marshal.ReleaseComObject(app);
            OnProgressChanged(100, $"读取完成");
            return OutputLedger;
        }

        private Ledger IterateRowItems(EXCEL.Worksheet worksheet
            , string startingRangeAddress, string side)
        {

            var itemCollection = new Ledger();
            Debug.Print($"[DEBUG] Test _count field availability: {itemCollection.Count}");
            string year = (string)worksheet.Range["A4"].Value;
            EXCEL.Range startRange = worksheet.Range[startingRangeAddress];
            int TotalRow = startRange.CurrentRegion.Rows.Count - 5;
            int EndRow = startRange.Row + TotalRow;
            Debug.Print($"[DEBUG] Total Row is: {TotalRow}");
            Debug.Print($"[DEBUG] End Row is: {EndRow}");
            //await Task.Delay(5000);
            int CycleCounter = 0;
            SyncItemOffset offset = new SyncItemOffset
            {
                Identifier = 3,
                Summary = 4,
                Debit = 5,
                Credit = 6,
                Direction = 7,
                RemainingFund = 8
            };
            switch (side)
            {
                case "company":
                    offset.Identifier = 3;
                    offset.Summary = 4;
                    offset.Debit = 5;
                    offset.Credit = 6;
                    offset.Direction = 7;
                    offset.RemainingFund = 8;
                break;
                case "bank":
                    offset.Identifier = 2;
                    offset.Summary = 3;
                    offset.Debit = 5;
                    offset.Credit = 6;
                    offset.Direction = 7;
                    offset.RemainingFund = 8;
                    break;
                default:
                    break;
            }
            for (EXCEL.Range r = startRange; r.Row != EndRow; r = r.Offset[1, 0])
            {
                //Debug.Print($"{r.Value} is empty?");

                decimal Increment = Convert.ToDecimal(CycleCounter++) / Convert.ToDecimal(TotalRow) * 70M;
                int IncrementProgress = Convert.ToInt32(Increment);
                Debug.Print($"计算增加进度:{CycleCounter}/{TotalRow} = {IncrementProgress}");
                Debug.Print($"同步台账条目:{IncrementProgress}/{TotalRow}");
                /*if (IncrementProgress % 10 == 0)
                {
                    await Task.Run(() => OnProgressChanged(20 + IncrementProgress, $"同步台账条目:{IncrementProgress}/{TotalRow}"));
                }*/ //每10位进一次progress

                OnProgressChanged(20 + IncrementProgress, $"同步台账条目:{IncrementProgress}/{TotalRow}"); //每一位进一次progress

                //await Task.Delay(100);
                List<string> ParmCollection = new List<string>();

                Debug.Print("正在同步工作表地址: " + r.Address);


                LedgerItem item = new LedgerItem
                {
                    EntryDate = DateTime.Now, //测试用
                    
                    Identifier = (string)r.Offset[0, offset.Identifier].Value,
                    Summary = (string)r.Offset[0, offset.Summary].Value,
                    Debit = ConvertDecimal(r.Offset[0, offset.Debit]),
                    Credit = ConvertDecimal(r.Offset[0, offset.Credit]),
                    Direction = (string)r.Offset[0, offset.Direction].Value,
                    RemainingFund = ConvertDecimal(r.Offset[0, offset.RemainingFund]),
                    Side = side
                };
                Debug.Print("正在试图添加到集合 itemCollection");
                
                itemCollection.Add(item);
            }

            return itemCollection;

        }

        /// <summary>
        /// Converts a string to decimal, if cannot, return 0
        /// </summary>
        /// <param name="input"> Potential Decimal String </param>
        /// <returns></returns>
        private double ConvertDecimal(EXCEL.Range range)
        {
            double result = 0;
            if (range.Value == null) return 0;
            double.TryParse(range.Value.ToString(), out result);
            return result;
        }
    }

    struct SyncItemOffset
    {
        public int Identifier { get; set; }
        public int Summary { get; set; }
        public int Debit { get; set; }
        public int Credit { get; set; }
        public int Direction { get; set; }
        public int RemainingFund { get; set; }
    }
    
#endif

}
