using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using System.Configuration;

namespace AutoRewards
{
    class BingSearcher
    {
        public static void GetPoints(bool mobile, char letter)
        {
            string userDataDir = ConfigurationManager.AppSettings["ChromeUserDataDir"] ?? "";
            int pointsBySearch = Int32.Parse(ConfigurationManager.AppSettings["PointsBySearch"] ?? "3");
            int totalMobilePoints = Int32.Parse(ConfigurationManager.AppSettings["TotalMobilePoints"] ?? "60");
            int totalDesktopPoints = Int32.Parse(ConfigurationManager.AppSettings["TotalDesktopPoints"] ?? "90");

            ChromeOptions options = new();
            options.AddArgument($"user-data-dir={userDataDir}");
            options.AddArgument("profile-directory=Default");
            options.AddArgument("start-maximized");
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

            ChromeDriver driver = new(options);

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

                //Console.WriteLine(searchString);

                driver.Url = "https://www.bing.com/";
                Thread.Sleep(500);

                driver.FindElement(By.Id("sb_form_q")).Click();
                driver.FindElement(By.Id("sb_form_q")).SendKeys(searchString);
                driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Delete);
                driver.FindElement(By.Id("sb_form")).Submit();

                Thread.Sleep(500);
            }

            driver.Close();
            driver.Dispose();

        }


    }
}
