using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WorldCitiesAPI.Domain.ApplicationUser;

namespace WorldCitiesAPI.Application.Accounts.Commands.CreateDefaultUsers
{
    public class CreateDefaultUsersCommand : IRequest<Result<List<ApplicationUser>>>
    {
    }

    public class CreateDefaultUsersCommandHandler : IRequestHandler<CreateDefaultUsersCommand, Result<List<ApplicationUser>>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public CreateDefaultUsersCommandHandler(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }
        public async Task<Result<List<ApplicationUser>>> Handle(CreateDefaultUsersCommand request, CancellationToken cancellationToken)
        {
            // setup the default role names
            try
            {
                string role_RegisteredUser = "RegisteredUser";
                string role_Administrator = "Administrator";

                // create the default roles (if they don't exist yet)
                if (await _roleManager.FindByNameAsync(role_RegisteredUser) == null)
                    await _roleManager.CreateAsync(new IdentityRole(role_RegisteredUser));

                if (await _roleManager.FindByNameAsync(role_Administrator) == null)
                    await _roleManager.CreateAsync(new IdentityRole(role_Administrator));

                // create a list to track the newly added users
                var addedUserList = new List<ApplicationUser>();

                // check if the admin user already exists
                var email_Admin = "admin@email.com";

                if (await _userManager.FindByNameAsync(email_Admin) == null)
                {
                    // create a new admin ApplicationUser account
                    var user_Admin = new ApplicationUser()
                    {
                        SecurityStamp = Guid.NewGuid().ToString(),
                        Email = email_Admin,
                        UserName = email_Admin
                    };

                    // insert the admin user into the DB
                    await _userManager.CreateAsync(user_Admin, _configuration["DefaultPasswords:Administrator"]);

                    // assign the "RegisteredUser" and "Administrator" roles
                    await _userManager.AddToRoleAsync(user_Admin, role_RegisteredUser);
                    await _userManager.AddToRoleAsync(user_Admin, role_Administrator);

                    // confirm the e-mail and remove lockout
                    user_Admin.EmailConfirmed = true;
                    user_Admin.LockoutEnabled = false;
                    // add the admin user to the added users list
                    addedUserList.Add(user_Admin);
                }

                // check if the standard user already exists
                var email_User = "user@email.com";
                if (await _userManager.FindByEmailAsync(email_User) == null)
                {
                    // create a new standard ApplicationUser account
                    var user_User = new ApplicationUser()
                    {
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = email_User,
                        Email = email_User
                    };

                    // insert the standard user into the DB
                    await _userManager.CreateAsync(user_User, _configuration["DefaultPasswords:RegisteredUser"]);

                    // assign the "RegisteredUser" role
                    await _userManager.AddToRoleAsync(user_User, role_RegisteredUser);

                    // confirm the e-mail and remove lockout
                    user_User.EmailConfirmed = true;
                    user_User.LockoutEnabled = false;

                    // add the standard user to the added users list
                    addedUserList.Add(user_User);

                    // if we added at least one user, persist the changes into the DB
                    if (addedUserList.Count > 0)
                    {
                        var saveResult = await _context.SaveChangesAsyncWithValidation();

                        if (saveResult.IsFailure)
                            return Result.Failure<List<ApplicationUser>>(saveResult.Error);
                    }
                }

                return Result.Success(addedUserList);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<ApplicationUser>>(ex.Message);
            }
        }
    }
}
