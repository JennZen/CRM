using CRM.Application.Interfaces.Services;
using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Seed
{
    public static class DbSeeder
    {
        public static void SeedAdmin(UnitOfWork uow, IHasherService passwordHasher, string email, string password)
        {
            var existingAdmin = new XPQuery<UserDb>(uow)
                .FirstOrDefault(u => u.Role == UserRole.Admin && u.Email == email);

            if (existingAdmin != null)
                return;

            var admin = new UserDb(uow)
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = email,
                PasswordHash = passwordHasher.HashPassword(password),
                Role = UserRole.Admin,
                CreatedAt = DateTime.Now
            };

            uow.CommitChanges();
        }
    }
}
