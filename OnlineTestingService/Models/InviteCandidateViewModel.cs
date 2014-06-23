using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineTestingService.Models
{
    public class InviteCandidateViewModel
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public Guid TestId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}