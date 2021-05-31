#include <ArduinoJson.h>
#include <WiFi.h>
#include <ArduinoNvs.h>
#include <LiquidCrystal_I2C.h>
#include "DHT.h"
#include "MHZ.h"
#define SerialNumber "0A0A0A0A"
#define RESETCONFIGPIN 4

bool isProvisioned = false;

void setup(){
  Serial.begin(115200);
  while(!Serial) {;}
  Serial.println("Device starting up");

  buzzer_setup();
  led_control_setup();
  screen_setup();
  config_setup();

  isProvisioned = sps_provisioning_checks();
  
  if (isProvisioned) {
    main_normal_setup();
  } else {
    main_provisioning_setup();
  }
}


void loop(){
  if (isProvisioned) {
    main_normal_loop();
  } else {
    main_provisioning_loop();
  }
}


bool sps_provisioning_checks() {
  if (!config_get_isProvisioned()) {
    return false;
  }
  
  sps_check_for_config_reset();
  if (config_get_isProvisioned()) {
    return true;
  }
  return false;
}


void sps_check_for_config_reset() {
  led_control_blink_interval(250);
  led_control_blink_on();
  
  Serial.println("Detecting button press for 1.5secs");
  screen_text("Press to reset");
  unsigned long endTime = millis() + 1500;
  int downReadings = 0;
  int totalReadings = 0;
  pinMode(RESETCONFIGPIN, INPUT);
  while (millis() < endTime) {
    led_control_loop();
    if (digitalRead(RESETCONFIGPIN) == HIGH) {
      downReadings++;
    }
    totalReadings++;
  }

  led_control_blink_off();
  led_control_off();

  if (downReadings > totalReadings / 2) {
    Serial.println("Resetting config");
    screen_text("Resetting config");
    config_reset();
  }
}
