using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SimpleApp.Data;
using SimpleApp.Models;
using System.Diagnostics;
using System.Net;

namespace SimpleApp_Tests;

/**
 * This is the recommended way to test using the in-memory db.  Seed the db and then write your tests based only on the seed
 * data + anything else you do to it.  No other tests will modify the db for that test.  Every test gets a brand new seeded db.
 * 
 */
public class OneHelperDbPerTest_Tests
{
    private static readonly string _seedFile = @"..\..\..\Data\SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private InMemoryDbHelper<SimpleDbContext> _dbHelper = new InMemoryDbHelper<SimpleDbContext>(_seedFile, DbPersistence.OneDbPerTest);

    [Test]
    public void SimpleContext_Add_UserLog_IsSuccessful()
    {
        // And then get your context
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
        using SimpleDbContext context = _dbHelper.GetContext();

        Assert.That(context.UserLogs.Count(), Is.EqualTo(4));
    }
}