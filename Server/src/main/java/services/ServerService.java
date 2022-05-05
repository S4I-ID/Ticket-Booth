package services;

import domain.*;
import service_base.*;

import java.time.LocalDate;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ServerService implements MainServerServiceInterface {
    private SaleService saleService;
    private ShowService showService;
    private UserService userService;
    private Map<String, ServerObserver> clients;
    private final int defaultThreads=6;

    public ServerService(SaleService saleService, ShowService showService, UserService userService) {
        this.saleService = saleService;
        this.showService = showService;
        this.userService = userService;
        clients = new ConcurrentHashMap<>();    // LIST OF CLIENTS LOGGED IN
    }

    private void notifySaleAdded() {    // CREATE THREADS FOR EACH CLIENT
        ExecutorService executor = Executors.newFixedThreadPool(defaultThreads);
        for (var client : clients.entrySet()) {
            if(client.getValue()!=null) {
                executor.execute(()->{
                    try {
                        System.out.println("Notifying ["+client.getKey()+"] of sales...");
                        client.getValue().updateShowList(getAllShows());    // SEND DYNAMIC UPDATE TO ALL CLIENTS MAIN TABLE
                        //client.getValue().updateFilteredList(Collections.emptyList()); // RESET ALL CLIENTS' FILTERED LISTS TO EMPTY
                    }
                    catch (Exception e) {
                        System.err.println("Error notifying "+client.getKey());
                    }
                });
            }
        }
        executor.shutdown();    // SHUTDOWN THREADS [!]
    }

    @Override
    public synchronized void addSaleToShow(String buyerName, int ticketsBought, int showId) throws Exception {
        Show show = showService.findShow(showId);
        Show updatedShow = show;
        updatedShow.setSoldSeats(show.getSoldSeats() + ticketsBought);
        showService.updateShow(updatedShow);    // UPDATE SHOW
        try {   // ADD TICKETS
            saleService.addSale(buyerName, ticketsBought, showId);
            notifySaleAdded();
        }
        catch (Exception ex) {  // IF UNABLE TO ADD TICKETS TO SHOW, UNDO SHOW CHANGES
            showService.updateShow(show);
            throw new Exception(ex);
        }
    }

    @Override
    public List<Show> getAllShows() throws Exception {  // RETURNS ALL SHOWS
        return showService.getAllShows();
    }

    @Override
    public List<Show> getShowsByDate(LocalDate date) throws Exception { // RETURNS SHOWS FILTERED BY DATE
        return showService.getShowsByDate(date);
    }

    @Override
    public synchronized void login(String username, String password, ServerObserver client) throws Exception {
        int login = userService.login(new User(username, password));
        if (login==1) {
            if (clients.get(username)!=null)    // IF ALREADY LOGGED IN
                throw new Exception("User "+username+" already logged in.");    // ACTIVATE FUN POLICE
            clients.put(username, client);  // IF NOT LOGGED IN, LOG HIM IN
        }
        else throw new Exception("User "+username+" failed to login");
    }

    @Override
    public void logout(String username, ServerObserver client) throws Exception {
        ServerObserver localClient = clients.remove(username);
        if (localClient==null)
            throw new Exception ("User "+username+" is not logged in");
    }

}
