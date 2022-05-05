module MPP.Proiect.Proxy.Server.main {

    requires MPP.Proiect.Proxy.PersistenceAndServices.main;
    requires MPP.Proiect.Proxy.CommonDomain.main;
    requires com.google.protobuf;

    exports rpc_protocol;
    exports servers;
    exports services;
    exports protobuff_protocol;
}