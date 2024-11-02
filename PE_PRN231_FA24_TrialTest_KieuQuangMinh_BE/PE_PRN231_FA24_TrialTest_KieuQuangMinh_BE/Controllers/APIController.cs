using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories.Common;
using Repositories.Models;
using Repositories.Repositories;
using Repositories.RequestModel;
using Services;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PE_PRN231_FA24_TrialTest_KieuQuangMinh_BE.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class APIController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<APIController> _logger;
        private readonly VirusCureUserService _VCUService;
        private readonly PersonService _PService;

        public APIController(ILogger<APIController> logger, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _VCUService = new VirusCureUserService();
            _PService = new PersonService(mapper);

        }

        private string GenerateJwtToken(ViroCureUser account)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
            };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(120),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] ViroCureUserRequestModel person)
        {
            try
            {
                var response = _VCUService.Login(new ViroCureUser { Email = person.Email, Password = person.Password });
                return Ok(new
                {
                    message = "Login successful",
                    token = GenerateJwtToken(response),
                    user = new{
                        id=response.UserId,
                        email=response.Email,
                        role = ((RoleEnum)response.Role).ToString(),
                }
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new {error = ex.Message });
            }
        }

        [Authorize(Roles = "3")]
        [HttpPost("person")]
        public IActionResult CreatePerson(PersonRequestModel person)
        {
            try
            {
                person.PersonID = RandomNumberGenerator.GetInt32(0, 10000000);
                var response = _PService.CreatePerson(person);
                return Ok(new
                {
                    personId = response,
                    message = "Person and viruses added successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "1,2,3")]
        [HttpGet("person/{id}")]
        public IActionResult GetPersonById(int id)
        {
            try
            {
                var response = _PService.GetPerson(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "1,3")]
        [HttpGet("persons")]
        public IActionResult GetPersons()
        {
            try
            {
                var response = _PService.GetPersons();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "3")]
        [HttpPut("person/{id}")]
        public IActionResult PutPerson(int id, [FromBody]PersonUpdateRequestModel model)
        {
            try
            {
                _PService.UpdatePerson(id, model);
                return Ok(new{ message= "Person and viruses updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "3")]
        [HttpDelete("person/{id}")]
        public IActionResult DeletePerson(int id)
        {
            try
            {
                _PService.DeletePerson(id);
                return Ok(new { message = "Person and related viruses deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "1,3")]
        [HttpGet("viruses")]
        public IActionResult GetViruses()
        {
            try
            {
                var vService = new VirusService();
                return Ok(vService.GetViruses().Select(c => c.VirusName));
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }
    }
}
