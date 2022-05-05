using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using log4net;

namespace Persistence.Repository.Utils
{
    public class DbUtils
    {
        private static string connectionString1 = "Data Source = ";
        private static string connectionString2 = "; Version = 3; New = True; Compress = True;";
        private static string connectionString3 = ";";
        private string url;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DbUtils(string url) { this.url = url; }

        private SQLiteConnection instance = null;

        private SQLiteConnection GetNewConnection()
        {
            SQLiteConnection con;
            con = new SQLiteConnection(connectionString1 + url + connectionString2);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return con;
        }

        public SQLiteConnection GetConnection()
        {
            log.Info("Getting connection...");
            try
            {
                if (instance == null)
                    instance = GetNewConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                log.Error(ex);
            }
            log.Info(instance);
            return instance;
        }

        public string GetConnectionStringFull()
        {
            return connectionString1 + url + connectionString3;
        }
    }
}