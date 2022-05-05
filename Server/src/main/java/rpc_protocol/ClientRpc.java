package rpc_protocol;

import services.MainServerServiceInterface;
import services.ServerObserver;
import domain.*;

import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.lang.reflect.Method;
import java.net.Socket;
import java.net.SocketException;
import java.time.LocalDate;
import java.util.List;

public class ClientRpc implements Runnable, ServerObserver {
    private MainServerServiceInterface service;
    private Socket connection;

    private ObjectInputStream input;
    private ObjectOutputStream output;
    private volatile boolean connected;

    private final Answer answer_ok = new Answer.Builder().type(AnswerType.OK).data("OK").build();

    public ClientRpc (MainServerServiceInterface service, Socket connection) {
        this.service=service;
        this.connection=connection;
        try {
            output = new ObjectOutputStream(connection.getOutputStream());
            output.flush();
            input = new ObjectInputStream(connection.getInputStream());
            connected=true;
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public void run() {
        while(connected) {
            try {
                Object request = input.readObject();
                Answer answer = handleRequest( (Request)request );
                if (answer !=null)
                    sendResponse(answer);
            }
            catch (SocketException e) {
                connected=false;
            }
            catch (Exception e) {
                e.printStackTrace();
            }
            try {
                Thread.sleep(2000);
            }
            catch (Exception e) {
                e.printStackTrace();
            }
        }
        try {
            input.close();
            output.close();
            connection.close();
            System.out.println("Logged user out");

        }
        catch (Exception e) {
            System.out.println("Error "+e);
        }
    }

    @Override
    public void updateShowList(List<Show> shows) throws Exception {
        Answer answer = new Answer.Builder().type(AnswerType.FULL_SHOW_LIST_DYNAMIC_UPDATE).data(shows).build();
        try {
            sendResponse(answer);
            System.out.println("Full list sent for a client... " + answer.getType());
        }
        catch (Exception e) {
            throw new Exception("Sending error: "+e);
        }
    }

    @Override
    public void updateFilteredList(List<Show> shows) throws Exception {
        Answer answer = new Answer.Builder().type(AnswerType.FILTERED_SHOW_LIST_DYNAMIC_UPDATE).data(shows).build();
        try {
            sendResponse(answer);
            System.out.println("Filtered list for a client... "+answer.getType());
        }
        catch (Exception e) {
            throw new Exception("Sending error: "+e);
        }
    }

    private Answer handleRequest(Request request) {
        Answer answer;
        String handlerName = "handle"+request.getType();
        System.out.println("Handler is " + handlerName);
        try {
            Method method = this.getClass().getDeclaredMethod(handlerName,Request.class);
            answer =(Answer) method.invoke(this,request);
            System.out.println("Method "+handlerName+" invoked");
            return answer;
        }
        catch (Exception e){
            e.printStackTrace();
        }
        return new Answer.Builder().type(AnswerType.ERROR).build();
    }

    private void sendResponse(Answer answer) throws Exception {
        System.out.println("Sending response "+answer.toString());
        output.writeObject(answer);
        output.flush();
    }

    private Answer handleADD_SALE(Request request) {
        System.out.println("Add sale request...");
        Sale sale = (Sale)request.getData();
        try {
            service.addSaleToShow(sale.getBuyerName(), sale.getTicketsBought(),sale.getShowID());
            return answer_ok;
        }
        catch (Exception e){
            return new Answer.Builder().type(AnswerType.ERROR).data(e.getMessage()).build();
        }
    }

    private Answer handleFULL_SHOW_LIST(Request request) {
        System.out.println("Getting all shows...");
        try {
            return new Answer.Builder().type(AnswerType.FULL_SHOW_LIST).data(service.getAllShows()).build();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        return new Answer.Builder().type(AnswerType.ERROR).data("No list").build();
    }

    private Answer handleFILTERED_SHOW_LIST(Request request) {
        System.out.println("Filtering shows by...");
        try {
            return new Answer.Builder().type(AnswerType.FILTERED_SHOW_LIST).data(service.getShowsByDate((LocalDate) request.getData())).build();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        return new Answer.Builder().type(AnswerType.ERROR).data("No list after filtering").build();
    }

    private Answer handleLOGIN(Request request) {
        System.out.println("Login request...");
        User user = (User)request.getData();
        try {
            service.login(user.getUsername(),user.getPassword(),this);
            return answer_ok;
        }
        catch (Exception e) {
            connected=false;
            return new Answer.Builder().type(AnswerType.ERROR).data(e.getMessage()).build();
        }
    }

    private Answer handleLOGOUT(Request request) {
        System.out.println("Logging user out...");
        String username = (String) request.getData();
        try {
            service.logout(username,this);
            connected=false;
            return answer_ok;
        }
        catch (Exception e) {
            return new Answer.Builder().type(AnswerType.ERROR).data(e.getMessage()).build();

        }
    }

}
