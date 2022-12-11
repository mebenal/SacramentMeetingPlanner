namespace SacramentMeetingPlanner.Models
{
    public class Calendar
    {
        public Calendar(DateTime _calBegin, DateTime _calEnd, int _month, int _year)
        {
            calBegin = _calBegin;
            calEnd = _calEnd;
            Month = _month;
            Year = _year;
        }
        public DateTime calBegin { get; set; }
        public DateTime calEnd { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
