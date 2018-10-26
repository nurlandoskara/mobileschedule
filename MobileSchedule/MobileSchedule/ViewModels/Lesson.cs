using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MobileSchedule.ViewModels
{
    public class Lesson
    {
        public int Order { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
    }
}
