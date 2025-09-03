//using CrickerManagmentSystem_API_.Helper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace CrickerManagmentSystem_API_.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QueryController : ControllerBase
//    {
//        private readonly IChatCompletionService _chatService;

//        public QueryController(IChatCompletionService chatService)
//        {
//            _chatService = chatService;
//        }

//        [HttpPost("text-to-sql")]
//        public async Task<IActionResult> ConvertToSQL([FromBody] QueryRequest request)
//        {
//            string schema = "Table Players(Id INT, Name VARCHAR, Team VARCHAR, Runs INT)";
//            string question = $"Convert this to SQL based on schema:\n{schema}\nQuery: {request.Query}";

//            string sql = await _chatService.GetChatCompletionAsync(question);

//            return Ok(new { SQL = sql });
//        }

//    }
//}

//public class QueryRequest
//{
//    public string Query { get; set; }
//}
