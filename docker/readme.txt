To start containers:

docker-compose up


To connect to database container.


C:\data\db> docker exec -it 61f198fd41cd /bin/bash

C:\data\db> docker exec -it docker_db_1 /bin/bash

Restore postgres data from linked path

oot@61f198fd41cd:/# cd /data/db
oot@61f198fd41cd:/data/db# ./db_lean_restore


docker-compose exec web sh




