using Persistence.Repository.Utils;
using CommonDomain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class UserDatabaseRepository : UserRepository
    {
        private DbUtils dbUtils;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserDatabaseRepository( string url )
        {
            dbUtils = new DbUtils(url);
        }
        public string GetPasswordOfUser(string username)
        {
            log.Debug("acquiring password of " + username);
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT password FROM Users WHERE username=@1";
                command.Parameters.Add(new SQLiteParameter("@1", username));
                SQLiteDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                string password = dataReader.GetString(0);
                dataReader.Close();
                log.Debug("FOUND " + password);
                return password;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Invalid login!");
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

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
