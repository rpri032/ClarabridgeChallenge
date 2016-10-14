using System.Collections.Generic;

namespace ClarabridgeChallenge.Business.Responses
{
    public class ResponseInfoBase
    {
        public ResponseStatus Status { get; set; }
        public IList<Error> Errors { get; set; }
    }
}
