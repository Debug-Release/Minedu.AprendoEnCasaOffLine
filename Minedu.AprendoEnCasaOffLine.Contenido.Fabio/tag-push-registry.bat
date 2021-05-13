set ms=doctrina
docker-compose down
docker rmi registry.sunedu.gob.pe/siu-dev/%ms% --force
docker rmi %ms% --force
docker system prune --force
docker-compose build
docker tag %ms% registry.sunedu.gob.pe/siu-dev/%ms%
docker login registry.sunedu.gob.pe --username madiaz --password Pandemia2020
docker push registry.sunedu.gob.pe/siu-dev/%ms%
pause