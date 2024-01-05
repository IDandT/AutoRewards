# AutoRewards

Program to do searches for Microsoft Rewards in Bing automatically.

The program launches a browser, navigates to bing site and does the specified number of searches.

For each search it generates unique string (something like this: "21c63e3344ea4f3497159c430a45318a").


## Configuration parameters

To run the program, you will need to adjust the following settings in app.config file.

```
  <appSettings>
    <add key="ChromeUserDataDir" value="C:\Users\IDandT\AppData\Local\Google\Chrome\User Data"/>
    <add key="EdgeUserDataDir" value="C:\Users\IDandT\AppData\Local\Microsoft\Edge\User Data"/>
    <add key="LogLevel" value="3"/>
    <add key="Timeout" value="10"/>
  </appSettings>
```

## Program execution

AutoRewards is a console program. To run it, just invoke from console, using following arguments:

```
Supported browsers:
        --chrome        Use Chrome browser
        --edge          Use Edge browser

Supported search types:
        --mobile        Emulate mobile mode
        --desktop       Normal desktop mode

Other arguments:
        --count N       The number of sarches to do (example: --count 5)
        --help          Show this help
```

For 2024, Microsoft added to Bing system rewards a cooldown period, where only counts 3 searches every 15 minutes.

With this, you can schedule searches at windows task scheduler, to do, for example, 3 or 4 searches every 20 minutes.

Example:

```
AutoRewards --edge -mobile --count 3
```

### Very Important: 

Scheduling for example 3 mobile searches and 3 desktop searches, one just after another, probably not work for desktop, 
because the mobile searches reset cooldown time (independent of if you win points or not).

Be carefully with that. 

Yo can schedule --mobile at day, and -desktop at afternoon for example to avoid collisions.


## Release notes

Project solution has been upgraded to Visual Studio 2022 and targets .NET 7.0.

The program uses "Selenium.WebDriver" to automate browsers.

Assumed that you are logged in your microsoft account from both browsers. That is... when you go to rewards page, you can see your name and point balance on top.


### Important:

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

If program works but your points don't increases:

- Try to login in your microsoft account from both browsers, previously to execute program.
- Keep in mind the cooldown time every searches (just now, 3 searches every 15 minutes).
- Keep in ming the "Very Important" point at "Program execution" section.



Thanks!!

IDandT

