using Jobsity.Chatbot.Api.Features.Login.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Jobsity.Chatbot.Api.Features.Login
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IMediator mediator;

        public LoginController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(LoginModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> LoginAsync([FromBody] Create.CommandRequest command)
        {
            return await this.mediator.Send(command).ConfigureAwait(false);
        }
    }
}