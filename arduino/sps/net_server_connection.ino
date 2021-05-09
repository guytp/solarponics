#define SERVERPORT 5901
IPAddress netServerConnectionServerIp;
bool netServerConnectionIsStarted;

void net_server_connection_setup()
{
  String serverIp = config_get_server();
  netServerConnectionServerIp.fromString(serverIp);
  Serial.print("Remote server is at ");
  Serial.println(serverIp);
}

void net_server_connection_start()
{
  if (netServerConnectionIsStarted) {
    net_server_connection_stop();
  }
  
  Serial.println("Opening TCP/IP connection to server");
  netServerConnectionIsStarted = true;
}

void net_server_connection_stop()
{
  if (!netServerConnectionIsStarted)
  {
    return;
  }
  
  Serial.println("Closing TCP/IP connection to server");
  netServerConnectionIsStarted = false;
}
