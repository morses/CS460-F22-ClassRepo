using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SimpleApp.Data;
using SimpleApp.Models;
using System.Diagnostics;
using System.Net;
using SimpleApp.DAL.Abstract;
using SimpleApp.DAL.Concrete;

namespace SimpleApp_Tests;
/**
 * PLEASE NOTE: SQLite does not have a DateTime data type and stores date times as TEXT.  Therefore we must use the
 * YYYY-MM-DD HH:MM:SS literal rather than 
 * MM/DD/YYYY HH:MM:SS that SQL Server uses
 * 
 * There will likely be other differences between SQLite and SQL Server so watch out if a test is failing and you think
 * it is correct (as happened here for me).
 */
public class UserLogRepository_Tests
{
    private static readonly string _seedFile = @"..\..\..\Data\SEED.sql";  // relative path from where the executable is: bin/Debug/net7.0

    private InMemoryDbHelper<SimpleDbContext> _dbHelper = new InMemoryDbHelper<SimpleDbContext>(_seedFile, DbPersistence.OneDbPerTest);

    [Test]
    public void MostRecentVisit_ForUserWithTwoVisits_ReturnsNewest()
    {
        using SimpleDbContext context = _dbHelper.GetContext();
        IUserLogRepository repo = new UserLogRepository(context);
        // The db has been seeded

        // Act
        var logs = repo.MostRecentVisit("4b7959dc-2e9f-4fa9-ad38-d49ea70c8d32", 1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(logs.Count, Is.EqualTo(1));
            Assert.That(logs[0].TimeStamp, Is.EqualTo(DateTime.Parse("04/01/2019 12:32:00")));
        });
    }

    [Test]
    public void MostRecentVisit_ForUserWithTwoVisits_ReturnsTwoLogsInCorrectOrder()
    {
        using SimpleDbContext context = _dbHelper.GetContext();
        IUserLogRepository repo = new UserLogRepository(context);
        // The db has been seeded

        // Act
        var logs = repo.MostRecentVisit("4b7959dc-2e9f-4fa9-ad38-d49ea70c8d32", 2);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(logs.Count, Is.EqualTo(2));
            Assert.That(logs[0].TimeStamp, Is.EqualTo(DateTime.Parse("04/01/2019 12:32:00")));
            Assert.That(logs[1].TimeStamp, Is.EqualTo(DateTime.Parse("12/04/2017 08:44:03")));
        });
    }

    [Test]
    public void MostRecentVisit_ForUserWithNoVisits_ReturnsNoLogs()
    {
        using SimpleDbContext context = _dbHelper.GetContext();
        IUserLogRepository repo = new UserLogRepository(context);
        // The db has been seeded

        // Act
        var logs = repo.MostRecentVisit("44556677-aabb-ccdd-eeff-001122334455", 2);

        // Assert
        Assert.That(logs.Count, Is.EqualTo(0));
    }
}
