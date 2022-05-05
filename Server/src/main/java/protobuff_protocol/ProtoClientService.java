package protobuff_protocol;

import domain.Sale;
import domain.Show;
import domain.User;
import services.MainServerServiceInterface;
import services.ServerObserver;

import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.time.LocalDate;
import java.util.List;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

public class ProtoClientService implements MainServerServiceInterface {
    private String host;
    private int port;

    private ServerObserver client;

    private InputStream input;
    private OutputStream output;
    private Socket connection;

    private BlockingQueue<Proto.Answer> answers;
    private volatile boolean finished;

    public ProtoClientService(String host, int port) {
        this.host = host;
        this.port = port;
        answers = new LinkedBlockingQueue<Proto.Answer>();
    }

    @Override
    public void addSaleToShow(String buyerName, int ticketsBought, int showId) throws Exception {
        sendRequest(ProtoUtils.createAddSaleRequest(new Sale(buyerName,ticketsBought,showId)));
        Proto.Answer answer = readAnswer();
        if (answer.getType()==Proto.Answer.Type.OK) {
            System.out.println("Adding show OK");
            System.out.println("Waiting for list update...");
        }
        else {
            String error = answer.getError();
            throw new Exception(error);
        }
    }

    @Override
    public List<Show> getAllShows() throws Exception {
        sendRequest(ProtoUtils.createFullShowListRequest());
        Proto.Answer answer = readAnswer();
        if (answer.getType()==Proto.Answer.Type.FullShowList) {
            return ProtoUtils.getShowsListFromAnswer(answer);
        }
        else {
            String error = answer.getError();
            throw new Exception(error);
        }
    }

    @Override
    public List<Show> getShowsByDate(LocalDate date) throws Exception {
        sendRequest(ProtoUtils.createFilteredShowListRequest(date));
        Proto.Answer answer = readAnswer();
        if (answer.getType()==Proto.Answer.Type.FilteredShowList) {
            return ProtoUtils.getShowsListFromAnswer(answer);
        }
        else {
            String error = answer.getError();
            throw new Exception(error);
        }
    }

    @Override
    public void login(String username, String password, ServerObserver client) throws Exception {
        initializeConnection();
        sendRequest(ProtoUtils.createLoginRequest(new User(username, password)));
        Proto.Answer answer = readAnswer();
        if (answer.getType()==Proto.Answer.Type.OK) {
            this.client=client;
            return;
        }
        if (answer.getType()==Proto.Answer.Type.Error) {
            String error = answer.getError();
            closeConnection();
            throw new Exception(error);
        }
    }

    @Override
    public void logout(String username, ServerObserver client) throws Exception {
        sendRequest(ProtoUtils.createLogoutRequest(username));
        Proto.Answer answer = readAnswer();
        closeConnection();
        if (answer.getType()==Proto.Answer.Type.Error) {
            String error = answer.getError();
            throw new Exception(error);
        }
    }

    private void sendRequest(Proto.Request request) throws Exception {
        try {
            request.writeDelimitedTo(output);
            output.flush();
            System.out.println("Sent request.");
        }
        catch (Exception e) {
            throw new Exception("Error sending object "+e);
        }
    }

    private void initializeConnection() throws Exception {
        try {
            connection = new Socket(host,port);
            output = connection.getOutputStream();
            input = connection.getInputStream();
            finished=false;
            startReader();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void closeConnection() {
        finished=true;
        try {
            input.close();
            output.close();
            connection.close();
            client=null;
        }
        catch (Exception e){
            e.printStackTrace();
        }
    }

    private Proto.Answer readAnswer() {
        Proto.Answer answer = null;
        try {
            answer = answers.take();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        return answer;
    }

    private void startReader() {
        Thread tw = new Thread(new ReaderThread());
        tw.start();
    }

    private void handleUpdate (Proto.Answer answer) throws Exception {
        if (answer.getType()==Proto.Answer.Type.DynamicFullShowList) {
            client.updateShowList(ProtoUtils.getShowsListFromAnswer(answer));
        }
        if (answer.getType()==Proto.Answer.Type.DynamicFilteredShowList) {
            client.updateFilteredList(ProtoUtils.getShowsListFromAnswer(answer));
        }
    }

    private boolean isUpdate (Proto.Answer answer) {
        return answer.getType()==Proto.Answer.Type.DynamicFullShowList;
    }

    private class ReaderThread implements Runnable {
        public void run() {
            while(!finished) {
                try {
                    Proto.Answer answer = Proto.Answer.parseDelimitedFrom(input);
                    System.out.println("Answer received "+answer);
                    if (isUpdate(answer)) { // IF RECEIVING UNREQUESTED ANSWER FROM SERVER
                        handleUpdate(answer);   // HANDLE IT LMAO
                    }
                    else {  // IF WAITING FOR AN ANSWER FROM THE SERVER
                        try {
                            answers.put(answer);
                        }
                        catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                }
                catch (Exception e) {
                    System.out.println("Error at reader "+e);
                }
            }
        }
    }
}
