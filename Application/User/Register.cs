using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public class Register
    {

        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                this._jwtGenerator = jwtGenerator;
                this._userManager = userManager;
                this._context = context;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if(await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {Email = " Email already exist"});

                if(await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {UserName = " UserName already exist"});

                var user = new AppUser 
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.UserName
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return new User 
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        UserName = user.UserName
                    };
                } 
                    
                throw new Exception("Problem creating user");

            }

        }
    }
}