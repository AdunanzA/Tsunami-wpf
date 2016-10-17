using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class ErrorCode
    {
        public string Message { get; set; }
        public int Value { get; set; }

        public ErrorCode() { /* nothing to do. just for serializator */ }

        public ErrorCode(Core.ErrorCode er)
        {
            Message = er.message();
            Value = er.value();
        }

    }
}
