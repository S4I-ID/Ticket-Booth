using CommonDomain;
using Persistence.Repository;
using Persistence.Repository.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace Persistence.Repository
{
    public class UserORMRepository : UserRepository
    {
        DbUtils dbUtils;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserORMRepository( string url )
        {
            dbUtils = new DbUtils( url );
        }

        public string GetPasswordOfUser(string username)
        {
            log.Debug("acquiring password of " + username);
            try
            {
                var con = dbUtils.GetConnection();
                string sql = "SELECT password FROM Users WHERE username=@id";
                string password = con.Query<string>(sql, new { id = new[] { username } }).First();
                log.Debug("FOUND "+password);
                return password;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public User Delete(int id)
        {
            throw new NotImplementedException();
        }

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }
        public User Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Size()
        {
            throw new NotImplementedException();
        }

    }
}
