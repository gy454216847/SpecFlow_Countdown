using log4net;
using log4net.Config;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpecFlow_Countdown.Core
{
    internal class CommonWeb
    {
        public static string CurPath = new DirectoryInfo("../../../").FullName;

        public static IWebDriver driver;

        private static int timeout = 10;

        public static ILog Log()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var logCfg = new FileInfo(CurPath + "Config/log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            return LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static void Open(string browserType, string url)
        {
            Log().Info(string.Format("Open Browser: {0}", url));
            if (browserType.ToLower() == "chrome")
            {
                driver = new ChromeDriver(CurPath + "Driver/");
                driver.Navigate().GoToUrl(url);
            }
            else if (browserType.ToLower() == "firefox")
            {
                driver = new FirefoxDriver();
                driver.Navigate().GoToUrl(url);
            }
            else if (browserType.ToLower() == "ie")
            {
                driver = new InternetExplorerDriver();
                driver.Navigate().GoToUrl(url);
            }
            else if (browserType.ToLower() == "safari")
            {
                driver = new SafariDriver();
                driver.Navigate().GoToUrl(url);
            }
            else if (browserType.ToLower() == "edge")
            {
                driver = new EdgeDriver();
                driver.Navigate().GoToUrl(url);
            }
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
        }

        private enum BrowserType
        {
            Chrome,
            Firefox,
            IE,
            Edge,
            Opera,
            Safari
        }

        public static IWebElement GetElement(string xpath)
        {
            if (!xpath.Contains("="))
            {
                throw new Exception("Positioning syntax errors, lack of '='.");
            }

            var by = xpath.Split('=')[0].ToLower();
            var value = xpath.Split('=')[1];

            if (by.Equals("id"))
            {
                return driver.FindElement(By.Id(value));
            }

            if (by.Equals("name"))
            {
                return driver.FindElement(By.Name(value));
            }
            if (by.Equals("class") || by.Equals("classname"))
            {
                return driver.FindElement(By.ClassName(value));
            }

            if (by.Equals("linktext") || by.Equals("link"))
            {
                return driver.FindElement(By.LinkText(value));
            }
            if (by.Equals("xpath"))
            {
                return driver.FindElement(By.XPath(value));
            }

            if (by.Equals("css") || by.Equals("cssselector"))
            {
                return driver.FindElement(By.CssSelector(value));
            }
            else
            {
                throw new Exception("Please enter the correct targeting elements,'id','name','class','xpath','css'.");
            }
        }

        public static void WaitElement(string xpath)
        {
            if (!xpath.Contains("="))
            {
                throw new Exception("Positioning syntax errors, lack of '='.");
            }

            var by = xpath.Split('=')[0].ToLower();
            var value = xpath.Split('=')[1];

            if (by.Equals("id"))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => d.FindElement(By.Id(value)));
            }
            else if (by.Equals("name"))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => d.FindElement(By.Name(value)));
            }
            else if (by.Equals("class") || by.Equals("classname"))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => d.FindElement(By.ClassName(value)));
            }
            else if (by.Equals("link") || by.Equals("linktext"))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => d.FindElement(By.LinkText(value)));
            }
            else if (by.Equals("xpath"))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => d.FindElement(By.XPath(value)));
            }
            else if (by.Equals("css") || by.Equals("cssselector"))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => d.FindElement(By.CssSelector(value)));
            }
            else
            {
                throw new Exception("Please enter the correct targeting elements,'id','name','class','xpath','css'.");
            }
        }

        public static string GetScreenShot(string filename)
        {
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(CurPath + "Screenshots/" + DateTime.Now.ToString("hh_mm_ss_tt_dddd_yyyy-MM-dd_") + filename + ".png",
                ScreenshotImageFormat.Png);
            Log().Info("Get Screenshot");
            return CurPath + "Screenshots/" + DateTime.Now.ToString("hh_mm_ss_tt_dddd_yyyy-MM-dd_") + filename + ".png";
        }

        public static void SetWindowSize(int wide, int high)
        {
            driver.Manage().Window.Size = new Size(wide, high);
            Log().Info(string.Format("Set Window's wide is: {0}, high is : {1}", wide, high));
        }

        public static int[] GetWindowSize()
        {
            int wide = driver.Manage().Window.Size.Width;
            int high = driver.Manage().Window.Size.Height;
            Log().Info(string.Format("The Window's wide is: {0},high is: {1}", wide, high));
            return new int[] { wide, high };
        }

        public static void MaxWindow()
        {
            driver.Manage().Window.Maximize();
            Log().Info("Set max window");
        }

        public static void Forward()
        {
            driver.Navigate().Forward();
            Log().Info("Forward browser");
        }

        public static void Back()
        {
            driver.Navigate().Back();
            Log().Info("Back browser");
        }

        public static void Close()
        {
            driver.Close();
            Log().Info("Close the window");
        }

        public static void Quit()
        {
            driver.Quit();
            Log().Info("Quit the browser");
        }

        public static void Click(string xpath)
        {
            WaitElement(xpath);
            GetElement(xpath).Click();
            Log().Info("Click element");
        }

        public static void DoubleClick(string xpath)
        {
            Log().Info("Double click the element");
            WaitElement(xpath);
            Actions actions = new Actions(driver);
            actions.DoubleClick(GetElement(xpath)).Perform();
        }

        public static void Type(string xpath, string text)
        {
            WaitElement(xpath);
            GetElement(xpath).Clear();
            GetElement(xpath).Click();
            GetElement(xpath).SendKeys(text);
            Log().Info(string.Format("Type the element with: {0}", text));
        }

        public static void Clear(string xpath)
        {
            GetElement(xpath).Clear();
            Log().Info("Clear the element");
        }

        public static void RightClick(string xpath)
        {
            WaitElement(xpath);
            Actions action = new Actions(driver);
            action.ContextClick(GetElement(xpath)).Perform();
            Log().Info("Right click the element");
        }

        public static void ClickAndHold(string xpath)
        {
            WaitElement(xpath);
            var action = new Actions(driver);
            action.ClickAndHold(GetElement(xpath)).Perform();
            Log().Info("Click and Hold the element");
        }

        public static void DragAndDrop(string elXpath, string taXpath)
        {
            WaitElement(elXpath);
            WaitElement(taXpath);
            var action = new Actions(driver);
            action.DragAndDrop(GetElement(elXpath), GetElement(taXpath)).Perform();
            Log().Info("Drag and Drop");
        }

        public static void SelectValue(string xpath, string value)
        {
            WaitElement(xpath);
            var select = new SelectElement(GetElement(xpath));
            select.SelectByValue(value);
            Log().Info(string.Format("Select by value: {0}", value));
        }

        public static void ScrolltoPresence(string xpath)
        {
            IWebElement element = GetElement(xpath);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element); 
            //根据定位的元素放在页面顶端                                                                            
            // js.ExecuteScript("arguments[0].scrollIntoView(false);", element);
            //根据定位的元素放在页面底端
        }

        public static void ScrolltoBottom()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0,document.body.scrollHeight)");
        }

        public static void ScrolltoTop()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(document.body.scrollHeight,0)");
        }

        public static void Refresh()
        {
            driver.Navigate().Refresh();
            Log().Info("Fresh website");
        }

        public static void EnterFrame(string xpath)
        {
            WaitElement(xpath);
            driver.SwitchTo().Frame(GetElement(xpath));
            Log().Info("Enter to frame");
        }

        public static void LeaveFrame()
        {
            driver.SwitchTo().DefaultContent();
            Log().Info("leave frame");
        }

        public static void WaitPage()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
        }

        public static string GetText(string xpath)
        {
            WaitElement(xpath);
            string text = GetElement(xpath).Text;
            Log().Info(string.Format("The elements' text is : {0}", text));
            return text;
        }

        public static string GetPlaceholder(string xpath)
        {
            WaitElement(xpath);
            string placeholder = GetElement(xpath).GetAttribute("placeholder");
            Log().Info(string.Format("The elements' placeholder is : {0}", placeholder));
            return placeholder;
        }

        public static string GetTitle()
        {
            WaitPage();
            string title = driver.Title;
            Log().Info(string.Format("The title is : {0}", title));
            return title;
        }

        public static string GetCurrentUrl()
        {
            WaitPage();
            string url = driver.Url;
            Log().Info(string.Format("The windows' url is : {0}", url));
            return url;
        }

        public static string GetAttribute(string xpath, string attribute)
        {
            string at = GetElement(xpath).GetAttribute(attribute);
            Log().Info(string.Format("The element' {0} is :  {1}", attribute, at));
            return at;
        }

        public static void AcceptAlter()
        {
            driver.SwitchTo().Alert().Accept();
            Log().Info(string.Format("Accept the alter"));
        }

        public static void DismissAlter()
        {
            driver.SwitchTo().Alert().Dismiss();
            Log().Info("Dismiss the alter");
        }

        public static bool IsElementExist(string xpath)
        {
            try
            {
                WaitElement(xpath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetCurrentTime()
        {
            Console.WriteLine(DateTime.Now.ToString("hh_mm_ss_tt_dddd_yyyy-MM-dd"));
            return DateTime.Now.ToString("hh_mm_ss_tt_dddd_yyyy-MM-dd");
        }

        public static String GetRandomNumber(int length)
        {
            string buffer = "0123456789";// 随机字符中也可以为汉字（任何）
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            int range = buffer.Length;
            for (int i = 0; i < length; i++)
            {
                sb.Append(buffer.Substring(r.Next(range), 1));
            }
            return sb.ToString();
        }

        public static string GetRandomString(int length)
        {
            string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(str[new Random(Guid.NewGuid().GetHashCode()).Next(0, str.Length - 1)]);
            }
            return sb.ToString();
        }

        public static String GetRandom(int length)
        {
            string code = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rand = new Random();
            char[] result = new char[length];
            string r = null;
            Regex digit = new Regex("\\d");
            Regex letter = new Regex("[a-zA-Z]");
            do
            {
                for (int i = 0; i < length; i++)
                    result[i] = code[rand.Next(0, code.Length)];
                r = new String(result);
            } while (!(digit.IsMatch(r) && letter.IsMatch(r)));

            return r;
        }
    }
}