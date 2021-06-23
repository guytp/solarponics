#define MENU_IDX_HWINFO 0
#define MENU_IDX_REBOOT 1
#define MENU_IDX_CO2CALIBRATION 2
#define MENU_IDX_RESET 3
#define MENU_IDX_MAX 3

bool menuIsVisible = false;
int menuIndexMain = 0;
int menuIndexSecondary = 0;
int menuLevel = 0;

void menu_back() {
  if (menuLevel == 0 || !menuIsVisible) {
    return;
  }

  Serial.println("Menu back");
  menuLevel--;
  if (menuLevel == 0) {
    menuIsVisible = false;
  }
  
  menu_draw();
}


void menu_enter() {
  // Turn on menu if not already visible
  if (!menuIsVisible) {
    menuIsVisible = true;
    menuIndexMain = 0;
    menuLevel = 1;
    Serial.println("Menu turn on");
    menu_draw();
    return;
  }

  // If at top level we make a selection
  if (menuLevel == 1) {
    menuLevel++;
    menu_draw();
    Serial.println("Select top level menu");
    return;
  }

  if (menuLevel == 2) {
    // Here we must be in a top-level menu item
    if (menuIndexMain == MENU_IDX_RESET) {
      Serial.println("Config reset requested");
      config_reset();
      ESP.restart();
    } else if (menuIndexMain == MENU_IDX_CO2CALIBRATION) {
      menuLevel++;
      menuIndexSecondary = 0;
      menu_draw();
    } else if (menuIndexMain == MENU_IDX_REBOOT) {
      Serial.println("Reboot requested");
      ESP.restart();
    }
  }

  if (menuLevel == 3) {
    if (menuIndexMain == MENU_IDX_CO2CALIBRATION) {
      if (menuIndexSecondary == 0) {
        carbon_dioxide_calibrate_auto(true);
        menuLevel++;
      } else if (menuIndexSecondary == 1) {
        carbon_dioxide_calibrate_auto(false);
        menuLevel++;
      } else if (menuIndexSecondary == 2) {
        carbon_dioxide_calibrate_zero();
        menuLevel++;
      }
    }
  }
}

void menu_up() {
  if (menuLevel == 0 || menuIndexMain >= MENU_IDX_MAX) {
    return;
  }

  Serial.println("Menu up");
  menuIndexMain++;
  menu_draw();
}

void menu_down() {
  if (menuLevel == 0 || menuIndexMain <= 0) {
    return;
  }

  Serial.println("Menu down");
  menuIndexMain--;
  menu_draw();
}

void menu_draw() {
  if (menuLevel == 0 || !menuIsVisible) {
    screen_menu_mode(false);
    return;
  }
  
  screen_menu_mode(true);
  if (menuLevel == 1) {
    if (menuIndexMain == MENU_IDX_HWINFO) {
      screen_menu_text_main("Hardware Info");
    } else if (menuIndexMain == MENU_IDX_REBOOT) {
      screen_menu_text_main("Reboot");
    } else if (menuIndexMain == MENU_IDX_RESET) {
      screen_menu_text_main("Reset Device");
    } else if (menuIndexMain == MENU_IDX_CO2CALIBRATION) {
      screen_menu_text_main("CO2 Calibration");
    }
    screen_menu_buttons(true, menuIndexMain != 0, menuIndexMain < MENU_IDX_MAX, true);
    return;
  }

  if (menuLevel == 2) {
    if (menuIndexMain == MENU_IDX_HWINFO) {
      String msg = "S/N: ";
      msg += SerialNumber;
      screen_menu_text_main(msg);
      msg = "Fw: ";
      msg += FirmwareVersion;
      screen_menu_text_secondary(msg);
      screen_menu_buttons(true, false, false, false);
      return;
    } else if (menuIndexMain == MENU_IDX_RESET) {
      screen_menu_text_main("Reset device? Config");
      screen_menu_text_secondary("will be removed");
      screen_menu_buttons(true, false, false, true);
      return;
    } else if (menuIndexMain == MENU_IDX_REBOOT) {
      screen_menu_text_main("Reboot?");
      screen_menu_buttons(true, false, false, true);
      return;
    } else if (menuIndexMain == MENU_IDX_CO2CALIBRATION) {
      screen_menu_text_main("Calibrate CO2?");
      screen_menu_buttons(true, false, false, true);
      return;
    }
  }

  if (menuLevel == 3) {
    if (menuIndexMain == MENU_IDX_CO2CALIBRATION) {
      if (menuIndexSecondary == 0) {
        screen_menu_text_main("Calibrate CO2");
        screen_menu_text_secondary("Auto On");
        screen_menu_buttons(true, false, true, true);
      } else if (menuIndexSecondary == 1) {
        screen_menu_text_main("Calibrate CO2");
        screen_menu_text_secondary("Auto Off");
        screen_menu_buttons(true, true, true, true);
      } else if (menuIndexSecondary == 2) {
        screen_menu_text_main("Calibrate CO2");
        screen_menu_text_secondary("Zero");
        screen_menu_buttons(true, true, false, true);
      }
    }
  }

  if (menuLevel == 4) {
    if (menuIndexMain == MENU_IDX_CO2CALIBRATION) {
      screen_menu_text_main("Calibration started");
      screen_menu_buttons(true, false, false, false);
    }
  }
}
