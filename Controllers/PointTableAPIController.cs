using CrickerManagmentSystem_API_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrickerManagmentSystem_API_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointTableAPIController : ControllerBase
    {
        #region Dependencies
        private readonly CricketLeagueContext _context;
        public PointTableAPIController(CricketLeagueContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllPointTables
        [HttpGet]
        public async Task<IActionResult> GetPoinTable()
        {
            var pointTables = await _context.PointTables.ToListAsync();
            return Ok(pointTables);
        }
        #endregion

        #region GetPointTableById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPointTableById(int id)
        {
            var pointTable = await _context.PointTables.FindAsync(id);
            if (pointTable == null)
            {
                return NotFound();
            }
            return Ok(pointTable);
        }
        #endregion

        //#region AddPointTable
        //[HttpPost("AddPointTable")]
        //public async Task<IActionResult> AddPointTable(PointTable pointTable)
        //{
        //    pointTable.Created = DateTime.Now;
        //    _context.PointTables.Add(pointTable);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetPointTableById), new { id = pointTable.PointTableId }, pointTable);
        //}
        //#endregion

        //#region UpdatePointTable
        //[HttpPut("UpdatePointTable/{id}")]
        //public async Task<IActionResult> UpdatePointTable(int id, PointTable pointTable)
        //{
        //    if (id != pointTable.PointTableId)
        //    {
        //        return BadRequest("Point table ID mismatch");
        //    }
        //    pointTable.Modified = DateTime.Now;
        //    _context.Entry(pointTable).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
        //#endregion

        #region DynamicPointTable
        [HttpGet("dynamic")]
        public async Task<IActionResult> GetDynamicPointTable()
        {
            var teams = await _context.Teams
                .Select(team => new
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,

                    MatchPlayed = _context.Matches
                        .Count(m => m.Status == "Completed" && (m.TeamAid == team.TeamId || m.TeamBid == team.TeamId)),

                    Win = _context.Matches
                        .Count(m => m.Status == "Completed" && m.WinnerTeamId == team.TeamId),

                    Loss = _context.Matches
                        .Count(m => m.Status == "Completed"
                                 && (m.TeamAid == team.TeamId || m.TeamBid == team.TeamId)
                                 && m.WinnerTeamId != null && m.WinnerTeamId != team.TeamId),

                    
                    Points = _context.Matches
                        .Count(m => m.Status == "Completed" && m.WinnerTeamId == team.TeamId) * 2
                        + _context.Matches
                        .Count(m => m.Status == "Completed"
                                 && (m.TeamAid == team.TeamId || m.TeamBid == team.TeamId)
                                 && m.WinnerTeamId == null) // tie +1
                })
                .OrderByDescending(t => t.Points)
                .ThenBy(t => t.TeamName)
                .ToListAsync();

            return Ok(teams);
        }
        #endregion
    }
}
