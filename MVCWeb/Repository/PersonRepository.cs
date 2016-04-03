using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCWeb.Controllers;

namespace MVCWeb.Repository
{
    public class PersonRepository
    {
        public static List<Person> GetList()
        {
            var persons = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "PersonOne",
                    Addresses = new List<Address>
                    {
                        new Address
                        {
                            AddressId = 1,
                            AddressLine1 = "1 High Street",
                            City = "Daventry",
                            State = States.Northamptonshire
                        },
                        new Address
                        {
                            AddressId = 2,
                            AddressLine1 = "20 Main Street",
                            City = "Irvine",
                            State = States.Ayrshire
                        },
                    }

                },
                new Person
                {
                    Id = 2,
                    Name = "PersonTwo",
                    Addresses = new List<Address>
                    {
                        new Address
                        {
                            AddressId = 1,
                            AddressLine1 = "2 High Street",
                            City = "Weedon",
                            State = States.Northamptonshire
                        },
                    }

                },
                new Person
                {
                    Id = 3,
                    Name = "PersonThree",
                    Addresses = new List<Address>{}
                },
                new Person
                {
                    Id = 4,
                    Name = "PersonFour",
                    Addresses = null
                }
            };
            return persons;
        }
    }
}