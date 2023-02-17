using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SimpleApp.Data;
using SimpleApp.Models;
using System.Diagnostics;
using System.Net;

namespace SimpleApp_Tests;

/**
 * WARNING, WARNING, WARNING!
 * 
 * The order in which you run the tests can affect the outcome of the tests.
 * 
 * When you use the same db for all tests, if you're not careful, some tests can fail because an earlier
 * test modified the state of the db to what you're not expecting.  
 * 
 * Only re-use the db if every one of your tests only depends on the seed data
 * and is 100% independent of every other test.
 */
public class ReuseTheDb_Tests
{
    private static readonly string _seedFile = @"..\..\..\Data\SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    private InMemoryDbHelper<SimpleDbContext> _dbHelper = new InMemoryDbHelper<SimpleDbContext>(_seedFile, DbPersistence.ReuseDb);

    [Test]
    public void SimpleContext_Add_UserLog_IsSuccessful()
    {
        // Can we use the dbcontext directly to add an entity?
        using SimpleDbContext context = _dbHelper.GetContext();

        // Arrange
        DateTime timestamp = new DateTime(2022, 9, 23, 6, 15, 22);
        IPAddress ip = IPAddress.Parse("140.211.115.34");
        string userAgent = "Mozilla/5.0 (iPad; U; CPU OS 3_2_1 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Mobile/7B405";
        string aspnetIdentityId = "4b7959dc-2e9f-4fa9-ad38-d49ea70c8d32";
        UserLog expected = new UserLog
        {
            TimeStamp = timestamp,
            Ipaddress = ip.ToString(),
            UserAgent = userAgent,
            AspnetIdentityId = aspnetIdentityId
        };

        // Act
        context.UserLogs.Add(expected);
        context.SaveChanges();

        // Assert
        Assert.That(context.UserLogs.Count(
            ul => ul.TimeStamp == expected.TimeStamp &&
                  ul.Ipaddress == expected.Ipaddress &&
                  ul.UserAgent == expected.UserAgent &&
                  ul.AspnetIdentityId == expected.AspnetIdentityId), Is.EqualTo(1));

    }

    [Test]
    public void SimpleContext_HasBeenSeeded()
    {
        SimpleDbContext context = _dbHelper.GetContext();
        // !!! This test will fail if another test adds an item to the userlogs table.
        // But that doesn't mean it isn't correct!  Incorrect could be correct ^~~((
        Assert.That(context.UserLogs.Count(), Is.EqualTo(5));
    }
}
