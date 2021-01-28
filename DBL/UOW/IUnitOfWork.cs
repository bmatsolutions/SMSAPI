using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        ISmsRepository SmsRepository { get; }
        ISecurityRepository SecurityRepository { get; }
    }
}
