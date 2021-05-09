#define BUZZERPIN 18
bool isBuzzCycleOn = false;
bool buzzerIsOn = false;
unsigned long nextBuzz = 0;
bool isNextBuzzOn = false;
int buzzInterval = 1000;

void buzzer_setup() {
  pinMode(BUZZERPIN, OUTPUT);
  digitalWrite(BUZZERPIN, LOW);
}

void buzzer_on() {
  buzzerIsOn = true;
}

void buzzer_off() {
  buzzerIsOn = false;
}

void buzzer_cycle_on() {
  isBuzzCycleOn = true;
  isNextBuzzOn = true;
}

void buzzer_cycle_off() {
  isBuzzCycleOn = false;
  isNextBuzzOn = false;
  buzzer_off();
}

void buzzer_cycle_set_interval(int ms) {
  buzzInterval = ms;
}

void buzzer_loop() {
  if (isBuzzCycleOn) {
    unsigned long now = millis();
    if (now >= nextBuzz) {
      if (isNextBuzzOn) {
        buzzer_on();
      } else {
        buzzer_off();
      }
      isNextBuzzOn = !isNextBuzzOn;
      nextBuzz = now + buzzInterval;
    }
  }

  if (buzzerIsOn) {
    int loops = isBuzzCycleOn ? buzzInterval / 2 : 250;
    for(int i = loops; i > 0 ; --i)
    {
      digitalWrite(BUZZERPIN, HIGH);
      delay(1);
      digitalWrite(BUZZERPIN, LOW);
      delay(1);
    }
  }
}
