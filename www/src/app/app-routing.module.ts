import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SensorModulesComponent } from "./sensor-modules/sensor-modules.component";
import { ProvisionSensorModuleComponent } from "./provision-sensor-module/provision-sensor-module.component";

const routes: Routes = [
  { path: 'sensor-modules', component: SensorModulesComponent },
  { path: 'provisioning/sensor-modules', component: ProvisionSensorModuleComponent },
  { path: '',   redirectTo: '/sensor-modules', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
