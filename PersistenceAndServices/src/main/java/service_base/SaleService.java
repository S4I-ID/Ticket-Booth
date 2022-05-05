package service_base;

import domain.Sale;
import repository.SaleDatabaseRepository;
import service_base.validators.SaleValidator;

public class SaleService {
    private SaleDatabaseRepository repository;
    private final SaleValidator validator;

    public SaleService(SaleDatabaseRepository repository) {
        this.repository = repository;
        this.validator = new SaleValidator();
    }

    public void addSale (String buyerName, int ticketsBought, int showId) throws Exception {
        Sale sale = new Sale(buyerName,ticketsBought,showId);
        validator.validate(sale);
        repository.add(sale);
    }

    public Sale deleteSale(int id) throws Exception {
        repository.find(id);
        return repository.delete(id);
    }
}
