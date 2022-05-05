package repository;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import domain.Show;
import repository.utils.JdbcUtils;

import java.sql.*;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

public class ShowDatabaseRepository implements ShowRepository{
    private JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public ShowDatabaseRepository (Properties props) {
        logger.info("Initialized ShowDatabaseRepository with properties: {}",props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public Show find(Integer id) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT * FROM Shows WHERE id = ?");
            statement.setInt(1,id);
            ResultSet result = statement.executeQuery();
            result.next();
            int found_id = result.getInt("id");
            String artistName = result.getString("artist_name");
            String showName   = result.getString("show_name");
            String address    = result.getString("address");
            LocalDateTime date = LocalDateTime.parse(result.getString("start_time"));
            int availableSeats = result.getInt("available_seats");
            int soldSeats = result.getInt("sold_seats");

            Show show = new Show(artistName,showName,address,date,availableSeats,soldSeats);
            show.setID(found_id);
            result.close();
            logger.traceExit(show.getID()+show.getSoldSeats());
            return show;
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("No such ID exists!");
        }
    }

    @Override
    public List<Show> getAll() throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        List<Show> shows = new ArrayList<>();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT * FROM Shows");
            ResultSet result = statement.executeQuery();
            while (result.next()) {
                int found_id = result.getInt("id");
                String artistName = result.getString("artist_name");
                String showName   = result.getString("show_name");
                String address    = result.getString("address");
                LocalDateTime date = LocalDateTime.parse(result.getString("start_time"));
                int availableSeats = result.getInt("available_seats");
                int soldSeats = result.getInt("sold_seats");

                Show show = new Show(artistName,showName,address,date,availableSeats,soldSeats);
                show.setID(found_id);
                shows.add(show);
            }
            result.close();
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("Error getting all sales!");
        }
        logger.traceExit(shows.size());
        return shows;
    }

    @Override
    public void add(Show show) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("INSERT INTO Shows(artist_name,show_name,address,start_time,available_seats,sold_seats) VALUES(?,?,?,?,?,?)");
            statement.setString(1,show.getArtistName());
            statement.setString(2,show.getShowName());
            statement.setString(3,show.getAddress());
            statement.setString(4, show.getStartTime().toString());
            statement.setInt(5,show.getAvailableSeats());
            statement.setInt(6,show.getSoldSeats());
            int result = statement.executeUpdate();
            logger.trace("Saved {} instance: {}",result,show.getShowName());
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("Adding show failed!");
        }
    }

    @Override
    public Show delete(Integer id) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("DELETE FROM Shows WHERE id = ?");
            statement.setInt(1,id);
            Show deleted = find(id);
            int result = statement.executeUpdate();
            logger.trace("Deleted {} instance",result);
            logger.traceExit(deleted);
            return deleted;
        }
        catch (Exception ex) {
            logger.error(ex);
            throw new Exception("No such ID exists!");
        }
    }

    @Override
    public Show update(Show show) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("UPDATE Shows SET artist_name=?, show_name=?,address=?,start_time=?,available_seats=?,sold_seats=? WHERE id=?");
            statement.setString(1,show.getArtistName());
            statement.setString(2,show.getShowName());
            statement.setString(3,show.getAddress());
            statement.setString(4, show.getStartTime().toString());
            statement.setInt(5,show.getAvailableSeats());
            statement.setInt(6,show.getSoldSeats());
            statement.setInt(7,show.getID());
            Show updated = find(show.getID());
            int result = statement.executeUpdate();
            logger.trace("Updated {} instances: {} seats sold",result,show.getSoldSeats());
            logger.traceExit(updated);
            return updated;
        }
        catch (Exception ex) {
            logger.error(ex);
            throw new Exception("Updating show failed!");
        }
    }

    @Override
    public Integer size() throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT COUNT(*) AS count FROM Shows");
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
}
