# DALHelper 
DALHelper is a helper library that underneath uses ADO.Net to facilitate insertion and retrieval of data from a SQL Server database. DALHelper is an application of the Facade design pattern and uses disconnected data types. The library enforces the user to run all the transactions at the SQL Server level in the form of an inline query or stored procedure, and deliberately does not allow running of transactions using ADO.Net.

DALHelper is available as a NuGet package from NuGet gallery
```PowerShell
Install-package ThinkingCog.DALHelper.SQLServer
```