package domain;

import java.io.Serializable;

public class Sale extends Entity<Integer> {
    String buyerName;
    Integer ticketsBought;
    Integer showID;


    public Sale(String buyerName, Integer ticketsBought, Integer showID) {
        this.buyerName = buyerName;
        this.ticketsBought = ticketsBought;
        this.showID = showID;
    }

    public String getBuyerName() {
        return buyerName;
    }

    public void setBuyerName(String buyerName) {
        this.buyerName = buyerName;
    }

    public Integer getTicketsBought() {
        return ticketsBought;
    }

    public void setTicketsBought(Integer ticketsBought) {
        this.ticketsBought = ticketsBought;
    }

    public Integer getShowID() {
        return showID;
    }

    public void setShowID(Integer showID) {
        this.showID = showID;
    }
}