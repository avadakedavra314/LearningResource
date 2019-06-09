using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using WebApp.Services.Interfaces;
using WebApp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Models;

namespace WebApp.Services
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IServiceProvider _provider;
        public IConfiguration _configuration { get; }
        public DatabaseInitializer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _provider = serviceProvider;
            _configuration = configuration;
        }

        public void Seed()
        {
            var required = _configuration.GetSection("Settings")["DatabaseSeedRequired"].ToBoolean();
            if (required)
            {
                using (IServiceScope scope = _provider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<WebAppContext>();

                    Role adminRole = _context.Roles.FirstOrDefault
                        (x => x.Name == _configuration.GetSection("DatabaseSeedData")["AdministratorRoleName"]);
                    Role userRole = _context.Roles.FirstOrDefault
                        (x => x.Name == _configuration.GetSection("DatabaseSeedData")["DefaultRoleName"]);

                    var groupDefaultNormalized = Convert.ToInt32(_configuration.GetSection("DatabaseSeedData")["DefaultGroupName"]);

                    Group groupDefault = _context.Groups.FirstOrDefault
                        (x => x.Name == groupDefaultNormalized);

                    if (adminRole == null)
                    {
                        adminRole = new Role { Name = _configuration.GetSection("DatabaseSeedData")["AdministratorRoleName"] };
                        _context.Roles.Add(adminRole);
                    }
                    _context.SaveChanges();

                    if (userRole == null)
                    {
                        userRole = new Role { Name = _configuration.GetSection("DatabaseSeedData")["DefaultRoleName"] };
                        _context.Roles.Add(userRole);
                    }
                    _context.SaveChanges();

                    if(groupDefault == null)
                    {
                        groupDefault = new Group { Name = Convert.ToInt32(_configuration.GetSection("DatabaseSeedData")["DefaultGroupName"]) };
                        _context.Groups.Add(groupDefault);
                    }
                    _context.SaveChanges();


                    User admin = _context.Users.FirstOrDefault
                        (u => u.Login == _configuration.GetSection("DatabaseSeedData")["AdministratorLogin"]);
                    if (admin == null)
                    {
                        _context.Users.Add(new User
                        {
                            FirstName = "Рустам",
                            LastName = "Асылгареев",
                            Patronymic = "Фанилевич",
                            Login = _configuration.GetSection("DatabaseSeedData")["AdministratorLogin"],
                            Password = _configuration.GetSection("DatabaseSeedData")["AdministratorPassword"],
                            Role = adminRole,
                            Group = groupDefault,
                        });
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
