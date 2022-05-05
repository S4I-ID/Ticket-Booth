package domain;

import java.time.LocalDateTime;
import java.util.Objects;

public class Show extends Entity<Integer> {
    String artistName;
    String showName;
    String address;
    LocalDateTime startTime;
    Integer availableSeats;
    Integer soldSeats;

    public Show(String artistName, String showName, String address,
                LocalDateTime startTime, Integer availableSeats,
                Integer soldSeats) {
        this.artistName = artistName;
        this.showName = showName;
        this.address = address;
        this.startTime = startTime;
        this.availableSeats = availableSeats;
        this.soldSeats = soldSeats;
    }

    public String getArtistName() {
        return artistName;
    }

    public void setArtistName(String artistName) {
        this.artistName = artistName;
    }

    public String getShowName() {
        return showName;
    }

    public void setShowName(String showName) {
        this.showName = showName;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public LocalDateTime getStartTime() {
        return startTime;
    }

    public void setStartTime(LocalDateTime startTime) {
        this.startTime = startTime;
    }

    public Integer getAvailableSeats() {
        return availableSeats;
    }

    public void setAvailableSeats(Integer availableSeats) {
        this.availableSeats = availableSeats;
    }

    public Integer getSoldSeats() {
        return soldSeats;
    }

    public void setSoldSeats(Integer soldSeats) {
        this.soldSeats = soldSeats;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Show show = (Show) o;
        return Objects.equals(artistName, show.artistName) && Objects.equals(showName, show.showName) &&
                Objects.equals(address, show.address) && Objects.equals(startTime, show.startTime) &&
                Objects.equals(availableSeats, show.availableSeats) &&
                Objects.equals(soldSeats, show.soldSeats);
    }

}