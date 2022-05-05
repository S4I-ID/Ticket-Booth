package repository;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import domain.User;
import repository.utils.JdbcUtils;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.List;
import java.util.Properties;

public class UserDatabaseRepository implements UserRepository{
    private JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public UserDatabaseRepository (Properties props) {
        logger.info("Initialized UserDatabaseRepository with properties: {}",props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public String getPasswordOfUser (String username) throws Exception {
        logger.traceEntry();
        Connection con = dbUtils.getConnection();
        try {
            PreparedStatement statement = con.prepareStatement
                    ("SELECT password FROM Users WHERE username = ?");
            statement.setString(1,username);
            ResultSet result = statement.executeQuery();
            result.next();

            String hashedPassword = result.getString("password");
            result.close();
            logger.traceExit(hashedPassword);
            return hashedPassword;
        }
        catch (SQLException ex) {
            logger.error(ex);
            throw new Exception("User not found!\n");
        }
    }

    @Override
    public User find(Integer integer) throws Exception {
        return null;
    }

    @Override
    public List<User> getAll() throws Exception {
        return null;
    }

    @Override
    public void add(User entity) throws Exception {

    }

    @Override
    public User delete(Integer integer) throws Exception {
        return null;
    }

    @Override
    public User update(User entity) throws Exception {
        return null;
    }

    @Override
    public Integer size() throws Exception {
        return null;
    }

}
