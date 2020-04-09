using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.Core.CustomException
{
    public class TokenExpiredException : Exception
    {
        public TokenExpiredException() { }

        public TokenExpiredException(string message) : base(message)
        {

        }
    }
}
