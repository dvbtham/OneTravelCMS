using System.ComponentModel.DataAnnotations;

namespace OneTravelApi.Core
{
    public class ForeignRequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!int.TryParse(value.ToString(), out var getal)) return false;

            if (getal == 0)
                return false;

            return getal > 0;
        }
    }
}
