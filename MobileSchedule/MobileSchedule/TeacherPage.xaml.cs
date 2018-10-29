using MobileSchedule.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileSchedule
{
    public partial class TeacherPage : ContentPage
    {
        public ObservableCollection<GroupOrTeacher> Teachers { get; set; }

        public TeacherPage()
        {
            InitializeComponent();
            ShowTeachers();
        }

        private static async Task<dynamic> GetGroupsAsync()
        {
            var queryString = "http://sschedule.azurewebsites.net/api/teachers";
            var teachers = await DataService.GetData(queryString).ConfigureAwait(false);
            return teachers;
        }

        private async void ShowTeachers()
        {
            try
            {
                var teachersList = new List<GroupOrTeacher>();
                TeachersList.IsRefreshing = true;
                var teachers = await GetGroupsAsync();
                TeachersList.EndRefresh();
                if (teachers != null)
                {
                    foreach (var teacher in teachers)
                    {
                        teachersList.Add(new GroupOrTeacher() { DisplayName = teacher.DisplayName, Id = teacher.Id });
                    }
                }
                Teachers = new ObservableCollection<GroupOrTeacher>(teachersList);
                TeachersList.ItemsSource = Teachers;
            }
            catch (Exception e)
            {
                await DisplayAlert("Серверге қосылу мүмкін емес", "Интернет қосылуын тексеріңіз", "OK");
                return;
            }
        }


        public async void TeacherSelected()
        {
            var group = TeachersList.SelectedItem as GroupOrTeacher;
            Application.Current.Properties["teacherId"] = group?.Id;
            Application.Current.Properties["teacherName"] = group?.DisplayName;
            var page = new TeacherSchedulePage(){Title = "Мұғалім кестесі"};
            await Navigation.PushAsync(page);
        }

        private void TeachersList_OnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            TeacherSelected();
        }

        private void TeachersList_OnRefreshing(object sender, EventArgs e)
        {
            ShowTeachers();
        }

        private void SearchTeacherText_OnSearchButtonPressed(object sender, EventArgs e)
        {
            string text = ((SearchBar) sender).Text;
            TeachersList.ItemsSource = Teachers.Where(x => x.DisplayName.ToLower().Contains(text.ToLower())).ToList();
        }

        private void SearchTeacherText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((SearchBar)sender).Text;
            TeachersList.ItemsSource = Teachers.Where(x => x.DisplayName.ToLower().Contains(text.ToLower())).ToList();
        }
    }
}
