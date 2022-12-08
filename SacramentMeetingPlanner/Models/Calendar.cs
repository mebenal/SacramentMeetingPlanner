namespace SacramentMeetingPlanner.Models
{
    public class Calendar
    {
        public Calendar(DateTime _calBegin, DateTime _calEnd)
        {
            calBegin = _calBegin;
            calEnd = _calEnd;
        }
        public DateTime calBegin { get; set; }
        public DateTime calEnd { get; set; }
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
