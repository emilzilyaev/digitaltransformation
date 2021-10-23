using AutoMapper;
using AutoMapper.Configuration;
using RecomendationForStartups.ServiceModel;
using RecomendationForStartups.ServiceModel.Types;

namespace RecomendationForStartups
{
    public class AppModelMapping
    {
        // TODO : IoC IMapper
        public static Mapper ConfigureMapping()
        {
            var cfg = new MapperConfigurationExpression();
            ConfigureModelMapping(cfg);
            var mapperConfiguration = new MapperConfiguration(cfg);
            mapperConfiguration.AssertConfigurationIsValid();
            mapperConfiguration.CompileMappings();
            var mapper = new Mapper(mapperConfiguration);
            return mapper;
        }

        private static void ConfigureModelMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Domain.ParameterDefinition, ParameterDefinition>();
            cfg.CreateMap<Domain.ParameterType, ParameterType>();
            cfg.CreateMap<Domain.ParameterValue, ParameterValue>().ReverseMap();
            cfg.CreateMap<Domain.ParametersCombination, ParametersCombination>();
            cfg.CreateMap<Domain.Recommendation, RecommendationInfo>()
                .ForMember(m => m.MatchPercentage, e => e.Ignore());
        }
    }
}
