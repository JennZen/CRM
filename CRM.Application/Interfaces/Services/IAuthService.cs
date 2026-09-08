using CRM.Application.DTOs.User;
using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthResultDto?> LoginAsync(UserLoginDto dto);
    }
}
