using System;

namespace QShirt.Application.Exceptions;

public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message) : base(message) { }

    public AccessDeniedException(string message, Exception innerException) : base(message, innerException) { }

    public int? ErrorCode { get; set; }
}
