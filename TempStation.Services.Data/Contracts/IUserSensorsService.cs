using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempStation.Data.Models;

namespace TempStation.Services.Data.Contracts
{
    public interface IUserSensorsService
    {
        IQueryable<UserSensor> AllByUserId(string id);

        Task<int> Add(UserSensor userSensor);

        Task<int> DeleteById(string id)
    }
}
