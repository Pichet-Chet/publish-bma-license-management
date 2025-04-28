
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.CustomModel.Model
{
    public class ValidGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                return Guid.TryParse(str, out _);
            }
            return false;
        }
    }
}
