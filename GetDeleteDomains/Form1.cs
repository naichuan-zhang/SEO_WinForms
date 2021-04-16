//using Spire.Xls;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetDeleteDomains
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 需要验证的域名
        /// </summary>
        private List<string> ColumnDB;

        /// <summary>
        /// 符合条件的域名
        /// </summary>
        private Dictionary<string, string> MeetCondition;

        /// <summary>
        /// 被机器人判定挡住的域名
        /// </summary>
        private Dictionary<string, string> TryAgain;

        /// <summary>
        /// 记录访问多少次谷歌后被机器人
        /// </summary>
        private int googleCount;

        /// <summary>
        /// 记录一分钟访问多少次whois被机器人
        /// </summary>
        private int whoisCount;

        private DateTime whoisVisitTime;

        private List<string> FilesPath;

        IWebDriver selenium;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ColumnDB = new List<string>();
            MeetCondition = new Dictionary<string, string>();
            TryAgain = new Dictionary<string, string>();
            FilesPath = new List<string>();
        }

        private void OpenFileClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;//可以多选
            ofd.DefaultExt = ".csv";
            ofd.Filter = "csvFiles|*.csv";

            string files = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] strNames = ofd.FileNames;
                for (int i = 0; i < strNames.Length; i++)
                {
                    FilesPath.Add(strNames[i]);
                    files = $"{strNames[i]}\n{files}";
                }
                fileNameRichTextBox.Text = files;
            }

            //单选文件
            //ofd.ShowDialog();
            //string filePath = ofd.FileName;
            //fileNameListBox.Text = filePath;
        }

        private void Run(object sender, EventArgs e)
        {

            try
            {
                googleCount = 0;

                whoisCount = 0;

                selenium = new ChromeDriver();

                whoisVisitTime = DateTime.Now;

                foreach (var filePath in FilesPath)
                {
                    LogTextBox.AppendText($"Open File:{filePath}./n");

                    var fileName = CsvToExcel(filePath);

                    GetColumnDB(fileName);

                    if (ColumnDB.Count > 0)
                        foreach (var domain in ColumnDB)
                            SearchGoogleResult(domain);

                    SaveExcel();
                }

                LogTextBox.AppendText("Complete!");
            }
            finally
            {
                selenium.Quit();
            }
        }

        private void SaveExcel()
        {
            //创建一个workbook实例
            Spire.Xls.Workbook wb = new Spire.Xls.Workbook();

            //清除默认的工作表
            wb.Worksheets.Clear();

            //添加一个工作表并指定表名
            Spire.Xls.Worksheet sheet = wb.Worksheets.Add("MeetCondition");

            DataTable dt = new DataTable();

            dt.Columns.Add("domain");
            dt.Columns.Add("time");

            if (MeetCondition.Count > 0)
                foreach (var meet in MeetCondition)
                {
                    dt.Rows.Add(meet.Key, meet.Value);
                }
            sheet.InsertDataTable(dt, true, 1, 1, true);

            //添加一个工作表并指定表名
            Spire.Xls.Worksheet sheet2 = wb.Worksheets.Add("TryAgain");

            DataTable dt2 = new DataTable();

            dt2.Columns.Add("domain");
            dt2.Columns.Add("time");

            if (TryAgain.Count > 0)
                foreach (var ta in TryAgain)
                {
                    dt2.Rows.Add(ta.Key, ta.Value);
                }
            sheet2.InsertDataTable(dt2, true, 1, 1, true);

            var xlsxName = $"ExpiredDomainName{DateTime.Now.ToLocalTime().ToString("yyMMddHHmmss")}.xlsx";

            LogTextBox.AppendText("Save the file we need... \n");

            //保存为.xlsx文件
            wb.SaveToFile(xlsxName, Spire.Xls.FileFormat.Version2013);
        }

        private void SearchGoogleResult(string domain)
        {
            try
            {
                if (googleCount == 100)
                {
                    Thread.Sleep(60000);
                    //googleCount = 0;
                }

                LogTextBox.AppendText("Visit Google now! \n");

                string goolgeInputXpath = "";

                string goolgeInputButtonXpath = "";

                //测试网站：conpoder.net、musiki-cm

                //如果有原来就在谷歌网站里，就不用再跳转了
                if (selenium.Url.Contains("https://www.google.com"))
                {
                    Thread.Sleep(3000);
                    goolgeInputXpath = ConfigurationManager.AppSettings["goolgeInputXpath2"].ToString();
                    goolgeInputButtonXpath = ConfigurationManager.AppSettings["goolgeInputButtonXpath2"].ToString();
                }
                else
                {
                    selenium.Navigate().GoToUrl("https://www.google.com.hk");
                    goolgeInputXpath = ConfigurationManager.AppSettings["goolgeInputXpath"].ToString();
                    goolgeInputButtonXpath = ConfigurationManager.AppSettings["goolgeInputButtonXpath"].ToString();
                    //确保页面内容已加载完成
                    while (string.IsNullOrEmpty(selenium.Title))
                    {
                        Task.Delay(100).GetAwaiter().GetResult();
                    }
                }

                var goolgeSreachResultXpath = ConfigurationManager.AppSettings["goolgeSreachResultXpath"].ToString();
                var topStuffXpath = ConfigurationManager.AppSettings["topStuffXpath"].ToString();

                #region 如果被当作机器人
                if (!ElementExist(By.XPath(goolgeInputXpath)))
                {
                    Thread.Sleep(60000);
                    selenium.Navigate().GoToUrl("https://www.google.com.hk");
                    goolgeInputXpath = ConfigurationManager.AppSettings["goolgeInputXpath"].ToString();
                    goolgeInputButtonXpath = ConfigurationManager.AppSettings["goolgeInputButtonXpath"].ToString();
                    //确保页面内容已加载完成
                    while (string.IsNullOrEmpty(selenium.Title))
                    {
                        Task.Delay(100).GetAwaiter().GetResult();
                    }
                }
                #endregion

                //取到输入框，通过xpath选择器
                var input = selenium.FindElement(By.XPath(goolgeInputXpath));
                input.Clear();
                input.SendKeys($"site:wikipedia.org \"{domain}\"");
                //input.SendKeys($"site:wikipedia.org \"baidu.com\"");
                selenium.FindElement(By.XPath(goolgeInputButtonXpath)).Click();

                googleCount++;

                //等待，确保网页有响应
                while (string.IsNullOrEmpty(selenium.Title))
                {
                    Task.Delay(100).GetAwaiter().GetResult();
                }

                Thread.Sleep(3000);

                if (!string.IsNullOrEmpty(selenium.FindElement(By.XPath(topStuffXpath)).Text))
                    return;

                string result = "";

                if (ElementExist(By.XPath(goolgeSreachResultXpath)))
                    result = selenium.FindElement(By.XPath(goolgeSreachResultXpath)).Text;

                Regex regex = new Regex(@"(\d+,)*\d+");

                string resultStr = regex.Match(result).Value;

                int resultNum = resultStr == "" ? 0 : int.Parse(resultStr.Replace(",", ""));

                if (resultNum >= wikiNumber.Value)
                    SearchWhoisResult(domain);

                return;
            }
            catch (Exception e)
            {
                selenium.Quit();
                SaveExcel();
                WriteTxt(domain);
                throw e;
            }
        }

        /// <summary>
        /// 判断元素是否存在
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        private bool ElementExist(By locator)
        {
            try
            {
                selenium.FindElement(locator);
                return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 在Whois里验证能否购买
        /// </summary>
        private void SearchWhoisResult(string domain)
        {
            try
            {
                //访问10次暂停一分钟
                if (whoisCount == 10)
                {
                    Thread.Sleep(60000);
                    whoisCount = 0;
                }

                LogTextBox.AppendText("Visit Whois now! \n");

                selenium.Navigate().GoToUrl("https://whois.net/");

                //确保页面内容已加载完成
                while (string.IsNullOrEmpty(selenium.Title))
                {
                    Task.Delay(100).GetAwaiter().GetResult();
                }

                var whoisInputXpath = ConfigurationManager.AppSettings["whoisInputXpath"].ToString();
                var whoisInputButtonXpath = ConfigurationManager.AppSettings["whoisInputButtonXpath"].ToString();
                var whoisSreachResultXpath = ConfigurationManager.AppSettings["whoisSreachResultXpath"].ToString();


                //取到输入框，通过xpath选择器
                var input = selenium.FindElement(By.XPath(whoisInputXpath));
                input.Clear();
                input.SendKeys(domain);
                selenium.FindElement(By.XPath(whoisInputButtonXpath)).Click();

                if (LessAMinute())
                {
                    whoisCount++;
                }
                else
                {
                    whoisCount = 0;
                }

                whoisVisitTime = DateTime.Now;

                while (string.IsNullOrEmpty(selenium.Title))
                {
                    Task.Delay(100).GetAwaiter().GetResult();
                }

                //等待3秒，确保js有响应
                Thread.Sleep(3000);

                #region 如果被当作机器人
                //发现人机验证等10s再重新尝试。
                if (!ElementExist(By.XPath(whoisSreachResultXpath)))
                {
                    Thread.Sleep(10000);

                    //这次以人机验证界面右上角的搜索框来输入
                    var whoisInputXpath2 = ConfigurationManager.AppSettings["whoisInputXpath2"].ToString();
                    var whoisInputButtonXpath2 = ConfigurationManager.AppSettings["whoisInputButtonXpath2"].ToString();
                    var input2 = selenium.FindElement(By.XPath(whoisInputXpath2));
                    input2.Clear();
                    input2.SendKeys(domain);
                    selenium.FindElement(By.XPath(whoisInputButtonXpath2)).Click();
                }
                #endregion

                var domainResult = selenium.FindElement(By.XPath(whoisSreachResultXpath)).Text;

                if (domainResult.Contains("already registered"))
                    return;
                else if (domainResult.Contains("available"))
                    MeetCondition.Add(domain, DateTime.Now.ToLocalTime().ToString("yyMMdd,HH:mm:ss"));
                else
                    TryAgain.Add(domain, DateTime.Now.ToLocalTime().ToString("yyMMdd,HH:mm:ss"));

                return;
            }
            catch (Exception e)
            {
                selenium.Quit();
                SaveExcel();
                WriteTxt(domain);
                throw e;
            }

        }

        private bool LessAMinute()
        {
            var time = DateTime.Now - whoisVisitTime;

            if (time.TotalMinutes < 1)
                return true;
            else
                return false;
        }

        private void WriteTxt(string domain)
        {

            Encoding code = Encoding.GetEncoding("gb2312");  //编码格式   

            string str = $"Goole:{googleCount}\n Whois:{whoisCount}\n Now sreach:{domain}\n";     //写入内容   

            StreamWriter sw = null;
            {

                try

                {

                    sw = new StreamWriter("Warning.txt", false, code);

                    sw.Write(str);

                    sw.Flush();

                }

                catch { }

            }

            sw.Close();

            sw.Dispose();

        }

        /// <summary>
        /// Csv转换为Excel文件
        /// </summary>
        /// <param name="filePath"></param>
        private string CsvToExcel(string filePath)
        {
            //初始化Workbook对象
            Spire.Xls.Workbook wb = new Spire.Xls.Workbook();

            //加载CSV文件
            wb.LoadFromFile(filePath, ";", 1, 1);

            //将第一个工作表命名为“导入Excel”
            wb.Worksheets[0].Name = "Sheet1";

            var fileName = $"CsvtoExcel{DateTime.Now.ToLocalTime().ToString("yyMMddHHmmss")}";

            //if (File.Exists(fileName + ".xlsx"))
            //    fileName = fileName + DateTime.Now.ToLocalTime().ToString("yyMMddHHmmss");

            LogTextBox.AppendText($"The .csv File Convert To new .xlsx File :{fileName}.xlsx /n");

            //转换为Excel文件
            wb.SaveToFile(fileName + ".xlsx", Spire.Xls.ExcelVersion.Version2013);

            return fileName + ".xlsx";
        }

        /// <summary>
        /// 获取Excel某列数据
        /// </summary>
        /// <param name="ExcelName"></param>
        private void GetColumnDB(string ExcelName)
        {
            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbooks wbs = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            Microsoft.Office.Interop.Excel.Worksheet ws = null;
            Microsoft.Office.Interop.Excel.Range range1 = null;
            object Nothing = System.Reflection.Missing.Value;
            try
            {
                string path = Application.StartupPath + "\\" + ExcelName;

                //创建 Excel对象
                excel = new Microsoft.Office.Interop.Excel.Application();
                //获取缺少的object类型值
                object missing = Missing.Value;

                //打开指定的Excel文件
                wb = excel.Workbooks.Open(path, missing, missing, missing, missing,
                    missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                wbs = excel.Workbooks;

                //获取选选择的工作表
                //方法一：指定工作表名称读取
                ws = ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets["Sheet1"]);

                //方法二：通过工作表下标读取
                //Worksheet ws = (Worksheet)openwb.Worksheets.get_Item(1);


                //获取工作表中的行数
                int rows = ws.UsedRange.Rows.Count;

                //获取工作表中的列数
                int columns = ws.UsedRange.Columns.Count;

                int column = 2;
                //提取对应行列的数据并将其存入数组中
                for (int i = 2; i < rows; i++)
                {
                    string a = ((Microsoft.Office.Interop.Excel.Range)ws.Cells[i, column]).Text.ToString();
                    ColumnDB.Add(a);
                }
            }
            finally
            {
                //释放内存
                if (excel != null)
                {
                    if (wbs != null)
                    {
                        if (wb != null)
                        {
                            if (ws != null)
                            {
                                if (range1 != null)
                                {
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range1);
                                    range1 = null;
                                }
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                                ws = null;
                            }
                            wb.Close(false, Nothing, Nothing);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                            wb = null;
                        }
                        wbs.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(wbs);
                        wbs = null;
                    }
                    excel.Application.Workbooks.Close();
                    excel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    excel = null;
                    GC.Collect();
                }
            }
        }


    }
}
