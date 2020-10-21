docker-compose down
docker rmi aprendo-en-casa-offline-contenido.api --force
docker system prune --force
docker-compose build
docker network create ms
docker-compose up -d
pause