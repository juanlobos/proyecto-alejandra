using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class User
    {
        public User(string name, string password, Guid token)
        {
            this.Name = name;
            this.Password = password;
            this.Token = token;
        }

        public string Name { get; private set; }
        public string Password { get; private set; }
        [Key]
        public Guid Token { get; private set; }
    }
}
