import { Component, Input } from '@angular/core';
import { Sensor } from "../models/sensor";
import { SensorReadingApiClient } from "../api-client/sensor-reading-api-client";
import { SensorReading } from "../models/sensor-reading";
import { Subscription, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-sensor-chart',
  templateUrl: './sensor-chart.component.html',
  styleUrls: ['./sensor-chart.component.css']
})
export class SensorChartComponent   {
  private subscription: Subscription;
  private _sensor: Sensor;
  error: any;
  chartData: any[];
  readings: SensorReading[];
  private _period: string = "OneMinute";

  get period(): string {
    return this._period;
  }

  set period(val: string) {
    this._period = val;
    this.refreshData();
  }

  get sensorType(): string {
    if (this.sensor.type == "1")
      return "Temperature";
    else if (this.sensor.type == "2")
      return "Carbon Dioxide";
    else if (this.sensor.type == "3")
      return "Humidity";
    return `Unknown (${this.sensor.type})`;
  }
  get sensor(): Sensor{
    return this._sensor;
  }

  @Input()
  set sensor(val: Sensor) {
    this._sensor = val;
  }

  ngOnInit() {
    this.subscription = timer(0, 15000).subscribe(e => {
      this.refreshData();
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  private refreshData() {
    if (!this.sensor || !this.period) {
      return;
    }
    this.apiClient.getSensorReadings(this.sensor.id, this.period).subscribe(
      (data: SensorReading[]) => {
        // < 3 doesn't render well
        if (data && data.length < 3)
          data = null;

        this.readings = data;
        if (!data) {
          this.chartData = null;
          return;
        }

        let tempChartData = new Array(1);
        const series = new Array(data.length);
        let isChanged = false;
        for (let i = 0; i < data.length; i++) {
          const r = data[i];
          const date = new Date(Date.parse(r.time));
          let dateLabel: string;
          if (this.period === "OneMinute" || this.period === "FiveMinutes" || this.period === "FifteenMinutes" || this.period === "ThirtyMinutes")
            dateLabel = date.toLocaleTimeString().substr(0, 5);
          else if (this.period === "OneDay")
            dateLabel = date.toLocaleDateString().substr(0, 5);
          else
            dateLabel = date.toLocaleDateString().substr(0, 5) + " " + date.toLocaleTimeString().substr(0, 5);
          series[i] = {
            min: r.minimum,
            max: r.maximum,
            value: r.average,
            name: dateLabel
          };

          if (!isChanged &&
            this.chartData &&
            (series[i].min != this.chartData[0].series[i].min ||
              series[i].max != this.chartData[0].series[i].max ||
              series[i].value != this.chartData[0].series[i].value ||
              series[i].name != this.chartData[0].series[i].name)) {
            isChanged = true;
          }
        }
        tempChartData [0] = {
          name: this.sensorType,
          series: series
        };

        if (!this.chartData) {
          isChanged = true;
        }

        if (!isChanged && tempChartData[0].name !== this.chartData[0].name) {
          isChanged = true;
        }

        if (!isChanged && series.length !== this.chartData[0].series.length) {
          isChanged = true;
        }

        if (isChanged)
          this.chartData = tempChartData;

      }, // success path
      error => this.error = error // error path)
    );
  }

  constructor(private readonly apiClient: SensorReadingApiClient) {
  }
}
