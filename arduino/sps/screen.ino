LiquidCrystal_I2C lcd(0x27,20,4);
bool screenMenuVisible;

void screen_setup() {
  lcd.init();
  lcd.backlight();
  screen_clear();
}

void screen_text(String text) {
  if (screenMenuVisible)
    return;
  String message = String(text);
  if (message.length() > 20) {
    message = message.substring(0, 20);
  }
  int left = (20 - message.length()) / 2;
  lcd.setCursor(left, 1);
  lcd.print(text);
}

void screen_clear() {
  if (screenMenuVisible)
    return;
  lcd.clear();
  lcd.setCursor(4,0);
  lcd.print("Solarponics");
}

void screen_provisioning() {
  if (screenMenuVisible)
    return;
  screen_clear();
  lcd.setCursor(4, 2);
  lcd.print("Provisioning");
  lcd.setCursor(1, 3);
  lcd.print("  S/N:  "SerialNumber);
}

void screen_wifi_status(String ssid, bool isConnected) {
  if (screenMenuVisible)
    return;
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

void screen_display_sensor_data(float temperature, float humidity, int co2) {
  if (screenMenuVisible)
    return;
  char buffer[20];
  sprintf(buffer, "T: %.1f C", temperature);
  lcd.setCursor(0, 3);
  lcd.print(buffer);

  sprintf(buffer, "H: %.1f%%", humidity);
  lcd.setCursor(20 - strlen(buffer), 3);
  lcd.print(buffer);

  if (co2 > 0) {
    sprintf(buffer, "CO2: %dppm", co2);
    int leftOffset = (20 - strlen(buffer)) / 2;
    lcd.setCursor(leftOffset, 2);
    lcd.print(buffer);
  }
}

void screen_menu_mode(bool enabled) {
  screenMenuVisible = enabled;
  if (enabled)
    lcd.clear();
  else {
    screen_clear();
    sensor_runner_screen_refresh_needed();
    net_wifi_update_screen();
  }
}

void screen_menu_text_main(String text) {
  if (!screenMenuVisible)
    return;
  String message = String(text);
  if (message.length() > 20) {
    message = message.substring(0, 20);
  }
  int left = (20 - message.length()) / 2;
  lcd.setCursor(left, 0);
  lcd.print(text);

}

void screen_menu_text_secondary(String text) {  
  if (!screenMenuVisible)
    return;
  String message = String(text);
  if (message.length() > 20) {
    message = message.substring(0, 20);
  }
  int left = (20 - message.length()) / 2;
  lcd.setCursor(left, 1);
  lcd.print(text);
}

void screen_menu_buttons(bool back, bool down, bool up, bool enter) {
  if (back) {
    lcd.setCursor(0, 3);
    lcd.print("Bk");
  }
  
  if (down) {
    lcd.setCursor(6, 3);
    lcd.print("Dn");
  }

  if (up) {
    lcd.setCursor(12, 3);
    lcd.print("Up");
  }

  if (enter) {
    lcd.setCursor(18, 3);
    lcd.print("OK");
  }
}
