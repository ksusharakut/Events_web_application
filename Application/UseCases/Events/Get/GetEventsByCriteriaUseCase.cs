using Application.UseCases.DTOs;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Get
{
    public class GetEventsByCriteriaUseCase : IGetEventsByCriteriaUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventsByCriteriaUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EventReturnDTO>> ExecuteAsync(DateTime? date, string? location, string? category, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventRepository.GetEventsByFiltersAsync(date, location, category, cancellationToken);
            return _mapper.Map<List<EventReturnDTO>>(events);
        }
    }
}
