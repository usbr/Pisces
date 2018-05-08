#start containers:

docker-compose up

#connect to database container.
docker exec -it docker_db_1 /bin/bash

# connect to database container
docker exec -it docker_api_1 bash

Restore postgres data from linked path

oot@61f198fd41cd:/# cd /data/db
oot@61f198fd41cd:/data/db# ./db_lean_restore



###  other examples....
docker-compose exec web sh

docker run -v ${PWD}:/data/api api /bin/bash  

docker run -v c:\data\api:/data/api -p 5000:5000 api

docker -D run -v c:\data\api:/data/api -p 5000:5000 -e PiscesAPIDatabase=postgres -e ConnectionString="server=db;database=timeseries;user id=web_user" api


