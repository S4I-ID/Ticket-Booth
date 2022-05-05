package proxy.client;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.stage.Stage;
import proxy.client.ui.LoginController;
import services.ClientService;
import services.MainServerServiceInterface;

import java.util.Properties;

public class ShowApp extends Application {
    private Stage stage;

    private String serverIP;
    private int serverPort;

    private static int defaultPort=55555;
    private static String defaultServer="localhost";

    private void initializeProperties() {
        System.out.println("Starting client...");
        Properties clientProps = new Properties();
        try {
            clientProps.load(ShowApp.class.getResourceAsStream("client.properties"));
            System.out.println("Client properties set");
            clientProps.list(System.out);
        }
        catch (Exception e) {
            System.err.println("Cannot find client properties "+e);
            return;
        }
        serverIP = clientProps.getProperty("server.host",defaultServer);
        serverPort = defaultPort;
        try {
            serverPort=Integer.parseInt(clientProps.getProperty("server.port"));
        }
        catch (Exception e) {
            System.err.println("Wrong port number "+e.getMessage());
            System.out.println("Using default port: "+defaultPort);
        }
    }

    @Override
    public void start(Stage primaryStage) throws Exception { // JAVA SERVER
        initializeProperties();
        MainServerServiceInterface service = new ClientService(serverIP,serverPort);
        LoginController loginController = new LoginController(service);
        FXMLLoader fxmlLoader = new FXMLLoader(ShowApp.class.getResource("login.fxml"));
        fxmlLoader.setController(loginController);
        Scene loginScene = new Scene(fxmlLoader.load());
        stage = new Stage();
        stage.setTitle("Ticket sale application");
        stage.setScene(loginScene);
        stage.show();
    }
}