using System;

namespace QShirt.Application.Exceptions;

public class SecurityWarningException : Exception
{
    public SecurityWarningException(string message) : base(message) { }

    public SecurityWarningException(string message, Exception innerException) : base(message, innerException) { }

    public int? ErrorCode { get; set; }
}
