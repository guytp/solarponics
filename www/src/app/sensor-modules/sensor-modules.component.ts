import { Component, OnInit } from '@angular/core';
import { SensorModule, SensorModule as ISensorModule } from "../models/sensor-module";
import { DataAggregationService } from "../data-aggregation-service/data-aggregation-service";

@Component({
  selector: 'app-sensor-modules',
  templateUrl: './sensor-modules.component.html',
  styleUrls: ['./sensor-modules.component.css']
})
export class SensorModulesComponent implements OnInit {
  sensorModules: SensorModule[];
  error: any;

  constructor(private readonly dataAggregationService: DataAggregationService) { }

  ngOnInit(): void {
    this.sensorModules = this.dataAggregationService.sensorModules;
    this.dataAggregationService.sensorModulesObservable.subscribe((sensorModules) => {
      this.sensorModules = sensorModules;
    });
  }

  reloadData() {
    this.dataAggregationService.reloadData(true);
  }
}
