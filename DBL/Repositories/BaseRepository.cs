using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Repositories
{
    public abstract class BaseRepository
    {
        protected string _connString;

        public BaseRepository(string connectionString)
        {
            _connString = connectionString;
        }

        public string DeleteStatement(string tableName, string idColumn)
        {
            return string.Format("Delete From {0} Where {1} = @Id", tableName, idColumn);
        }

        public string FindStatement(string tableName, string idColumn)
        {
            return string.Format("Select * From {0} Where {1} = @Id", tableName, idColumn);
        }

        public string FindStatement(string tableName, string idColumn, int top)
        {
            return string.Format("Select top {2} * From {0} Where {1} = @Id", tableName, idColumn, top);
        }

        public string GetAllStatement(string tableName)
        {
            return string.Format("Select * From {0}", tableName);
        }

        public string GetAllStatement(string tableName, int top)
        {
            return string.Format("Select top {1} * From {0}", tableName, top);
        }

        //public void ExecuteQuery(string query)
        //{
        //    Connection.Execute(query);
        //}
    }
}
