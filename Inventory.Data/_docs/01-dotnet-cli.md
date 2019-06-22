# dotnet-cli

## .NET packages used

Because we a target .NET Core 2.1, we have to specify version

```dotnet

REM Required for dotnet ef
dotnet add package Microsoft.EntityFrameworkCore.Design --version 2.1.8

REM Entity Framework Providers for Sqlite and MySql
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 2.1.8
dotnet add package MySql.Data.EntityFrameworkCore

```

## Project references

```dotnet

```