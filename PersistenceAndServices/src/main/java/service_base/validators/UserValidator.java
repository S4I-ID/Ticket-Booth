package service_base.validators;

import domain.User;

public class UserValidator implements Validator<User> {
    @Override
    public void validate(User entity) throws Exception {
        String errors="";

        if (entity.getUsername()==null || entity.getUsername()=="")
            errors+="Invalid username!\n";
        if (entity.getPassword()==null || entity.getPassword()=="")
            errors+="Invalid password!\n";

        if (!errors.equals(""))
            throw new Exception("Invalid login information!\n");
    }
}
