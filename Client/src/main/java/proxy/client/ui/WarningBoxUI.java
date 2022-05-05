package proxy.client.ui;

import javafx.scene.control.Alert;

public class WarningBoxUI { // WARNING BOX FOR UI, CAN BE MOVED WITHIN THE UI PACKAGE
    public static void show(String message) {
        Alert alert = new Alert(Alert.AlertType.ERROR);
        alert.setTitle("Error");
        alert.setHeaderText("");
        alert.setContentText(message);
        alert.showAndWait();
    }
}
