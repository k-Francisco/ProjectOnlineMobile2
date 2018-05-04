using ProjectOnlineMobile2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public interface ISharepointApi
    {
        Task<UserModel> GetCurrentUser();

        Task<FormDigestModel> GetFormDigest();
    }
}
