LiquidCrystal_I2C lcd(0x27,20,4);

void screen_setup() {
  lcd.init();
  lcd.backlight();
  screen_clear();
}

void screen_text(String text) {
  String message = String(text);
  if (message.length() > 20) {
    message = message.substring(0, 20);
  }
  int left = (20 - message.length()) / 2;
  lcd.setCursor(left, 1);
  lcd.print(text);
}

void screen_clear() {
  lcd.clear();
  lcd.setCursor(4,0);
  lcd.print("Solarponics");
}

void screen_provisioning() {
  screen_clear();
  lcd.setCursor(4, 2);
  lcd.print("Provisioning");
  lcd.setCursor(1, 3);
  lcd.print("  S/N:  "SerialNumber);
}

void screen_wifi_status(String ssid, bool isConnected) {
  String message = "WiFi ";
  if (ssid.length() %2 == 0)
  {
    message = message + " ";
  }
  message = message  + ssid;
  if (message.length() > 20)
  {
    message = message.substring(0, 20);
  }
  int leftOffset = (20 - message.length()) / 2;
  lcd.setCursor(leftOffset, 1);
  lcd.print(message);
  lcd.setCursor(3, 2);
  if (!isConnected)
  {
    lcd.print("NOT  CONNECTED");
  } else {
    lcd.print("              ");
  }
}

void screen_display_sensor_data(float temperature, float humidity) {
  char buffer[20];
  sprintf(buffer, "T: %.1f C", temperature);
  lcd.setCursor(0, 3);
  lcd.print(buffer);

  sprintf(buffer, "H: %.1f%%", humidity);
  lcd.setCursor(20 - strlen(buffer), 3);
  lcd.print(buffer);
}
