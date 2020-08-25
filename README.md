# WinDriver
A simple wrap-up around UIAutomation. Allow to run UIAutomation tests remotely. Can be also configured to run UI Win tests parallely on different machines (client can be configured to be proxy which handle requests and send to available machines)

## How to use
### Run `WinDriver.Client.exe`
1. Open project in VS
2. Restore NuGet dependencies
3. Build project
4. Change `rootUrl` path value in App.Config in `WinDriver.Client` project. By default will be used http://localhost:8888
5. Run  `WinDriver.Client.exe` from corresponding build output directory. Make sure that corresponding port is opened in winwows firewall. If you'll use ip address to access service instead of localhost run `WinDriver.Client.exe` under admin.

### Write your tests
1. Reference `WinDriver` & `WinDriver.Core` to your test project
2. Start writing tests

```C#
var options = new WinDriverOptions
{
    App = @"path to your app",
    ImplicitWaitTimeout = 5,
    AppStartUpTimeOut = 10
};

driver = new WinDriver(new Uri("http://localhost:8888"), options);
//Inspect elemnts using Inspect.exe or UISpy and interact with them
var element = driver.FindElementByName('element name');
element.Click();
//etc...
```

# TODO
- [ ] Refactor this
- [ ] Make simplier search methods. Replate to one FindElement(By condition)
- [ ] implement simple xpath parser (//MenuItem[@ClassName='TestClass'][@AutomationId='some id'])
- [ ] Make `WinDriver.Client.exe` be able to handle multiple requests and proxy them to another clients (like selenium grid). Will allow to run UI Win tests in parallel on different machines
- [ ] some other interesting stuff