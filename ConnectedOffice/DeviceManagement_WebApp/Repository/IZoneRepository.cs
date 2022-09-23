using DeviceManagement_WebApp.Models;

namespace DeviceManagement_WebApp.Repository
{
    // Zone interface repository
    public interface IZoneRepository : IGenericRepository<Zone>
    {
        // Gets most recently added zone
        public Zone GetMostRecentZone();
    }
}
