#define LED_PIN 2
bool isBlinking = false;
unsigned long nextBlink = 0;
bool isNextBlinkOn = false;
int blinkInterval = 1000;

void led_control_setup() {
  pinMode(LED_PIN, OUTPUT);  
}

void led_control_on() {
  digitalWrite(LED_PIN, HIGH);
}

void led_control_off() {
  digitalWrite(LED_PIN, LOW);
}

void led_control_blink_on() {
  isBlinking = true;
  isNextBlinkOn = true;
}

void led_control_blink_off() {
  isBlinking = false;
  isNextBlinkOn = false;
}

void led_control_blink_interval(int ms) {
  blinkInterval = ms;
}

void led_control_loop() {
  if (!isBlinking) {
    return;
  }

  unsigned long now = millis();
  if (now < nextBlink) {
    return;
  }
  if (isNextBlinkOn) {
    led_control_on();
  } else {
    led_control_off();
  }
  isNextBlinkOn = !isNextBlinkOn;
  nextBlink = now + blinkInterval;
}
