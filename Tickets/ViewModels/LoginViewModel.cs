using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Tickets.Models;
using Tickets.Services;

namespace Tickets.ViewModels
{
    public class LoginViewModel : User
    {

        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;
        #endregion

        #region Properties
        public User UserLogged
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.dialogService = new DialogService();
            this.navigationService = new NavigationService();
        }
		#endregion


		#region Singleton
        static LoginViewModel instance;

        public static LoginViewModel GetInstance()
		{
			if (instance == null)
			{
                instance = new LoginViewModel();
			}

			return instance;
		}
		#endregion

		#region Commands
		public ICommand LoginCommand
        {
            get{
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if(string.IsNullOrEmpty(Email)){

				await dialogService.ShowMessage("Error", "You must enter a valid email");
				return;
            }

            if(string.IsNullOrEmpty(Password)){
				await dialogService.ShowMessage("Error", "You must enter password");
				return;
            }

			var values = new Dictionary<string, string>{
					{"Email", Email},
					{"Password", Password}
				};

            var response = await apiService.Login("http://checkticketsback.azurewebsites.net",
                                                  "/api", "/Users/Login", values);
            UserLogged = (User)response.Result;

            if(!response.IsSuccess){
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            await navigationService.Navigate("CheckTicketPage");
        }
        #endregion
    }
}
