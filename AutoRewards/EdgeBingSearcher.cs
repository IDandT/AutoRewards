using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Configuration;
using System.Diagnostics;

namespace AutoRewards
{
    class EdgeBingSearcher
    {
        public static void GetPoints(char letter)
        {
            string userDataDir = ConfigurationManager.AppSettings["EdgeUserDataDir"] ?? "";
            int pointsBySearch = Int32.Parse(ConfigurationManager.AppSettings["pointsbysearch"] ?? "3");
            int totalEdgePoints = Int32.Parse(ConfigurationManager.AppSettings["TotalEdgePoints"] ?? "12");
            int logLevel = Int32.Parse(ConfigurationManager.AppSettings["LogLevel"] ?? "3");
            int timeout = Int32.Parse(ConfigurationManager.AppSettings["Timeout"] ?? "30");

            EdgeOptions options = new();
            options.AddArgument($"user-data-dir={userDataDir}");
            options.AddArgument("profile-directory=Default");
            options.AddArgument("--start-maximized");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument($"--log-level={logLevel}");
            options.AddArgument("--no-sandbox");
            options.AddUserProfilePreference("profile.cookie_controls_mode", 1);    //Allow 3rd party cookies

            // Kill al browser processes
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


            int pointsToReach = totalEdgePoints;

            int searchesToDo = pointsToReach / pointsBySearch;

            String fullSearhString = new(letter, searchesToDo + 1);

            //searchesToDo = 1;

            for (int i = searchesToDo; i > 0; i--)
            {
                String searchString;
                searchString = String.Concat("EDGE", fullSearhString[..i]);

            Retry:

                Console.WriteLine($"Search #{searchesToDo - i + 1}/{searchesToDo}:  {searchString}");

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
                    foreach (Process p in Process.GetProcessesByName("msedge")) p.Kill();

                    driver = null;
                    wait = null;

                    while (driver == null)
                    {
                        driver = new EdgeDriver(options);
                        if (driver == null)
                        {
                            Console.WriteLine("ERROR: Create driver fails.. Retry...");
                            Thread.Sleep(1000);
                        }
                    }
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                    goto Retry;
                }
            }

            driver.Close();
            driver.Dispose();
        }
    }
}
