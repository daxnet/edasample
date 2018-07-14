using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.Model
{
    public class Customer
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

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}
