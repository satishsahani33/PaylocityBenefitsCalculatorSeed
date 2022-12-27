using System;
namespace Api.CustomException
{
    [Serializable]
    public class DependentCustomException : Exception
    {
        public DependentCustomException()
        {
        }

        public DependentCustomException(string message)
            : base(message)
        {
        }

        public DependentCustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}