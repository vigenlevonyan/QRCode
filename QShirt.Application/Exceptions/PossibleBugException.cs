using System;

namespace QShirt.Application.Exceptions;

public class PossibleBugException : Exception
{
    public PossibleBugException(string message) : base(message) { }

    public PossibleBugException(string message, Exception innerException) : base(message, innerException) { }
}