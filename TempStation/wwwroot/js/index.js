var forecastHubconnection = new signalR.HubConnectionBuilder()
    .withUrl("/forecastHub")
    .withAutomaticReconnect()
    .build();

forecastHubconnection.on('ReceiveForecast', function (forecastData, tempSensorData) {
    updateForecastData(forecastData);
    updateSensorData(tempSensorData);
});

forecastHubconnection.start().catch(function (err) {
    return console.error(err.toString());
});

function updateForecastData(forecastData) {
    if (!forecastData) {
        return;
    }

    $('#forecast-icon').attr('src','http://openweathermap.org/img/wn/' + forecastData.icon + '@4x.png');
    $('#forecast-temp').text(forecastData.temperature);
    $('#forecast-high').text(forecastData.maxTemperature);
    $('#forecast-low').text(forecastData.minTemperature);
    $('#forecast-datetime').text(forecastData.takenAtTime);
}

function updateSensorData(tempSensorData) {
    if (!tempSensorData) {
        return;
    }

    $('#temp-data').text(tempSensorData.currentTemperature);
    $('#humi-data').text(tempSensorData.currentHumidity);
    $('#tempreature-datetime').text(tempSensorData.takenAtTime);
}
