using Cimas.Application.Features.Auth.Commands.Register;
using Cimas.Application.Features.Auth.Commands.RegisterOwner;
using Cimas.Application.Features.Users.Commands.RegisterNonOwner;
using Mapster;

namespace Cimas.Api.Common.Mapping
{
    public static class HandlerMappingConfig
    {
        public static TypeAdapterConfig AddHandlerMappingConfig(this TypeAdapterConfig config)
        {
            config.NewConfig<(Guid CompanyId, string Role, RegisterOwnerCommand Requset), RegisterCommand>()
                .Map(dest => dest.CompanyId, src => src.CompanyId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest, src => src.Requset);

            config.NewConfig<(Guid CompanyId, string Role, RegisterNonOwnerCommand Requset), RegisterCommand>()
                .Map(dest => dest.CompanyId, src => src.CompanyId)
                .Map(dest => dest, src => src.Requset);

            return config;
        }
    }
}
