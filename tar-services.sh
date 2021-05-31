#!/bin/bash
cd /c/code/solarponics
rm ingestion-server.tar provisioning-server.tar 2> /dev/null
cd Backend/ProvisioningServer/bin/Release/netcoreapp3.1/publish
rm ingestion-server.tar provisioning-server.tar 2> /dev/null
tar -cf /c/code/solarponics/provisioning-server.tar .
cd /c/code/solarponics
cd Backend/IngestionServer/bin/Release/netcoreapp3.1/publish
rm *.tar 2>/dev/null
tar -cf /c/code/solarponics/ingestion-server.tar .
cd /c/code/solarponics
sftp solarponics@10.2.42.232 << eof
put provisioning-server.tar
put ingestion-server.tar
exit
eof
rm ingestion-server.tar provisioning-server.tar
