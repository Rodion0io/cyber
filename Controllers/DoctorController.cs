using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;


namespace hospital_api.Controllers
{
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {
        private readonly IDoctorServic _doctorServic;

        public DoctorController(IDoctorServic doctorServic)
        {
            _doctorServic = doctorServic;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] DoctorRegisterModel model)
        {
            if (model == null)
            {
                return BadRequest("error");
            }
        
            try
            {
                await _doctorServic.Registeration(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsModel loginRequest)
        {
            try
            {
                var token = _doctorServic.Login(loginRequest.email, loginRequest.password);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //Вопрос в заголовке Authorization будет только одно же тело, отвечающее за токен?
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
        
            string token = authHeader.ToString().Split(" ")[1];
        
            _doctorServic.GetDataInClaim(token);
        
            //почему так не работает?
            // HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            
            return Ok();
        
        }
        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            
            // Так стоит получать id для дальнеших операций?
            var authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
        
            DoctorModel doctor = _doctorServic.GetDoctorInfa(token);
            
            return Ok(_doctorServic.GetDoctorInfa(token));
        }
        
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> PutProfile([FromBody] DoctorEditModel model)
        {
            // Так стоит получать id для дальнеших операций?
            var authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
            
            
            await _doctorServic.ChangeDatas(model, token);
        
            return Ok();
        }
    }
}