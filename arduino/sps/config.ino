#include "EEPROM.h"

void config_setup() {
  Serial.println("Starting up nvram");
  NVS.begin();
}

void config_reset() {
  Serial.println("Wiping nvram");
  NVS.eraseAll();
}

bool config_get_isProvisioned() {
  int val = NVS.getInt("isProvisioned") == 1;
  Serial.print("Config isProvisioned = ");
  Serial.println(val);
  return val == 1;
}

void config_set_isProvisioned() {
  Serial.println("Setting isProvisioned = 1");
  NVS.setInt("isProvisioned", 1);
}

bool config_get_use_static_ip() {
  int val = NVS.getInt("useStaticIp") == 1;
  Serial.print("Config useStaticIp = ");
  Serial.println(val);
  return val == 1;
}

void config_set_use_static_ip(bool value) {
  Serial.print("Setting useStaticIp = ");
  Serial.println(value ? "1" : "0");
  NVS.setInt("useStaticIp", value ? 1 : 0);
}

bool config_get_use_wifi() {
  int val = NVS.getInt("useWifi") == 1;
  Serial.print("Config useWifi= ");
  Serial.println(val);
  return val == 1;
}

void config_set_use_wifi(bool value) {
  Serial.print("Setting useWifi = ");
  Serial.println(value ? "1" : "0");
  NVS.setInt("useWifi", value ? 1 : 0);
}

String config_get_ssid() {
  String val = NVS.getString("ssid");
  Serial.print("Config ssid = ");
  Serial.println(val);
  return val;
}

void config_set_ssid(char * value) {
  Serial.print("Setting ssid = ");
  Serial.println(value);
  NVS.setString("ssid", value);
}

String config_get_ssid_key() {
  String val = NVS.getString("netWifiKey");
  Serial.print("Config netWifiKey = ");
  Serial.println(val);
  return val;
}


void config_set_ssid_key(char * value) {
  Serial.print("Setting netWifiKey = ");
  Serial.println(value);
  NVS.setString("netWifiKey", value);
}

String config_get_ip() {
  String val = NVS.getString("ip");
  Serial.print("Config ip = ");
  Serial.println(val);
  return val;
}

void config_set_ip(char * value) {
  Serial.print("Setting ip = ");
  Serial.println(value);
  NVS.setString("ip", value);
}


String config_get_broadcast() {
  String val = NVS.getString("broadcast");
  Serial.print("Config broadcast = ");
  Serial.println(val);
  return val;
}

void config_set_broadcast(char * value) {
  Serial.print("Setting broadcast = ");
  Serial.println(value);
  NVS.setString("broadcast", value); 
}

String config_get_gateway() {
  String val = NVS.getString("gateway");
  Serial.print("Config gateway = ");
  Serial.println(val);
  return val;
}

void config_set_gateway(char * value) {
  Serial.print("Setting gateway = ");
  Serial.println(value);
  NVS.setString("gateway", value);
}

String config_get_dns() {
  String val = NVS.getString("dns");
  Serial.print("Config dns = ");
  Serial.println(val);
  return val;
}

void config_set_dns(char * value) {
  Serial.print("Setting dns = ");
  Serial.println(value);
  NVS.setString("dns", value);
}

String config_get_server() {
  String val = NVS.getString("server");
  Serial.print("Config server = ");
  Serial.println(val);
  return val;
}

void config_set_server(char * value) {
  Serial.print("Setting server = ");
  Serial.println(value);
  NVS.setString("server", value);
}
