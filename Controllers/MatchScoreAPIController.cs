using CrickerManagmentSystem_API_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrickerManagmentSystem_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchScoreAPIController : ControllerBase
    {
        #region Dependencies
        private readonly CricketLeagueContext _context;
        public MatchScoreAPIController(CricketLeagueContext context)
        {
            _context = context;
        }
        #endregion
        #region GetAllMatchScores
        [HttpGet]
        public async Task<IActionResult> GetAllMatchScores()
        {
            return Ok(await _context.MatchScores
                .Include(ms => ms.Match)
                .Include(ms => ms.Team)
                .Include(ms => ms.User)
                .Select(ms => new
                {
                    ms.ScoreId,
                    ms.MatchId,
                    ms.TeamId,
                    ms.Runs,
                    ms.Wicket,
                    ms.Over,
                    ms.Created,
                    ms.Modified,
                    TeamName = ms.Team.TeamName,
                })
                .ToListAsync());
        }
        #endregion
        #region GetMatchScoreById
        [HttpGet("{id}")]
        public IActionResult GetMatchScoreById(int id)
        {
            var matchScore = _context.MatchScores.Find(id);
            if (matchScore == null)
            {
                return NotFound();
            }
            return Ok(matchScore);
        }
        #endregion
        #region AddMatchScore
        [Authorize]
        [HttpPost("AddMatchScore")]
        public async Task<IActionResult> AddMatchScore(MatchScore matchScore)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Unauthorized("Only admins can add match scores");
            }
            if (matchScore == null)
            {
                return BadRequest("Match score cannot be null");

            }
            matchScore.Created = DateTime.Now;
            matchScore.Modified = DateTime.Now;
            _context.MatchScores.Add(matchScore);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMatchScoreById), new { id = matchScore.ScoreId }, matchScore);
        }
        #endregion
        #region UpdateMatchScore
        [Authorize]
        [HttpPut("UpdateMatchScore/{id}")]
        public async Task<IActionResult> UpdateMatchScore(int id , MatchScore matchScore)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True")
            {
                return Unauthorized("Only admins can update match scores");
            }
            if (id != matchScore.ScoreId)
            {
                return BadRequest("Match score ID mismatch");
            }
            matchScore.Modified = DateTime.Now;
            _context.Entry(matchScore).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
        #region DeleteMatchScore
        [Authorize]
        [HttpDelete("DeleteMatchScore/{id}")]
        public async Task<IActionResult> DeleteMatchScore(int id)
        {
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
            if (isAdmin != "True") {
                return Unauthorized("Only admins can delete match scores");
            }
            var matchScore = _context.MatchScores.Find(id);
            if (matchScore == null)
            {
                return NotFound();
            }
            _context.MatchScores.Remove(matchScore);
            _context.SaveChanges();
            return NoContent();
        }
        #endregion
    }
}
