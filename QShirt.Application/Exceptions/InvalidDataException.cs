using System;

namespace QShirt.Application.Exceptions;

public class InvalidDataException : Exception
{
    public InvalidDataException(string message) : base(message)
    {
    }

    public InvalidDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}