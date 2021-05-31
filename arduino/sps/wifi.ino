const char * wifi_get_status_message(int status) {
    switch (status) {
    case WL_NO_SHIELD:
      return "No Shield";
    case WL_IDLE_STATUS:
      return "Idle";
    case WL_NO_SSID_AVAIL:
      return "SSID Avail";
    case WL_SCAN_COMPLETED:
      return "Scan Completed";
    case WL_CONNECTED:
      return "Connected";
    case WL_CONNECT_FAILED:
      return "Connect Failed";
    case WL_CONNECTION_LOST:
      return "Connection Lost";
    case WL_DISCONNECTED:
      return "Disconnected";
    default:
      char buffer [4];
      return itoa(status, buffer, 10);
  }
}


void wifi_connect(const char * ssid, const char * key, bool useStaticIp) {
  // ESP32, particularly with lower power, can do weird things sometimes and this
  // setup seems to force it to behave better
  WiFi.persistent(false);
  WiFi.disconnect(true);
  WiFi.mode(WIFI_OFF);
  WiFi.mode(WIFI_STA);
  if (useStaticIp) {
    String netWifiKey = config_get_ssid_key();
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
  } else {
    Serial.println("Configuring WiFi for DHCP");
  }
  delay(1000);
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, key);
}
