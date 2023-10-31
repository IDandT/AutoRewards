using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using System.Configuration;
using System.Diagnostics;

namespace AutoRewards
{
    class ChromeBingSearcher
    {
        public static void GetPoints(bool mobile, char letter)
        {
            string userDataDir = ConfigurationManager.AppSettings["ChromeUserDataDir"] ?? "";
            int pointsBySearch = Int32.Parse(ConfigurationManager.AppSettings["PointsBySearch"] ?? "3");
            int totalMobilePoints = Int32.Parse(ConfigurationManager.AppSettings["TotalMobilePoints"] ?? "60");
            int totalDesktopPoints = Int32.Parse(ConfigurationManager.AppSettings["TotalDesktopPoints"] ?? "90");
            int logLevel = Int32.Parse(ConfigurationManager.AppSettings["LogLevel"] ?? "3");
            int timeout = Int32.Parse(ConfigurationManager.AppSettings["Timeout"] ?? "30");

            ChromeOptions options = new();
            options.AddArgument($"user-data-dir={userDataDir}");
            options.AddArgument("profile-directory=Default");
            options.AddArgument("start-maximized");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument($"--log-level={logLevel}");
            options.AddArgument("--no-sandbox");
            options.AddUserProfilePreference("profile.cookie_controls_mode", 1);    //Allow 3rd party cookies

            if (mobile)
            {
                ChromiumMobileEmulationDeviceSettings CMEDS = new()
                {
                    Width = 1280,
                    Height = 720,
                    PixelRatio = 1.0,
                    UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25"
                };
                options.EnableMobileEmulation(CMEDS);
            }

            // Kill al browser processes
            foreach (Process p in Process.GetProcessesByName("chrome")) p.Kill();

            ChromeDriver? driver = null;
            while (driver == null)
            {
                driver = new ChromeDriver(options);
                if (driver == null)
                {
                    Console.WriteLine("ERROR: Create ChromeDriver fails.. Retry...");
                    Thread.Sleep(1000);
                }
            }


            int pointsToReach;

            if (mobile)
            {
                pointsToReach = totalMobilePoints;
            }
            else
            {
                pointsToReach = totalDesktopPoints;
            }

            int searchesToDo = pointsToReach / pointsBySearch;

            String fullSearhString = new(letter, searchesToDo + 1);

            //searchesToDo = 1;

            for (int i = searchesToDo; i > 0; i--)
            {
                String searchString;
                searchString = fullSearhString[..i];

            Retry:

                Console.WriteLine($"Search #{searchesToDo - i + 1}/{searchesToDo}:  {searchString}");

                try
                {
                    Task search = Task.Run(() =>
                    {
                        driver.Navigate().GoToUrl("https://www.bing.com/");
                        Thread.Sleep(100);
                        driver.FindElement(By.Id("sb_form_q")).Click();
                        driver.FindElement(By.Id("sb_form_q")).SendKeys(searchString);
                        driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Delete);
                        driver.FindElement(By.Id("sb_form")).Submit();
                        //driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Enter);
                    });

                    if (search.Wait(TimeSpan.FromSeconds(timeout)))
                        continue;
                    else
                        throw new Exception($"Max timeout exceeded ({timeout} seconds)");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR: {e.Message}");
                    Console.WriteLine("Retry search...");
                    Console.WriteLine("Creating new driver instance...");

                    // Kill al browser processes
                    foreach (Process p in Process.GetProcessesByName("chrome")) p.Kill();

                    driver = null;

                    while (driver == null)
                    {
                        driver = new ChromeDriver(options);
                        if (driver == null)
                        {
                            Console.WriteLine("ERROR: Create driver fails.. Retry...");
                            Thread.Sleep(1000);
                        }
                    }

                    goto Retry;
                }
            }

            driver.Close();
            driver.Dispose();
        }
    }
}
