using Microsoft.AspNetCore.Mvc;
//using WebApi.api.Models;
using System;
using WebApi.api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.api.Controllers;

[ApiController]
[Route("")]
public class UserController : ControllerBase
{
    private static List<User> users = new List<User>()
    {
        new User()
        {
            Username = "Bob",
            Password = "Bob-123"
        },
        new User()
        {
            Username = "John",
            Password = "I<3-C#"
        },
        new User()
        {
            Username = "Steve",
            Password = "123-Steve"
        }
    };


    private readonly ILogger<WeatherForecastController> _logger;

    public UserController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "ReadUsers")]
    public ActionResult<IEnumerable<User>> Get()
    {
        return users;
    }

    [HttpGet("{username:datetime}", Name = "ReadWeatherForecastByDate")]
    public ActionResult<User> Get(string username)
    {
        User user = GetUser(username);
        if (user == null)
            return NotFound();

        return user;
    }


    [HttpPost(Name = "CreateWeatherForecast")]
    public ActionResult Add(User user)
    {
        if (GetUser(user.Username) != null)
            return BadRequest("User with name" + user.Username + " already exists.");

        users.Add(user);
        return Created();
    }


    [HttpPut("{username:string}", Name = "UpdateUser")]
    public IActionResult Update(string username, User newUser)
    {
        if (username != newUser.Username)
            return BadRequest("The id of the object did not match the id of the route");

        User userToUpdate = GetUser(newUser.Username);
        if (userToUpdate == null)
            return NotFound();

        users.Remove(userToUpdate);
        users.Add(newUser);

        return Ok();
    }

    [HttpDelete("{username:string}", Name = "DeleteUser")]
    public IActionResult Update(string username)
    {
        User userToDelete = GetUser(username);
        if (userToDelete == null)
            return NotFound();

        users.Remove(userToDelete);
        return Ok();
    }

    private User GetUser(string username)
    {
        foreach (User user in users)
        {
            if (user.Username == username)
                return user;
        }

        return null;
    }
}
