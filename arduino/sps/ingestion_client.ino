WiFiClient ingestionClientClient;
DynamicJsonDocument ingestionClientDoc(4096);
bool ingestionClientStarted;
unsigned long ingestionClientNextTimeCheck = 0;
unsigned long ingestionClientTimestamp = 0;
unsigned long ingestionClientTimestampUpdatedAt = 0;
unsigned long ingestionClientSequence = 0;
unsigned long ingestionClientNextSensorPush = 0;
bool ingestionClientAuthenticated;
unsigned long ingestionClientExpectedAck = 0;

void ingestion_client_start()
{
  net_client_instant_fail(false);
  String server = config_get_server();
  Serial.print("Remote server is at ");
  Serial.println(server);
  if (!ingestionClientClient.connect(server.c_str(), 4201)) {
    net_client_fail("Cannot connect to ingestion server", "Cannot Connect Ing");
    return;
  }

  ingestionClientStarted = true;
}

void ingestion_client_loop()
{
  if (!ingestionClientStarted) {
    return;
  }

  if (!ingestionClientClient.connected()) {
    net_client_fail("No longer connected to server", "Disconnected");
    return;
  }

  if (!ingestionClientAuthenticated) {
    Serial.println("Authenticating against server");
    ingestionClientExpectedAck = ingestionClientSequence;
    String json = "{\"Sequence\": ";
    json += ingestionClientSequence++;
    json += ", \"SerialNumber\": \"";
    json += SerialNumber;
    json += "\", \"OpCode\": 4}";
    ingestion_client_command(4, json.c_str());
  }

  if (millis() > ingestionClientNextTimeCheck) {
    Serial.println("Updating server time");
    String json = "{\"Sequence\": ";
    json += ingestionClientSequence++;
    json += ", \"OpCode\": 6}";
    ingestion_client_command(6, json.c_str());
  }

  if (millis() > ingestionClientNextSensorPush) {
    ingestion_client_send_reading(sensor_runner_temperature(), 1);
    ingestion_client_send_reading(carbon_dioxide_value(), 2);
    ingestion_client_send_reading(sensor_runner_humidity(), 3);
    ingestionClientNextSensorPush = millis() + 15000;
  }
}

void ingestion_client_send_reading(float value, uint8_t type) {
  if (value <= 0) {
    Serial.print("Skipping reading of ");
    Serial.print(type);
    Serial.println(" as it is <= 0");
    return;
  }
  
  Serial.print("Sending ");
  Serial.print(type);
  Serial.print(" reading of ");
  Serial.println(value);
  unsigned long timestamp = (millis()/1000) - ingestionClientTimestampUpdatedAt + ingestionClientTimestamp;
  ingestionClientExpectedAck = ingestionClientSequence;
  String json = "{\"Sequence\": ";
  json += ingestionClientSequence++;
  json += ", \"Reading\": ";
  json += value;
  json += ", \"Type\": ";
  json += type;
  json += ", \"Timestamp\": ";
  json += timestamp;
  json += ", \"Number\": 1, \"OpCode\": 3}";
  ingestion_client_command(3, json.c_str());
}

void ingestion_client_command(uint8_t opCode, String json) {
  int strLen = json.length();
  Serial.print("Request is ");
  Serial.print(strLen);
  Serial.println(" bytes");
  Serial.println(json);
  uint8_t * buf = (uint8_t*)malloc(strLen + 2);
  buf[0] = opCode;
  json.getBytes(buf + 1, strLen + 1);
  ingestionClientClient.write(buf, strLen + 1);
  free(buf);

  uint8_t messageType = net_client_response_read(ingestionClientClient);

  // We've got data
  Serial.println("Response data received");  
  if (messageType == 5) {
    net_client_error_handle(ingestionClientDoc, ingestionClientClient);
    return;
  } else if (messageType == 1) {
    ingestion_client_handle_server_handshake();
    return;
  } else if (messageType == 2) {
    ingestion_client_handle_ack();
    return;
  } else if (messageType == 7) {
    ingestion_client_handle_time();
    return;
  }
  Serial.print("Message type ");
  Serial.print(messageType);
  Serial.println(" is not supported");
  net_client_fail("Unexpected response received", "Unexpected Resp");

}

void ingestion_client_handle_server_handshake() {
  net_client_json_parse(ingestionClientDoc, ingestionClientClient);
  if (ingestionClientAuthenticated) {
    net_client_fail("Received auth response, but already authenticated", "Unexpected Auth");
    return;
  }
  ingestionClientAuthenticated = true;
  Serial.println("Authenticated to server");
}

void ingestion_client_handle_ack() {
  net_client_json_parse(ingestionClientDoc, ingestionClientClient);
  unsigned long ack = ingestionClientDoc["OriginalSequence"];
  if (ack != ingestionClientExpectedAck) {
    net_client_fail("Unexpected ack from server", "Unexpected Ack"); 
  }
  
  if (!ingestionClientAuthenticated) {
    ingestionClientAuthenticated = true;
    Serial.println("Authenticated to server");
    return;
  }

  Serial.println("Server acknowledged request");
}

void ingestion_client_handle_time() {
  net_client_json_parse(ingestionClientDoc, ingestionClientClient);
  ingestionClientTimestamp = ingestionClientDoc["Timestamp"];
  Serial.print("Time from server: ");
  Serial.println(ingestionClientTimestamp);
  ingestionClientTimestampUpdatedAt = millis() / 1000;
  ingestionClientNextTimeCheck = millis() + 300000;
}


/*
 * specific time handling
 * 
 * send correct time command
 * 
 * test this work
 */
