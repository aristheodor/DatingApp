using API.Data;
using API.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[Controller]")] // /api/users
public class UsersController : ControllerBase
{
    private readonly DataContext _context;


    public UsersController(DataContext context)
    {
        _context = context;

    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {

        var users = await _context.Users.ToListAsync();

        return users;
    }

    [HttpGet("{id}")]  // /api/users/2

     public async Task<ActionResult<AppUser>> GetUsers(int Id){

        return await _context.Users.FindAsync(Id);
     }
}
