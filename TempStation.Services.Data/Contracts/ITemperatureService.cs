﻿using System;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;

namespace TempStation.Services.Data.Contracts
{
    public interface ITemperatureService
    {
        IQueryable<SensorTemperature> All();

        IQueryable<SensorTemperature> GetByTimeIntervalGroupedByHour(DateTime from, DateTime? To = null, string sensorId = null);

        Task<SensorTemperature> GetLatest();

        Task<int> AddAsync(SensorTemperature temperature);
    }
}
