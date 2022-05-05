module MPP.Proiect.Proxy.CommonDomain.main {
    exports domain to MPP.Proiect.Proxy.Server.main, MPP.Proiect.Proxy.PersistenceAndServices.main, MPP.Proiect.Proxy.Client.main;
    opens domain to javafx.base;
}