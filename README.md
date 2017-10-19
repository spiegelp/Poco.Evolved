# Poco.Evolved
Poco.Evolved is a framework based on .NET Standard for schema and data migrations on databases. 

It consists of the following packages: 
* Poco.Evolved.Core
* Poco.Evolved.LiteDB
* Poco.Evolved.SQL

## Poco.Evolved.Core
Poco.Evolved.Core the basic framework for schema and data migrations on databases. It provides a database independent API to be used with any kind of database. 

## Poco.Evovled.LiteDB
Poco.Evolved.LiteDB is a specific implementation of Poco.Evolved for [LiteDB](http://www.litedb.org). As a document database, LiteDB is schema-less. However you might update or reorganize some persistent data with your new app version. 

## Poco.Evovled.SQL
Poco.Evolved.SQL is a specific implementation of Poco.Evolved for SQL databases. It uses generic SQL to be compatible with every database using standard SQL. 

Furthermore, Poco.Evolved.SQL has special support for the following databases to utilize their specific features:
* Firebird
* SQLite

## Installation
Binaries are available as NuGet package for .NET Standard 2.0 and .NET Framework 4.5: 
* [Poco.Evolved.Core](https://www.nuget.org/packages/Poco.Evolved.Core/)
* [Poco.Evolved.LiteDB](https://www.nuget.org/packages/Poco.Evolved.LiteDB)
* [Poco.Evolved.SQL](https://www.nuget.org/packages/Poco.Evolved.SQL/)

## Usage
You will find the documentation inside the [wiki](https://github.com/spiegelp/Poco.Evolved/wiki). Furthermore you can have a look at the Poco.Evolved.???.Demo projects for usage examples. 

## License
Poco.Evolved is licensed under the [MIT](https://github.com/spiegelp/Poco.Evolved/blob/master/LICENSE) license. 
