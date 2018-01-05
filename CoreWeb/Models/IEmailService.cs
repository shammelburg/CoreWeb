using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb.Models
{
    public interface IEmailService
    {
        void Send(string Subject, string Body);
    }
}
