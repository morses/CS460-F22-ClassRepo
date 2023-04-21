using Standups.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standups_Tests
{
    [TestFixture]
    public class DateTimeCriteriaTests
    {
        // Setup: class meets at 12:00 noon on MTWR
        int classtime = 12;
        DayOfWeek[] classdays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday };

        // Sunday
        [Test]
        public void TestSunday_Should_Be_Valid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 16, 14, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;

            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        [Test]
        public void TestSunday_Should_Be_InValid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 16, 2, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.False);
        }

        // Monday
        [Test]
        public void TestMonday_Should_Be_Valid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 17, 2, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        [Test]
        public void TestMonday_Should_Be_Valid_Also()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 17, 14, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        // Tuesday
        [Test]
        public void TestTuesday_Should_Be_Valid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 18, 2, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        [Test]
        public void TestTuesday_Should_Be_Valid_Also()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 18, 14, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        // Wednesday
        [Test]
        public void TestWednesday_Should_Be_Valid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 19, 2, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        [Test]
        public void TestWednesday_Should_Be_Valid_Also()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 19, 14, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        // Thursday
        [Test]
        public void TestThursday_Should_Be_Valid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 20, 2, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.True);
        }

        [Test]
        public void TestThursday_Should_Be_InValid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 20, 14, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.False);
        }

        // Friday
        [Test]
        public void TestFriday_Should_Be_InValid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 21, 2, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.False);
        }
        [Test]
        public void TestFriday_Should_Be_InValid_Also()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 21, 14, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.False);
        }

        // Saturday
        [Test]
        public void TestSaturday_Should_Be_InValid()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 22, 2, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.False);
        }
        [Test]
        public void TestSaturday_Should_Be_InValid_Also()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 4, 22, 14, 30, 52, new TimeSpan(-8, 0, 0));
            DateTime date = dto.UtcDateTime;
            Supmeeting mtg = new Supmeeting() { SubmissionDate = date };

            Assert.That(mtg.TimeStampMeetsCriteria(classtime, classdays), Is.False);
        }

    }
}
