﻿using System.Linq;
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

        public UserSensor GetBySensorId(string sensorId)
        {
            var result = _userSensors
                .All()
                .FirstOrDefault(us => us.Id == sensorId);

            return result;
        }

        public IQueryable<UserSensor> AllByUserId(string userId)
        {
            var result = _userSensors
                .All()
                .Where(us => us.TempStationUserId == userId);

            return result;
        }

        public async Task<int> Add(UserSensor userSensor)
        {
            _userSensors.Add(userSensor);
            int result = await _userSensors.SaveChangesAsync();
            return result;
        }

        public async Task<int> DeleteById(string id)
        {
            _userSensors.Delete(id);
            var result = await _userSensors.SaveChangesAsync();
            return result;
        }
    }
}
