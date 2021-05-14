import { Component, OnInit } from '@angular/core';
import { SensorModule, SensorModule as ISensorModule } from "../models/sensor-module";
import { SensorModuleApiClient } from "../api-client/sensor-module-api-client";

@Component({
  selector: 'app-sensor-modules',
  templateUrl: './sensor-modules.component.html',
  styleUrls: ['./sensor-modules.component.css']
})
export class SensorModulesComponent implements OnInit {
  sensorModules: SensorModule[];
  error: any;

  constructor(private readonly apiClient: SensorModuleApiClient) { }

  ngOnInit(): void {
    this.reloadData();
  }

  reloadData() {
    this.apiClient.getSensorModules().subscribe(
      (data: ISensorModule[]) => this.sensorModules = data, // success path
      error => this.error = error // error path
    );
  }
}
