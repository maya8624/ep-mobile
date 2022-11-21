using ep.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Validations
{
    public class ValidatableObject<T> : ViewModelBase, IsValidity
    {
        private readonly List<IValidationRule<T>> _validations;
        private List<string> _errors;
        private bool _isValid;
        private T _value;

        public List<IValidationRule<T>> Validations => _validations;

        public ValidatableObject()
        {
            _errors = new List<string>();
            _isValid = true;
            _validations = new List<IValidationRule<T>>();
        }

        public List<string> Errors  
        {
            get => _errors;
            set
            {
                _errors = value;
                RaisePropertyChanged(() => Errors);
            }
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = _validations
                .Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList() ?? Enumerable.Empty<string>().ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
    }
}
