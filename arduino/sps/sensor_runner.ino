#define DHTPIN 13
#define DHTTYPE DHT22

float temperature = 0;
float humidity = 0;
unsigned long nextDue = 0;
DHT dht(DHTPIN, DHTTYPE);

void sensor_runner_setup() {  
  Serial.println("Start DHT");
  dht.begin();
}

void sensor_runner_loop() {
  // Only update if 3 seconds has happened
  unsigned long now = millis();
  if (now < nextDue) {
    return;
  }

  sensor_runner_update_dht_readings();
  
  nextDue = millis() + 3000;

  int co2Value = carbon_dioxide_value();
  screen_display_sensor_data(temperature, humidity, co2Value);
}

void sensor_runner_screen_refresh_needed() {
  nextDue = millis();
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
