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
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatDividerModule } from "@angular/material/divider";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatGridListModule } from "@angular/material/grid-list";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatTableModule } from "@angular/material/table";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxChartsModule }from '@swimlane/ngx-charts';
import { SensorModuleReadingsComponent } from './sensor-module-readings/sensor-module-readings.component';
import { SensorModuleApiClient } from "./api-client/sensor-module-api-client";
import { SensorChartComponent } from './sensor-chart/sensor-chart.component';
import { SensorReadingApiClient } from "./api-client/sensor-reading-api-client";
import { SensorModulesComponent } from './sensor-modules/sensor-modules.component';
import { ProvisionSensorModuleComponent } from './provision-sensor-module/provision-sensor-module.component';
import { DataAggregationService } from "./data-aggregation-service/data-aggregation-service";

@NgModule({
  declarations: [
    AppComponent,
    SensorModuleReadingsComponent,
    SensorChartComponent,
    SensorModulesComponent,
    ProvisionSensorModuleComponent
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
    MatIconModule,
    MatInputModule,
    MatDividerModule,
    MatAutocompleteModule,
    MatGridListModule,
    MatTableModule,
    NgxChartsModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    SensorModuleApiClient,
    SensorReadingApiClient,
    DataAggregationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
