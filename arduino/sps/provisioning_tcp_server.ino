
WiFiServer server(5900);

void provisioning_tcp_server_setup() {
  server.begin();
}

void provisioning_tcp_server_loop() {
  
 WiFiClient client = server.available();
  
  if (!client) {
    return;
  }
  Serial.println("New WiFi client");

  while (client.connected()) {
    unsigned long mustCompleteBy = millis() + 1000;

    while (!client.available()) {
      delay(10);
      if (millis() > mustCompleteBy) {
        Serial.println("WiFi client did not respond in time, disconnecting them");
        client.stop();
        return;
      }
    }

    byte bytesToRead = client.read();
    Serial.print("Inbound request for ");
    Serial.print(bytesToRead);
    Serial.println(" bytes");
    String total = String();
    while(bytesToRead > 0) {
      if (millis() > mustCompleteBy) {
        Serial.println("WiFi client did not respond in time, disconnecting them");
        client.stop();
        return;
      }
          
      while (!client.available()) {
        delay(10);
        if (millis() > mustCompleteBy) {
          Serial.println("WiFi client did not respond in time, disconnecting them");
          client.stop();
          return;
        }
      }

      int toRead = client.available();
      Serial.print("Available ");
      Serial.print(toRead);
      Serial.println(" bytes");
      if (toRead > bytesToRead) {
        toRead = bytesToRead;
      }
      Serial.print("Reading ");
      Serial.print(toRead);
      Serial.println(" bytes");

      for (int i = 0; i < toRead; i++) {
        char c = client.read();
        total += c;
        bytesToRead--;
      }
    }

    Serial.print("Received: ");
    Serial.println(total);
    
    client.print("res=ok&ser="SerialNumber);
    delay(150);
    client.stop();

    char * message = (char *)total.c_str();
    char * ssid = provisioning_tcp_server_query_split(message, "si");
    config_set_ssid(ssid);
    char * ssidKey = provisioning_tcp_server_query_split(message, "sk");
    config_set_ssid_key(ssidKey);
    char * ip = provisioning_tcp_server_query_split(message, "ip");
    config_set_ip(ip);
    char * broadcast = provisioning_tcp_server_query_split(message, "bc");
    config_set_broadcast(broadcast);
    char * gateway = provisioning_tcp_server_query_split(message, "gw");
    config_set_gateway(gateway);
    char * dns = provisioning_tcp_server_query_split(message, "d1");
    config_set_dns(dns);
    char * server = provisioning_tcp_server_query_split(message, "sr");
    config_set_server(server);

    Serial.println("Setting as provisioned");
    config_set_isProvisioned();
    Serial.println("Rebooting in 3 seconds...");
    delay(3000);
    Serial.println("Rebooting...");
    ESP.restart();
  }
  Serial.println("Finished with WiFi client");
}


char* provisioning_tcp_server_query_split(char * a_tag_list, char* a_tag)
{
    /* 'strtok' modifies the string. */
    char* tag_list_copy = (char*)malloc(strlen(a_tag_list) + 1);
    char* result        = 0;
    char* s;

    strcpy(tag_list_copy, a_tag_list); //original to copy


    s = strtok(tag_list_copy, "&"); //Use delimiter "&"
    while (s)
    {
        char* equals_sign = strchr(s, '=');
        if (equals_sign)
        {
            *equals_sign = 0;
            //Use string compare to find required tag
            if (0 == strcmp(s, a_tag))
            {
                equals_sign++;
                result = (char*)malloc(strlen(equals_sign) + 1);
                strcpy(result, equals_sign);
            }
        }
        s = strtok(0, "&");
    }
    free(tag_list_copy);
    return result;
}
