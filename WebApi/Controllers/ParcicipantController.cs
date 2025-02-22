using Application.UseCases.DTOs;
using Application.UseCases.EventParticipant.Create;
using Application.UseCases.EventParticipant.Delete;
using Application.UseCases.Participant.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/participant")]
    [ApiController]
    public class ParcicipantController : ControllerBase
    {
        private readonly IGetParticipantUseCase _getParticipantUseCase;
        private readonly IRegisterParticipantForEventUseCase _registerParticipantForEventUseCase;
        private readonly IRemoveParticipantFromEventUseCase _removeParticipantFromEventUseCase;

        public ParcicipantController(IGetParticipantUseCase getParticipantUseCase,
            IRegisterParticipantForEventUseCase registerParticipantForEventUseCase,
            IRemoveParticipantFromEventUseCase removeParticipantFromEventUseCase)
        {
            _getParticipantUseCase = getParticipantUseCase;
            _registerParticipantForEventUseCase = registerParticipantForEventUseCase;
            _removeParticipantFromEventUseCase = removeParticipantFromEventUseCase;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ParticipantReturnDTO>> GetParticipantById(int id, CancellationToken cancellationToken)
        {
            var participant = await _getParticipantUseCase.ExecuteAsync(id, cancellationToken);
            return Ok(participant);
        }

        [HttpPost("register")]
        [Authorize(Policy = "ParticipantOnly")]
        public async Task<ActionResult> RegisterParticipant([FromBody] RegisterParticipantDTO request, CancellationToken cancellationToken)
        {
            await _registerParticipantForEventUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Participant successfully registered for the event.");
        }

        [HttpDelete("unregister")]
        [Authorize(Policy = "ParticipantOnly")]
        public async Task<ActionResult> UnregisterParticipant([FromBody] RegisterParticipantDTO request, CancellationToken cancellationToken)
        {
            await _removeParticipantFromEventUseCase.ExecuteAsync(request.EventId, cancellationToken);
            return Ok("Participant successfully unregistered from the event.");
        }
    }
}
