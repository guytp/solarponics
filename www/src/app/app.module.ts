import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SensorModuleReadingsComponent } from './sensor-module-readings/sensor-module-readings.component';
import { SensorModuleApiClient } from "./api-client/sensor-module-api-client";
import { SensorChartComponent } from './sensor-chart/sensor-chart.component';
import { SensorReadingApiClient } from "./api-client/sensor-reading-api-client";

@NgModule({
  declarations: [
    AppComponent,
    SensorModuleReadingsComponent,
    SensorChartComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [
    SensorModuleApiClient,
    SensorReadingApiClient
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
