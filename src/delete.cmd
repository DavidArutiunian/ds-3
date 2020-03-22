@echo off
docker rmi backendapi:%1
docker rmi frontendclient:%1
docker image prune -f
