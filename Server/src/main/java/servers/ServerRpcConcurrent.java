package servers;

import rpc_protocol.ClientRpc;
import services.MainServerServiceInterface;

import java.net.Socket;

public class ServerRpcConcurrent extends AbstractConcurrentServer {
    private MainServerServiceInterface serviceController;

    public ServerRpcConcurrent(int port, MainServerServiceInterface service) {
        super(port);
        this.serviceController=service;
        System.out.println("OPENING RPC PROTOCOL CONCURRENT SERVER...");
    }

    @Override
    protected Thread createThread(Socket client) {
        ClientRpc worker = new ClientRpc(serviceController, client);
        Thread tw = new Thread(worker);
        return tw;
    }

    @Override
    public void stop() {
        System.out.println("Stopping services...");
    }
}
