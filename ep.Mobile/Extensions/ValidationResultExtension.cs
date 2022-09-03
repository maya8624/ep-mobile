using FluentValidation.Results;
using System.Text;

namespace ep.Mobile.Extensions
{
    public static class ValidationResultExtension
    {
        public static string GetErrorMesages(this ValidationResult result)
        {
            var errors = new StringBuilder();
            foreach (var error in result.Errors)
            {
                errors.Append($"{error}\n");
            };
            return errors.ToString();
        }
    }
}
