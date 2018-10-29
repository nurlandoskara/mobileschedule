using MobileSchedule.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileSchedule
{
    public partial class GroupPage : ContentPage
    {
        public ObservableCollection<GroupOrTeacher> Groups { get; set; }

        public GroupPage()
        {
            InitializeComponent();
            ShowGroups();
        }

        private static async Task<dynamic> GetGroupsAsync()
        {
            var queryString = "http://sschedule.azurewebsites.net/api/groups";
            var groups = await DataService.GetData(queryString).ConfigureAwait(false);
            return groups;
        }

        private async void ShowGroups()
        {
            try
            {
                var groupsList = new List<GroupOrTeacher>();
                GroupsList.IsRefreshing = true;
                var groups = await GetGroupsAsync();
                GroupsList.EndRefresh();
                if (groups != null)
                {
                    foreach (var group in groups)
                    {
                        groupsList.Add(new GroupOrTeacher() { DisplayName = group.DisplayName, Id = group.Id });
                    }
                }
                Groups = new ObservableCollection<GroupOrTeacher>(groupsList);
                GroupsList.ItemsSource = Groups;
            }
            catch (Exception e)
            {
                await DisplayAlert("Серверге қосылу мүмкін емес", "Интернет қосылуын тексеріңіз", "OK");
                return;
            }
        }


        public async void GroupSelected()
        {
            var group = GroupsList.SelectedItem as GroupOrTeacher;
            Application.Current.Properties["groupId"] = group?.Id;
            Application.Current.Properties["groupName"] = group?.DisplayName;
            var page = new SchedulePage(){Title = "Сабақ кестесі"};
            await Navigation.PushAsync(page);
        }

        private void GroupsList_OnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            GroupSelected();
        }

        private void GroupsList_OnRefreshing(object sender, EventArgs e)
        {
            ShowGroups();
        }

        private void SearchGroupText_OnSearchButtonPressed(object sender, EventArgs e)
        {
            string text = ((SearchBar) sender).Text;
            GroupsList.ItemsSource = Groups.Where(x => x.DisplayName.ToLower().Contains(text.ToLower())).ToList();
        }

        private void SearchGroupText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((SearchBar)sender).Text;
            GroupsList.ItemsSource = Groups.Where(x => x.DisplayName.ToLower().Contains(text.ToLower())).ToList();
        }
    }
}
