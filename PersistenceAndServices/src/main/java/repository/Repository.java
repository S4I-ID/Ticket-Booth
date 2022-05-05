package repository;

import domain.Entity;
import java.util.List;


public interface Repository<ID, E extends Entity<ID>> {
    E find(ID id) throws Exception;
    List<E> getAll() throws Exception;
    void add(E entity) throws Exception;
    E delete(ID id) throws Exception;
    E update(E entity) throws Exception;
    Integer size() throws Exception;

}