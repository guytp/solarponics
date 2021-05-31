unsigned long stopBuzzerAfter;
bool readyToProvision = false;

void main_provisioning_setup() {
  Serial.println("Entering provisioning mode");
  screen_clear();
  screen_provisioning();
  led_control_blink_interval(1000);
  led_control_blink_on();
  provisioning_net_wifi_setup();
  buzzer_cycle_on();
  buzzer_cycle_set_interval(250);
  stopBuzzerAfter = millis() + 2000;
}

void main_provisioning_loop() {
  if (millis() > stopBuzzerAfter) {
    buzzer_cycle_off();
    readyToProvision = true;
  }
  led_control_loop();
  if (!readyToProvision) {
    buzzer_loop();
  } else {
    provisioning_net_wifi_loop();
  }
}
