using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileSchedule.Helpers
{
    public static class DictionaryHelper
    {
        private static Dictionary<int, string> DayOfWeeks { get; set; } = new Dictionary<int, string>
        {
            {1, "Дүйсенбі"},
            {2, "Сейсенбі"},
            {3, "Сәрсенбі"},
            {4, "Бейсенбі"},
            {5, "Жұма"},
            {6, "Сенбі"},
            {7, "Жексенбі"}
        };

        public static string GetDayOfWeekName(int day)
        {
            return DayOfWeeks.FirstOrDefault(x => x.Key == day).Value;
        }
    }
}
