using AutoMapper;
using ConfTool.Server.Hubs;
using ConfTool.Server.Models;
using ConfTool.Shared.DTO;
using ConfTool.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ConfTool.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ConferencesController: ControllerBase
    {
        private readonly ConferencesDbContext context;
        private readonly IMapper mapper;
        private readonly IHubContext<ConferencesHub> hub;

        public ConferencesController(ConferencesDbContext context, IMapper mapper,
            IHubContext<ConferencesHub> hub)
        {
            this.context = context;
            this.mapper = mapper;
            this.hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> GetConferencesAsync()
        {
            var confs = await context.Conferences.ToListAsync();
            var result = mapper.Map<List<ConferenceOverview>>(confs);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConferenceDetails([FromRoute] Guid id)
        {
            var result = await context.Conferences.FirstOrDefaultAsync(c => c.ID == id);
            var conf = mapper.Map<ConferenceDetail>(result);
            return Ok(conf);
        }

        [HttpPost]
        public async Task<IActionResult> AddConferenceDetails([FromBody] ConferenceDetail conference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conf = mapper.Map<Conference>(conference);
            var entry = await context.Conferences.AddAsync(conf);
            await context.SaveChangesAsync();

            await hub.Clients.All.SendAsync(SignalRMethodNames.AddedConference, entry.Entity.ID);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConferenceDetails([FromRoute] Guid id, [FromBody] ConferenceDetail conference)
        {
            if (conference == null)
            {
                return BadRequest();
            }

            var conf = mapper.Map<Conference>(conference);
            context.Entry(conf).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConferenceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ConferenceExists(Guid id)
        {
            return context.Conferences.Any(e => e.ID == id);
        }
    }
}
