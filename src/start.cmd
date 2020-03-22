@echo off
call config.cmd
docker run -d --rm -p %BACKEND_API_PORT%:5000 --name backendapi backendapi:%1
docker run -e BACKEND_API_HOST=%BACKEND_API_HOST% -d --rm --link %BACKEND_API_HOST% -p %FRONTEND_CLIENT_PORT%:80 --name frontendclient frontendclient:%1
