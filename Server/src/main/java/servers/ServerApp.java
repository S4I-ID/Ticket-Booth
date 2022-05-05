package servers;

import repository.SaleDatabaseRepository;
import repository.ShowDatabaseRepository;
import repository.UserDatabaseRepository;
import services.ServerService;
import service_base.*;
import services.MainServerServiceInterface;

import java.io.IOException;
import java.util.Properties;

public class ServerApp {
    private static final int defaultPort = 55555;

    public static void main(String[] args) {
        Properties serverProps = new Properties();
        try {
            serverProps.load(ServerApp.class.getResourceAsStream("server.properties"));
            System.out.println("Server properties set");
            serverProps.list(System.out);
        }
        catch (IOException e) {
            e.printStackTrace();
        }

        // INITIALIZE PERSISTENCE AND BUSINESS LAYER
        SaleDatabaseRepository saleDatabaseRepository = new SaleDatabaseRepository(serverProps);
        ShowDatabaseRepository showDatabaseRepository = new ShowDatabaseRepository(serverProps);
        UserDatabaseRepository userDatabaseRepository = new UserDatabaseRepository(serverProps);
        SaleService saleService = new SaleService(saleDatabaseRepository);
        ShowService showService = new ShowService(showDatabaseRepository);
        UserService userService = new UserService(userDatabaseRepository);
        MainServerServiceInterface serviceController = new ServerService(saleService,showService,userService);

        // SET UP SERVER PORT
        int serverPort = defaultPort;
        try {
            serverPort=Integer.parseInt(serverProps.getProperty("server.port"));
        }
        catch (NumberFormatException e) {
            System.err.println("Wrong port number " + serverPort);
            System.err.println("Using default port " + defaultPort);
        }

        System.out.println("Setting server on port " + serverPort);
        AbstractServer server = new ServerRpcConcurrent(serverPort, serviceController);
        try {
            server.start();
        }
        catch (Exception e) {
            System.err.println("Error starting the server" + e.getMessage());
        }
        finally {
            try {
                server.stop();
            }
            catch (Exception e) {
                System.err.println("Error stopping server" + e.getMessage());
            }
        }
    }
}
