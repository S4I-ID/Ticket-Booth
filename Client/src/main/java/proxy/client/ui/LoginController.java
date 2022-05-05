package proxy.client.ui;

import javafx.application.Platform;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import javafx.stage.WindowEvent;
import proxy.client.ShowApp;
import services.MainServerServiceInterface;

public class LoginController {
    @FXML
    private Button loginButton;
    @FXML
    private TextField usernameTextField;
    @FXML
    private PasswordField passwordTextField;

    private MainServerServiceInterface serviceController;
    public LoginController(MainServerServiceInterface serviceController) {
        this.serviceController = serviceController;
    }

    @FXML
    protected void loginButtonClick(ActionEvent event) {
        try {
            String username = usernameTextField.getText();
            String password = passwordTextField.getText();

            // CREATE NEW MAIN WINDOW
            MainPageController mainController = new MainPageController(serviceController);
            serviceController.login(username,password,mainController);

            FXMLLoader fxmlLoader = new FXMLLoader(ShowApp.class.getResource("mainpage.fxml"));
            mainController.setUsername(username);
            fxmlLoader.setController(mainController);
            Scene mainScene = new Scene(fxmlLoader.load());
            Stage mainStage = new Stage();
            mainStage.setOnCloseRequest(new EventHandler<WindowEvent>() {
                    @Override   // IF WINDOW IS MANUALLY CLOSED, LOGOUT THE CLIENT, THANKS CLIENT FOR BREAKING MY SERVER
                    public void handle(WindowEvent event) {
                        try {
                            serviceController.logout(mainController.username,mainController);
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                        System.exit(0);
                    }
                });
            mainStage.setTitle("Ticket application");
            mainStage.setScene(mainScene);
            mainStage.show();   // SHOW NEW WINDOW
            ((Node) (event.getSource())).getScene().getWindow().hide(); // HIDE CURRENT LOGIN WINDOW
        }
        catch (Exception e) {
            WarningBoxUI.show(e.getMessage());
        }
    }
}