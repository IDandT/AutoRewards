
# AutoRewards

Program to do searches for Microsoft Rewards in Bing automatically.


## Configuration parameters

To run the program, you will need to adjust the following settings in app.config file.

```
  <appSettings>
    <add key="ChromeUserDataDir" value="C:\Users\IDandT\AppData\Local\Google\Chrome\User Data"/>
    <add key="EdgeUserDataDir" value="C:\Users\IDandT\AppData\Local\Microsoft\Edge\User Data"/>
    <add key="PointsBySearch" value="3"/>
    <add key="TotalDesktopPoints" value="90"/>
    <add key="TotalMobilePoints" value="60"/>
    <add key="TotalEdgePoints" value="12"/>
    <add key="LogLevel" value="3"/>
    <add key="Timeout" value="10"/>
  </appSettings>
```


## Release notes

Project solution has been upgraded to Visual Studio 2022 and targets .NET 7.0.

The program uses "Selenium.WebDriver" to automate browsers.

Assumed that you are logged in your microsoft account from both browsers. That is... when you go to rewards page, you can see your name and point balance on top.

### NOTE: 

In rewards are 3 types of searches to accomplish:

-- Desktop
-- Mobile
-- Edge

In this way, **two browsers are being used**, although the ideal is to do everything with Edge now (only with Chrome you can't accomplish all three objectives).

Having the code for both can be interesting for someone, although they are practically the same.

Anyway, the code is super simple and you can modify it to do everything with Edge, remove the Chrome part, or do what you want.


### NOTE 2: 

If Chrome/Edge updates to new version that breaks compatibility, may be you need to update Nuget reference and recompile project.


## Support

Console window may show some like that while running. 

The parameter "LogLevel" established to "3" should avoid most of this traces.

```
ERROR:device_event_log_impl.cc

ERROR:fallback_task_provider.cc

Etc...
```

Anyway, is ok, and not inferfers with normal execution of program.

If program crashes, try to update "Selenium.WebDriver" Nuget package and recompile project.

If program works but your points don't increases, try to login in your microsoft account from both browsers, previously to execute program.

