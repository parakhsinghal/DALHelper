# DALHelper Documentation

Also available in PDF and EPUB formats: [Download](Documentation_528534)

## Table of Contents
# Introduction
# Small introduction to Code Contracts
# How to use DALHelper library
# Methods available in DALHelper
## ExecSQL
## GetRowsAffected
## GetDataTable
## GetDataView
## GetDataSet
# Future Roadmap
# Helpful Links

## 1. Introduction
The [DALHelper](DALHelper.CodePlex.com) is a project that was born out of my needs to connect to [SQL Server](http://www.microsoft.com/en-in/SQLserver/default.aspx) databases for my projects. I generally work on projects and design them using domain model and to keep them flexible, understandable and fast, I prefer to connect to the database using ADO.NET. Writing ADO.NET commands to connect to database can become monotonous and repetitive. Using Microsoft's [Enterprise Library Data Access Application Block](entlib.codeplex.com) is one solution, but I generally find it requiring a bit more effort than should be invested. Of course it gives you the freedom to change your supporting back end database, but only when you have exercised caution of not using any particular database specific commands. Generally back end databases in applications do not change, especially in enterprise environments and small applications for which the express version of SQL Server can be sufficient, and if you work with such applications then, I believe DALHelper can make your life a bit easier. 

The DALHelper is meant to be an assistant library that can help in making software applications that use ADO.NET technology when it comes to using SQL Server database as a data store. 

**The DALHelper library can only be used with SQL Server databases as it uses SQL Server specific ADO.NET commands.**

The library contains various helper methods that can simplify the task of doing CRUD (Create, Read, Update and Delete) on a SQL Server database in a .NET application. The library can be used with any .NET compliant language, is open sourced and will remain that way. 

DALHelper touches upon the following technologies:
1.	Visual C# 4.0
2.	.NET Framework 4.0
3.	ADO.NET technology, part of .NET framework, used to connect to databases.
4.	Code Contracts, Microsoft’s implementation of Design by Contact paradigm.

While the usage of first three technologies is very much self-explanatory, I have a given a shot at explaining the concept of Design by Contract and its implementation as done by Microsoft in the form of Code Contracts.

## 2. Small introduction to Code Contracts
Design by Contract is a philosophy that specifies that software components should have a formal, precise and verifiable1. That when taken in a real life practical sense means that there exists a set of conditions – pre-conditions, post-conditions and invariant conditions applicable to code units, capable of performing some action. Those units of code are methods, and contracts are terms and conditions applicable to parameters and return values. 

Pre-conditions are the conditions that the parameters should comply with before they are passed for processing, e.g. value of an integer type parameter be greater than a certain 10 and less than 99.

Post-conditions are the conditions that are generally applicable to return values, so that the return values fit a certain criteria when passed from the methods to the calling code. The post-conditions may also be applicable to parameters just to ensure that some manipulation has taken place upon them, e.g. perimeter of a circle can never be a negative quantity.

Invariant conditions are the conditions that dictate that certain variables inside method (parameters, return values) remain compliant with some condition, for example the value of pi should always be 3.141 and not change while we calculate the perimeter of a circle.

Design by Contract ensures that the code produced, performs in a predictable fashion and produces predictable outcomes. Because of the very nature of the Design by Contract philosophy, it is suited to be used to make libraries and frameworks that are supposed to be consumed by third parties. In fact, Microsoft has started implementing the contracts in .NET framework, and can be found in some namespaces.

Languages like Eiffel, Ada 2012 etc. implement the design by contract as a feature of the language itself, but Microsoft has implemented the philosophy with the help of [Code Contracts](http://research.microsoft.com/en-us/projects/contracts). Code Contracts is two part approach in implementing the Design by Contract philosophy that uses the .NET framework and add-ons to Visual Studio. The programming part is available in the .NET framework in the namespace System.Diagnostics.Contracts. All the attributes that are applicable in using contracts can be applied once we include the namespace in the project. The other add-ins (Visual Studio GUI to control how contracts behave and editor extension for static checking need to be downloaded and installed). 

I leave the exercise of understanding Code Contracts to the user, since it does not fit the scope of this documentation. However I have provided a list of helpful links that will save lot of time, if the keen users want more information on the topic.

## 3. How to use DALHelper library
The procedure to use the DALHelper library is pretty straightforward:
# Click on Add Reference and browse to the location where you have stored the library.
# Add just the DALHelper.dll in your project. The library in the Code Contract folder is the assembly that gets formed at the time of compilation of the actual project and contains the contract documentation for the main library. Similarly the provided XML file contains the comments about the code generated at the time of compilation.
# Add the reference in your code to DALHelper by adding the using clause.
# The DALHelper namespace contains the DBDataHelper class that contains all the methods that can be used to execute methods against the SQL Server database. You are not required to write any ADO.NET code.
# In order to use the library you will have to provide the static property ConnectionString of the DBDataHelper class, the connection string to your database. This can be provided by pointing to the connection stored in your app.config (in case of Windows application) or web.config (in case of web application), or can be even provided a simple valid address of string type. This is static property ConnectionString enables to execute commands in the library without providing the connection string to the database repeatedly. This is an example of Dependency Injection using property injection.

## 4. Methods available in DALHelper
The DALHelper library has been designed to be provided with the connection string to the database once and then perform various operations as desired by the user with the help of the methods provided in the library. Some of the best practices that have been followed while the methods were designed are the following:
# The methods follow a pattern in their signature and are easy to use. 
# The methods provided in the library are overloaded, thus providing you just the signature that you need in order to accomplish what you desire, without cluttering up your code.
# The names of the methods are very much self-explanatory in context what they service they are supposed to provide.
The variety of methods available should fit majority of the requirements found in any small to medium scale enterprise application. 
Every method available in the library allows you to execute a SQL query or a stored procedure.
Every method available in the library has two overloads:
# Without parameters, and
# With parameters
Every method available in the library has the signature of the format:
# SQL text: Is the SQL query or stored procedure that needs to be executed.
# SQLTextType: An enum which signifies whether the SQL text parameter is a SQL query or a stored procedure. By default, the choice is that of a stored procedure.
# List<SqlParameter>: An optional generic list of SQL parameters type that might be required by the SQL query or stored procedure. The parameters can be of both input and output type.

Here is a brief description of methods available in the library:
1. ExecSQL
The ExecSQL method can be used to execute a SQL statement or a stored procedure without returning any value. Available in two overloads:
{{public void ExecSQL(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)}}
{{public void ExecSQL(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)}}

2. GetRowsAffected
The GetRowsAffected method can be used to execute a SQL query or a stored procedure and return the total number of rows affected by execution. Available in two overloads:
{{public int GetRowsAffected(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)}}
{{public int GetRowsAffected(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)}}

3. GetDataTable
The GetDataTable method can used to execute a SQL query or a stored procedure and return the result set in a data table. Available in two overloads:
{{public DataTable GetDataTable(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)}}
{{public DataTable GetDataTable(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)}}

4. GetDataView
The GetDataView method can be used to execute a SQL query or a stored procedure and return the result set as a data view. Available in two overloads:
{{public DataView GetDataView(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)}}
{{public DataView GetDataView(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)}}

5. GetDataSet
The GetDataSet method can be used to execute a SQL query or a stored procedure and return the result set(s) in a dataset. Available in two overloads:
{{public DataSet GetDataSet(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc)}}
{{public DataSet GetDataSet(string sqlText, SQLTextType sqlTextType = SQLTextType.Stored_Proc, List<SqlParameter> parameterCollection = null)}}

## Future Roadmap
In future I plan, to extend this library into a framework, supporting various data sources such as XML, Excel, CSV files etc. 

## Helpful Links
Some of the links that might be helpful in understanding the programming concepts and technologies that have gone in making of this library:

Visual C#:
# [http://msdn.microsoft.com/en-us/vstudio/hh341490.aspx](http___msdn.microsoft.com_en-us_vstudio_hh341490.aspx)

.NET Framework:
# [http://www.microsoft.com/net](http___www.microsoft.com_net)
# [http://msdn.microsoft.com/en-US/vstudio/aa496123](http___msdn.microsoft.com_en-US_vstudio_aa496123)

ADO.NET:
# [http://msdn.microsoft.com/en-us/library/h43ks021%28v=vs.100%29.aspx](http___msdn.microsoft.com_en-us_library_h43ks021%28v=vs.100%29.aspx)

Code Contracts:
# [http://research.microsoft.com/en-us/projects/contracts](http___research.microsoft.com_en-us_projects_contracts)
# [http://msdn.microsoft.com/en-us/devlabs/dd491992.aspx](http___msdn.microsoft.com_en-us_devlabs_dd491992.aspx)
# [http://devjourney.com/blog/code-contracts-part-1-introduction](http___devjourney.com_blog_code-contracts-part-1-introduction)

Dependency Injection:
# [http://en.wikipedia.org/wiki/Dependency_injection](http___en.wikipedia.org_wiki_Dependency_injection)
# [http://msdn.microsoft.com/en-us/magazine/cc163739.aspx](http___msdn.microsoft.com_en-us_magazine_cc163739.aspx)