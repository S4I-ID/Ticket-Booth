package services;

import domain.Show;

import java.time.LocalDate;
import java.util.List;

public interface MainServerServiceInterface {
    void addSaleToShow(String buyerName, int ticketsBought, int showId) throws Exception;
    List<Show> getAllShows() throws Exception;
    List<Show> getShowsByDate(LocalDate date) throws Exception;
    void login(String username, String password, ServerObserver client) throws Exception;
    void logout(String username, ServerObserver client) throws Exception;
}
