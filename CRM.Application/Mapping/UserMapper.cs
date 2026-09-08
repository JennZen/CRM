using CRM.Application.DTOs.User;
using CRM.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Mapping
{
    [Mapper]
    public partial class UserMapper
    {
        public partial UserDetailsDto ToDetailsDto(User u);

        public partial List<UserDetailsDto> ToDetailsDtos(List<User> u);

        public partial User ToDomain(UserCreateDto dto);

        public partial User ToDomain(UserUpdateDto dto);
    }
}
