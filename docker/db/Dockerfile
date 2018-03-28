 #Use an official Centos/postgres
FROM postgres:9.6

ENV POSTGRESQL_PASSWORD=hydromet 

COPY config-pisces.sql /docker-entrypoint-initdb.d/

