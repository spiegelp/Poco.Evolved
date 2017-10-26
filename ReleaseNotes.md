# Release notes

## Poco.Evolved.Core

### v1.0.0
#### Features
* Abstract framework for data and schema migrations
* Android's SQLite like migrations
* Class based migrations
* Targets .NET Standard 2.0 and .NET 4.5
### v1.0.1
#### Bugfixes
* Use a transaction for creating the table (or equivalent for non-SQL databases) for installed versions

### vX.X.X (upcoming release)
#### Features
* Targets .NET Standard 1.3, .NET Standard 2.0 and .NET 4.5
* Apply migrations `async`
* Skip the initialization of the storage of installed versions

## Poco.Evolved.LiteDB

### v1.0.0-beta
#### Features
* Implementation of Poco.Evolved.Core for [LiteDB](http://www.litedb.org/) version v4.0.0-beta1
* Targets .NET Standard 2.0 and .NET 4.5

### v1.0.0
#### Features
* Implementation of Poco.Evolved.Core for [LiteDB](http://www.litedb.org/) version v4.0.0
* Targets .NET Standard 1.3, .NET Standard 2.0 and .NET 4.5

## Poco.Evolved.SQL

### v1.0.0
#### Features
* Implementation of Poco.Evolved.Core for SQL databases
* Generic support for databases implementing ANSI SQL
* Special support for:
  * Firebird
  * SQLite
* Targets .NET Standard 2.0 and .NET 4.5

### vX.X.X (upcoming release)
#### Features
* Targets .NET Standard 1.3, .NET Standard 2.0 and .NET 4.5
* Run migrations via SQL files
