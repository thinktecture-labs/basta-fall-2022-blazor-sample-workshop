using AutoMapper;
using ConfTool.Client.Features.Conferences;
using ConfTool.Server.Hubs;
using ConfTool.Server.Models;
using ConfTool.Shared.DTO;
using ConfTool.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ConfTool.Server.GrpcServices
{
    [Authorize]
    public class ConferencesService : IConferencesService
    {
        private ConferencesDbContext context;
        private IMapper mapper;
        private IHubContext<ConferencesHub> hub;

        public ConferencesService(ConferencesDbContext context, IMapper mapper,
            IHubContext<ConferencesHub> hub)
        {
            this.context = context;
            this.mapper = mapper;
            this.hub = hub;
        }

        public async Task<ConferenceDetail> AddNewConferenceAsync(ConferenceDetail conference)
        {
            var conf = mapper.Map<Conference>(conference);

            var entry = context.Conferences.Add(conf);
            await context.SaveChangesAsync();

            await hub.Clients.All.SendAsync("NewConferenceAdded", entry.Entity.ID);
            
            return mapper.Map<ConferenceDetail>(conf);

        }

        public async Task<ConferenceResult> GetConferenceByIdAsync(ConferenceRequestModel request)
        {
            var result = await context.Conferences.FirstOrDefaultAsync(c => c.ID == request.Id);
            
            var conf = mapper.Map<ConferenceDetail>(result);

            return new ConferenceResult
            {
                Successfull = false,
                Error = "Das ist ein Error"
            };
        }

        public async Task<List<ConferenceOverview>> GetConferencesAsync()
        {
            var confs = await context.Conferences.ToListAsync();
            var result = mapper.Map<List<ConferenceOverview>>(confs);

            return result;
        }

        public async Task UpdateConferenceAsync(ConferenceDetail conference)
        {
            var conf = mapper.Map<Conference>(conference);
            context.Entry(conf).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Conferences.Any(c => c.ID == conf.ID))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
