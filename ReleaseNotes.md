# Release notes
## Poco.Evolved.Core
### v1.0.0
#### Features
* Abstract framework for data and schema migrations
* Android's SQLite like migrations
* Class based migrations
### v1.0.1
#### Bugfixes
* Use a transaction for creating the table (or equivalent for non-SQL databases) for installed versions
## Poco.Evolved.LiteDB
### v1.0.0-beta
#### Features
* Implementation of Poco.Evolved.Core for [LiteDB](http://www.litedb.org/) version v4.0.0-beta1
## Poco.Evolved.SQL
### v1.0.0
#### Features
* Implementation of Poco.Evolved.Core for SQL databases
* Generic support for databases implementing standard SQL
* Special support for:
  * Firebird
  * SQLite
