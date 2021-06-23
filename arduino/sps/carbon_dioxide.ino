#include <MHZ.h>
#define MHZRX 27
#define MH_Z19_TX 14

MHZ carbonDioxideSensor(MHZRX, MH_Z19_TX, MHZ14A);
int carbonDioxideValue = 0;
unsigned long carbonDioxideNextDue = 0;

void carbon_dioxide_setup() {
  Serial.println("Carbon dioxide setup");
  carbonDioxideSensor.setDebug(true);
}

void carbon_dioxide_loop() {
  unsigned long now = millis();
  if (now < carbonDioxideNextDue) {
    return;
  }
  carbonDioxideNextDue = millis() + 15000;
  
  if (carbonDioxideSensor.isPreHeating()) {
    Serial.println("CO2 Pre-heating");
    return;
  }
  int co2Reading = carbonDioxideSensor.readCO2UART();
  if (co2Reading <= 0)
    return;
  carbonDioxideValue = co2Reading;
  Serial.print("Updated CO2 to: ");
  Serial.println(carbonDioxideValue);  
}

int carbon_dioxide_value() {
  return carbonDioxideValue;
}

void carbon_dioxide_calibrate_zero() {
  Serial.println("CO2 zero calibrate");
  //carbonDioxideSensor.calibrateZero();
}

void carbon_dioxide_calibrate_auto(bool value) {
  Serial.println("CO2 auto calibrate");
  //carbonDioxideSensor.setAutoCalibrate(value);
}
