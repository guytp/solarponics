#include <WiFi.h>
String netWifiSsid;
int netWifiLastStatus = -777;
bool netWiFiBuzzerOn;
unsigned long netWiFiBuzzerStart;

void net_wifi_setup() {
  netWifiSsid = config_get_ssid();
  String ssidKey = config_get_ssid_key();
  String ipString = config_get_ip();
  String gatewayString = config_get_gateway();
  String broadcastString = config_get_broadcast();
  String dnsString = config_get_dns();
  IPAddress ip;
  IPAddress gateway;
  IPAddress broadcast;
  IPAddress dns;
  ip.fromString(ipString);
  gateway.fromString(gatewayString);
  broadcast.fromString(broadcastString);
  dns.fromString(dnsString);
  Serial.print("Configuring WiFi for ");
  Serial.print(ip);
  Serial.print(" ");
  Serial.print(broadcast);
  Serial.print(" gw ");
  Serial.print(gateway);
  Serial.print(" dns ");
  Serial.println(dns);
  WiFi.config(ip, dns, gateway, broadcast);
  Serial.print("Connecting to ");
  Serial.println(netWifiSsid);
  WiFi.begin(netWifiSsid.c_str(), ssidKey.c_str());
}

void net_wifi_loop() {
  int status = WiFi.status();
  if (status == netWifiLastStatus)
  {
    if (!netWiFiBuzzerOn && status != WL_CONNECTED && millis() > netWiFiBuzzerStart)
    {
      buzzer_cycle_on();
      buzzer_cycle_set_interval(1000);
      netWiFiBuzzerOn = true;
    }
    return;
  }

  bool isConnected = status == WL_CONNECTED;
  Serial.print("WiFi status changed to ");
  Serial.print(status);
  Serial.print(". Connected? ");
  Serial.println(isConnected ? "yes" : "no");
  screen_wifi_status(netWifiSsid, isConnected);

  if (isConnected) {
    net_server_connection_start();
    buzzer_cycle_off();
    netWiFiBuzzerOn = false;
  } else {
    net_server_connection_stop();
    netWiFiBuzzerStart = millis() + 5000;
  }
  netWifiLastStatus = status;
}
