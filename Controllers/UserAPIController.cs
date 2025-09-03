using CrickerManagmentSystem_API_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrickerManagmentSystem_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        #region Dependencies
        private readonly CricketLeagueContext _context;
        public UserAPIController(CricketLeagueContext context)
        {
            _context = context;
        }
        #endregion
        #region GetAllUsers
        [HttpGet]
        public IActionResult GetUsers(CricketLeagueContext _context)
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
        #endregion
        #region GetUserById
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        #endregion
        #region AddUser
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(User user)
        {
            user.Created = DateTime.Now;
            user.Modified = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
            //return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }
        #endregion
        #region DeleteUser
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
        #region UpdateUser
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
           if(id != user.UserId)
            {
                return BadRequest("User ID mismatch");
            }

           var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Mobile = user.Mobile;
            existingUser.Modified = DateTime.Now;

            //_context.Entry(user).State = EntityState.Modified;
            if(!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = user.Password; // Assuming you want to update the password
            }
            await _context.SaveChangesAsync();
            return Ok(existingUser);
        }
        #endregion

        
    }
}
