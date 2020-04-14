using System;

namespace Game.Exceptions
{
    /// <summary>
    /// Represents an error where a player did not return a valid choice of action
    /// </summary>
    class InvalidPlayerResponseException : Exception
    {
        public InvalidPlayerResponseException()
        {
        }

        public InvalidPlayerResponseException(string message)
            : base(message)
        {
        }

        public InvalidPlayerResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
