using DBL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface ISecurityRepository
    {
        Task<GenericModel> AuthorizeApp(string appid, string appkey);
        Task<AppResponse> CreateApp(App model);
    }
}
