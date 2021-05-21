import { Component, OnInit, OnDestroy } from '@angular/core';
import { SensorModule, SensorModule as ISensorModule } from "../models/sensor-module";
import { DataAggregationService } from "../data-aggregation-service/data-aggregation-service";
import { Subscription } from "rxjs";

@Component({
  selector: 'app-sensor-modules',
  templateUrl: './sensor-modules.component.html',
  styleUrls: ['./sensor-modules.component.css']
})
export class SensorModulesComponent implements OnInit, OnDestroy {
  sensorModules: SensorModule[];
  error: any;
  private subscription: Subscription;

  constructor(private readonly dataAggregationService: DataAggregationService) { }

  ngOnInit(): void {
    this.sensorModules = this.dataAggregationService.sensorModules;
    this.subscription = this.dataAggregationService.sensorModulesObservable.subscribe((sensorModules) => {
      this.sensorModules = sensorModules;
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  reloadData() {
    this.dataAggregationService.reloadData(true);
  }
}
