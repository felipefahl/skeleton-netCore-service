using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Messages.Interfaces;
using Skeleton.ServiceName.Messages.Models;
using Skeleton.ServiceName.Utils.Enumerators;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.ViewModel.People;



namespace Skeleton.ServiceName.Business.Implementations
{
    internal class PersonService : ServiceCrud<Person, PersonViewModel>, IPersonService
    {
        public PersonService(IRepository<Person> person,
                             IMapper mapper,
                             IServiceBus serviceBus,
                             IApplicationInsights applicationInsights)
            :base(person, mapper, serviceBus, applicationInsights)
        {

        }

        //Verificar a necessidade de transformar esse método em genérico e movê-lo apra a classe ServiceCrud
        public async Task<PersonViewModel> SaveAsync(PersonViewModel model)
        {
            var person = _mapper.Map<Person>(model);

            if (model.Id > 0)
            {
                await _repository.UpdateAsync(person);
            }
            else
            {
                await _repository.InsertAsync(person);
            }

            _applicationInsights.ChoreographyStackSent(new List<string>() { this.GetType().Name }, person);

            var serviceBusModel = new ServiceBusModel()
            {
                Date = DateTimeHelper.BrazilNow,
                ERestOperation = ERestOperation.POST,
                Uri = "",
                ERestOperationReturn = ERestOperation.GET,
                UriReturn = "",
                Stack = new List<string>() { this.GetType().Name },
                Obj = person               
            };

            await _serviceBus.SendAsync(serviceBusModel);

            return await GetAsync(person.Id);
        }
    }
}
