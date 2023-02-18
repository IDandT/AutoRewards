using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;

namespace AutoRewards
{
    class BingSearcher
    {
        public static void GetPoints(bool mobile, char letter)
        {


            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:\\Users\\IDandT\\AppData\\Local\\Google\\Chrome\\User Data");
            options.AddArgument("profile-directory=Default");
            options.AddArgument("start-maximized");
            options.AddUserProfilePreference("profile.cookie_controls_mode", 1);    //Allow 3rd party cookies
            //options.setExperimentalOptions("excludeSwitches", "enable-logging"); <- Search c# equivalent

            if (mobile)
            {
                ChromiumMobileEmulationDeviceSettings CMEDS = new ChromiumMobileEmulationDeviceSettings();
                CMEDS.Width = 1280;
                CMEDS.Height = 720;
                CMEDS.PixelRatio = 1.0;
                CMEDS.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25";
                options.EnableMobileEmulation(CMEDS);
            }


            ChromeDriver driver = new ChromeDriver(options);


            int pointsToReach = 0;

            if (mobile)
            {
                pointsToReach = 60;   //Mobile
            }
            else
            {
                pointsToReach = 90;   //Desktop
            }

            int pointsBySearch = 3;
            int searchesToDo = pointsToReach / pointsBySearch;

            String fullSearhString = new String(letter, searchesToDo + 1);

            //searchesToDo = 1;

            for (int i = searchesToDo; i > 0; i--)
            {
                String searchString;
                searchString = fullSearhString.Substring(0, i);

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
