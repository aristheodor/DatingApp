using API.Data;
using API.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;


    public UsersController(DataContext context)
    {
        _context = context;

    }

    [AllowAnonymous]
    [HttpGet]

    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {

        var users = await _context.Users.ToListAsync();

        return users;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]  // /api/users/2

    public async Task<ActionResult<AppUser>> GetUsers(int Id)
    {

        return await _context.Users.FindAsync(Id);
    }
}
