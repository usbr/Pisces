# build postgres database with empty hydromet schema
# docker build --tag db .
# docker run --name db -p 5432:5432 db

FROM postgres:9.6

ENV POSTGRESQL_PASSWORD=hydromet 

COPY hydromet.sql /docker-entrypoint-initdb.d/

