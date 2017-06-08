using System;
namespace Tickets.ViewModels
{
    public class MainViewModel
    {
        #region
        public LoginViewModel Login
        {
            get;
            set;
        }

        public CheckTicketViewModel CheckTicket
        {
            get;
            set;
        }
        #endregion

        public MainViewModel()
        {
            instance = this;
            Login = new LoginViewModel();
            //CheckTicket = new CheckTicketViewModel();
        }

		#region Singleton
        static MainViewModel instance;

        public static MainViewModel GetInstance()
		{
			if (instance == null)
			{
                instance = new MainViewModel();
			}

			return instance;
		}
		#endregion
	}
}
