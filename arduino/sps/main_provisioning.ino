unsigned long stopBuzzerAfter;

void main_provisioning_setup() {
  Serial.println("Entering provisioning mode");
  screen_clear();
  screen_provisioning();
  led_control_blink_interval(1000);
  led_control_blink_on();
  provisioning_access_point_setup();
  provisioning_tcp_server_setup();
  buzzer_cycle_on();
  buzzer_cycle_set_interval(250);
  stopBuzzerAfter = millis() + 2000;
}

void main_provisioning_loop() {
  if (millis() > stopBuzzerAfter) {
    buzzer_cycle_off();
  }
  led_control_loop();
  buzzer_loop();
  provisioning_access_point_loop();
  provisioning_tcp_server_loop();
}
