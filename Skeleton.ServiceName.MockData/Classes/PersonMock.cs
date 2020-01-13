using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Collections.Generic;

namespace Skeleton.ServiceName.MockData.Classes
{
    public static class PersonMock
    {
        public static IList<Person> ListPerson() => new List<Person> {
            new Person {
                Id = Guid.Parse("0c1704a8-71f1-4dc8-a737-b15421a1661b"),
                FirstName = "Teste",
                LastName = "Last Name",
                BirthDate = DateTime.Parse("17/12/2019"),
                Active = true
            },
            new Person {
                Id = Guid.Parse("9fd8bfbf-d8f8-47ec-941b-d37e2a211a9d"),
                FirstName = "Teste 2",
                LastName = "Last Name 2",
                BirthDate = DateTime.Parse("17/02/2019"),
                Active = true
            }
        };

        public static Person GetPerson(Guid id) => new Person
        {
            Id = id,
            FirstName = "Teste",
            LastName = "Last Name",
            BirthDate = DateTime.Parse("17/12/2019"),
            Active = true
        };

        public static Person NewPerson() => new Person
        {
            FirstName = "Teste",
            LastName = "Last Name",
            BirthDate = DateTime.Parse("17/12/2019"),
            Active = true
        };

        public static IList<PersonViewModel> ListPersonViewModel() => new List<PersonViewModel> {
            new PersonViewModel {
                Id = Guid.Parse("0c1704a8-71f1-4dc8-a737-b15421a1661b"),
                FirstName = "Teste",
                LastName = "Last Name",
                BirthDate = DateTime.Parse("17/12/2019"),
                Active = true
            },
            new PersonViewModel {
                Id = Guid.Parse("9fd8bfbf-d8f8-47ec-941b-d37e2a211a9d"),
                FirstName = "Teste 2",
                LastName = "Last Name 2",
                BirthDate = DateTime.Parse("17/02/2019"),
                Active = true
            }
        };
        public static PersonViewModel GetPersonViewModel(Guid id) => new PersonViewModel
        {
            Id = id,
            FirstName = "Teste",
            LastName = "Last Name",
            BirthDate = DateTime.Parse("17/12/2019"),
            Active = true
        };

        public static PersonViewModel NewPersonViewModel() => new PersonViewModel
        {
            FirstName = "Teste",
            LastName = "Last Name",
            BirthDate = DateTime.Parse("17/12/2019"),
            Active = true
        };
    }
}
