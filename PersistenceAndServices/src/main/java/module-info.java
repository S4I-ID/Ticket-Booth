module MPP.Proiect.Proxy.PersistenceAndServices.main {

    requires MPP.Proiect.Proxy.CommonDomain.main;

    requires org.apache.logging.log4j;
    requires org.apache.logging.log4j.core;
    requires java.scripting;
    requires java.sql;
    requires de.mkammerer.argon2;

    exports repository;
    exports service_base to MPP.Proiect.Proxy.Server.main;
}