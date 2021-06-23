bool isBuzzerOn = true;

void main_normal_setup() {
  Serial.println("Device provisioned, commencing normal startup");
  screen_clear();
  carbon_dioxide_setup();
  sensor_runner_setup();
  net_wifi_setup();
  led_control_on();
  buzzer_on();
  button_setup();
}

void main_normal_loop() {
  led_control_loop();
  buzzer_loop();
  carbon_dioxide_loop();
  sensor_runner_loop();
  net_wifi_loop();
  ingestion_client_loop();
  button_loop();
  if (isBuzzerOn) {
    buzzer_off();
  }
}
