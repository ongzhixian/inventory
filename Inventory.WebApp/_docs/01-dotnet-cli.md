# dotnet-cli

## .NET packages used

Because we a target .NET Core 2.1, we have to specify version

```dotnet

REM Required for dotnet aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 2.1.9

REM Required for dotnet ef
dotnet add package Microsoft.EntityFrameworkCore.Design --version 2.1.8

REM Entity Framework Providers for Sqlite and MySql
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 2.1.8
dotnet add package MySql.Data.EntityFrameworkCore

REM Configuration
dotnet add package Microsoft.Extensions.Configuration --version 2.1.1
dotnet add package Microsoft.Extensions.Configuration.Json --version 2.1.1

REM For bundling and minification
dotnet add package BuildBundlerMinifier

```

## Project references

```dotnet
dotnet add Inventory.WebApp.csproj reference ..\Inventory.Data\Inventory.Data.csproj
```
