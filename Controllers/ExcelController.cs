//using System.ComponentModel;
//using CrickerManagmentSystem_API_.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Nest;
//using OfficeOpenXml;
////using OfficeOpenXml;

//namespace CrickerManagmentSystem_API_.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ExcelController : ControllerBase
//    {
//        private readonly CricketLeagueContext _context;

//        public ExcelController(CricketLeagueContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("players")]
//        public async Task<IActionResult> ExportToExcel()
//        {
//            //ExcelPackage.License = new OfficeOpenXml.License.License
//            //{
//            //    LicenseType = OfficeOpenXml.License.eLicenseType.NonCommercial
//            //};
//            //ExcelPackage.License = new OfficeOpenXml.LicenseContext.Li

//            var players = await _context.Players
//                .Include(p => p.Team)
//                .ToListAsync();

//            using (var package = new ExcelPackage())
//            {
//                var worksheet = package.Workbook.Worksheets.Add("Players");

//                worksheet.Cells[1, 1].Value = "PlayerId";
//                worksheet.Cells[1, 2].Value = "PlayerName";
//                worksheet.Cells[1, 3].Value = "TeamName";
//                worksheet.Cells[1, 4].Value = "Role";
//                worksheet.Cells[1, 5].Value = "DateOfBirth";
//                worksheet.Cells[1, 6].Value = "Nationality";
//                worksheet.Cells[1, 7].Value = "Gender";

//                for (int i = 0; i < players.Count; i++)
//                {
//                    var player = players[i];
//                    worksheet.Cells[i + 2, 1].Value = player.PlayerId;
//                    worksheet.Cells[i + 2, 2].Value = player.PlayerName;
//                    worksheet.Cells[i + 2, 3].Value = player.Team?.TeamName ?? "N/A";
//                    worksheet.Cells[i + 2, 4].Value = player.Role;
//                    worksheet.Cells[i + 2, 5].Value = player.DateOfBirth.ToString("yyyy-MM-dd");
//                    worksheet.Cells[i + 2, 6].Value = player.Nationality;
//                    worksheet.Cells[i + 2, 7].Value = player.Gender;
//                }

//                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

//                var excelBytes = package.GetAsByteArray();

//                return File(
//                    excelBytes,
//                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                    "Players.xlsx"
//                );
//            }
//        }
//    }
//}
