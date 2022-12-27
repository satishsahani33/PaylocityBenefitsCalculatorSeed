using System;
namespace Api.CustomException
{
    [Serializable]
    public class PayCheckCustomException : Exception
    {
        public PayCheckCustomException()
        {
        }

        public PayCheckCustomException(string message)
            : base(message)
        {
        }

        public PayCheckCustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}