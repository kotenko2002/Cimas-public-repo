using Mapster;

namespace Cimas.Api.Common.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .AddControllerMappingConfig()
                .AddHandlerMappingConfig();
        }
    }
}
