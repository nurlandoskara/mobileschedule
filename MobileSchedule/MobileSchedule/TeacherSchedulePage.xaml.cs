using MobileSchedule.Helpers;
using MobileSchedule.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileSchedule
{
    public partial class TeacherSchedulePage : ContentPage
    {
        public ObservableCollection<Day> Days { get; set; }

        public TeacherSchedulePage()
        {
            InitializeComponent();
            ShowSchedule();
        }

        private static async Task<dynamic> GetDaysAsync(int teacherId)
        {
            var queryString = "http://sschedule.azurewebsites.net/api/tschedule?teacherId=" + teacherId;
            var days = await DataService.GetData(queryString).ConfigureAwait(false);
            return days;
        }

        private async void ShowSchedule()
        {
            if (Application.Current.Properties.ContainsKey("teacherId"))
            {
                var teacherId = (int)Application.Current.Properties["teacherId"];
                var teacherName = Application.Current.Properties["teacherName"].ToString();
                ScheduleName.Text = teacherName + " сабақ кестесі";
                try
                {
                    var schedule = new List<Day>();
                    ScheduleTable.IsRefreshing = true;
                    var days = await GetDaysAsync(teacherId);
                    ScheduleTable.IsRefreshing = false;
                    if (days != null)
                    {
                        foreach (var item in days)
                        {
                            var day = new Day { DayOfWeek = DictionaryHelper.GetDayOfWeekName((int)item.DayOfWeek) };
                            var lessons = new List<Lesson>();
                            foreach (var itemLesson in item.Lessons)
                            {
                                var lesson = new Lesson
                                {
                                    Order = itemLesson.Order,
                                    GroupOrTeacherName = itemLesson.GroupOrTeacherName,
                                    SubjectName = itemLesson.SubjectName,
                                    AuditoryName = itemLesson.AuditoryName
                                };
                                lessons.Add(lesson);
                            }
                            day.Lessons = lessons;
                            schedule.Add(day);
                        }
                    }
                    Days = new ObservableCollection<Day>(schedule);
                    ScheduleTable.ItemsSource = Days;
                }
                catch (Exception e)
                {
                    await DisplayAlert("Серверге қосылу мүмкін емес", "Интернет қосылуын тексеріңіз", "OK");
                    return;
                }
            }
        }

        private void ScheduleTable_OnRefreshing(object sender, EventArgs e)
        {
            ShowSchedule();
        }
    }
}