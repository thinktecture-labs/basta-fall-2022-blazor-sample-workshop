using AutoMapper;

namespace ConfTool.Server.MapperProfiles
{
    public class ConferenceMapper : Profile
    {
        public ConferenceMapper()
        {
            CreateMap<Models.Conference, Shared.DTO.ConferenceOverview>();
            CreateMap<Shared.DTO.ConferenceOverview, Models.Conference>();
            CreateMap<Models.Conference, Shared.DTO.ConferenceDetail>();
            CreateMap<Shared.DTO.ConferenceDetail, Models.Conference>();
        }
    }
}
