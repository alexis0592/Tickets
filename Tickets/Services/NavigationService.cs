using System;
using System.Threading.Tasks;
using Tickets.Views;
using Xamarin.Forms;

namespace Tickets.Services
{
    public class NavigationService
    {
		public async Task Navigate(string pageName)
		{
			switch (pageName)
			{
				case "CheckTicketPage":
                    await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CheckTicketPage()));
					break;
				default:
					break;
			}
		}

		public async Task Back()
		{
			await App.Current.MainPage.Navigation.PopAsync();
		}
    }
}
