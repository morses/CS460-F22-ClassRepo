using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standups.Models
{
    // Use partial class feature of C# to extend the functionality of our models.
    // Putting it here keeps it from being overwritten when we next reverse engineer our models
    public partial class Supquestion
    {
        public int CountOfCommentsWithAdvisorApproval()
        {
            return Supcomments.Count(c => c.AdvisorRating > 0);
        }
    }

    public partial class Supuser
    {
        public bool HasGroup() => Supgroup != null && SupgroupId != null;

        public string FullName => FirstName + " " + LastName;

        public override string ToString()
        {
            string groupName = this.Supgroup?.Name ?? "No group assigned";
            return String.Format("SUPUser: {0}, {1}, {2}", this.Id, this.FullName, groupName);
        }
    }

    public partial class Supadvisor
    {
        [Display(Name = "Advisor Name")]
        public string FullName => FirstName + " " + LastName;
    }

    /// <summary>
    /// Functionality added to the Supmeeting class.  We are hard-coding this to Pacific time zone.
    /// </summary>
    public partial class Supmeeting
    {
        // Set this to true before displaying a report to the user so they can't edit it in the view
        // this property is used in the view in order to set the readonly property when it's rendered
        // The NotMapped attribute is to keep EF from trying to get/set it in the database
        [NotMapped]
        public bool ReadOnly { get; set; } = false;

        //public DateTime SubmissionDate { get; set; }
        private TimeZoneInfo pacificTZI = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        public DateTime TimeStampInPacificTimeZone
        {
            get
            {
                DateTime pstTime = TimeZoneInfo.ConvertTimeFromUtc(SubmissionDate, pacificTZI);
                return pstTime;
            }
        }

        public string PSTorPDT
        {
            get
            {
                //TimeZoneInfo pacificTZI = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                // Full spelling: Pacific Daylight Time
                //return TimeStampInPacificTimeZone.IsDaylightSavingTime() ? pacificTZI.DaylightName : pacificTZI.StandardName;
                // Abbreviated spelling: PDT
                return TimeStampInPacificTimeZone.IsDaylightSavingTime() ? "PDT" : "PST";
            }
        }

        public string DayOfWeekString
        {
            get
            {
                return TimeStampInPacificTimeZone.ToString("dddd");
            }
        }

        /// <summary>
        /// Does this timestamp correspond to a valid time, i.e. it was submitted on a class day
        /// prior to the class time
        /// </summary>
        /// <param name="duetime">e.g. 12 for 12 noon, 0-23 for the hour of the day</param>
        /// <param name="days">An array of DayOfWeek containing all class days</param>
        /// <returns>true if the timestamp for this meeting occurs on one of the given days prior to the given class time</returns>
        public bool TimeStampBeforeClassTimeDays(int duetime, DayOfWeek[] days)
        {
            if (days == null)
                return false;
            // check class day
            DayOfWeek tsDay = TimeStampInPacificTimeZone.DayOfWeek;
            // and time of day
            int tshours = TimeStampInPacificTimeZone.TimeOfDay.Hours;
            return tshours < duetime && days.Contains(tsDay);
        }

        /// <summary>
        /// Does this timestamp correspond to a valid time. The criteria for this method
        /// is that the report was submitted before class on the due date, OR was submitted
        /// the previous day AFTER class time
        /// </summary>
        /// <param name="duetime">e.g. 12 for 12 noon, 0-23 for the hour of the day</param>
        /// <param name="days">An array of DayOfWeek containing all class days</param>
        /// <returns>true if the timestamp meets criteria, false otherwise</returns>
        public bool TimeStampMeetsCriteria(int duetime, DayOfWeek[] days)
        {
            if (days == null)
                return false;

            // check class day
            DayOfWeek tsDay = TimeStampInPacificTimeZone.DayOfWeek;
            int tsDayi = (int)tsDay;

            // and time of day
            int tshours = TimeStampInPacificTimeZone.TimeOfDay.Hours;

            bool result = false;

            foreach (DayOfWeek classDay in days)
            {
                // on the same day prior to class time
                result |= tshours < duetime && tsDayi == (int)classDay;
                // on the day before, after class time
                if (classDay != DayOfWeek.Sunday)
                {
                    result |= tshours >= duetime && tsDayi == (int)classDay - 1;
                }
            }
            return result;
        }
    }

}
