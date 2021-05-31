#define DHTPIN 13     // what digital pin we're connected to on the node mcu 1.0
#define DHTTYPE DHT22   // DHT 22  (AM2302), AM2321
#define MHZRX 12
#define MH_Z19_TX 14

float temperature = 0;
float humidity = 0;
unsigned long nextDue = 0;
DHT dht(DHTPIN, DHTTYPE);
//MHZ co2(MHZRX, MH_Z19_TX, 27, MHZ19B);


void sensor_runner_setup() {  
  Serial.println("Start DHT");
  dht.begin();
    pinMode(27, INPUT);
//  co2.setDebug(true);
}

void sensor_runner_loop() {
  // Only update if 3 seconds has happened
  unsigned long now = millis();
  if (now < nextDue) {
    return;
  }

/*  if (co2.isPreHeating()) {
    Serial.println("CO2 Pre-heating");
  } else {
    int ppm_uart = co2.readCO2UART();
    Serial.print("PPMuart: ");
    if (ppm_uart > 0) {
      Serial.print(ppm_uart);
    } else {
     Serial.print("n/a");
    } 
    
  int ppm_pwm = co2.readCO2PWM();
  Serial.print(", PPMpwm: ");
  Serial.print(ppm_pwm);
  
    int temperature = co2.getLastTemperature();
    Serial.print(", Temperature: ");
    if (temperature > 0) {
      Serial.println(temperature);
    } else {
      Serial.println("n/a");
    }
  }*/

  sensor_runner_update_dht_readings();
  
  nextDue = millis() + 3000;

  screen_display_sensor_data(temperature, humidity);
}

void sensor_runner_update_dht_readings() {
  float h = dht.readHumidity();
  float t = dht.readTemperature();

    // Check if any reads failed and exit early (to try again).
  if (isnan(h) || isnan(t)){
    Serial.println("Failed DHT");
    return;
  }

  humidity = h;
  temperature = t;
}

float sensor_runner_temperature() {
  return temperature;
}

float sensor_runner_humidity() {
  return humidity;
}
