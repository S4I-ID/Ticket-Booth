package service_base.validators;

import domain.Show;

public class ShowValidator implements Validator<Show> {
    @Override
    public void validate(Show entity) throws Exception {
        String errors="";

        if (entity.getAvailableSeats()==null)
            errors+="Seats are null!\n";
        if (entity.getAddress()==null)
            errors+="Address is null!\n";
        if (entity.getShowName()==null)
            errors+="Show name is null!\n";
        if (entity.getArtistName()==null)
            errors+="Artist name is null!\n";
        if (entity.getSoldSeats()==null)
            errors+="Sold seats is null!\n";
        if (entity.getStartTime()==null)
            errors+="Date is null!\n";
        if (entity.getAvailableSeats()<entity.getSoldSeats())
            errors+="Not enough seats left!\n";
        if (!errors.equals(""))
            throw new Exception("Invalid show!\n"+errors);
    }
}
