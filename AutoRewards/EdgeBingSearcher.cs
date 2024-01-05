using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Configuration;
using System.Diagnostics;

namespace AutoRewards
{
    class EdgeBingSearcher
    {
        public static void GetPoints(SearchType? searchType, int? searchesToDo)
        {
            Console.WriteLine("Search mode: EDGE + {0}", searchType == SearchType.Mobile ? "MOBILE" : "DESKTOP");

            string userDataDir = ConfigurationManager.AppSettings["EdgeUserDataDir"] ?? "";
            int logLevel = Int32.Parse(ConfigurationManager.AppSettings["LogLevel"] ?? "3");
            int timeout = Int32.Parse(ConfigurationManager.AppSettings["Timeout"] ?? "30");
            int pauseBetweenSearches = Int32.Parse(ConfigurationManager.AppSettings["PauseBetweenSearches"] ?? "0");

            EdgeOptions options = new();
            options.AddArgument($"user-data-dir={userDataDir}");
            options.AddArgument("profile-directory=Default");
            options.AddArgument("--start-maximized");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument($"--log-level={logLevel}");
            options.AddArgument("--no-sandbox");
            options.AddUserProfilePreference("profile.cookie_controls_mode", 1);    //Allow 3rd party cookies

            if (searchType == SearchType.Mobile)
            {
                ChromiumMobileEmulationDeviceSettings mobileSettings = new()
                {
                    Width = 1280,
                    Height = 720,
                    PixelRatio = 1.0,
                    UserAgent = "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Mobile Safari/537.36"
                };
                options.EnableMobileEmulation(mobileSettings);
                Console.WriteLine("Mobile emulation ON");
            }

            // Kill al browser instances
            foreach (Process p in Process.GetProcessesByName("msedge")) p.Kill();

            EdgeDriver? driver = null;
            WebDriverWait? wait = null;

            while (driver == null)
            {
                driver = new EdgeDriver(options);
                if (driver == null)
                {
                    Console.WriteLine("ERROR: Create EdgeDriver fails.. Retry...");
                    Thread.Sleep(1000);
                }
            }
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));


            // Search loop
            for (int i = 1; i <= searchesToDo; i++)
            {
                // We use random GUID as search string
                String searchString = Guid.NewGuid().ToString("N");

            Retry:

                Console.WriteLine($"Search #{i}/{searchesToDo}:  {searchString}");

                try
                {
                    Task search = Task.Run(() =>
                    {
                        driver.Navigate().GoToUrl("https://www.bing.com/");

                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sb_form_q")));
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("sb_form_q")));

                        driver.FindElement(By.Id("sb_form_q")).Click();
                        driver.FindElement(By.Id("sb_form_q")).SendKeys(searchString);
                        driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Delete);
                        driver.FindElement(By.Id("sb_form")).Submit();
                        //driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Enter);                        

                        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sb_form_q")));
                    });

                    if (search.Wait(TimeSpan.FromSeconds(timeout)))
                    {
                        // We have to wait 5 seconds between searches (Dic 2023)
                        Thread.Sleep(pauseBetweenSearches);
                        continue;
                    }
                    else
                        throw new Exception($"Max timeout exceeded ({timeout} seconds)");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR: {e.Message}");
                    Console.WriteLine("Retry search...");
                    Console.WriteLine("Creating new driver instance...");

                    // Kill al browser instances
                    foreach (Process p in Process.GetProcessesByName("msedge")) p.Kill();

                    // End current driver session
                    driver.Quit();

                    driver = null;
                    wait = null;

                    while (driver == null)
                    {
                        driver = new EdgeDriver(options);
                        if (driver == null)
                        {
                            Console.WriteLine("ERROR: Create EdgeDriver fails.. Retry...");
                            Thread.Sleep(1000);
                        }
                    }
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                    goto Retry;
                }
            }

            driver.Quit();
        }
    }
}
