using CrickerManagmentSystem_API_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrickerManagmentSystem_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchAPIController : ControllerBase
    {
        #region Dependencies
        private readonly CricketLeagueContext _context;
        public MatchAPIController(CricketLeagueContext context)
        {
            _context = context;
        }
        #endregion
        #region GetAllMatches
        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .Select(m => new
                {
                    m.MatchId,
                    m.TeamAid,
                    m.TeamBid,
                    m.MatchDate,
                    m.StartTime,
                    m.Venue,
                    m.Status,
                    m.WinnerTeamId,
                    m.Created,
                    m.Modified,
                    TeamAName = m.TeamA.TeamName,
                    TeamBName = m.TeamB.TeamName
                })
                .ToListAsync();
            return Ok(matches);
        }
        #endregion
        #region GetMatchById
        [HttpGet("{id}")]
        public IActionResult GetMatchById(int id)
        {
            var match = _context.Matches.Find(id);
            if (match == null)
            {
                return NotFound();
            }
            return Ok(match);
        }
        #endregion
        #region AddMatch
        [Authorize]
        [HttpPost("AddMatch")]
        public async Task<IActionResult> AddMatch([FromBody] Match match)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Forbid("You are not authorized to add matches.");
            }
            match.Created = DateTime.Now;
            match.Modified = DateTime.Now;
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMatchById), new { id = match.MatchId }, match);
        }
        #endregion
        #region UpdateMatch
        [Authorize]
        [HttpPut("UpdateMatch/{id}")]
        public async Task<IActionResult> UpdateMatch(int id, Match match)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Forbid("You are not authorized to Update matches.");
            }
            if (id != match.MatchId)
            {
                return BadRequest("Match ID mismatch.");
            }
            var existingMatch = await _context.Matches.FindAsync(id);

            if (existingMatch == null)
            {
                return NotFound("Match not found.");
            }

            existingMatch.TeamAid = match.TeamAid;
            existingMatch.TeamBid = match.TeamBid;
            existingMatch.MatchDate = match.MatchDate;
            existingMatch.StartTime = match.StartTime;
            existingMatch.Venue = match.Venue;
            existingMatch.Status = match.Status;
            existingMatch.WinnerTeamId = match.WinnerTeamId;
            existingMatch.Modified = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();

        }
        #endregion
        #region DeleteMatch
        [Authorize]
        [HttpDelete("DeleteMatch/{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isAdmin != "True")
            {
                return Forbid("You are not authorized to delete matches.");
            }
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound();

            // Delete related scores first
            var scores = _context.MatchScores.Where(ms => ms.MatchId == id);
            _context.MatchScores.RemoveRange(scores);

            // Delete match
            _context.Matches.Remove(match);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        #endregion
        #region UpcomingMatches
        [HttpGet("UpcomingMatches")]
        public async Task<ActionResult<IEnumerable<object>>> GetUpcomingMatches()
        {
            var upcomingmatches = await _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .Where(m => m.Status == "Upcoming")
                .Select(m => new
                {
                    m.MatchId,
                    m.TeamAid,
                    m.TeamBid,
                    m.MatchDate,
                    m.StartTime,
                    m.Venue,
                    m.Status,
                    m.WinnerTeamId,
                    m.Created,
                    m.Modified,
                    TeamAName = m.TeamA.TeamName,
                    TeamBName = m.TeamB.TeamName
                })
                .ToListAsync();

            return Ok(upcomingmatches);
        }
        #endregion


        #region CompletedMatches
        [HttpGet("CompletedMatches")]
        public async Task<ActionResult<IEnumerable<object>>> GetCompletedMatches()
        {
            var upcomingmatches = await _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .Include(m => m.WinnerTeam)
                .Where(m => m.Status == "Completed")
                .Select(m => new
                {
                    m.MatchId,
                    m.TeamAid,
                    m.TeamBid,
                    m.MatchDate,
                    m.StartTime,
                    m.Venue,
                    m.Status,
                    m.WinnerTeamId,
                    m.Created,
                    m.Modified,
                    TeamAName = m.TeamA.TeamName,
                    TeamBName = m.TeamB.TeamName,
                    Winner = m.WinnerTeam.TeamName
                })
                .ToListAsync();

            return Ok(upcomingmatches);
        }
        #endregion

        #region GetPageMatches
        [HttpGet("GetPageMatches")]
        public async Task<IActionResult> GetPlayerMatches(int pagenumber = 1, int pagesize = 6, string? status = null)
        {
            var query = _context.Matches
                .Include(m => m.TeamA)
                .Include(m => m.TeamB)
                .AsQueryable();
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(m => m.Status == status);
            }
            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pagesize);
            var matches = await query
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .Select(m => new
                {
                    m.MatchId,
                    m.TeamAid,
                    m.TeamBid,
                    m.MatchDate,
                    m.StartTime,
                    m.Venue,
                    m.Status,
                    m.WinnerTeamId,
                    m.Created,
                    m.Modified,
                    TeamAName = m.TeamA.TeamName,
                    TeamBName = m.TeamB.TeamName
                })
                .ToListAsync();
            var response = new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = pagenumber,
                PageSize = pagesize,
                Matches = matches
            };
            return Ok(response);
        }
        #endregion
    }
}
