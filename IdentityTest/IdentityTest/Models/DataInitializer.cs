using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace IdentityTest.Models
{
    public class DataInitializer
    {

        internal static void Seed(IServiceScope scoped)
        {
            using (var context = scoped.ServiceProvider.GetRequiredService<UserDbContext>())
            {
                var manager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (!manager.Users.Any())
                {
                    var config = scoped.ServiceProvider.GetRequiredService<IConfiguration>();
                    AppUser appUser = new AppUser
                    {
                        UserName = config["User:username"],
                        Email = config["User:email"]
                    };
                    manager.CreateAsync(appUser, config["User:password"]).GetAwaiter().GetResult();

                    var rolemanager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                    List<IdentityRole<int>> roles = new List<IdentityRole<int>>();



                    if (!rolemanager.Roles.Any())
                    {
                        string[] rollar = config.GetSection("Role").Value.Split(",");
                        foreach (var item in rollar)
                        {
                            IdentityRole<int> identityRole = new IdentityRole<int>
                            {
                                Name = item
                            };
                            roles.Add(identityRole);
                            rolemanager.CreateAsync(identityRole).GetAwaiter().GetResult();
                        }
                    }
                    manager.AddToRoleAsync(appUser, roles[0].Name).GetAwaiter().GetResult();
                }
            }
        }
    }
}
