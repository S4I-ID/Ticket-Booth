package repository;

import domain.User;

public interface UserRepository extends Repository<Integer, User>{
    String getPasswordOfUser (String username) throws Exception;
}