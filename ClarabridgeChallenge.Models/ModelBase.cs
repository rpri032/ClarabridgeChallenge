using System;

namespace ClarabridgeChallenge.Models
{
    public class ModelBase
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
