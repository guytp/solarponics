bool isBuzzerOn = true;

void main_normal_setup() {
  Serial.println("Device provisioned, commencing normal startup");
  screen_clear();
  sensor_runner_setup();
  net_wifi_setup();
  led_control_on();
  buzzer_on();
}

void main_normal_loop() {
  led_control_loop();
  buzzer_loop();
  sensor_runner_loop();
  net_wifi_loop();
  ingestion_client_loop();
  if (isBuzzerOn) {
    buzzer_off();
  }
}
