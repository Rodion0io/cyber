using Microsoft.AspNetCore.Mvc;
using hospital_api.Modules;
using hospital_api.Services;
using hospital_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
                
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _doctorServic.Registeration(model);
            return Ok();
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginCredentialsModel loginRequest) // аналогично
        {
            
            var token = _doctorServic.Login(loginRequest.email, loginRequest.password);
            // Console.WriteLine($"{loginRequest.email}, {loginRequest.password}");
            return Ok(token);
        }
        
    }
    

        
}
