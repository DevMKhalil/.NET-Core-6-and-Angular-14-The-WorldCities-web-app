using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using WorldCitiesAPI.Common;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Domain.ApplicationUser;
using WorldCitiesAPI.Resources;

namespace WorldCitiesAPI.Application.Accounts.Queries.Login
{
    public class LoginQuery : IRequest<LoginResult>
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHandler _jwtHandler;
        public LoginQueryHandler(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            JwtHandler jwtHandler)
        {
            _context = context;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }
        public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);

            if (user == null
                || !await _userManager.CheckPasswordAsync(user, request.Password))
                return new LoginResult()
                {
                    Success = false,
                    Message = Resource.InvalidEmailOrPassword
                };

            var secToken = await _jwtHandler.GetTokenAsync(user); 
            var jwt = new JwtSecurityTokenHandler().WriteToken(secToken); 
            return new LoginResult()
            {
                Success = true,
                Message = Resource.LoginSuccessful,
                Token = jwt
            };
        }
    }
}
