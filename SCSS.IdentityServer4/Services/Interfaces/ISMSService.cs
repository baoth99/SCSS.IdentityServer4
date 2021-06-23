using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Services.Interfaces
{
    public interface ISMSService
    {
        Task SendSMS(string phone, string message);
    }
}
