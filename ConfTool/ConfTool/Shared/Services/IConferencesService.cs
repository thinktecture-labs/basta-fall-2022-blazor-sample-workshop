using ConfTool.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConfTool.Shared.Services
{
    [ServiceContract]
    public interface IConferencesService
    {
        Task<List<ConferenceOverview>> GetConferencesAsync();
        Task<ConferenceResult> GetConferenceByIdAsync(ConferenceRequestModel request);
        Task<ConferenceDetail> AddNewConferenceAsync(ConferenceDetail conference);
        Task UpdateConferenceAsync(ConferenceDetail conference);
    }
}
