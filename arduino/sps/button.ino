#define BUTTON1_PIN 4
#define BUTTON2_PIN 23
#define BUTTON3_PIN 26
#define BUTTON4_PIN 25
int buttonPins[4];
int buttonState[4];

void button_setup() {
  //pinMode(BUTTON1_PIN, INPUT_PULLUP);
  pinMode(BUTTON2_PIN, INPUT_PULLUP);
  pinMode(BUTTON3_PIN, INPUT_PULLUP);
  pinMode(BUTTON4_PIN, INPUT_PULLUP);
  buttonPins[0] = BUTTON1_PIN;
  buttonPins[1] = BUTTON2_PIN;
  buttonPins[2] = BUTTON3_PIN;
  buttonPins[3] = BUTTON4_PIN;
}

void button_loop() {
  for (int i = 0; i < 4; i++)
  {
    if (button_loop(i) != -1)
      continue;

    if (i == 0)
      menu_back();
    else if (i == 1)
      menu_down();
    else if (i == 2)
      menu_up();
    else
      menu_enter();
  }
}

int button_loop(int button) {

  int state = digitalRead(buttonPins[button]);
  if (state == buttonState[button]) {
    // No change
    return 0;
  }

  Serial.print("Button ");
  Serial.print((button + 1));
  Serial.println(state == 1 ? " pressed" : " released");
  buttonState[button] = state;

  return state == HIGH ? 1 : -1;
}
