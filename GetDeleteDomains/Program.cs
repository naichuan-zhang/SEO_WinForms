using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetDeleteDomains
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        /// <summary>
        /// 通过Selenium调用谷歌浏览器来爬取
        /// </summary>
        /// <param name="url"></param>
        static void SeleniumMothed(string url)
        {
            //启动谷歌浏览器
            IWebDriver selenium = new ChromeDriver();
            //浏览器跳转到我们要爬取的url
            selenium.Navigate().GoToUrl(url);
            //确保页面内容已加载完成
            while (string.IsNullOrEmpty(selenium.Title))
            {
                Task.Delay(100).GetAwaiter().GetResult();
            }


            //取到标题信息，通过css选择器
            var title = selenium.FindElement(By.CssSelector("h1.title-article")).Text;
            //发布时间
            var time = selenium.FindElement(By.CssSelector("span.time")).Text;
            //博主名
            var name = selenium.FindElement(By.CssSelector("a.follow-nickName")).Text;
            //阅读数
            var nums = selenium.FindElement(By.CssSelector("span.read-count")).Text;
            //正文，由于id固定，我们直接用id选择器获取
            var content = selenium.FindElement(By.Id("article_content")).Text;
            Console.WriteLine("标题：" + title);
            Console.WriteLine("发布时间：" + time);
            Console.WriteLine("博主名：" + name);
            Console.WriteLine("阅读数：" + nums);
            Console.WriteLine("正文：" + content);
        }
    }
}
