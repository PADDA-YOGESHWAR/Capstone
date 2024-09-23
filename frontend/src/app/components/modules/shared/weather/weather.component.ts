import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrl: './weather.component.css'
})
export class WeatherComponent implements OnInit {
  location: any;
  currentWeather: any;
  forecast: any;
  humidityChart: any;
  temperatureChart: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get('http://api.weatherapi.com/v1/forecast.json?key=62357cdb6141429d88841203241209&q=Kulathoor, Thiruvananthapuram, Kerala&days=1&aqi=no&alerts=no')
      .subscribe((data: any) => {
       
        this.location = data.location;
        this.currentWeather = data.current;
        this.forecast = data.forecast.forecastday[0];
        this.createHumidityChart();
        this.createTemperatureChart();
      });
      
  }

 
  createHumidityChart(): void {
    const ctx = document.getElementById('humidityChart') as HTMLCanvasElement;
    if (ctx) {
      this.humidityChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
          labels: ['Humidity'],
          datasets: [{
            label: 'Humidity',
            data: [this.currentWeather.humidity, 100 - this.currentWeather.humidity],
            backgroundColor: ['rgba(54, 162, 235, 0.6)', 'rgba(200, 200, 200, 0.6)'],
            borderColor: ['rgba(54, 162, 235, 1)', 'rgba(200, 200, 200, 1)'],
            borderWidth: 1
          }]
        },
        options: {
          responsive: true,
          plugins: {
            title: {
              display: true,
              text: `Humidity - ${this.currentWeather.humidity}%`,
              font: {
                size: 18
              }
            }
          }
        }
      });
    }
  }

  createTemperatureChart(): void {
    const ctx = document.getElementById('temperatureChart') as HTMLCanvasElement;
    if (ctx) {
      this.temperatureChart = new Chart(ctx, {
        type: 'line',
        data: {
          labels: this.forecast.hour.map((hour: any) => hour.time),
          datasets: [{
            label: 'Temperature (°C)',
            data: this.forecast.hour.map((hour: any) => hour.temp_c),
            backgroundColor: 'rgba(255, 99, 132, 0.2)',
            borderColor: 'rgba(255, 99, 132, 1)',
            borderWidth: 2,
            fill: true
          }]
        },
        options: {
          responsive: true,
          plugins: {
            title: {
              display: true,
              text: 'Hourly Temperature Forecast',
              font: {
                size: 18
              }
            }
          },
          scales: {
            x: {
              title: {
                display: true,
                text: 'Time',
                font: {
                  size: 14
                }
              }
            },
            y: {
              beginAtZero: true,
              title: {
                display: true,
                text: 'Temperature (°C)',
                font: {
                  size: 14
                }
              }
            }
          }
        }
      });
    }
  }
}
