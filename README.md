# Poco.Evolved
Poco.Evolved is a framework based on .NET Standard for schema and data migrations on databases.

It consists of the following packages:
* Poco.Evolved.Core
* Poco.Evolved.LiteDB

## Poco.Evolved.Core
Poco.Evolved.Core the basic framework for schema and data migrations on databases. It provides a database independent API to be used with any kind of database.

## Poco.Evovled.LiteDB
Poco.Evolved.LiteDB is a specific implementation of Poco.Evolved for [LiteDB](http://www.litedb.org). As a document database, LiteDB is schema-less. However you might update or reorganize some persistent data with your new app version.
