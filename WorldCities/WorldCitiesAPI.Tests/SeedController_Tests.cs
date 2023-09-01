using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCitiesAPI.Application.Accounts.Commands.CreateDefaultUsers;
using WorldCitiesAPI.Domain.ApplicationUser;

namespace WorldCitiesAPI.Tests
{
    public class SeedController_Tests
    {
        /// <summary>
        /// Test the CreateDefaultUsers() method
        /// </summary>
        [Fact]
        public async Task CreateDefaultUsers()
        {
            // Arrange

            // create the option instances required by the
            // ApplicationDbContext
            // create a ApplicationDbContext instance using the // in-memory DB
            var context = Common.GetApplicationDbContext();

            //// create a IWebHost environment mock instance
            //var mockEnv = Mock.Of<IWebHostEnvironment>();

            // create a IConfiguration mock instance
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "DefaultPasswords:RegisteredUser")]).Returns("M0ckP$$word");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "DefaultPasswords:Administrator")]).Returns("M0ckP$$word");

            // create a RoleManager instance
            var roleManager = IdentityHelper.GetRoleManager(new RoleStore<IdentityRole>(context));

            // create a UserManager instance
            var userManager = IdentityHelper.GetUserManager(new UserStore<ApplicationUser>(context));

            // define the variables for the users we want to test
            ApplicationUser user_Admin = null!;
            ApplicationUser user_User = null!;
            ApplicationUser user_NotExisting = null!;

            var createDefaultUsersCommand = new CreateDefaultUsersCommand();
            var createDefaultUsersCommandHandler = new CreateDefaultUsersCommandHandler(roleManager, userManager,context,mockConfiguration.Object);

            //Act
            var createDefaultUsersResult = await createDefaultUsersCommandHandler.Handle(createDefaultUsersCommand, default);

            // Assert
            (await userManager.FindByEmailAsync("admin@email.com")).Should().NotBeNull();
            (await userManager.FindByEmailAsync("user@email.com")).Should().NotBeNull();
            (await userManager.FindByEmailAsync("notexisting@email.com")).Should().BeNull();
        }
    }
}
