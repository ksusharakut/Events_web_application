using Application.UseCases.DTOs;
using Application.UseCases.Participant.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/participant")]
    [ApiController]
    public class ParcicipantController : ControllerBase
    {
        private readonly IGetParticipantUseCase _getParticipantUseCase;

        public ParcicipantController(IGetParticipantUseCase getParticipantUseCase)
        {
            _getParticipantUseCase = getParticipantUseCase;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ParticipantReturnDTO>> GetParticipantById(int id, CancellationToken cancellationToken)
        {
            var participant = await _getParticipantUseCase.ExecuteAsync(id, cancellationToken);
            return Ok(participant);
        }
    }
}
