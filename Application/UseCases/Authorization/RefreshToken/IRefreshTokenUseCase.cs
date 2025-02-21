using Application.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Authorization.RefreshToken
{
    public interface IRefreshTokenUseCase
    {
        Task<AuthResultDTO> Handle(RefreshTokenRequest request, CancellationToken cancellationToken);
    }
}
