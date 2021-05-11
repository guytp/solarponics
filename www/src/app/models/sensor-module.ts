import { Sensor } from "./sensor";

export interface SensorModule {
  room: string;
  location: string;
  id: number;
  sensors: Sensor[];
  serialNumber: string;
  name: string;
}
