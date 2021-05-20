import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Subscription, timer, BehaviorSubject } from 'rxjs';
import { SensorModuleApiClient } from "../api-client/sensor-module-api-client";
import { SensorModule } from "../models/sensor-module";


@Injectable()
export class DataAggregationService {
  private subscription: Subscription;
  sensorModules: SensorModule[];
  private sensorModulesSubject: BehaviorSubject<SensorModule[]> = new BehaviorSubject<SensorModule[]>(null);
  sensorModulesObservable = this.sensorModulesSubject.asObservable();

  constructor(private sensorModuleApiClient : SensorModuleApiClient) {
    this.subscription = timer(0, 15000).subscribe(e => {
      this.reloadData(false);
    });
  }

  reloadData(force: boolean) {
    if (!this.sensorModules || force) {
      this.sensorModuleApiClient.getSensorModules().subscribe((sensorModules) => {
        this.sensorModules = sensorModules;
        this.sensorModulesSubject.next(sensorModules);
      });
    }
  }

}
