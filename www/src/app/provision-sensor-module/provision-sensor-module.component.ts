import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormControl, FormGroupDirective, NgForm, Validators, FormGroup } from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";
import { DataAggregationService } from "../data-aggregation-service/data-aggregation-service";
import { Subscription } from "rxjs";
import { SensorModuleApiClient } from "../api-client/sensor-module-api-client";
import { SensorModuleConfig } from "../models/sensor-module-config";
import { WirelessConfig } from "../models/wireless-config";
import { IpConfig } from "../models/ip-config";
import { SensorModuleSensorConfig } from "../models/sensor-module-sensor-config";

export class DefaultErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return (control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: "app-provision-sensor-module",
  templateUrl: "./provision-sensor-module.component.html",
  styleUrls: ["./provision-sensor-module.component.css"]
})
export class ProvisionSensorModuleComponent implements OnInit, OnDestroy {
  private sensorModulesSubscription: Subscription;
  awaitingProvisioningConfigs: SensorModuleConfig[];
  displayedColumns: string[] = [ "SerialNumber", "Name", "Location", "Room", "NumberTemperature", "NumberHumidity", "NumberCarbonDioxide", "NetworkType", "WirelessSsid", "WirelessKey", "IpConfig", "IpAddress", "IpBroadcast", "IpGateway", "IpDns", "ServerAddress", "Buttons" ];
  private wirelessSsidFormControl =
    new FormControl("", (e) => this.validatorWirelessRequiredIfCorrectNetworkType(this, e));
  private wirelessKeyFormControl =
    new FormControl("", (e) => this.validatorWirelessRequiredIfCorrectNetworkType(this, e));
  private ipAddressFormControl = new FormControl("", (e) => this.validatorStaticIpRequiredIfCorrectIpType(this, e));
  private ipBroadcastFormControl = new FormControl("", (e) => this.validatorStaticIpRequiredIfCorrectIpType(this, e));
  private ipGatewayFormControl = new FormControl("", (e) => this.validatorStaticIpRequiredIfCorrectIpType(this, e));
  private ipDnsFormControl = new FormControl("", (e) => this.validatorStaticIpRequiredIfCorrectIpType(this, e));
  formGroup = new FormGroup({
    "serialNumber": new FormControl("", [Validators.required]),
    "name": new FormControl("", [Validators.required]),
    "room": new FormControl("", [Validators.required]),
    "location": new FormControl("", [Validators.required]),
    "numberTemperatureSensors": new FormControl("", [Validators.required]),
    "numberHumiditySensors": new FormControl("", [Validators.required]),
    "numberCarbonDioxideSensors": new FormControl("", [Validators.required]),
    "networkType": new FormControl("", [Validators.required]),
    "wirelessSsid": this.wirelessSsidFormControl,
    "wirelessKey": this.wirelessKeyFormControl,
    "ipType": new FormControl("", [Validators.required]),
    "serverAddress": new FormControl("", [Validators.required]),
  });
  locations: string[] = [];
  rooms: string[] = [];

  get serialNumber() {
    return this.formGroup.get("serialNumber");
  }

  get name() {
    return this.formGroup.get("name");
  }

  get room() {
    return this.formGroup.get("room");
  }

  get location() {
    return this.formGroup.get("location");
  }

  get numberTemperatureSensors() {
    return this.formGroup.get("numberTemperatureSensors");
  }

  get numberHumiditySensors() {
    return this.formGroup.get("numberHumiditySensors");
  }

  get numberCarbonDioxideSensors() {
    return this.formGroup.get("numberCarbonDioxideSensors");
  }

  get networkType() {
    return this.formGroup.get("networkType");
  }

  get wirelessSsid() {
    return this.formGroup.get("wirelessSsid");
  }

  get wirelessKey() {
    return this.formGroup.get("wirelessKey");
  }

  get ipType() {
    return this.formGroup.get("ipType");
  }

  get ipAddress() {
    return this.formGroup.get("ipAddress");
  }

  get ipBroadcast() {
    return this.formGroup.get("ipBroadcast");
  }

  get ipGateway() {
    return this.formGroup.get("ipGateway");
  }

  get ipDns() {
    return this.formGroup.get("ipDns");
  }

  get serverAddress() {
    return this.formGroup.get("serverAddress");
  }

  matcher = new DefaultErrorStateMatcher();

  constructor(private readonly dataAggregationService: DataAggregationService, private readonly sensorModuleApiClient: SensorModuleApiClient) {
  }

  ngOnInit(): void {
    this.sensorModulesSubscription = this.dataAggregationService.sensorModulesObservable.subscribe(() => this.locations = this.dataAggregationService.locations);
    this.resetData();
    this.reloadData();
    this.networkType.valueChanges.subscribe(() => {
      if (this.networkType.value === "Wireless") {
        this.formGroup.addControl("wirelessSsid", this.wirelessSsidFormControl);
        this.formGroup.addControl("wirelessKey", this.wirelessKeyFormControl);
      } else {
        this.formGroup.removeControl("wirelessSsid");
        this.formGroup.removeControl("wirelessKey");
      }
    });
    this.ipType.valueChanges.subscribe(() => {
      if (this.ipType.value === "Static") {
        this.formGroup.addControl("ipAddress", this.ipAddressFormControl);
        this.formGroup.addControl("ipBroadcast", this.ipBroadcastFormControl);
        this.formGroup.addControl("ipGateway", this.ipGatewayFormControl);
        this.formGroup.addControl("ipDns", this.ipDnsFormControl);
      } else {
        this.formGroup.removeControl("ipAddress");
        this.formGroup.removeControl("ipBroadcast");
        this.formGroup.removeControl("ipGateway");
        this.formGroup.removeControl("ipDns");
      }
    });
    this.location.valueChanges.subscribe(() => this.rooms = this.dataAggregationService.roomsForLocation(this.location.value));
    this.locations = this.dataAggregationService.locations;
  }

