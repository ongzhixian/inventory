# dotnet-cli

1.  Create global.json file to set the version of .NET Core SDK to use

```dotnet
dotnet new globaljson --sdk-version 2.1.603
```

2.  Create new solution

```dotnet
dotnet new sln -n Inventory
```

3.  Create new website

```dotnet
dotnet new mvc -n Inventory.WebApp
```

4.  Create new library

```dotnet
dotnet new classlib -n Inventory.Data
dotnet new classlib -n Inventory.Data.Tests
```

5.  Adding project to solution

Assumes the solution file is in the directory where the command is executed and
that the project file is in a sub-directory call Csi.WebApp

```dotnet
dotnet sln Inventory.sln add Inventory.WebApp/Inventory.WebApp.csproj
dotnet sln Inventory.sln add Inventory.Data/Inventory.Data.csproj
```
