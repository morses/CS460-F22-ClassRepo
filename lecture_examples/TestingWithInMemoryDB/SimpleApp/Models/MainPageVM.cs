using Humanizer;

namespace SimpleApp.Models
{
    public class MainPageVM
    {
        public bool HasUser { get; set; } = false;
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private string _lastVisitAgo = null;

        public void SetVisitTimes(DateTime tLast, DateTime tNow)
        {
            // https://github.com/Humanizr/Humanizer
            _lastVisitAgo = tNow.Subtract(tLast).Humanize();
        }

        public string GetLastVisitHumanized()
        {
            return _lastVisitAgo ?? "";
        }

    }
}
