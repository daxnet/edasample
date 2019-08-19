using EdaSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.Model
{
    public class Customer : IEntity
    {
        public Customer()
            : this(Guid.NewGuid(), null)
        { }

        public Customer(string name)
            : this(Guid.NewGuid(), name) { }

        public Customer(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Customer(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
            Credit = 5000; // By default, set Credit to 100.
        }

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public int Credit { get; set; }
    }
}
