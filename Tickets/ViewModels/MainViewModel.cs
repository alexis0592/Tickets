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
        #endregion

        public MainViewModel()
        {
            Login = new LoginViewModel();
        }
    }
}
