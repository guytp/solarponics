int provisioningNetWifiLastStatus = -777;
bool provisioningNetProvisioningHasStarted = false;
bool provisioningNetFirstCheck = false;
bool provisioningNetSecondCheck = false;
const char * provisioningNetSsid = "SpProvisioning";
const char * provisioningNetWifiKey = "S0larP0nics!";
  
void provisioning_net_wifi_setup() {
  Serial.println("Configuring WiFi for provisioning");
  wifi_connect(provisioningNetSsid, provisioningNetWifiKey, false);
}

void provisioning_net_wifi_loop() {
  int status = WiFi.status();
  bool isConnected = status == WL_CONNECTED;
  
  if (status == provisioningNetWifiLastStatus)
  {
    // If we cannot connect within 15 seconds, reboot
    if (!isConnected and millis() > 5000 && !provisioningNetFirstCheck) {
      Serial.println("Resetting WiFi (try 1)");
      wifi_connect(provisioningNetSsid, provisioningNetWifiKey, false);
      provisioningNetFirstCheck = true;
    }
    else if (!isConnected and millis() > 10000 && !provisioningNetSecondCheck) {
      Serial.println("Resetting WiFi (try 2)");
      wifi_connect(provisioningNetSsid, provisioningNetWifiKey, false);
      provisioningNetSecondCheck = true;
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

  if (isConnected) {
      Serial.print("IP Address ");
      Serial.println(WiFi.localIP());
  }
  screen_wifi_status("SpProvisioning", isConnected);

  if (isConnected) {
    provisioningNetProvisioningHasStarted = true;
    provisioning_client_start();
  } else if (provisioningNetProvisioningHasStarted) {
    Serial.println("Provisioning aborted due to new WiFi status, rebooting");
    ESP.restart();
  }
  provisioningNetWifiLastStatus = status;
}
