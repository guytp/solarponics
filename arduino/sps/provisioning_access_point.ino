#include <WiFi.h>
#define ProvisioningSsid "Sp-"SerialNumber
#define ProvisioningSsidKey "S014rP0n1cs"

//IPAddress local_IP(192,168,4,1);
//IPAddress gateway(192,168,4,1);
//IPAddress subnet(255,255,255,0);
int connectedStations = 0;

void provisioning_access_point_setup() {
  Serial.println("Setting up access point "ProvisioningSsid" with key "ProvisioningSsidKey);
  WiFi.softAP(ProvisioningSsid, ProvisioningSsidKey);
  //WiFi.softAPConfig(local_IP, gateway, subnet);
  // Note: Can crash - see: https://github.com/espressif/arduino-esp32/issues/2025
  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);
  delay(500); // Needed so TCP Server can startup OK
}


void provisioning_access_point_loop() {
  int stations = WiFi.softAPgetStationNum();
  if (stations == connectedStations) {
    return;
  }

  Serial.print("Connected WiFi stations: ");
  Serial.println(stations);
  connectedStations = stations;
}
