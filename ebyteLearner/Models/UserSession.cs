using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ebyteLearner.Models
{
    public class UserSession
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid SessionId { get; set; }
        public Session Session { get; set; }
    }
}
