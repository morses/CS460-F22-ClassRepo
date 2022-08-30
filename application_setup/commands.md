## Miscellaneous
.NET 6.0 is the current "main" release.  Let's use that one.
```
dotnet --list-sdks
dotnet help
dotnet new -h
dotnet help new
```

## Create solution named "Sample" and project also named "Sample" in a subfolder
```
dotnet new mvc --output Sample/Sample --framework net6.0 --auth None --use-program-main true
dotnet new sln -o Sample
dotnet sln Sample add Sample/Sample
```

## Add required packages
[Documentation for command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package)
[NuGet Repository](https://www.nuget.org/)
Either manually add version information (--version) or double check that you got the ones you wanted

```
cd Sample
dotnet list package
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
# If planning to use lazy loading
dotnet add package Microsoft.EntityFrameworkCore.Proxies
```

## Run and open
```
dotnet dev-certs https --trust
dotnet build
dotnet run
code .
```

## Add data model and finish setting up EF
Install or update the tool (install is only needed once as it installs the tool globally)
```
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

### Add DB connection with connection string named SampleConnection.
Sample connection strings.  NOTE: The `\\` is for when this connection string is written inside the appsettings.json file, which must be in a JSON format.  To get a `\` char in JSON you must escape it as `\\`.  When you put this connection string into the dotnet user-secrets, to hide your username and password, you no longer need the escape sequence.

```
# Docker (password is in single quotes in case it has special characters, would still
# need to escape a ' as ''):
Data Source=localhost;Initial Catalog=AuctionHouse;User Id=sa;Password='Hello123#';

# LocalDB
Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AuctionHouse;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
```

### Create Model classes and the DBContext subclass
Reverse engineer C# model files from the DB schema found through the SampleConnection, placing them in the Models folder, and also create a DBContext subclass called SampleDbContext in the Models folder.  If older models exist already, the --force will overwrite them.
```
dotnet ef dbcontext scaffold Name=SampleConnection Microsoft.EntityFrameworkCore.SqlServer --context SampleDbContext --context-dir Models --output-dir Models --verbose --force
# or if you want to use data annotations in your model classes rather than defining things in the context class
# try them one after another to see the difference in the generated classes
dotnet ef dbcontext scaffold Name=SampleConnection Microsoft.EntityFrameworkCore.SqlServer --context SampleDbContext --context-dir Models --output-dir Models --verbose --force  --data-annotations
```
