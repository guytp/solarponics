bool netClientInstantFail = false;
int netClientFailCount = 0;

void net_client_instant_fail(bool value) {
  netClientInstantFail = value;
}

void net_client_fail(String log, String screen) {
  if (!netClientInstantFail) {
    netClientFailCount++;      
  }

  if (netClientInstantFail || netClientFailCount > 5)
  {
    Serial.println("Fatal network error");
    Serial.println(log);
    screen_clear();
    screen_text(screen);
    delay(5000);
    ESP.restart();
  } else {
    Serial.print("Network error, ignoring it this time ");
    Serial.print(netClientFailCount);
    Serial.println("/5");
    Serial.println(log);
  }
}

void net_client_json_parse(JsonDocument &doc, WiFiClient &client) {
  deserializeJson(doc, client);
  Serial.println("JSON received");
  serializeJson(doc, Serial);
  Serial.println("");
}

uint8_t net_client_response_read(WiFiClient &client) {
  // Wait for up to 500ms for a response
  Serial.print("Waiting for response");
  ulong waitUntil = millis() + 5000;
  while (!client.available() && millis() < waitUntil) {
    Serial.print(".");
    delay(25);
  }
  Serial.println("");
  if (!client.available()) {
    net_client_fail("No response from server", "Resp Timeout");
    return -1;
  }

  uint8_t messageType = client.read();
  return messageType;
}

void net_client_error_handle(JsonDocument &doc, WiFiClient &client) {
  net_client_json_parse(doc, client);
  const char * urn = doc["Urn"];
  const char * message = doc["Message"];
  if (urn == NULL || strlen(urn) == 0) {
    net_client_fail(message, "urn:sp:unknown");
    return;
  }
  net_client_fail(message, urn);
}
