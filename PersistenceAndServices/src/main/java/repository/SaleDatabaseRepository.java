package repository;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import domain.Sale;
import repository.utils.JdbcUtils;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

public class SaleDatabaseRepository implements SaleRepository {
    private JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public SaleDatabaseRepository(Properties props) {
        logger.info("Initialized SaleDatabaseRepository with properties: {}",props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public Sale find(Integer id) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT id,buyer_name,tickets_bought,show_id FROM Sales WHERE id = ?");
            statement.setInt(1,id);
             ResultSet result = statement.executeQuery();
             result.next();
             int found_id = result.getInt("id");
             String buyerName = result.getString("buyer_name");
             int ticketsBought = result.getInt("tickets_bought");
             int show_id = result.getInt("show_id");
             Sale sale = new Sale(buyerName,ticketsBought,show_id);
             sale.setID(found_id);
             result.close();
             logger.traceExit(sale);
             return sale;
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("No such ID exists!");
        }
    }

    @Override
    public List<Sale> getAll() throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        List<Sale> sales = new ArrayList<>();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT id,buyer_name,tickets_bought,show_id FROM Sales");
            ResultSet result = statement.executeQuery();
            while (result.next()) {
                int id = result.getInt("id");
                String buyerName = result.getString("buyer_name");
                int ticketsBought = result.getInt("tickets_bought");
                int show_id = result.getInt("show_id");
                Sale sale = new Sale(buyerName, ticketsBought, show_id);
                sale.setID(id);
                sales.add(sale);
            }
            result.close();
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("Error getting all sales!");
        }
        logger.traceExit(sales);
        return sales;
    }

    @Override
    public void add(Sale sale) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("INSERT INTO Sales(buyer_name,tickets_bought,show_id) VALUES(?,?,?)");
            statement.setString(1,sale.getBuyerName());
            statement.setInt(2,sale.getTicketsBought());
            statement.setInt(3,sale.getShowID());
            int result = statement.executeUpdate();
            logger.trace("Saved {} instances",result);
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("Adding sale failed!");
        }
    }

    @Override
    public Sale delete(Integer id) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("DELETE FROM Sales WHERE id = ?");
            statement.setInt(1,id);
            Sale deleted = find(id);
            int result = statement.executeUpdate();
            logger.trace("Deleted {} instances",result);
            logger.traceExit(deleted);
            return deleted;
        }
        catch (Exception ex) {
            logger.error(ex);
            throw new Exception("No such ID exists!");
        }
    }

    @Override
    public Sale update(Sale sale) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("UPDATE Sales SET buyer_name=?, tickets_bought=?,show_id=? WHERE id=?");
            statement.setString(1,sale.getBuyerName());
            statement.setInt(2,sale.getTicketsBought());
            statement.setInt(3,sale.getShowID());
            statement.setInt(4,sale.getID());
            Sale updated = find(sale.getID());
            int result = statement.executeUpdate();
            logger.trace("Updated {} instances",result);
            logger.traceExit(updated);
            return updated;
        }
        catch (Exception ex) {
            logger.error(ex);
            throw new Exception("Updating sale failed!");
        }
    }

    @Override
    public Integer size() throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT COUNT(*) AS count FROM Sales");
            ResultSet result = statement.executeQuery();
            result.next();
            int size = result.getInt("count");
            result.close();
            logger.traceExit(size);
            return size;
        }
        catch (Exception ex) {
            logger.error(ex);
            throw new Exception("Error in database!");
        }
    }

    @Override
    public List<Sale> findSalesForShow(Integer showID) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        List<Sale> sales = new ArrayList<>();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT id,buyer_name,tickets_bought,show_id FROM Sales WHERE show_id=?");
            statement.setInt(1,showID);
            ResultSet result = statement.executeQuery();
            while (result.next()) {
                int id = result.getInt("id");
                String buyerName = result.getString("buyer_name");
                int ticketsBought = result.getInt("tickets_bought");
                int show_id = result.getInt("show_id");
                Sale sale = new Sale(buyerName, ticketsBought, show_id);
                sale.setID(id);
                sales.add(sale);
            }
            result.close();
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("Error getting all sales!");
        }
        logger.traceExit(sales);
        return sales;
    }
}
