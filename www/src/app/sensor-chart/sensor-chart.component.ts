import { Component, Input } from '@angular/core';
import { Sensor } from "../models/sensor";
import { SensorReadingApiClient } from "../api-client/sensor-reading-api-client";
import { SensorReading } from "../models/sensor-reading";

@Component({
  selector: 'app-sensor-chart',
  templateUrl: './sensor-chart.component.html',
  styleUrls: ['./sensor-chart.component.css']
})
export class SensorChartComponent   {
  private _sensor: Sensor;
  error: any;
  readings: SensorReading[];

  get sensor(): Sensor{
    return this._sensor;
  }

  @Input()
  set sensor(val: Sensor) {
    this._sensor = val;
    if (!val) {
      return;
    }
    this.apiClient.getSensorReadings(val.id, 'OneMinute').subscribe(
      (data: SensorReading[]) => this.readings = data, // success path
      error => this.error = error // error path)
    );
  }

  constructor(private readonly apiClient: SensorReadingApiClient) {
  }
}
