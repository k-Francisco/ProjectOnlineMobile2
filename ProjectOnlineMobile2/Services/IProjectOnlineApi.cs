using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    [Headers("Content-Type: application/json")]
    public interface IProjectOnlineApi
    {
        Task<String> GetFormDigest();
    }
}
