namespace Athena.Domain.Common.Const
{
    public static class Days
    {
        private static List<string> DaysOfWeek = new List<string>
        {
            "saturday",
            "sunday",
            "monday",
            "tuesday",
            "wednesday",
            "thursday",
            "friday"
        };

        public static bool IsDayOfWeek(string dayOfWeek)
        {
            return DaysOfWeek.Contains(dayOfWeek.ToLower());
        }

    }
}
