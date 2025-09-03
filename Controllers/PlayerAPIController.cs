using CrickerManagmentSystem_API_.Helper;
using CrickerManagmentSystem_API_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrickerManagmentSystem_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerAPIController : ControllerBase
    {
        #region Dependencies
        private readonly CricketLeagueContext _context;
        public PlayerAPIController(CricketLeagueContext context)
        {
            _context = context;
        }
        #endregion
        #region GetAllPlayers
        [HttpGet]
        public async Task<IActionResult> GetPlayer()
        {
            var players = await _context.Players
                .Include(p => p.Team)
                .Select(p => new
                {
                    p.PlayerId,
                    p.PlayerName,
                    p.Role,
                    p.DateOfBirth,
                    p.Nationality,
                    p.Gender,
                    p.Image,
                    p.TeamId,
                    TeamName = p.Team.TeamName
                })
                .ToListAsync();

            return Ok(players);
        }
        #endregion
        #region GetPlayerById
        [HttpGet("{id}")]
        public IActionResult GetPlayerById(int id)
        {
            var player = _context.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }
        #endregion
        #region AddPlayer
        [Authorize]
        [HttpPost("AddPlayer")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddPlayer([FromForm] Player player)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Unauthorized("Only Admin add player");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (player.File != null)
            {
                var relativepath = ImageHelper.SaveImageToFile(player.File, "Images/Players");
                player.Image = relativepath;
            }
            player.Created = DateTime.Now;
            player.Modified = DateTime.Now;
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlayerById), new { id = player.PlayerId }, player);
        }
        #endregion
        #region UpdatePlayer
        [Authorize]
        [HttpPut("UpdatePlayer/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromForm] Player player)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Unauthorized("Only Admin can update player");
            }
            var existingPlayer = await _context.Players.FindAsync(id);
            if (existingPlayer == null)
            {
                return NotFound();
            }
            if (player.File != null)
            {
                if (!string.IsNullOrEmpty(existingPlayer.Image))
                {
                    string fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPlayer.Image);
                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                }
                var relativepath = ImageHelper.SaveImageToFile(player.File, "Images/Players");
                player.Image = relativepath;
            }

            existingPlayer.Nationality = player.Nationality;
            existingPlayer.PlayerName = player.PlayerName;
            existingPlayer.Role = player.Role;
            existingPlayer.DateOfBirth = player.DateOfBirth;
            existingPlayer.TeamId = player.TeamId;
            existingPlayer.Gender = player.Gender;
            existingPlayer.Image = player.Image;
            existingPlayer.Modified = DateTime.Now;



            await _context.SaveChangesAsync();
            return NoContent();
        }

        #endregion
        #region DeletePlayer
        [Authorize]
        [HttpDelete("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Unauthorized("Only Admin can delete player");
            }
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(player.Image))
            {
                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", player.Image);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
            }
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region TeamDropDown
        [HttpGet("Team/DropDown")]
        public async Task<ActionResult<IEnumerable<object>>> GetTeams()
        {
            return await _context.Teams
                .Select(t => new { t.TeamId, t.TeamName })
                .ToListAsync();

        }
        #endregion

        #region PlayerDropDown
        [HttpGet("Player/DropDown")]

        public async Task<ActionResult<IEnumerable<object>>> GetPlayerDropDOwn()
        {
            return await _context.Players
                .Select(t => new { t.PlayerId, t.PlayerName })
                .ToListAsync();

        }
        #endregion

        #region GetpagePlayer
        [HttpGet("GetpagePlayer")]
        public async Task<IActionResult> GetPagePlayer(int pagenumber = 1, int pagesize = 10)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            if (pagesize < 1)
            {
                pagesize = 10;
            }

            var totalPlayers = await _context.Players.CountAsync();
            var players = _context.Players
                            .OrderBy(p => p.PlayerId)
                            .Skip((pagenumber - 1) * pagesize)
                            .Take(pagesize)
                            .ToListAsync();

            var result = new
            {
                TotalPlayers = totalPlayers,
                PageNumber = pagenumber,
                PageSize = pagesize,
                Players = await players
            };

            return Ok(result);

        }
        #endregion

        #region Filtering
        [HttpGet("FilterPlayers")]
        public async Task<IActionResult> GetPlayerFilter(
            [FromQuery] string? role,
            [FromQuery] string? name,
            [FromQuery] int pagenumber = 1,
            [FromQuery] int pagesize = 10)
        {
            var query = _context.Players
                .Include(p => p.Team) // include Team to get TeamName
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(p => p.Role.Contains(role));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.PlayerName.Contains(name));
            }

            // Total count before pagination
            var totalPlayers = await query.CountAsync();

            // Pagination
            var players = await query
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .Select(p => new
                {
                    p.PlayerId,
                    p.PlayerName,
                    p.Role,
                    p.Nationality,
                    p.Gender,
                    p.DateOfBirth,
                    p.Image,
                    TeamName = p.Team.TeamName
                })
                .ToListAsync();

            var response = new
            {
                players,
                totalPlayers,
                pageNumber = pagenumber,
                pageSize = pagesize
            };

            return Ok(response);
        }
        #endregion

    }
}