#include <WiFi.h>
String netWifiSsid;
String netWifiKey;
int netWifiLastStatus = -777;
bool netWifiUseWifi;
bool netWifiUseStaticIp;
bool netWifiFirstCheck = false;
bool netWifiSecondCheck = false;

void net_wifi_setup() {
  netWifiUseWifi = config_get_use_wifi();
  if (!netWifiUseWifi)
    return;

  netWifiSsid = config_get_ssid();
  netWifiKey  = config_get_ssid_key();

  netWifiUseStaticIp = config_get_use_static_ip();
  wifi_connect(netWifiSsid.c_str(), netWifiKey.c_str(), netWifiUseStaticIp);
}

void net_wifi_loop() {
  int status = WiFi.status();
  bool isConnected = status == WL_CONNECTED;
  
  if (status == netWifiLastStatus)
  {
    // If we cannot connect within 15 seconds, reboot
    if (!isConnected and millis() > 5000 && !netWifiFirstCheck) {
      Serial.println("Resetting WiFi (try 1)");
      wifi_connect(netWifiSsid.c_str(), netWifiKey.c_str(), netWifiUseStaticIp);
      netWifiFirstCheck = true;
    }
    else if (!isConnected and millis() > 10000 && !netWifiSecondCheck) {
      Serial.println("Resetting WiFi (try 2)");
      wifi_connect(netWifiSsid.c_str(), netWifiKey.c_str(), netWifiUseStaticIp);
      netWifiSecondCheck = true;
    }
    else if (!isConnected && millis() > 15000) {
      Serial.println("WiFi not connected, rebooting");
      ESP.restart();
    }
    return;
  }

  Serial.print("WiFi status changed to: ");
  const char * statusMessage = wifi_get_status_message(status);
  Serial.println(statusMessage);
  screen_wifi_status(netWifiSsid, isConnected);

  if (isConnected) {
      Serial.print("IP Address ");
      Serial.println(WiFi.localIP());
  }

  if (isConnected) {
    // TODO: Start netServerConnection here
  } else if (netWifiLastStatus == WL_CONNECTED) {
    Serial.println("Disconnected, restarting");
    screen_clear();
    screen_text("Disconnected");
    delay(5000);
    ESP.restart();
  }
  
  netWifiLastStatus = status;
}
