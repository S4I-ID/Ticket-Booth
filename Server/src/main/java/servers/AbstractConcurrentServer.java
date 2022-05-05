package servers;

import java.net.Socket;

public abstract class AbstractConcurrentServer extends AbstractServer {
    public AbstractConcurrentServer(int port ){
        super(port);
    }

    protected void processRequest(Socket client) {
        Thread tw = createThread(client);
        tw.start();
    }

    protected abstract Thread createThread(Socket client);
}
