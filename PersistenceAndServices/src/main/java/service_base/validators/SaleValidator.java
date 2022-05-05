package service_base.validators;

import domain.Sale;

public class SaleValidator implements Validator<Sale> {
    @Override
    public void validate(Sale entity) throws Exception {
        String errors="";

        if (entity.getBuyerName()==null)
            errors+="Buyer name is null!\n";
        if (entity.getShowID()==null)
            errors+="Show ID is null!\n";
        if (entity.getTicketsBought()<=0 || entity.getTicketsBought()==null)
            errors+="Invalid ticket number!\n";
        if (!errors.equals(""))
            throw new Exception("Invalid sale!\n"+errors);
    }
}
