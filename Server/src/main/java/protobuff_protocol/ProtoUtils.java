package protobuff_protocol;

import domain.Show;
import domain.Sale;
import domain.User;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.List;

public class ProtoUtils {

    public static Proto.Request createAddSaleRequest(Sale sale) {
        Proto.Sale protoSale = Proto.Sale.newBuilder().setBuyerName(sale.getBuyerName())
                .setTicketsBought(sale.getTicketsBought()).setShowId(sale.getShowID()).build();
        return Proto.Request.newBuilder()
                .setType(Proto.Request.Type.AddSale).setSale(protoSale).build();
    }

    public static Proto.Request createFullShowListRequest() {
        return Proto.Request.newBuilder().setType(Proto.Request.Type.FullShowList)
                .setDatetime("data").build();
    }

    public static List<Show> getShowsListFromAnswer(Proto.Answer answer) {
        List<Show> shows = new ArrayList<>();
        for (Proto.Show show : answer.getShowsList()) {
            LocalDateTime date = LocalDateTime.parse(show.getTime());
            Show domainShow = new Show(show.getArtistName(),show.getShowName(),show.getAddress(),
                    date, show.getAvailableSeats(), show.getSoldSeats());
            domainShow.setID(show.getId());
            shows.add(domainShow);
        }
        return shows;
    }

    public static Proto.Request createFilteredShowListRequest(LocalDate date) {
        return Proto.Request.newBuilder().setType(Proto.Request.Type.FilteredShowList)
                .setDatetime(date.toString()).build();
    }

    public static Proto.Request createLoginRequest(User user) {
        Proto.User protoUser = Proto.User.newBuilder()
                .setUsername(user.getUsername()).setPassword(user.getPassword()).build();
        return Proto.Request.newBuilder().setType(Proto.Request.Type.Login)
                .setUser(protoUser).build();
    }

    public static Proto.Request createLogoutRequest(String username) {
        return Proto.Request.newBuilder().setType(Proto.Request.Type.Logout)
                .setUsername(username).build();
    }
}
