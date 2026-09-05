using CRM.Application.Interfaces.Repositories;
using CRM.Application.Mapping;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWork _uow;

        private readonly UserMapper _mapper;

        public UserRepository(UnitOfWork uow, UserMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
    }
}
