using EventPlatform.Application.Commands.IdentityCommands;
using EventPlatform.Application.Entities;
using EventPlatform.WebApi.Contracts.IdentityContracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers;

public class IdentityController : BaseApiController
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        
        
        var users = new AppUserEntity[]
        {
            new() { Id = "1", UserName = "SomeUserName" },
            new() { Id = "2", UserName = "SomeUserName2" },
            new() { Id = "3", UserName = "SomeUserName3" }
        };
        
        return Ok(users);
    }


    [HttpPost]
    [Route(ApiRoutes.Identity.Register)]
    public async Task<IActionResult> Register(UserRegistration user)
    {
        var command = new RegisterUserCommand
        {
            FullName = user.FullName, Email = user.Email,
            Password = user.Password, Phone = user.Phone
        };
        
        var response = await _mediator.Send(command);

        return Ok();
    }

}