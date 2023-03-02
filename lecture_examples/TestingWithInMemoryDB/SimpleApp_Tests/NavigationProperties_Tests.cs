using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SimpleApp.Data;
using SimpleApp.Models;
using System.Diagnostics;
using System.Net;

namespace SimpleApp_Tests;

/**
 * Test to make sure navigation properties are being set when using the SQLite in-memory db for testing
 */
public class NavigationProperties_Tests
{
    private static readonly string _seedFile = @"..\..\..\Data\SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    // Create this helper like this, for whatever context you desire
    private InMemoryDbHelper<SimpleDbContext> _dbHelper = new InMemoryDbHelper<SimpleDbContext>(_seedFile, DbPersistence.OneDbPerTest);

    [Test]
    public void Colors_Are_Seeded()
    {
        using SimpleDbContext context = _dbHelper.GetContext();

        Assert.That(context.Colors.Count(),Is.EqualTo(3));
    }

    [Test]      // Test the "to-many" nav property from Color to UserLog
    public void Color_UserLogs_NavProperty_IsPopulated()
    {
        using SimpleDbContext context = _dbHelper.GetContext();

        Assert.Multiple(() =>
        {
            Assert.That(context.Colors.Single(c => c.HexValue == "0xFF0000").UserLogs, Is.Not.Null);
            Assert.That(context.Colors.Single(c => c.HexValue == "0x00FF00").UserLogs, Is.Not.Null);
            Assert.That(context.Colors.Single(c => c.HexValue == "0x0000FF").UserLogs, Is.Not.Null);
            Assert.That(context.Colors.Single(c => c.HexValue == "0xFF0000").UserLogs.Count(), Is.EqualTo(2));
            Assert.That(context.Colors.Single(c => c.HexValue == "0x00FF00").UserLogs.Count(), Is.EqualTo(1));
            Assert.That(context.Colors.Single(c => c.HexValue == "0x0000FF").UserLogs.Count(), Is.EqualTo(1));
        });
    }

    [Test]      // Test the "to-one" nav property from UserLog to Color
    public void UserLogs_Color_IsPopulated()
    {
        using SimpleDbContext context = _dbHelper.GetContext();

        Assert.Multiple(() =>
        {
            Assert.That(context.UserLogs.Single(ul => ul.Id == 1).Color, Is.Not.Null);
            Assert.That(context.UserLogs.Single(ul => ul.Id == 2).Color, Is.Not.Null);
            Assert.That(context.UserLogs.Single(ul => ul.Id == 3).Color, Is.Not.Null);
            Assert.That(context.UserLogs.Single(ul => ul.Id == 4).Color, Is.Not.Null);
            Assert.That(context.UserLogs.Single(ul => ul.Id == 1).Color.HexValue, Is.EqualTo("0xFF0000"));
            Assert.That(context.UserLogs.Single(ul => ul.Id == 2).Color.HexValue, Is.EqualTo("0xFF0000"));
            Assert.That(context.UserLogs.Single(ul => ul.Id == 3).Color.HexValue, Is.EqualTo("0x00FF00"));
            Assert.That(context.UserLogs.Single(ul => ul.Id == 4).Color.HexValue, Is.EqualTo("0x0000FF"));
        });
        
    }
}