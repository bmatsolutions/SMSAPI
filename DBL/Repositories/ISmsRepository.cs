using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface ISmsRepository
    {
        Task<string> GetTransactionRef();
        Task<DataTable> GetInfo(string sql);
        Task<string> UpdateLog(string sql);
    }
}
