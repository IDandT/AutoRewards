
# AutoRewards

Program to do searches for Microsoft Rewards in Bing (via Chrome) automatically.



## Configuration parameters

To run the program, you will need to adjust the following settings in app.config file.

```
  <appSettings>
    <add key="ChromeUserDataDir" value="C:\Users\IDandT\AppData\Local\Google\Chrome\User Data"/>
    <add key="PointsBySearch" value="3"/>
    <add key="TotalDesktopPoints" value="90"/>
    <add key="TotalMobilePoints" value="60"/>
  </appSettings>
```


## Release notes

Project solution has been upgraded to Visual Studio 2022 and targets .NET 7.0.

The program uses "Selenium.WebDriver" to automate Chrome browser.

If Chrome updates to new version that breaks compatibility, may be you need to update Nuget reference and recompile project.

I don't included the Edge browser searches, but you are free to add code for do it.

For chrome i used:

```
using OpenQA.Selenium.Chrome
```

For Edge should be easy using Edge import:

```
using OpenQA.Selenium.Edge;
```


## Support

Console window may show some errors about USB devices and others while runing . It's ok, and not inferfers with normal execution of program.

If program crashes, try to update "Selenium.WebDriver" Nuget package and recompile project.

If program works but your points don't increases, try to login in your microsoft account previously to execute program.

