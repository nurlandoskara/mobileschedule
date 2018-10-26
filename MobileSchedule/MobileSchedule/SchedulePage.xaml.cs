using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileSchedule.Helpers;
using MobileSchedule.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileSchedule
{
    public partial class SchedulePage : ContentPage
    {
        public ObservableCollection<Day> Days { get; set; }

        public SchedulePage()
        {
            InitializeComponent();
            ShowSchedule();
        }

        private static async Task<dynamic> GetDaysAsync(int groupId)
        {
            var queryString = "http://sschedule.azurewebsites.net/api/schedule?groupId=" + groupId;
            var days = await DataService.GetData(queryString).ConfigureAwait(false);
            return days;
        }

        private async void ShowSchedule()
        {
            if (Application.Current.Properties.ContainsKey("groupId"))
            {
                var groupId = (int)Application.Current.Properties["groupId"];
                var groupName = Application.Current.Properties["groupName"].ToString();
                GroupName.Text = groupName + " сыныбына арналған сабақ кестесі";
                try
                {
                    var schedule = new List<Day>();
                    ScheduleTable.IsRefreshing = true;
                    var days = await GetDaysAsync(groupId);
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
                                    TeacherName = itemLesson.TeacherName,
                                    SubjectName = itemLesson.SubjectName
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