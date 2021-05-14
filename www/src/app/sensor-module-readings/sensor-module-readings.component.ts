import { Component, Input } from '@angular/core';
import { Sensor } from "../models/sensor";

@Component({
  selector: 'app-sensor-module-readings',
  templateUrl: './sensor-module-readings.component.html',
  styleUrls: ['./sensor-module-readings.component.css']
})
export class SensorModuleReadingsComponent {
  @Input()
  sensors: Sensor[];
}
