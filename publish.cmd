@echo  off
dotnet publish -r linux-arm --self-contained

ssh a.nrf.mn.lan "sudo killall RpiNrf"

echo Copying files...
scp -qr ./bin/Debug/netcoreapp2.1/linux-arm/publish a.nrf.mn.lan:~/nrf/a

ssh a.nrf.mn.lan "chmod +x ~/nrf/a/publish/RpiNrf && sudo /home/alarm/nrf/a/publish/RpiNrf"