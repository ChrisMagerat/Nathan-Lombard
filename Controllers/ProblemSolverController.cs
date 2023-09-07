using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nathan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemSolverController : ControllerBase
    {
        [HttpGet]
        [Route("GetAmountSeconds")]
        public async Task<IActionResult> GetAmountSeconds(string word)
        {
            try
            {

                string alphabet = "abcdefhijklmnopqrstuvwxyz";

                
                int currentPosition = 0;
                
                
                double timeTotal = 0;

                foreach (char c in word)
                {
                    
                    int target = alphabet.IndexOf(c);

                    
                    int clockwiseTime = Math.Abs(target - currentPosition) * 5;
                    int counterclockwiseTime = (25 - Math.Abs(target - currentPosition)) * 5;

                  
                    int moveTime = Math.Min(clockwiseTime, counterclockwiseTime);

  
                    if (moveTime == 0)
                    {
                        timeTotal += 2.5;
                    }
                    else
                    {
                        timeTotal += moveTime;
                    }

                    currentPosition = target;
                }


                return Ok(timeTotal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
