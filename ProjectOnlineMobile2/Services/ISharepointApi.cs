using ProjectOnlineMobile2.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public interface ISharepointApi
    {
        [Get("/_api/web/currentUser?")]
        Task<UserModel> GetCurrentUser();

        [Post("/_api/contextinfo")]
        Task<FormDigestModel> GetFormDigest();
    }
}
