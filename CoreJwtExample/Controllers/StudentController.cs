using CoreJwtExample.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreJwtExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IStudentRepository studentRepository;

        public StudentController(IConfiguration configuration, IStudentRepository studentRepository)
        {
            this.configuration = configuration;
            this.studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("Gets1")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Gets1()
        {
            var list = await studentRepository.Gets1();
            return Ok(list);
        }

        [HttpGet]
        [Route("Gets2")]
        public async Task<IActionResult> Gets2()
        {
            var list = await studentRepository.Gets2();
            return Ok(list);
        }

    }
}
