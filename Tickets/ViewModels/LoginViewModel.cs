using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Tickets.Models;
using Tickets.Services;

namespace Tickets.ViewModels
{
    public class LoginViewModel : User, INotifyPropertyChanged
    {
        
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;
        private bool isRunning;
        #endregion

        #region Properties
        public User UserLogged
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get{
                return isRunning;
            }
            set{
                if(isRunning != value){
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
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


            IsRunning = true;
            var response = await apiService.Post("http://checkticketsback.azurewebsites.net",
                                                  "/api", "/Users/Login", new User{Email = Email,
                                                                            Password = Password});
            UserLogged = (User)response.Result;


            if(!response.IsSuccess){
                await dialogService.ShowMessage("Error", response.Message);
                IsRunning = false;
                return;
            }

            IsRunning = false;

            var mainVewModel = MainViewModel.GetInstance();
            mainVewModel.CheckTicket = new CheckTicketViewModel();
            await navigationService.Navigate("CheckTicketPage");
        }
        #endregion
    }
}
