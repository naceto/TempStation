using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Data.Repositories;
using TempStation.Services.Data.Contracts;

namespace TempStation.Services.Data
{
    public class UserSensorsService : IUserSensorsService
    {
        private readonly IRepository<UserSensor> _userSensors;

        public UserSensorsService(IRepository<UserSensor> userSensors)
        {
            _userSensors = userSensors;
        }

        public IQueryable<UserSensor> AllByUserId(string id)
        {
            return _userSensors.All().Where(us => us.TempStationUserId == id);
        }

        public async Task<int> Add(UserSensor userSensor)
        {
            _userSensors.Add(userSensor);
            int result = await _userSensors.SaveChangesAsync();
            return result;
        }
    }
}
