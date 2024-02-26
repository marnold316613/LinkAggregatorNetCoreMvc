using LinkAggregatorMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LinkAggregatorMVC.Authorization;

namespace LinkAggregatorMVC.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
           

            using (var context = new ApplicationDbContext(
                  serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
               

                var adminID = await EnsureUser(serviceProvider, testUserPw, "frank@outlook.com");
                if (adminID==null)
                {

                }
                _ = await EnsureRole(serviceProvider, adminID!, AuthorizationConstants.AdministratorRole);


                // allowed user can create and edit contacts that they create
                var powerUserID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com");
                await EnsureRole(serviceProvider, powerUserID!, AuthorizationConstants.PowerUserRole);
                await EnsureRoleClaim(serviceProvider, "ManageLinks", "True",AuthorizationConstants.PowerUserRole);

                var userID = await EnsureUser(serviceProvider, testUserPw, "user@contoso.com");
                await EnsureRole(serviceProvider, userID!, AuthorizationConstants.UserRole);
                SeedDB(context);
            }
        }
        private static async Task<string?> EnsureUser(IServiceProvider serviceProvider,
                                                   string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>()!;
            
           

            try
            {
                var user = await userManager!.FindByNameAsync(UserName);
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = UserName,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(user, testUserPw);
                }

                if (user == null)
                {
                    throw new Exception("The password is probably not strong enough!");
                }

                return user.Id;
            }
            catch (Exception ex)
            {
             //   throw;
            }
            return null;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR;
            try
            {
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                if (roleManager == null)
                {
                    throw new Exception("roleManager null");
                }

              
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IR = await roleManager.CreateAsync(new IdentityRole(role));
                }
            } catch (Exception ex) { }
           

           
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        private static async Task  EnsureRoleClaim(IServiceProvider serviceProvider,
                                                                     string setting, string value, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>() ?? throw new Exception("roleManager null");
            var identityrole = await roleManager.FindByNameAsync(role);
            var claimsList = await roleManager.GetClaimsAsync(identityrole!);
            IdentityResult IR;
            if (!claimsList.Any(c =>c.Type==setting))
            {
              IR=  await roleManager.AddClaimAsync(identityrole!, new System.Security.Claims.Claim(setting, "True"));
            }


           
        }

        private static void SeedDB(ApplicationDbContext context)
        {
            if (!context.CategoriesLookup.Any())
            {
                context.CategoriesLookup.AddRange(
                    new CategoriesLookup
                    {
                        Category = "World News"
                    },
                    new CategoriesLookup
                    {
                        Category = "US News"
                    },
                    new CategoriesLookup
                    {
                        Category = "Politics"
                    },
                    new CategoriesLookup
                    {
                        Category = "Sports"
                    },
                    new CategoriesLookup
                    {
                        Category = "Test"

                    }

                    );

                context.SaveChanges();
            }
            if (!context.Links.Any())
            {
                context.AddRange(
                    new Links
                    {
                        Caption = "Citizen Free Press",
                        URL = "https://citizenfreepress.com",
                        Importance = 1,
                        TotalClicks = 0,
                        LinkPostingDate = DateTime.Now,
                        Rating = 5
                    }
                    );


                context.SaveChanges();
            }

        }

    }
}
