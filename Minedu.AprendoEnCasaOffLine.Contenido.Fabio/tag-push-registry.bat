set ms=doctrina
docker-compose down
docker rmi 192.168.0.10/linu1.0/%ms% --force
docker rmi %ms% --force
docker system prune --force
docker-compose build
docker tag %ms% 192.168.0.10/linu1.0/%ms%
docker login 192.168.0.10 --username madiaz --password Pandemia2020
docker push 192.168.0.10/linu1.0/%ms%
pause