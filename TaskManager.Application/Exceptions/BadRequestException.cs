

using FluentValidation.Results;

namespace TaskManager.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, ValidationResult validationResult) : base(message)
    {
        ValidationErrors = validationResult.Errors
            .Select(x => x.ErrorMessage)
            .ToList();
    }

    public  List<string> ValidationErrors { get; set; }

}
