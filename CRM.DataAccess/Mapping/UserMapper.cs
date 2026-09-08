using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Mapping
{
    [Mapper]
    public partial class UserMapper
    {
        [MapProperty(nameof(UserDb.Oid), nameof(User.Id))]
        [MapperIgnoreSource(nameof(UserDb.Requests))]
        public partial User ToDomain(UserDb u);

        public partial List<User> ToDomains(List<UserDb> u);
    }
}
