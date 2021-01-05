// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var data = document.getElementById('temp-data').innerText;
var parsedData = JSON.parse(data);

var tempCtx = document.getElementById('temp-chart').getContext('2d');
var chart = new Chart(tempCtx, {
    // The type of chart we want to create
    type: 'line',

    // The data for our dataset
    data: parsedData.temperatureChartData,

    // Configuration options go here
    options: {
        responsive: true,
        hoverMode: 'index',
        stacked: false,
        title: {
            display: true,
            text: 'Temperature 24 Hours'
        },
        scales: {
            yAxes: [{
                type: 'linear', // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                display: true,
                position: 'right',
                id: 'y-axis-1',
                ticks: {
                    suggestedMin: 15,
                    suggestedMax: 35,
                }
            }],
        }
    }
});

var humiCtx = document.getElementById('humi-chart').getContext('2d');
var chart = new Chart(humiCtx, {
    // The type of chart we want to create
    type: 'line',

    // The data for our dataset
    data: parsedData.humidityChartData,

    // Configuration options go here
    options: {
        responsive: true,
        hoverMode: 'index',
        stacked: false,
        title: {
            display: true,
            text: 'Humidity 24 Hours'
        },
        scales: {
            yAxes: [{
                type: 'linear', // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                display: true,
                position: 'right',
                id: 'y-axis-2',
                ticks: {
                    suggestedMin: 30,
                    suggestedMax: 80
                }
            }],
        }
    }
});


var forecastHubconnection = new signalR.HubConnectionBuilder().withUrl("/forecastHub").build();

forecastHubconnection.on("ReceiveForecast", function (forecastData) {
    console.log(forecastData);
});

forecastHubconnection.start().catch(function (err) {
    return console.error(err.toString());
});