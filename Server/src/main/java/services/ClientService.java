package services;

import domain.Sale;
import domain.Show;
import domain.User;
import rpc_protocol.Answer;
import rpc_protocol.AnswerType;
import rpc_protocol.Request;
import rpc_protocol.RequestType;

import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.time.LocalDate;
import java.util.List;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

public class ClientService implements MainServerServiceInterface {
    private String host;
    private int port;

    private ServerObserver client;

    private ObjectInputStream input;
    private ObjectOutputStream output;
    private Socket connection;

    private BlockingQueue<Answer> answers;
    private volatile boolean finished;

    public ClientService(String host, int port) {
        this.host = host;
        this.port = port;
        answers = new LinkedBlockingQueue<Answer>();
    }

    @Override
    public void addSaleToShow(String buyerName, int ticketsBought, int showId) throws Exception {
        Request req = new Request.Builder().type(RequestType.ADD_SALE).data(new Sale(buyerName,ticketsBought,showId)).build();
        sendRequest(req);
        Answer answer = readAnswer();
        if (answer.getType()==AnswerType.OK) {
            System.out.println("Adding show OK");
            System.out.println("Waiting for list update...");
        }
        else {
            String error = answer.getData().toString();
            throw new Exception(error);
        }
    }

    @Override
    public List<Show> getAllShows() throws Exception {
        Request req = new Request.Builder().type(RequestType.FULL_SHOW_LIST).data("full").build();
        sendRequest(req);
        Answer answer = readAnswer();
        if (answer.getType()==AnswerType.FULL_SHOW_LIST) {
            return (List<Show>) answer.getData();
        }
        else {
            String error = answer.getData().toString();
            throw new Exception(error);
        }
    }

    @Override
    public List<Show> getShowsByDate(LocalDate date) throws Exception {
        Request req = new Request.Builder().type(RequestType.FILTERED_SHOW_LIST).data(date).build();
        sendRequest(req);
        Answer answer = readAnswer();
        if (answer.getType()==AnswerType.FILTERED_SHOW_LIST) {
            return (List<Show>) answer.getData();
        }
        else {
            String error = answer.getData().toString();
            throw new Exception(error);
        }
    }

    @Override
    public void login(String username, String password, ServerObserver client) throws Exception {
        initializeConnection();
        User user = new User(username,password);
        Request req = new Request.Builder().type(RequestType.LOGIN).data(user).build();
        sendRequest(req);
        Answer answer = readAnswer();
        if (answer.getType()==AnswerType.OK) {
            this.client=client;
            return;
        }
        if (answer.getType()==AnswerType.ERROR) {
            String error = answer.getData().toString();
            closeConnection();
            throw new Exception(error);
        }
    }

    @Override
    public void logout(String username, ServerObserver client) throws Exception {
        Request req = new Request.Builder().type(RequestType.LOGOUT).data(username).build();
        sendRequest(req);
        Answer answer = readAnswer();
        closeConnection();
        if (answer.getType()==AnswerType.ERROR) {
            String error = answer.getData().toString();
            throw new Exception(error);
        }
    }

    private void sendRequest(Request request) throws Exception {
        try {
            output.writeObject(request);
            output.flush();
        }
        catch (Exception e) {
            throw new Exception("Error sending object "+e);
        }
    }

    private void initializeConnection() throws Exception {
        try {
            connection = new Socket(host,port);
            output = new ObjectOutputStream(connection.getOutputStream());
            output.flush();
            input = new ObjectInputStream(connection.getInputStream());
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

    private Answer readAnswer() {
        Answer answer = null;
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

    private void handleUpdate (Answer answer) throws Exception {
        if (answer.getType()==AnswerType.FULL_SHOW_LIST_DYNAMIC_UPDATE) {
            client.updateShowList((List<Show>) answer.getData());
        }
        if (answer.getType()==AnswerType.FILTERED_SHOW_LIST_DYNAMIC_UPDATE) {
            client.updateFilteredList((List<Show>) answer.getData());
        }
    }

    private boolean isUpdate (Answer answer) {
        return answer.getType()==AnswerType.FULL_SHOW_LIST_DYNAMIC_UPDATE;
    }

    private class ReaderThread implements Runnable {
        public void run() {
            while(!finished) {
                try {
                    Object answer = input.readObject();
                    System.out.println("Answer received "+answer);
                    if (isUpdate((Answer)answer)) { // IF RECEIVING UNREQUESTED ANSWER FROM SERVER
                        handleUpdate((Answer)answer);   // HANDLE IT LMAO
                    }
                    else {  // IF WAITING FOR AN ANSWER FROM THE SERVER
                        try {
                            answers.put((Answer)answer);
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
