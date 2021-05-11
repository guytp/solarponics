import { Sensor } from "./sensor";

export interface SensorReading {
  minimum: number;
  maximum: number;
  average: number;
  time: string;
}
