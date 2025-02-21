using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Domain.Exceptions;

namespace Application.UseCases.Participant.Get
{
    public class GetParticipantUseCase : IGetParticipantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ParticipantReturnDTO> ExecuteAsync(int participantId, CancellationToken cancellationToken)
        {
            var participantEntity = await _unitOfWork.EventRepository.GetByIdAsync(participantId, cancellationToken);
            if (participantEntity == null)
            {
                throw new NotFoundException($"No user with id {participantId}.");
            }

            return _mapper.Map<ParticipantReturnDTO>(participantEntity);
        }
    }
}
