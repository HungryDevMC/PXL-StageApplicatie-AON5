using System;
using System.Collections.Generic;

namespace StageAPI.Data.Proposal
{
    public class Proposal
    {
        public int ID { get; set; }

        public ProposalForm FormData { get; set; }

        public ProposalState State { get; set; }

        public DateTime DateSubmitted { get; set; }

        // Veranderd adhv status die veranderd van de proposal en dus uiteindelijk de goedkueringsdatum
        public DateTime DateOfState { get; set; }

        // null if state == 0/4 
        // public IDictionary<Account, string> Feedback { get; set;  }

    }
}