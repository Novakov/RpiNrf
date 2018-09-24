@echo  off
dotnet publish -r linux-arm --self-contained

scp -r bin/Debug/netcoreapp2.1/linux-arm/publish a.nrf.mn.lan:~/nrf/a