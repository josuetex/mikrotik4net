Provides set of interfaces and classes that wraps mikrotik objects. Uses MikrotikAPI (+ SSH, Telnet, another connection could be easily plugged-in) to access/manage mikrotik router.

# Features #
  * Extremely easy to use (a few lines of code)
  * Both R/O and R/W access to mikrotik router
  * TikSession + ITikConnector abstract interface to unify mikrotik connections
    * Mikrotik API connector included
    * Other connectors (SSH, TELNET) could be easily plugged-in into infrastructure without code changes.
  * Highlevel object API
    * Strong-typed objects for Mikrotik entities (e.q. QueueTree, FirewallMangle ...), strong-typed lists to manage collection of mikrotik objects
      * Ordered, Unordered lists
      * Auto load
      * Change tracking
    * Support for merging collection of mikrotik objects with state on mikrotik router (you could simply prepare expected state and only necessary operations are performed during merge)
  * Low-level API for special situations (tikSession.Connector ITikConnector API)
    * Send command + read response
    * Get, Set/Unset, Add, Remove, Move command
  * IApiConnector api for use mikrotik-API specific commands
  * Clean design, clear code (of course, it could be always better ...)

**Please**: Write your comments about design, features and roadmap in v1.0 [discussion issue](http://code.google.com/p/mikrotik4net/issues/detail?id=1).

# R/O Example #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    FirewallAddressListList addresses = new FirewallAddressListList();
    addresses.LoadAll();

    foreach (FirewallAddressList addr in addresses)
        Console.WriteLine("{0}={1}({2})", addr.Id, addr.Address, addr.Comment);
}
```

# R/W Example #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    FirewallFilterList filters = new FirewallFilterList();
    filters.LoadAll();

    //Update
    filters.First(f => f.Comment == "TEST_RULE").Disabled = false;
    //Insert
    filters.Add(new FirewallFilter() { Chain="forward", Protocol="tcp", Port=80, Action="accept" };
    //Delete
    filters.First(f => f.Comment == "TEST_RULE1").MarkDeleted();
    //Move
    filters.MoveToEnd(filters.First(f => f.Comment == "TEST_RULE2"));

    filters.Save();
}
```

# Simple 'add item' example #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    IpAddressList addresses = new IpAddressList();
    addresses.Add(new IpAddress() { Interface = "eth1", Address = "192.168.88.2/24" };
    addresses.Save(); //Save without previous load
}
```

# Create item via low-level (ITikConnector) api #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    string newId = session.Connector.ExecuteCreate("/ip/address", new Dictionary<string,string>
        {
            { "address", "192.168.88.2/24" },
            { "interface", "eth1" }
        });
    Console.WriteLine("Created item id: {0}", newId);
}
```

# Simple log management wrapper usage (via API) #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    LogList.LogInfo("Test"); //REMARKS works only with API connector
}
```

# API specific low-level code (IApiConnector) #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    IApiConnector apiConnector = (IApiConnector)session.Connector; //!!! MOST important row in sample

    List<ITikEntityRow> macScanList = apiConnector.ApiExecuteReader("/tool/mac-scan", new Dictionary<string,string>
        {
            {"interface", "ether1" },
            {"duration", "120" }
        });
    foreach (ITikEntityRow row in macScanList)
    {
        Console.WriteLine("Address: {0}, MAC: {1}",
            row.GetStringValueOrNull("address", true),
            row.GetStringValueOrNull("mac-address", true));
    }
}    
```

```
//execute reader without filter 
//note: exactly one row is always (expected) returned in this case
var result = apiConnector.ApiExecuteReader("/system/identity/print");
Console.WriteLine(result[0].GetStringValueOrNull("name", true));
```

# Read single value via API specific low-level code (IApiConnector) #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    IApiConnector apiConnector = (IApiConnector)session.Connector; //!!! MOST important row in sample
    //execute scalar - select for single value
    string result = apiConnector.ApiExecuteScalar("/ip/firewall/connection/print", 
        new Dictionary<string,string>{{"count-only", ""}});
    Console.WriteLine(result);
}    
```
# Wireless scan - duration parameter usage #
```
using (TikSession session = new TikSession(TikConnectorType.Api))
{
    session.Open("192.168.88.1", "admin", "");
    IApiConnector apiConnector = (IApiConnector)session.Connector; //!!! MOST important row in sample

    List<ITikEntityRow> macScanList = apiConnector.ApiExecuteReader("/interface/wireless/scan", 
        new Dictionary<string,string>
        {
            {".id", "wlan1" },
            {"duration", "30" }
        });
    foreach (ITikEntityRow row in macScanList)
    {
        Console.WriteLine("Address: {0}, SSID: {1}",
            row.GetStringValueOrNull("address", true),
            row.GetStringValueOrNull("ssid", true));
    }
}  
```

---

# Roadmap #
WARNING: I am currently in process of changing my employer, so timeline have to be shifted by 2 months (hopefully ;-) ). I hope it would not harm your development plans.
|RELEASED|0.9.0|Alpha 1 release - version for review|
|:-------|:----|:-----------------------------------|
|RELEASED|0.9.1|Alpha 2 release - minor bug fixies|
|RELEASED|0.9.2|Alpha 3 release - Examples, Support for bindings, Logging, ExecuteScalar, .id fix|
|08.2011|0.9.3|Beta 1 release - Bug fixies, code cleaning (= in data fix, ...), design review|
|09.2011|0.9.4|Beta 2 release - Bug fixies (if needed), Unit tests|
|09.2011|1.0.0|Release 1|

# Version 2.0 roadmap #
  * Background commands (scan, btest, ...)
  * More examples