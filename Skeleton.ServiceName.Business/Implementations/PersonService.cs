using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Skeleton.ServiceName.Business.Extensions;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Messages.Interfaces;
using Skeleton.ServiceName.Messages.Models;
using Skeleton.ServiceName.Utils.Enumerators;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.ViewModel.People;



namespace Skeleton.ServiceName.Business.Implementations
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Person> _person;
        private readonly IServiceBus _serviceBus;
        private readonly IApplicationInsights _applicationInsights;

        private const string _stack = "PersonService";

        public PersonService(IRepository<Person> person, 
                             IServiceBus serviceBus,
                             IApplicationInsights applicationInsights)
        {
            _person = person;
            _serviceBus = serviceBus;
            _applicationInsights = applicationInsights;
        }

        public IList<PersonViewModel> All()
        {
            var list = _person.All.ToList().ToViewList();


            _applicationInsights.LogInformation("ENTROU", "PersonService/All");

            return list;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var person = await _person.FindAsync(id);
            if (person == null)
            {
                return false;
            }

            await _person.DeleteAsync(person);

            return true;
        }

        public async Task<PersonViewModel> GetAsync(long id)
        {
            var person = await _person.FindAsync(id);
            return person.ToView();
        }

        public async Task<PersonViewModel> SaveAsync(PersonViewModel model)
        {
            var person = model.ToPerson();

            if(model.Id > 0)
            {
                await _person.UpdateAsync(person);
            }
            else
            {
                await _person.InsertAsync(person);
            }

            _applicationInsights.ChoreographyStackSent(new List<string>() { "PersonService" }, person);

            var serviceBusModel = new ServiceBusModel()
            {
                Date = DateTimeHelper.BrazilNow,
                ERestOperation = ERestOperation.POST,
                Uri = "",
                ERestOperationReturn = ERestOperation.GET,
                UriReturn = "",
                Stack = new List<string>() { _stack },
                Obj = person               
            };

            await _serviceBus.SendAsync(serviceBusModel);

            return await GetAsync(person.Id);
        }
    }
}
