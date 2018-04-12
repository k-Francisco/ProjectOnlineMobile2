using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    [Headers("Content-Type: application/json")]
    public interface ISharepointApi
    {
        [Get("/_api/web/currentUser?")]
        Task<String> GetCurrentUser();

        [Post("/_api/contextinfo")]
        Task<String> GetFormDigest();
    }
}
