using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.UOW
{
    public class UnitOfWork:IUnitOfWork
    {
        public string connString;
        private bool _disposed;

        private ISmsRepository smsRepository;
        private ISecurityRepository securityRepository;

        public ISmsRepository SmsRepository
        {
            get { return smsRepository ?? (smsRepository = new SmsRepository(connString)); }
        }

        public ISecurityRepository SecurityRepository
        {
            get { return securityRepository ?? (securityRepository = new SecurityRepository(connString)); }
        }

        public UnitOfWork(string dbConnString)
        {
            connString = dbConnString;
        }

        public void Reset()
        {
            smsRepository = null;
            securityRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
