namespace ReportingSystem.Test.GenerateData
{
    static public class GenerateDate
    {
        static public DateTime BetweenDates(DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            int range = (endDate - startDate).Days;
            return startDate.AddDays(random.Next(range));
        }

        static public List<DateTime> RangeDates(DateTime startWorkDate, int amount, bool includeWeekends)
        {
            var random = new Random();
            int range = (DateTime.Today - startWorkDate).Days;

            List<DateTime> workDates = new List<DateTime>();

            HashSet<DayOfWeek> weekendDays = new HashSet<DayOfWeek>
        {
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

            while (workDates.Count < amount)
            {
                DateTime currentDate = startWorkDate.AddDays(random.Next(range));

                if (!includeWeekends && weekendDays.Contains(currentDate.DayOfWeek))
                {
                    continue;
                }

                if (!workDates.Contains(currentDate))
                {
                    workDates.Add(currentDate);
                }
            }

            workDates.Sort();
            return workDates;
        }


        
    }
}
