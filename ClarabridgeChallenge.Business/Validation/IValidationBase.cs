using System.Collections.Generic;

namespace ClarabridgeChallenge.Business.Validation
{
    public interface IValidationBase
    {
        IList<Error> Validate(Models.ModelBase modelBase, ValidationType validationType);
    }
}
