WiFiClient provisioningClientClient;
DynamicJsonDocument provisioningClientDoc(4096);
    
void provisioning_client_start() {
  // Setup screen
  net_client_instant_fail(true);
  if (!provisioningClientClient.connect("192.168.42.1", 4203)) {
    net_client_fail("Cannot connect to provisioning server", "Cannot Connect Prov");
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
  provisioningClientClient.write(buf, strLen + 1);
  free(buf);

  uint8_t messageType = net_client_response_read(provisioningClientClient);

  // We've got data
  Serial.println("Response data received");  
  if (messageType == 5) {
    net_client_error_handle(provisioningClientDoc, provisioningClientClient);
    return;
  } else if (messageType == 33) {
    provisioning_client_response_handle();
    return;
  }
  Serial.print("Message type ");
  Serial.print(messageType);
  Serial.println(" is not supported");
  net_client_fail("Unexpected response received", "Unexpected Resp");
}

void provisioning_client_response_handle() {
  net_client_json_parse(provisioningClientDoc, provisioningClientClient);
  int networkType = provisioningClientDoc["Config"]["NetworkType"];
  bool useWiFi = networkType == 1;
  char * wifiSsid = "\0";
  char * wifiKey = "\0";
  if (useWiFi) {
    const char * ssid = provisioningClientDoc["Config"]["WirelessConfig"]["Ssid"];
    wifiSsid = (char *)ssid;
    const char * key = provisioningClientDoc["Config"]["WirelessConfig"]["Key"];
    wifiKey = (char *)key;
  }
  int ipConfigType = provisioningClientDoc["Config"]["IpConfigType"];
  bool useStaticIp = ipConfigType == 1;
  char * ipAddress = "\0";
  char * ipGateway = "\0";
  char * ipDns = "\0";
  char * ipNetwork = "\0";
  if (useStaticIp) {
    const char * add = provisioningClientDoc["Config"]["StaticIpConfig"]["Address"];
    ipAddress = (char *)add;
    const char * net = provisioningClientDoc["Config"]["StaticIpConfig"]["Broadcast"];
    ipNetwork = (char *)net;
    const char * gw = provisioningClientDoc["Config"]["StaticIpConfig"]["Gateway"];
    ipGateway = (char *)gw;
    const char * dns = provisioningClientDoc["Config"]["StaticIpConfig"]["Dns"];
    ipDns = (char *)dns;
  }
  const char * server = provisioningClientDoc["Config"]["ServerAddress"]; 
  const char * name = provisioningClientDoc["Config"]["Name"]; 
  
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
