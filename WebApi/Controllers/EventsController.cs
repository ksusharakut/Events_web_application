﻿using Application.UseCases.Events.Create;
using Application.UseCases.Events.Get;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.Events.Delete;
using Application.UseCases.Events.Update;
using Application.UseCases.DTOs;
using Domain.Entities;
using Application.UseCases.Participant.Get;
using Application.UseCases.EventParticipant.Create;
using Application.UseCases.EventParticipant.Delete;
using Application.UseCases.EventParticipant;

namespace WebApi.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ICreateEventUseCase _createEventUseCase;
        private readonly IGetEventUseCase _getEventUseCase;
        private readonly IGetEventByTitleUseCase _getEventByTitleUseCase;
        private readonly IDeleteEventUseCase _deleteEventUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IGetAllEventsUseCase _getAllEventsUseCase;
        private readonly IUploadEventImageUseCase _uploadEventImageUseCase;
        private readonly IRegisterParticipantForEventUseCase _registerParticipantForEventUseCase;
        private readonly IRemoveParticipantFromEventUseCase _removeParticipantFromEventUseCase;
        private readonly IGetParticipantsForEventUseCase _getParticipantsForEventUseCase;
        private readonly IGetEventsByCriteriaUseCase _getEventsByCriteriaUseCase;

        public EventsController(ICreateEventUseCase createEventUseCase, 
            IGetEventUseCase getEventUseCase, 
            IGetEventByTitleUseCase getEventByTitleUseCase,
            IDeleteEventUseCase deleteEventUseCase,
            IUpdateEventUseCase updateEventUseCase,
            IGetAllEventsUseCase getAllEventsUseCase,
            IUploadEventImageUseCase uploadEventImageUseCase,
            IRegisterParticipantForEventUseCase registerParticipantForEventUseCase,
            IRemoveParticipantFromEventUseCase removeParticipantFromEventUseCase,
            IGetParticipantsForEventUseCase getParticipantsForEventUseCase,
            IGetEventsByCriteriaUseCase getEventsByCriteriaUseCase
            )
        {
            _createEventUseCase = createEventUseCase;
            _getEventUseCase = getEventUseCase;
            _getEventByTitleUseCase = getEventByTitleUseCase;
            _deleteEventUseCase = deleteEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _getAllEventsUseCase = getAllEventsUseCase;
            _uploadEventImageUseCase = uploadEventImageUseCase;
            _registerParticipantForEventUseCase = registerParticipantForEventUseCase;
            _removeParticipantFromEventUseCase = removeParticipantFromEventUseCase;
            _getParticipantsForEventUseCase = getParticipantsForEventUseCase;
            _getEventsByCriteriaUseCase = getEventsByCriteriaUseCase;
        }

        [HttpGet("{eventId}/participants")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<List<Participant>>> GetParticipants(int eventId, CancellationToken cancellationToken)
        {
            var participants = await _getParticipantsForEventUseCase.ExecuteAsync(eventId, cancellationToken);
            return Ok(participants);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterParticipant([FromBody] RegisterParticipantDTO request, CancellationToken cancellationToken)
        {
            await _registerParticipantForEventUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Participant successfully registered for the event.");
        }

        [HttpDelete("unregister")]
        public async Task<ActionResult> UnregisterParticipant([FromBody] RegisterParticipantDTO request, CancellationToken cancellationToken)
        {
            await _removeParticipantFromEventUseCase.ExecuteAsync(request.EventId, cancellationToken);
            return Ok("Participant successfully unregistered from the event.");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO eventDto, CancellationToken cancellationToken)
        {
            await _createEventUseCase.ExecuteAsync(eventDto, cancellationToken);
            return Ok("Event created successfully.");
        }

        [HttpGet("byid/{eventId}")]
        public async Task<ActionResult> GetById(int eventId, CancellationToken cancellationToken)
        {
            return Ok(await _getEventUseCase.ExecuteAsync(eventId, cancellationToken));
        }

        [HttpGet("bytitle/{title}")]
        public async Task<ActionResult> GetByTitle(string title, CancellationToken cancellationToken)
        {
            return Ok(await _getEventByTitleUseCase.ExecuteAsync(title, cancellationToken));
        }

        [HttpDelete("{eventId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Delete(int eventId, CancellationToken cancellationToken)
        {
            await _deleteEventUseCase.ExecuteAsync(eventId, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDTO eventDto, CancellationToken cancellationToken)
        {
            await _updateEventUseCase.ExecuteAsync(id, eventDto, cancellationToken);
            return Ok("Event updated successfully.");
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAll(CancellationToken cancellationToken = default, int pageNumber = 1, int pageSize = 10)
        {
            var events = await _getAllEventsUseCase.ExecuteAsync(cancellationToken, pageNumber, pageSize);
            return Ok(events);
        }

        [HttpPost("{eventId}/upload-image")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadImage(int eventId, IFormFile imageFile, CancellationToken cancellationToken)
        {
            await _uploadEventImageUseCase.ExecuteAsync(eventId, imageFile, cancellationToken);
            return Ok("Image uploaded successfully.");
        }


        [HttpGet("filter")]
        public async Task<ActionResult<List<EventReturnDTO>>> GetEventsByCriteria(
       [FromQuery] DateTime? date,
       [FromQuery] string? location,
       [FromQuery] string? category,
       CancellationToken cancellationToken)
        {
            var events = await _getEventsByCriteriaUseCase.ExecuteAsync(date, location, category, cancellationToken);
            return Ok(events);
        }
    }
}