  ngOnDestroy(): void {
    this.sensorModulesSubscription.unsubscribe();
  }

  resetData(): void {
    this.serialNumber.setValue("");
    this.serialNumber.markAsPristine();
    this.serialNumber.markAsUntouched();
    this.name.setValue("");
    this.name.markAsPristine();
    this.name.markAsUntouched();
    this.room.setValue("");
    this.room.markAsPristine();
    this.room.markAsUntouched();
    this.location.setValue("");
    this.location.markAsPristine();
    this.location.markAsUntouched();
    this.numberCarbonDioxideSensors.setValue("1");
    this.numberCarbonDioxideSensors.markAsPristine();
    this.numberCarbonDioxideSensors.markAsUntouched();
    this.numberHumiditySensors.setValue("1");
    this.numberHumiditySensors.markAsPristine();
    this.numberHumiditySensors.markAsUntouched();
    this.numberTemperatureSensors.setValue("1");
    this.numberTemperatureSensors.markAsPristine();
    this.numberTemperatureSensors.markAsUntouched();
    this.networkType.setValue("Wireless");
    this.networkType.markAsPristine();
    this.networkType.markAsUntouched();
    this.wirelessSsidFormControl.setValue("Solarponics");
    this.wirelessSsidFormControl.markAsPristine();
    this.wirelessSsidFormControl.markAsUntouched();
    this.wirelessKeyFormControl.setValue("1100CCBBAA");
    this.wirelessKeyFormControl.markAsPristine();
    this.wirelessKeyFormControl.markAsUntouched();
    this.ipType.setValue("DHCP");
    this.ipType.markAsPristine();
    this.ipType.markAsUntouched();
    this.ipAddressFormControl.setValue("");
    this.ipAddressFormControl.markAsPristine();
    this.ipAddressFormControl.markAsUntouched();
    this.ipBroadcastFormControl.setValue("");
    this.ipBroadcastFormControl.markAsPristine();
    this.ipBroadcastFormControl.markAsUntouched();
    this.ipGatewayFormControl.setValue("");
    this.ipGatewayFormControl.markAsPristine();
    this.ipGatewayFormControl.markAsUntouched();
    this.ipDnsFormControl.setValue("");
    this.ipDnsFormControl.markAsPristine();
    this.ipDnsFormControl.markAsUntouched();
    this.serverAddress.setValue("ingestion.sp.local");
  }

  provisionSensor(): void {
    if (!this.formGroup.valid)
      return;

    const config = new SensorModuleConfig();
    config.serialNumber = this.serialNumber.value;
    config.name = this.name.value;
    config.location = this.location.value;
    config.room = this.room.value;
    config.networkType = this.networkType.value;
    config.ipConfigType = this.ipType.value == "DHCP" ? "Dhcp" : this.ipType.value;
    config.serverAddress = this.serverAddress.value;
    config.sensorConfig = new SensorModuleSensorConfig();
    config.sensorConfig.temperatureSensors = parseInt(this.numberTemperatureSensors.value);
    config.sensorConfig.carbonDioxideSensors = parseInt(this.numberCarbonDioxideSensors.value);
    config.sensorConfig.humiditySensors = parseInt(this.numberHumiditySensors.value);
    if (this.networkType.value === "Wireless") {
      config.wirelessConfig = new WirelessConfig();
      config.wirelessConfig.ssid = this.wirelessSsid.value;
      config.wirelessConfig.key = this.wirelessKey.value;
    }
    if (this.ipType.value === "Static") {
      config.staticIpConfig = new IpConfig();
      config.staticIpConfig.address = this.ipAddress.value;
      config.staticIpConfig.broadcast = this.ipBroadcast.value;
      config.staticIpConfig.gateway = this.ipGateway.value;
      config.staticIpConfig.dns = this.ipDns.value;
    }
    console.info(JSON.stringify(config));
    this.sensorModuleApiClient.provisioningQueueAdd(config).subscribe(() => {
      this.resetData();
      this.reloadData();
    });
  }

  deleteConfig(serialNumber: string) {
    if (!serialNumber)
      return;

    this.sensorModuleApiClient.provisioningQueueDelete(serialNumber).subscribe((() => this.reloadData()));
  }
  
  private reloadData(): void {
    this.sensorModuleApiClient.provisioningQueueGet().subscribe((data) => this.awaitingProvisioningConfigs = data);
  }

  validatorWirelessRequiredIfCorrectNetworkType(component: ProvisionSensorModuleComponent, formControl) {
    const control = component?.formGroup?.get("networkType");
    if (!control || control.value !== "Wireless") {
      return null;
    }


    return Validators.required(formControl);
  }

  validatorStaticIpRequiredIfCorrectIpType(component: ProvisionSensorModuleComponent, formControl) {
    const control = component?.formGroup?.get("ipType");
    if (!control || control.value !== "Static") {
      return null;
    }


    return Validators.required(formControl);
  }
}
