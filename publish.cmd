@echo  off
dotnet publish -r linux-arm --self-contained

echo Copying files...
scp -r ./bin/Debug/netcoreapp2.1/linux-arm/publish a.nrf.mn.lan:~/nrf/a

ssh a.nrf.mn.lan "chmod +x ~/nrf/a/publish/RpiNrf && sudo /home/alarm/nrf/a/publish/RpiNrf"