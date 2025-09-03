using CrickerManagmentSystem_API_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrickerManagmentSystem_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamWisePlayerAPIController : ControllerBase
    {
        #region Dependencies
        private readonly CricketLeagueContext _context;
        public TeamWisePlayerAPIController(CricketLeagueContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllTeamWisePlayers
        [HttpGet("GetAllTeamWisePlayers")]
        public async Task<IActionResult> GetAllTeamWisePlayers()
        {
            var data = await (from twp in _context.TeamWisePlayers
                              join p in _context.Players on twp.PlayerId equals p.PlayerId into playerJoin
                              from player in playerJoin.DefaultIfEmpty()
                              join t in _context.Teams on twp.TeamId equals t.TeamId into teamJoin
                              from team in teamJoin.DefaultIfEmpty()
                              select new
                              {
                                  teamWisePlayerId = twp.TeamWisePlayerId,
                                  playerId = twp.PlayerId,
                                  playerName = player != null ? player.PlayerName : "(Missing Player)",
                                  role = player != null ? player.Role : "(Missing Role)",
                                  imageurl = player != null ? player.Image : null,
                                  teamId = twp.TeamId,
                                  teamName = team != null ? team.TeamName : "(Missing Team)",
                                  userId = twp.UserId,
                                  created = twp.Created,
                                  modified = twp.Modified
                              }).ToListAsync();

            if (data.Count == 0)
            {
                return Ok(new { message = "No TeamWisePlayers found. Add rows in TeamWisePlayer table." });
            }

            return Ok(data);
        }
        #endregion

        #region GetTeamWisePlayerById
        [HttpGet("by-id/{id}")]
        public IActionResult GetTeamWisePlayerById(int id)
        {
            var teamWisePlayer = _context.TeamWisePlayers.Find(id);
            if (teamWisePlayer == null)
            {
                return NotFound();
            }
            return Ok(teamWisePlayer);
        }
        #endregion

        #region GetPlayersByTeamId
        [HttpGet("by-team/{teamId}")]
        public IActionResult GetPlayersByTeamId(int teamId)
        {
            var players = (from p in _context.Players
                           join twp in _context.TeamWisePlayers
                           on p.PlayerId equals twp.PlayerId
                           where twp.TeamId == teamId
                           select new
                           {
                               p.PlayerId,
                               p.PlayerName,
                               p.Image,
                               p.Role
                           }).ToList();

            if (!players.Any())
            {
                return NotFound(new { message = "No players found for this team." });
            }

            return Ok(players);
        }
        #endregion


        #region AddTeamWisePlayer
        [HttpPost("AddTeamWisePlayer")]
        public async Task<IActionResult> AddTeamWisePlayer(TeamWisePlayer teamWisePlayer)
        {
            teamWisePlayer.Created = DateTime.Now;
            teamWisePlayer.Modified = DateTime.Now;
            _context.TeamWisePlayers.Add(teamWisePlayer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeamWisePlayerById), new { id = teamWisePlayer.TeamWisePlayerId }, teamWisePlayer);
        }
        #endregion
        #region UpdateTeamWisePlayer
        [HttpPut("UpdateTeamWisePlayer/{id}")]
        public async Task<IActionResult> UpdateTeamWisePlayer(int id, TeamWisePlayer teamWisePlayer)
        {
            if (id != teamWisePlayer.TeamWisePlayerId)
            {
                return BadRequest("TeamWisePlayer ID mismatch.");
            }
            teamWisePlayer.Modified = DateTime.Now;
            _context.Entry(teamWisePlayer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
        #region DeleteTeamWisePlayer
        [HttpDelete("DeleteTeamWisePlayer/{id}")]
        public async Task<IActionResult> DeleteTeamWisePlayer(int id)
        {
            var teamWisePlayer = await _context.TeamWisePlayers.FindAsync(id);
            if (teamWisePlayer == null)
            {
                return NotFound();
            }
            _context.TeamWisePlayers.Remove(teamWisePlayer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
        #region GetPlayersByTeamId
        [HttpGet("player/{teamId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPlayerByTeamId(int teamId)
        {
            return await _context.Players
                .Where(s => s.TeamId == teamId)
                .Select(s=> new
                {
                    imageurl = string.IsNullOrEmpty(s.Image) ? null : "https://localhost:7232/" + s.Image,
                    s.PlayerId,
                    s.PlayerName,
                    s.Role
                })
                .ToListAsync();
        }
        #endregion
        
    }
}
