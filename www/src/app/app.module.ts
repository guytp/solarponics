import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatSelectModule } from "@angular/material/select";
import { MatFormFieldModule } from "@angular/material/form-field";
import { NgxChartsModule }from '@swimlane/ngx-charts';
import { SensorModuleReadingsComponent } from './sensor-module-readings/sensor-module-readings.component';
import { SensorModuleApiClient } from "./api-client/sensor-module-api-client";
import { SensorChartComponent } from './sensor-chart/sensor-chart.component';
import { SensorReadingApiClient } from "./api-client/sensor-reading-api-client";
import { SensorModulesComponent } from './sensor-modules/sensor-modules.component';

@NgModule({
  declarations: [
    AppComponent,
    SensorModuleReadingsComponent,
    SensorChartComponent,
    SensorModulesComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatExpansionModule,
    MatButtonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatSidenavModule,
    NgxChartsModule
  ],
  providers: [
    SensorModuleApiClient,
    SensorReadingApiClient
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
