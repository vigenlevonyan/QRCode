using System;

namespace QShirt.Application.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }

    public BusinessException(string message, Exception innerException) : base(message, innerException) { }

    public int? ErrorCode { get; set; }
}
