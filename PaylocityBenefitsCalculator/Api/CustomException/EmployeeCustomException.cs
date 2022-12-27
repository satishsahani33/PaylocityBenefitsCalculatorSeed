using System;
namespace Api.CustomException
{
    [Serializable]
    public class EmployeeCustomException : Exception
    {
        public EmployeeCustomException()
        {
        }

        public EmployeeCustomException(string message)
            : base(message)
        {
        }

        public EmployeeCustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}