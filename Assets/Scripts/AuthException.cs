// ILSpyBased#2
using UnityEngine;

public class AuthException : UnityException
{
    public const short INCORRECT_AUTH = 1;

    public AuthException()
    {
    }

    public AuthException(string message)
        : base(message)
    {
    }

    public AuthException(string message, UnityException inner)
        : base(message, inner)
    {
    }
}


