docker-compose -f .\docker-composev1.yml down
docker rmi aprendo-en-casa-offline-contenido.api:v1 --force
docker system prune --force
docker-compose -f .\docker-composev1.yml  build
docker network create ms
docker-compose -f .\docker-composev1.yml  up -d
pause