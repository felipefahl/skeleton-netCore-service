using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Business.Parameters;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.EfExtensions;
using Skeleton.ServiceName.ViewModel.People;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Implementations
{
    public class PersonService : ServiceCrud<Person, PersonViewModel>, IPersonService
    {
        public PersonService(IPersonRepository repository,
                             IMapper mapper)
            :base(repository, mapper)
        {

        }

        public IList<PersonViewModel> All(PersonParameters queryStringParameters)
        {
            var query = ApplyFilterParameters(_repository.All, queryStringParameters);


            query = query.Paged(queryStringParameters.PageNumber, queryStringParameters.PageSize);
            var list = _mapper.Map<IEnumerable<Person>, IList<PersonViewModel>>(query);

            return list;
        }

        private IQueryable<Person> ApplyFilterParameters(IQueryable<Person> query, PersonParameters queryStringParameters)
        {
            if (!string.IsNullOrEmpty(queryStringParameters.Name))
            {
                query = query.Where(o => (o.FirstName.ToLower().Contains(queryStringParameters.Name.Trim().ToLower())) ||
                                            (o.LastName.ToLower().Contains(queryStringParameters.Name.Trim().ToLower())) );
            }
            return query;
        }

        public async Task<long> CountAsync(PersonParameters queryStringParameters)
        {
            var query = ApplyFilterParameters(_repository.All, queryStringParameters);

            return await query.CountAsync();
        }
    }
}
