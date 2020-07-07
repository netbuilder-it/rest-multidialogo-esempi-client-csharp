using System;

public class ApiDialogException : Exception
{
    public ApiDialogException()
    {
    }

    public ApiDialogException(string message)
        : base(message)
    {
    }

    public ApiDialogException(string message, Exception inner)
        : base(message, inner)
    {
    }
}