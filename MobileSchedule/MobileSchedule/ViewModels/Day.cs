using System;
using System.Collections.Generic;
using System.Text;

namespace MobileSchedule.ViewModels
{
    public class Day
    {
        public string DayOfWeek { get; set; }
        public IList<Lesson> Lessons { get; set; }
    }
}
