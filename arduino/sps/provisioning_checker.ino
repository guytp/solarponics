#include <ArduinoJson.h>

WiFiClient provisioningCheckerClient;
DynamicJsonDocument provisioningCheckerDoc(4096);
    
void provisioning_checker_start() {
  // Setup screen
  if (!provisioningCheckerClient.connect("192.168.42.1", 4203)) {
    provisioning_checker_fail("Cannot connect to provisioning server", "Cannot Connect");
    return;
  }
  screen_text("Connected");

  // Send our request to the server
  String request = "{\"SerialNumber\": \""SerialNumber"\", \"OpCode\": 32, \"Sequence\": 0}";
  Serial.println(request);
  Serial.print("JSON is ");
  int strLen = request.length();
  Serial.print(strLen);
  Serial.println(" bytes");
  uint8_t * buf = (uint8_t*)malloc(strLen + 2);
  buf[0] = 0x20;
  request.getBytes(buf + 1, strLen + 1);
  provisioningCheckerClient.write(buf, strLen + 1);
  free(buf);

  // Wait for up to 500ms for a response
  Serial.print("Waiting for response");
  ulong waitUntil = millis() + 5000;
  while (!provisioningCheckerClient.available() && millis() < waitUntil) {
    Serial.print(".");
    delay(25);
  }
  Serial.println("");
  if (!provisioningCheckerClient.available()) {
    provisioning_checker_fail("No response from provisioning server", "Resp Timeout");
    return;
  }

  // We've got data
  Serial.println("Response data received");
  uint8_t messageType = provisioningCheckerClient.read();
  if (messageType == 5) {
    provisioning_checker_error_handle();
    return;
  } else if (messageType == 33) {
    provisioning_checker_response_handle();
    return;
  }
  Serial.print("Message type ");
  Serial.print(messageType);
  Serial.println(" is not supported");
  provisioning_checker_fail("Unexpected response received", "Unexpected Resp");

}


void provisioning_checker_fail(String log, String screen) {
  Serial.println("Failed to provision");
  Serial.println(log);
  screen_clear();
  screen_text(screen);
  delay(5000);
  ESP.restart();
}

void provisioning_checker_error_handle() {
 provisioning_checker_json_parse();
  const char * urn = provisioningCheckerDoc["Urn"];
  const char * message = provisioningCheckerDoc["Message"];
  if (urn == NULL || strlen(urn) == 0) {
    urn = "urn:sp:unknown";
  }
  provisioning_checker_fail(message, urn);
}

void provisioning_checker_response_handle() {
  provisioning_checker_json_parse();
  int networkType = provisioningCheckerDoc["Config"]["NetworkType"];
  bool useWiFi = networkType == 1;
  char * wifiSsid = "\0";
  char * wifiKey = "\0";
  if (useWiFi) {
    const char * ssid = provisioningCheckerDoc["Config"]["WirelessConfig"]["Ssid"];
    wifiSsid = (char *)ssid;
    const char * key = provisioningCheckerDoc["Config"]["WirelessConfig"]["Key"];
    wifiKey = (char *)key;
  }
  int ipConfigType = provisioningCheckerDoc["Config"]["IpConfigType"];
  bool useStaticIp = ipConfigType == 1;
  char * ipAddress = "\0";
  char * ipGateway = "\0";
  char * ipDns = "\0";
  char * ipNetwork = "\0";
  if (useStaticIp) {
    const char * add = provisioningCheckerDoc["Config"]["StaticIpConfig"]["Address"];
    ipAddress = (char *)add;
    const char * net = provisioningCheckerDoc["Config"]["StaticIpConfig"]["Broadcast"];
    ipNetwork = (char *)net;
    const char * gw = provisioningCheckerDoc["Config"]["StaticIpConfig"]["Gateway"];
    ipGateway = (char *)gw;
    const char * dns = provisioningCheckerDoc["Config"]["StaticIpConfig"]["Dns"];
    ipDns = (char *)dns;
  }
  const char * server = provisioningCheckerDoc["Config"]["ServerAddress"]; 
  const char * name = provisioningCheckerDoc["Config"]["Name"]; 
  
  config_set_ssid(wifiSsid);
  config_set_ssid_key(wifiKey);
  config_set_ip(ipAddress);
  config_set_broadcast(ipNetwork);
  config_set_gateway(ipGateway);
  config_set_dns(ipDns);
  config_set_server((char *)server);
  config_set_use_static_ip(useStaticIp);
  config_set_use_wifi(useWiFi);
  config_set_isProvisioned();

  Serial.println("Provisioning complete, restarting in 5 seconds");
  screen_clear();
  screen_text(name);
  delay(5000);
  ESP.restart();
}

void provisioning_checker_json_parse() {
  deserializeJson(provisioningCheckerDoc, provisioningCheckerClient);
  Serial.println("JSON received");
  serializeJson(provisioningCheckerDoc, Serial);
  Serial.println("");
}
