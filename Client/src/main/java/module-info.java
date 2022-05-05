module MPP.Proiect.Proxy.Client.main {
    requires javafx.controls;
    requires javafx.fxml;
    requires MPP.Proiect.Proxy.Server.main;
    requires MPP.Proiect.Proxy.CommonDomain.main;

    opens proxy.client.ui to javafx.fxml;
    exports proxy.client;
}