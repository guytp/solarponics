import { Component, OnInit } from '@angular/core';
import { SensorModuleApiClient } from "../api-client/sensor-module-api-client";
import { SensorModule } from "../models/sensor-module";

@Component({
  selector: 'app-sensor-module-readings',
  templateUrl: './sensor-module-readings.component.html',
  styleUrls: ['./sensor-module-readings.component.css']
})
export class SensorModuleReadingsComponent implements OnInit {
  sensorModules: SensorModule[];
  error: any;

  constructor(private readonly apiClient: SensorModuleApiClient) { }

  ngOnInit(): void {
    this.reloadData();
  }

  reloadData() {
    this.apiClient.getSensorModules().subscribe(
      (data: SensorModule[]) => this.sensorModules = data, // success path
      error => this.error = error // error path
    );
  }

}
