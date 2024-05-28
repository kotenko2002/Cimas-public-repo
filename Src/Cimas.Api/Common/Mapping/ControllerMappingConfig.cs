using Cimas.Api.Contracts;
using Cimas.Api.Contracts.Cinemas;
using Cimas.Api.Contracts.Films;
using Cimas.Api.Contracts.Halls;
using Cimas.Api.Contracts.Products;
using Cimas.Api.Contracts.Reports;
using Cimas.Api.Contracts.Sessions;
using Cimas.Api.Contracts.Tickets;
using Cimas.Api.Contracts.Users;
using Cimas.Application.Features.Cinemas.Commands.CreateCinema;
using Cimas.Application.Features.Cinemas.Commands.UpdateCinema;
using Cimas.Application.Features.Films.Commands.CreateFilm;
using Cimas.Application.Features.Halls.Commands.CreateHall;
using Cimas.Application.Features.Halls.Commands.UpdateHallSeats;
using Cimas.Application.Features.Products.Commands.CreateProduct;
using Cimas.Application.Features.Products.Commands.UpateProduct;
using Cimas.Application.Features.Sessions.Commands.CreateSession;
using Cimas.Application.Features.Sessions.Queries.GetSessionsByRange;
using Cimas.Application.Features.Tickets.Commands.CreateTickets;
using Cimas.Application.Features.Tickets.Commands.UpdateTickets;
using Cimas.Application.Features.Users.Commands.RegisterNonOwner;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Models.Auth;
using Mapster;

namespace Cimas.Api.Common.Mapping
{
    public static class ControllerMappingConfig
    {
        public static TypeAdapterConfig AddControllerMappingConfig(this TypeAdapterConfig config)
        {
            config
                .AddAuthControllerConfig()
                .AddCinemaControllerConfig()
                .AddFilmControllerConfig()
                .AddHallControllerConfig()
                .AddSessionControllerConfig()
                .AddTicketControllerConfig()
                .AddUserControllerConfig()
                .AddProductControllerConfig()
                .AddReportControllerConfig();

            return config;
        }

        private static TypeAdapterConfig AddAuthControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<AuthModel, AuthResponse>()
                .Map(dest => dest.AccessToken, src => src.AccessToken)
                .Map(dest => dest.User.FullName, src => $"{src.User.LastName} {src.User.FirstName}")
                .Map(dest => dest.User.Roles, src => src.Roles);

            return config;
        }

        private static TypeAdapterConfig AddCinemaControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid UserId, CreateCinemaRequest Requset), CreateCinemaCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<(Guid UserId, Guid CinemaId, UpdateCinemaRequest Requset), UpdateCinemaCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.CinemaId, src => src.CinemaId)
                .Map(dest => dest, src => src.Requset);

            return config;
        }

        private static TypeAdapterConfig AddFilmControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid UserId, Guid CinemaId, CreateFilmRequest Requset), CreateFilmCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.CinemaId, src => src.CinemaId)
                .Map(dest => dest, src => src.Requset);

            return config;
        }

        private static TypeAdapterConfig AddHallControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid UserId, Guid CinemaId, CreateHallRequest Requset), CreateHallCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.CinemaId, src => src.CinemaId)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<(Guid UserId, Guid HallId, UpdateHallSeatsRequst Requset), UpdateHallSeatsCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.HallId, src => src.HallId)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<Hall, HallResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.NumberOfSeats, src => src.Seats.Where(s => s.Status != HallSeatStatus.NotExists).Count());

            return config;
        }

        private static TypeAdapterConfig AddSessionControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid UserId, CreateSessionRequest Requset), CreateSessionCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<(Guid UserId, GetSessionsByRangeRequest Requset), GetSessionsByRangeQuery>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<Session, SessionResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.StartDateTime, src => src.StartDateTime)
                .Map(dest => dest.EndDateTime, src => src.StartDateTime + src.Film.Duration)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.HallName, src => src.Hall.Name)
                .Map(dest => dest.FilmName, src => src.Film.Name);

            return config;
        }

        private static TypeAdapterConfig AddTicketControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid UserId, Guid SessionId, CreateTicketsRequest Requset), CreateTicketsCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.SessionId, src => src.SessionId)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<(Guid UserId, UpdateTicketsRequest Requset), UpdateTicketsCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Requset);

            return config;
        }

        private static TypeAdapterConfig AddUserControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid UserId, RegisterNonOwnerRequest Requset), RegisterNonOwnerCommand>()
                .Map(dest => dest.OwnerUserId, src => src.UserId)
                .Map(dest => dest, src => src.Requset);

            return config;
        }

        private static TypeAdapterConfig AddProductControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid UserId, Guid CinemaId, CreateProductRequest Requset), CreateProductCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.CinemaId, src => src.CinemaId)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<(Guid UserId, UpdateProductsRequest Requset), UpateProductsCommand>()
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest, src => src.Requset);

            return config;
        }

        private static TypeAdapterConfig AddReportControllerConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<Report, ReportResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.StartDateTime, src => src.WorkDay.StartDateTime)
                .Map(dest => dest.EndDateTime, src => src.WorkDay.EndDateTime)
                .Map(dest => dest.Status, src => src.Status);

            return config;
        }
    }
}
