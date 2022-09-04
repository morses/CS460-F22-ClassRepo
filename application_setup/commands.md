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

### Enable Lazy loading of related properties
Open the DBContext subclass that was created in the last step.  In this example it is called `SampleDbContext.cs` and is located in the `Models` folder.  Find the `OnConfiguring` method and add the line to use lazy loading proxies:
```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder
            .UseLazyLoadingProxies()        // <-- add this line
            .UseSqlServer("Name=SampleConnection");
    }
}
```

### Use LINQPad 7 to explore your data and build Linq queries
Download and install [LINQPad 7](https://www.linqpad.net/) (sorry, Windows only).

Make sure your project has built, then open LINQPad and click `Add connection`.  Select the radio button for "Use a typed data context from your own assembly" and choose "EntityFramework Core (3.x -> 7.x)".  Click Next.  Click Browse for the "Path to Custom Assembly" and go find your applications `.dll`  For this example it is in the bin folder of your applications source at `bin\Debug\net6.0\Sample.dll`.  After the dialog finds your DbContext class and populates the second text field, choose "Via a constructor that accepts a DbContextOptions<>" from the "How should LINQPad instantiate your DbContext?".  Click Test and hopefully it shows you that it can connect to the database through your applications code.

This process will be demonstrated in class.