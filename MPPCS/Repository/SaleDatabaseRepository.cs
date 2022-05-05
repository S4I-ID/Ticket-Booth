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
    public class SaleDatabaseRepository : SaleRepository
    {
        private DbUtils dbUtils;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SaleDatabaseRepository(string url)
        {
            dbUtils = new DbUtils(url);
        }

        public void Add(Sale sale)
        {
            log.Debug("adding " + sale.GetName());
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "INSERT INTO Sales(buyer_name,tickets_bought,show_id) VALUES(@1,@2,@3)";
                command.Parameters.Add(new SQLiteParameter("@1", sale.GetName()));
                command.Parameters.Add(new SQLiteParameter("@2", sale.GetTicketsBought()));
                command.Parameters.Add(new SQLiteParameter("@3", sale.GetShowId()));

                int result = command.ExecuteNonQuery();
                log.Debug("SAVED: " + result);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public Sale Delete(int id)
        {
            log.Debug("deleting" + id);
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "DELETE FROM Sales WHERE id = @1";
                command.Parameters.Add(new SQLiteParameter("@1", id));
                Sale deleted = Find(id);
                int result = command.ExecuteNonQuery();
                log.Debug("DELETED: " + result);
                return deleted;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public Sale Find(int id)
        {
            log.Debug("finding " + id);
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT id,buyer_name,tickets_bought,show_id FROM Sales WHERE id = @1";
                command.Parameters.Add(new SQLiteParameter("@1", id));
                SQLiteDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                Sale sale = new Sale(
                    dataReader.GetString(1),
                    dataReader.GetInt32(2),
                    dataReader.GetInt32(3));
                sale.SetId(dataReader.GetInt32(0));
                dataReader.Close();
                log.Debug("FOUND " + sale.GetId());
                return sale;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public List<Sale> GetAll()
        {
            log.Debug("GetAll");
            List<Sale> sales = new List<Sale>();
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT * FROM Sales";
                SQLiteDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Sale sale = new Sale(
                        dataReader.GetString(1),
                        dataReader.GetInt32(2),
                        dataReader.GetInt32(3));
                    sale.SetId(dataReader.GetInt32(0));
                    sales.Add(sale);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
            log.Debug("GOT ALL " + sales.Count());
            return sales;
        }

        public int Size()
        {
            log.Debug("size");
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT COUNT(*) AS count FROM Sales";
                SQLiteDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                int size = dataReader.GetInt32(0);
                dataReader.Close();
                log.Debug("SIZE IS " + size);
                return size;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public Sale Update(Sale sale)
        {
            log.Debug("updating " + sale.GetId());
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "UPDATE Sales SET buyer_name=@1, tickets_bought=@2,show_id=@3 WHERE id=@4";
                command.Parameters.Add(new SQLiteParameter("@1", sale.GetName()));
                command.Parameters.Add(new SQLiteParameter("@2", sale.GetTicketsBought()));
                command.Parameters.Add(new SQLiteParameter("@3", sale.GetShowId()));
                command.Parameters.Add(new SQLiteParameter("@4", sale.GetId()));
                Sale updated = Find(sale.GetId());
                int result = command.ExecuteNonQuery();
                log.Debug("UPDATED " + result);
                return updated;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException();
            }
        }

        public List<Sale> FindSalesForShow(int id)
        {
            log.Debug("finding sales for show " + id);
            List<Sale> sales = new List<Sale>();
            SQLiteConnection con = dbUtils.GetConnection();
            try
            {
                SQLiteCommand command = con.CreateCommand();
                command.CommandText = "SELECT id,buyer_name,tickets_bought,show_id FROM Sales WHERE show_id=@1";
                command.Parameters.Add(new SQLiteParameter("@1", id));
                SQLiteDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Sale sale = new Sale(
                        dataReader.GetString(1),
                        dataReader.GetInt32(2),
                        dataReader.GetInt32(3));
                    sale.SetId(dataReader.GetInt32(0));
                    sales.Add(sale);
                }
                return sales;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}