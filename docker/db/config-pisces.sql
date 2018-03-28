-- sql script to configure timeseries database
CREATE ROLE hydromet NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;

Create DATABASE timeseries owner = hydromet;
\connect timeseries
CREATE SCHEMA hydromet AUTHORIZATION hydromet;
ALTER DATABASE timeseries   SET search_path TO 'hydromet';
GRANT ALL ON DATABASE timeseries TO hydromet;
GRANT ALL ON SCHEMA hydromet TO hydromet;

CREATE ROLE grp_readonly NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;
GRANT USAGE ON SCHEMA hydromet TO grp_readonly;

ALTER DEFAULT PRIVILEGES IN SCHEMA hydromet
    GRANT SELECT ON TABLES
    TO grp_readonly;
