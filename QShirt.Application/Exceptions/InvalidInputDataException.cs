using System;

namespace QShirt.Application.Exceptions;

public class InvalidInputDataException : PossibleBugException
{
    public InvalidInputDataException(string message) : base(message)
    {
    }

    public InvalidInputDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}