using System;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Tickets.Models;
using Tickets.Services;

namespace Tickets.ViewModels
{
    public class CheckTicketViewModel : Ticket, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private string checkTicketText;
        private string labelTextColor;
        private bool isRunning;
        #endregion

        #region Properties
        public string CheckTicketText
        {
            get{
                return checkTicketText;
            }
            set{
                if(checkTicketText != value){
                    checkTicketText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CheckTicketText"));
                }
            }
        }

        public string LabelTextColor
        {
            get{
                return labelTextColor;
            }
            set{
                if(labelTextColor != value){
                    labelTextColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LabelTextColor"));
                }
            }
        }

        public Ticket TicketToSave
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

        #region Contructors
        public CheckTicketViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            CheckTicketText = "Wait for Read a Ticket...";
            LabelTextColor = "Blue";

        }
        #endregion

        #region Commands
        public ICommand CheckCommand
        {
            get{
                return new RelayCommand(CheckTicket);
            }
        }

        private async void CheckTicket()
        {
            TicketCode = TicketCode;
            if(string.IsNullOrEmpty(TicketCode)){
				await dialogService.ShowMessage("Error", "Please Enter a Ticket Code");
				return;
            }

            if(TicketCode.Length != 4){
				await dialogService.ShowMessage("Error", "Ticket Code Length must be 4");
				return;
            }

            IsRunning = true;
            var response = await apiService.GetTicket<Ticket>("http://checkticketsback.azurewebsites.net",
                                                              "/api", "/Tickets/", TicketCode);
            TicketToSave = (Ticket)response.Result;

            IsRunning = false;
            if(!response.IsSuccess){
                await dialogService.ShowMessage("Save Ticket", "The Ticket you entered would be save");

				CheckTicketText = string.Format("{0}, ALLOW ACCESS", TicketCode);
				LabelTextColor = "Green";

                TicketToSave = new Ticket();
                TicketToSave.TicketCode = TicketCode;
                TicketToSave.DateTime = DateTime.Now.ToLocalTime();
				var loginViewModel = LoginViewModel.GetInstance();
                TicketToSave.UserId = loginViewModel.UserLogged.UserId;

                saveTicket(TicketToSave);
                return;
            }

            CheckTicketText = string.Format("{0}, TICKET READ BEFORE!", TicketCode);
            LabelTextColor = "Red";
        }

        private async void saveTicket(Ticket ticket){

            var response = await apiService.Post("http://checkticketsback.azurewebsites.net",
                                                  "/api", "/Tickets", ticket);

            if(!response.IsSuccess){
                await dialogService.ShowMessage("Error", "Error al guardar el ticket");
                return;
            }

        }

        #endregion
    }
}
