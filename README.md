LINQPadLog4jDriver
==================

LINQPad Driver for reading logs in Log4j file format

---

### Install

1) Download **Log4jLinqpadDriver x.x.x.x.zip**

2) Extract **lpx** files.

3) In LINQPad: Add Connection, View more drivers, browse for lpx file.

4) Complete the connection information dialog. Fill out logs path and filters if you need.

5) Create a new query using the Log4j connection that you just defined.

6) You're done. You can write some code against your log files now.

### Usage

```csharp
// c# expression
from l in Logs
where l.Message.Contains("Bad Things")
select l
```

if you choose "UseCache" option in connection dialog, driver will store all log entries in memory. All new queries will read data from that cache. 
To clear cache use following method:
```csharp
//Clearing cache
ClearCache()
```
Please remember that LINQPad query tab are loaded in separate domain, consequently cache visibility scope is restricted to an individual tab.

Happy querying! :-)
