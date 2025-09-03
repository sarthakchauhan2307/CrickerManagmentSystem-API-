using CrickerManagmentSystem_API_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrickerManagmentSystem_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamAPIController : ControllerBase
    {
        #region Dependencies
        private readonly CricketLeagueContext _context;
        public TeamAPIController(CricketLeagueContext context)
        {
            _context = context;
        }
        #endregion
        #region Get All Teams
        [HttpGet]
        public IActionResult GetTeams(CricketLeagueContext _context)
        {
            var teams = _context.Teams.ToList();
            return Ok(teams);

        }
        #endregion
        #region GetTeamById
        [HttpGet("{id}")]
        public IActionResult GetTeamById(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }
        #endregion
        #region AddTeam
        [Authorize]
        [HttpPost("AddTeam")]
       
        public async Task<IActionResult> AddTeam([FromBody] Team team)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin == null)
            {
                return Unauthorized("Only Admin can add team");
            }
            team.Created = DateTime.Now;
            team.Modified = DateTime.Now;
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeamById), new { id = team.TeamId }, team);
        }
        #endregion
        #region Delete
        [Authorize]
        [HttpDelete("{id}")]
  
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Unauthorized("Only Admin can delete team");
            }
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
        #region Update
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, Team team)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True") {
                return Unauthorized("Only Admin can update team");
            }
            if (id != team.TeamId)
            {
                return BadRequest();
            }
            var existingTeam = await _context.Teams.FindAsync(id);
            if (existingTeam == null)
            {
                return NotFound();
            }
            existingTeam.TeamName = team.TeamName;
            existingTeam.TeamDescription = team.TeamDescription;
            existingTeam.Modified = DateTime.Now;
            _context.Entry(existingTeam).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}
