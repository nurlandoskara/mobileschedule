using MobileSchedule.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileSchedule
{
    public partial class NewsPage : ContentPage
	{
	    public ObservableCollection<News> News { get; set; }

        public NewsPage ()
		{
			InitializeComponent ();
		    ShowNews();
		}

	    private static async Task<dynamic> GetNewsAsync()
	    {
	        var queryString = "http://sschedule.azurewebsites.net/api/news";
	        var news = await DataService.GetData(queryString).ConfigureAwait(false);
	        return news;
	    }

	    private async void ShowNews()
	    {
	        try
	        {
	            var newsList = new List<News>();
	            NewsList.IsRefreshing = true;
	            var news = await GetNewsAsync();
	            NewsList.EndRefresh();
	            if (news != null)
	            {
	                foreach (var item in news)
	                {
	                    newsList.Add(new News() { Title = item.Title, Description = item.Description, Id = item.Id});
	                }
	            }
	            News = new ObservableCollection<News>(newsList);
	            NewsList.ItemsSource = News;
	        }
	        catch (Exception e)
	        {
	            await DisplayAlert("Серверге қосылу мүмкін емес", "Интернет қосылуын тексеріңіз", "OK");
	            return;
	        }
	    }

	    private void NewsList_OnRefreshingList_OnRefreshing(object sender, EventArgs e)
	    {
	        ShowNews();
	    }

	    private void SearchNewsText_OnSearchButtonPressed(object sender, EventArgs e)
	    {
	        string text = ((SearchBar)sender).Text;
	        NewsList.ItemsSource = News.Where(x => x.Title.ToLower().Contains(text.ToLower())).ToList();
        }

	    private void SearchNewsText_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
	        string text = ((SearchBar)sender).Text;
	        NewsList.ItemsSource = News.Where(x => x.Title.ToLower().Contains(text.ToLower())).ToList();
        }
	}
}