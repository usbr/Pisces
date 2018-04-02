#build Database
docker build --tag pg .
# run the pg database in the background
docker run -d  --name pg -p 5432:5432 pg