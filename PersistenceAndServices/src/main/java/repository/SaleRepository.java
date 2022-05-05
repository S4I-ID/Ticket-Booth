package repository;

import domain.Sale;
import java.util.List;

public interface SaleRepository extends Repository<Integer, Sale> {
    List<Sale> findSalesForShow(Integer id) throws Exception;
}
