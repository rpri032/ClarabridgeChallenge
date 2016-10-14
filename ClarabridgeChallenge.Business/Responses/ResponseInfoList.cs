using System.Collections.Generic;
using ClarabridgeChallenge.Models;

namespace ClarabridgeChallenge.Business.Responses
{
    public class ResponseInfoList : ResponseInfoBase
    {
        public IList<ModelBase> Items { get; set; } 
    }
}
