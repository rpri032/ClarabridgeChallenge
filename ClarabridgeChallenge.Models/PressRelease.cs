
using System;

namespace ClarabridgeChallenge.Models
{
    public class PressRelease : ModelBase
    {
        public string Title { get; set; }
        public string DescriptionHtml { get; set; }
        public DateTime DatePublished { get; set; }
    }
}
