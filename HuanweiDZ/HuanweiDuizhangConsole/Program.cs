using HuanweiDuizhangConsole.ImportExcel;
using HuanweiDuizhangConsole.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HuanweiDuizhangConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("未输入文件路径");
                Console.WriteLine("请输入有效的文件地址，例如：");
                Console.WriteLine(@"PS> .\HuanweiDuizhangConsole.exe .\1月账单明细.xls");
                Console.WriteLine("或者");
                Console.WriteLine(@"PS> .\HuanweiDuizhangConsole.exe C:\User\Woziji\账单\1月账单明细.xls");
                return;
            }
            ExecuteReader(args[0]);
        }

        private static void ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            Debug.Print("Progress Report: {0}%, {1}", e.ProgressPercentage, e.UserState);
        }

        private static void ExecuteReader(string filePath)
        {
            ExcelReader reader = new ExcelReader();

            reader.ProgressChanged += ReportProgress;
            Ledger companyLedger = reader.ReadFromFile(filePath, "Company");
            companyLedger.PrintContents();

        }
    }
}
