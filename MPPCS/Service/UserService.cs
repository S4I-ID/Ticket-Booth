using Isopoh.Cryptography.Argon2;
using CommonDomain;
using Persistence.Repository;
using Persistence.Service.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Service
{
    public class UserService
    {
        private UserRepository repository;
        private UserValidator validator;

        public UserService(UserRepository repository)
        {
            this.repository = repository;
            this.validator = new UserValidator();
        }

        public int Login(User user)
        {
            validator.validate(user);
            string hashedPassword = repository.GetPasswordOfUser(user.GetUsername());            
            if (Argon2.Verify(hashedPassword, user.GetPassword()))
            {
                return 1;
            }
            else throw new Exception("Invalid login!\n");
        }
    }
}
