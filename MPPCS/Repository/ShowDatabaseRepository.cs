using CommonDomain;
using Persistence.Repository.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ShowDatabaseRepository : ShowRepository
    {
        private DbUtils dbUtils;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ShowDatabaseRepository(string url)
        {
            dbUtils = new DbUtils(url);
        }

        public void Add(Show show)
        {
            log.Debug("saving: "+show.GetShowName());
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "INSERT INTO Shows(artist_name,show_name,address,start_time,available_seats,sold_seats) VALUES(@1,@2,@3,@4,@5,@6)";
                command.Parameters.Add(new SQLiteParameter("@1", show.GetArtistName()));
                command.Parameters.Add(new SQLiteParameter("@2", show.GetShowName()));
                command.Parameters.Add(new SQLiteParameter("@3", show.GetAddress()));
                command.Parameters.Add(new SQLiteParameter("@4", show.GetStartTime()));
                command.Parameters.Add(new SQLiteParameter("@5", show.GetAvailableSeats()));
                command.Parameters.Add(new SQLiteParameter("@6",show.GetSoldSeats()));
                int result = command.ExecuteNonQuery();
                log.Debug("SAVED: " + result);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public Show Delete(int id)
        {
            log.Debug("deleting: "+id);
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "DELETE FROM Shows WHERE id = @1";
                command.Parameters.Add(new SQLiteParameter("@1", id));
                Show deleted = Find(id);
                int result = command.ExecuteNonQuery();
                log.Debug("DELETED " + result);
                return deleted;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public Show Find(int id)
        {
            log.Debug("finding: "+id);
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT * FROM Shows WHERE id = @1";
                command.Parameters.Add(new SQLiteParameter("@1", id));
                SQLiteDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                Show show = new Show(
                    dataReader.GetString(1),
                    dataReader.GetString(2),
                    dataReader.GetString(3),
                    DateTime.Parse(dataReader.GetString(4)),
                    dataReader.GetInt32(5),
                    dataReader.GetInt32(6));
                show.SetId(dataReader.GetInt32(0));
                dataReader.Close();
                log.Debug("FOUND " + show.GetId());
                return show;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public List<Show> GetAll()
        {
            log.Debug("GetAll");
            List<Show> shows = new List<Show>();
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT * FROM Shows";
                SQLiteDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Show show = new Show(
                    dataReader.GetString(1),
                    dataReader.GetString(2),
                    dataReader.GetString(3),
                    DateTime.Parse(dataReader.GetString(4)),
                    dataReader.GetInt32(5),
                    dataReader.GetInt32(6));
                    show.SetId(dataReader.GetInt32(0));
                    shows.Add(show);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
            log.Debug("GOT ALL "+shows.Count());
            return shows;
        }

        public int Size()
        {
            log.Debug("Size...");
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT COUNT(*) AS count FROM Shows";
                SQLiteDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                int size = dataReader.GetInt32(0);
                dataReader.Close();
                log.Debug("SIZE IS "+size);
                return size;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public Show Update(Show show)
        {
            log.Debug("updating: " +show.GetId());
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "UPDATE Shows SET artist_name=@1, show_name=@2,address=@3,start_time=@4,available_seats=@5,sold_seats=@6 WHERE id=@7";
                command.Parameters.Add(new SQLiteParameter("@1", show.GetArtistName()));
                command.Parameters.Add(new SQLiteParameter("@2", show.GetShowName()));
                command.Parameters.Add(new SQLiteParameter("@3", show.GetAddress()));
                command.Parameters.Add(new SQLiteParameter("@4", show.GetStartTime()));
                command.Parameters.Add(new SQLiteParameter("@5", show.GetAvailableSeats()));
                command.Parameters.Add(new SQLiteParameter("@6", show.GetSoldSeats()));
                command.Parameters.Add(new SQLiteParameter("@7", show.GetId()));
                Show updated = Find(show.GetId());
                int result = command.ExecuteNonQuery();
                log.Debug("UPDATED: " + result);
                return updated;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }
    }
}