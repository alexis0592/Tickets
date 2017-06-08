using System;
namespace Tickets.Models
{
    public class Ticket
    {
        #region Properties
        public string TicketCode
        {
            get;
            set;
        }

        public int TicketId
        {
            get;
            set;
        }

        public DateTime DateTime
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }
        #endregion

        public Ticket()
        {
        }
    }
}
