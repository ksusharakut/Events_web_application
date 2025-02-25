using Application.Common;
using Application.UseCases.Authorization.LogIn;
using Application.UseCases.Authorization.LogIn.Validators;
using Application.UseCases.Authorization.RefreshToken;
using Application.UseCases.Authorization.Register;
using Application.UseCases.Authorization.Register.Validators;
using Application.UseCases.DTOs;
using Application.UseCases.EventParticipant;
using Application.UseCases.EventParticipant.Create;
using Application.UseCases.EventParticipant.Delete;
using Application.UseCases.Events.Create;
using Application.UseCases.Events.Delete;
using Application.UseCases.Events.Get;
using Application.UseCases.Events.Update;
using Application.UseCases.Events.Validation;
using Application.UseCases.Participant.Get;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using FluentValidation;
using Infrastructure.Mapping;
using Infrastructure.Persistance.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }


        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(true);
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IParticipantEventRepository, ParticipantEventRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILogInParticipantUseCase, LogInParticipantUseCase>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<IRegisterParticipantUseCase, RegisterParticipantUseCase>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IUserValidationService, UserValidationService>();
            services.AddValidatorsFromAssemblyContaining<RegisterParticipantValidator>();
            services.AddScoped<ICreateEventUseCase, CreateEventUseCase>();
            services.AddScoped<IGetEventUseCase, GetEventUseCase>();
            services.AddScoped<IGetEventByTitleUseCase, GetEventByTitleUseCase>();
            services.AddScoped<IDeleteEventUseCase, DeleteEventUseCase>();
            services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();
            services.AddScoped<IGetAllEventsUseCase, GetAllEventsUseCase>();
            services.AddScoped<IImagePathService, ImagePathService>();
            services.AddScoped<IGetParticipantUseCase, GetParticipantUseCase>();
            services.AddScoped<IRegisterParticipantForEventUseCase, RegisterParticipantForEventUseCase>();
            services.AddScoped<IRemoveParticipantFromEventUseCase, RemoveParticipantFromEventUseCase>();
            services.AddScoped<IGetParticipantsForEventUseCase, GetParticipantsForEventUseCase>();
            services.AddScoped<IGetEventsByCriteriaUseCase, GetEventsByCriteriaUseCase>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddValidatorsFromAssemblyContaining<EventValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginParticipantValidator>();
        }

        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EventDTOToEventMappingProfile).Assembly);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"]
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole(Role.Admin.ToString()));
                options.AddPolicy("ParticipantOnly", policy =>
                    policy.RequireRole(Role.ParticipantOnly.ToString()));
            });
        }
    }
}
