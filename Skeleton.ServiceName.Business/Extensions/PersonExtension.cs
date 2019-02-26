using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.ViewModel.People;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.ServiceName.Business.Extensions
{
    public static class PersonExtension
    {
        public static PersonViewModel ToView(this Person person)
        {
            return new PersonViewModel
            {
                Id = person.Id,
                BirthDate = person.BirthDate,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
        }

        public static IList<PersonViewModel> ToViewList(this IList<Person> people)
        {
            return people.Select(x => x.ToView()).ToList();
        }

        public static Person ToPerson(this PersonViewModel model)
        {
            return new Person
            {
                Id = model.Id,
                BirthDate = model.BirthDate,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
        }

        public static IList<Person> ToPeople(this IList<PersonViewModel> peopleModel)
        {
            return peopleModel.Select(x => x.ToPerson()).ToList();
        }

        public static bool IsEqualTo(this Person _person, PersonViewModel model)
        {
            return _person.Id == model.Id
                && _person.FirstName == model.FirstName
                && _person.LastName == model.LastName
                && _person.BirthDate == model.BirthDate;
        }
    }
}
