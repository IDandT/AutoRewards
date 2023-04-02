using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Configuration;

namespace AutoRewards
{
    class EdgeBingSearcher
    {

        public static void GetPoints(char letter)
        {
            string userDataDir = ConfigurationManager.AppSettings["EdgeUserDataDir"] ?? "";
            int pointsBySearch = Int32.Parse(ConfigurationManager.AppSettings["pointsbysearch"] ?? "3");
            int totalEdgePoints = Int32.Parse(ConfigurationManager.AppSettings["TotalEdgePoints"] ?? "12");

            EdgeOptions options = new();
            options.AddArgument($"user-data-dir={userDataDir}");
            options.AddArgument("profile-directory=Default");
            options.AddArgument("--start-maximized");
            options.AddUserProfilePreference("profile.cookie_controls_mode", 1);    //Allow 3rd party cookies

            EdgeDriver driver = new(options);

            int pointsToReach = totalEdgePoints;

            int searchesToDo = pointsToReach / pointsBySearch;

            String fullSearhString = new(letter, searchesToDo + 1);

            //searchesToDo = 1;

            for (int i = searchesToDo; i > 0; i--)
            {
                String searchString;
                searchString = String.Concat("EDGE", fullSearhString[..i]);

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
