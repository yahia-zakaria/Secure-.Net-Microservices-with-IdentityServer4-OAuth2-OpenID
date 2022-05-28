using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; set; }
        public ValidationException() : base("One or more validation errors have occured")
        {

        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures.GroupBy(g=>g.PropertyName, g=>g.ErrorMessage)
                .ToDictionary(failures=>failures.Key, failures=>failures.ToArray());
        }
    }
}
