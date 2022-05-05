package service_base;

import de.mkammerer.argon2.Argon2;
import de.mkammerer.argon2.Argon2Factory;
import domain.User;
import repository.UserDatabaseRepository;
import service_base.validators.UserValidator;

public class UserService {
    private UserDatabaseRepository repository;
    private final UserValidator validator;

    public UserService(UserDatabaseRepository repository) {
        this.repository = repository;
        this.validator = new UserValidator();
    }

    public int login(User user) throws Exception {
        validator.validate(user);
        String hashedPassword = repository.getPasswordOfUser(user.getUsername());
        Argon2 A2 = Argon2Factory.create(16,32);
        if (A2.verify(hashedPassword,user.getPassword()))
            return 1;
        else throw new Exception("Invalid login!\n");
    }
}
