import { SensorModuleSensorConfig } from "./sensor-module-sensor-config";
import { WirelessConfig } from "./wireless-config";
import { IpConfig } from "./ip-config";

export class SensorModuleConfig {
  serialNumber: string;
  name: string;
  location: string;
  room: string;
  sensorConfig: SensorModuleSensorConfig;
  networkType: string;
  wirelessConfig: WirelessConfig;
  ipConfigType: string;
  staticIpConfig: IpConfig;
  serverAddress: string;
}
