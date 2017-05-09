--
-- PostgreSQL database dump
--

-- Dumped from database version 9.2.20
-- Dumped by pg_dump version 9.5.1

-- Started on 2017-04-07 10:28:45

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;


--
-- TOC entry 7 (class 2615 OID 16592)
-- Name: hydromet; Type: SCHEMA; Schema: -; Owner: postgres
--
CREATE ROLE grp_readonly
  NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;


CREATE SCHEMA hydromet
  AUTHORIZATION hydromet;

GRANT ALL ON SCHEMA hydromet TO hydromet;
GRANT USAGE ON SCHEMA hydromet TO grp_readonly;

ALTER DEFAULT PRIVILEGES IN SCHEMA hydromet
    GRANT SELECT ON TABLES
    TO grp_readonly;




GRANT ALL ON DATABASE timeseries TO hydromet;

--
-- TOC entry 1 (class 3079 OID 11727)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2118 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = hydromet, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 181 (class 1259 OID 16739)
-- Name: alarm_definition; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE alarm_definition (
    id integer NOT NULL,
    enabled boolean DEFAULT false NOT NULL,
    list character varying(256) DEFAULT ''::character varying NOT NULL,
    siteid character varying(256) DEFAULT ''::character varying NOT NULL,
    parameter character varying(256) DEFAULT ''::character varying NOT NULL,
    alarm_condition character varying(256) DEFAULT ''::character varying NOT NULL,
    clear_condition character varying(256) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE alarm_definition OWNER TO hydromet;

--
-- TOC entry 177 (class 1259 OID 16700)
-- Name: alarm_list; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE alarm_list (
    list character varying(256) NOT NULL
);


ALTER TABLE alarm_list OWNER TO hydromet;

--
-- TOC entry 184 (class 1259 OID 16779)
-- Name: alarm_log; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE alarm_log (
    datetime timestamp without time zone NOT NULL,
    message character varying(256) DEFAULT ''::character varying NOT NULL,
    alarm_phone_queue_id integer DEFAULT 0 NOT NULL
);


ALTER TABLE alarm_log OWNER TO hydromet;

--
-- TOC entry 182 (class 1259 OID 16753)
-- Name: alarm_phone_queue; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE alarm_phone_queue (
    id integer NOT NULL,
    alarm_definition_id integer DEFAULT (-1) NOT NULL,
    list character varying(256) DEFAULT ''::character varying NOT NULL,
    siteid character varying(256) DEFAULT ''::character varying NOT NULL,
    parameter character varying(256) DEFAULT ''::character varying NOT NULL,
    value double precision NOT NULL,
    status character varying(256) DEFAULT ''::character varying NOT NULL,
    status_time timestamp without time zone NOT NULL,
    confirmed_by character varying(256) DEFAULT ''::character varying NOT NULL,
    event_time timestamp without time zone NOT NULL,
    current_list_index integer DEFAULT (-1) NOT NULL,
    active boolean DEFAULT true NOT NULL
);


ALTER TABLE alarm_phone_queue OWNER TO hydromet;

--
-- TOC entry 180 (class 1259 OID 16729)
-- Name: alarm_recipient; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE alarm_recipient (
    id integer NOT NULL,
    list character varying(256) DEFAULT ''::character varying NOT NULL,
    call_order integer DEFAULT 0 NOT NULL,
    phone character varying(20) DEFAULT ''::character varying NOT NULL,
    name character varying(20) DEFAULT ''::character varying NOT NULL,
    email character varying(20) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE alarm_recipient OWNER TO hydromet;

--
-- TOC entry 183 (class 1259 OID 16769)
-- Name: alarm_scripts; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE alarm_scripts (
    id integer NOT NULL,
    text character varying(2048) DEFAULT ''::character varying NOT NULL,
    filename character varying(2048) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE alarm_scripts OWNER TO hydromet;

--
-- TOC entry 178 (class 1259 OID 16705)
-- Name: measurement; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE measurement (
    id integer NOT NULL,
    siteid character varying(256) DEFAULT ''::character varying NOT NULL,
    date_measured timestamp without time zone NOT NULL,
    stage double precision NOT NULL,
    discharge double precision NOT NULL,
    quality character varying(30) DEFAULT ''::character varying NOT NULL,
    party character varying(30) DEFAULT ''::character varying NOT NULL,
    notes character varying(300) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE measurement OWNER TO hydromet;

--
-- TOC entry 175 (class 1259 OID 16678)
-- Name: parametercatalog; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE parametercatalog (
    id character varying(100) NOT NULL,
    timeinterval character varying(1024) DEFAULT ''::character varying NOT NULL,
    units character varying(1024) DEFAULT ''::character varying NOT NULL,
    statistic character varying(1024) DEFAULT ''::character varying NOT NULL,
    name character varying(1024) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE parametercatalog OWNER TO hydromet;

--
-- TOC entry 169 (class 1259 OID 16593)
-- Name: piscesinfo; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE piscesinfo (
    name character varying(255) NOT NULL,
    value character varying(1024) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE piscesinfo OWNER TO hydromet;

--
-- TOC entry 172 (class 1259 OID 16638)
-- Name: quality_limit; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE quality_limit (
    tablemask character varying(100) NOT NULL,
    high double precision,
    low double precision,
    delta double precision
);


ALTER TABLE quality_limit OWNER TO hydromet;

--
-- TOC entry 179 (class 1259 OID 16717)
-- Name: rating_tables; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE rating_tables (
    id integer NOT NULL,
    version character varying(256) NOT NULL,
    siteid character varying(256) DEFAULT ''::character varying NOT NULL,
    x_variable character varying(256) DEFAULT ''::character varying NOT NULL,
    y_variable character varying(256) DEFAULT ''::character varying NOT NULL,
    csv_table character varying(100000) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE rating_tables OWNER TO hydromet;

--
-- TOC entry 171 (class 1259 OID 16625)
-- Name: scenario; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE scenario (
    sortorder integer DEFAULT 0 NOT NULL,
    name character varying(200) DEFAULT ''::character varying NOT NULL,
    path character varying(1024) DEFAULT ''::character varying NOT NULL,
    checked smallint DEFAULT 0 NOT NULL,
    sortmetric double precision DEFAULT 0 NOT NULL
);


ALTER TABLE scenario OWNER TO hydromet;

--
-- TOC entry 170 (class 1259 OID 16602)
-- Name: seriescatalog; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE seriescatalog (
    id integer NOT NULL,
    parentid integer DEFAULT 0 NOT NULL,
    isfolder smallint DEFAULT 0 NOT NULL,
    sortorder integer DEFAULT 0 NOT NULL,
    iconname character varying(100) DEFAULT ''::character varying NOT NULL,
    name character varying(200) DEFAULT ''::character varying NOT NULL,
    siteid character varying(2600) DEFAULT ''::character varying NOT NULL,
    units character varying(100) DEFAULT ''::character varying NOT NULL,
    timeinterval character varying(100) DEFAULT 'irregular'::character varying NOT NULL,
    parameter character varying(100) DEFAULT ''::character varying NOT NULL,
    tablename character varying(128) DEFAULT ''::character varying NOT NULL,
    provider character varying(200) DEFAULT ''::character varying NOT NULL,
    connectionstring character varying(2600) DEFAULT ''::character varying NOT NULL,
    expression character varying(2048) DEFAULT ''::character varying NOT NULL,
    notes character varying(2048) DEFAULT ''::character varying NOT NULL,
    enabled smallint DEFAULT 1 NOT NULL
);


ALTER TABLE seriescatalog OWNER TO hydromet;

--
-- TOC entry 176 (class 1259 OID 16690)
-- Name: seriesproperties; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE seriesproperties (
    id integer NOT NULL,
    seriesid integer DEFAULT 0 NOT NULL,
    name character varying(100) DEFAULT ''::character varying NOT NULL,
    value character varying(100) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE seriesproperties OWNER TO hydromet;

--
-- TOC entry 173 (class 1259 OID 16643)
-- Name: sitecatalog; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE sitecatalog (
    siteid character varying(255) NOT NULL,
    description character varying(1024) DEFAULT ''::character varying NOT NULL,
    state character varying(30) DEFAULT ''::character varying NOT NULL,
    latitude double precision DEFAULT 0,
    longitude double precision DEFAULT 0,
    elevation double precision DEFAULT 0,
    timezone character varying(30) DEFAULT ''::character varying NOT NULL,
    install character varying(30) DEFAULT ''::character varying NOT NULL,
    horizontal_datum character varying(30) DEFAULT ''::character varying NOT NULL,
    vertical_datum character varying(30) DEFAULT ''::character varying NOT NULL,
    vertical_accuracy double precision DEFAULT 0 NOT NULL,
    elevation_method character varying(100) DEFAULT ''::character varying NOT NULL,
    tz_offset character varying(10) DEFAULT ''::character varying NOT NULL,
    active_flag character varying(1) DEFAULT 'T'::character varying NOT NULL,
    type character varying(100) DEFAULT ''::character varying NOT NULL,
    responsibility character varying(30) DEFAULT ''::character varying NOT NULL,
    agency_region character varying(30) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE sitecatalog OWNER TO hydromet;

--
-- TOC entry 174 (class 1259 OID 16667)
-- Name: siteproperties; Type: TABLE; Schema: hydromet; Owner: hydromet
--

CREATE TABLE siteproperties (
    id integer NOT NULL,
    siteid character varying(256) DEFAULT ''::character varying NOT NULL,
    name character varying(1024) DEFAULT ''::character varying NOT NULL,
    value character varying(1024) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE siteproperties OWNER TO hydromet;

--
-- TOC entry 1984 (class 2606 OID 16752)
-- Name: alarm_definition_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY alarm_definition
    ADD CONSTRAINT alarm_definition_pkey PRIMARY KEY (id);


--
-- TOC entry 1976 (class 2606 OID 16704)
-- Name: alarm_list_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY alarm_list
    ADD CONSTRAINT alarm_list_pkey PRIMARY KEY (list);


--
-- TOC entry 1990 (class 2606 OID 16785)
-- Name: alarm_log_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY alarm_log
    ADD CONSTRAINT alarm_log_pkey PRIMARY KEY (datetime);


--
-- TOC entry 1986 (class 2606 OID 16768)
-- Name: alarm_phone_queue_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY alarm_phone_queue
    ADD CONSTRAINT alarm_phone_queue_pkey PRIMARY KEY (id);


--
-- TOC entry 1982 (class 2606 OID 16738)
-- Name: alarm_recipient_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY alarm_recipient
    ADD CONSTRAINT alarm_recipient_pkey PRIMARY KEY (id);


--
-- TOC entry 1988 (class 2606 OID 16778)
-- Name: alarm_scripts_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY alarm_scripts
    ADD CONSTRAINT alarm_scripts_pkey PRIMARY KEY (id);


--
-- TOC entry 1968 (class 2606 OID 16699)
-- Name: idx1; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY siteproperties
    ADD CONSTRAINT idx1 UNIQUE (siteid, name);


--
-- TOC entry 1978 (class 2606 OID 16716)
-- Name: measurement_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY measurement
    ADD CONSTRAINT measurement_pkey PRIMARY KEY (id);


--
-- TOC entry 1972 (class 2606 OID 16689)
-- Name: parametercatalog_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY parametercatalog
    ADD CONSTRAINT parametercatalog_pkey PRIMARY KEY (id, timeinterval);


--
-- TOC entry 1958 (class 2606 OID 16601)
-- Name: piscesinfo_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY piscesinfo
    ADD CONSTRAINT piscesinfo_pkey PRIMARY KEY (name);


--
-- TOC entry 1964 (class 2606 OID 16642)
-- Name: quality_limit_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY quality_limit
    ADD CONSTRAINT quality_limit_pkey PRIMARY KEY (tablemask);


--
-- TOC entry 1980 (class 2606 OID 16728)
-- Name: rating_tables_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY rating_tables
    ADD CONSTRAINT rating_tables_pkey PRIMARY KEY (id);


--
-- TOC entry 1962 (class 2606 OID 16637)
-- Name: scenario_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY scenario
    ADD CONSTRAINT scenario_pkey PRIMARY KEY (sortorder);


--
-- TOC entry 1960 (class 2606 OID 16624)
-- Name: seriescatalog_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY seriescatalog
    ADD CONSTRAINT seriescatalog_pkey PRIMARY KEY (id);


--
-- TOC entry 1974 (class 2606 OID 16697)
-- Name: seriesproperties_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY seriesproperties
    ADD CONSTRAINT seriesproperties_pkey PRIMARY KEY (id);


--
-- TOC entry 1966 (class 2606 OID 16666)
-- Name: sitecatalog_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY sitecatalog
    ADD CONSTRAINT sitecatalog_pkey PRIMARY KEY (siteid);


--
-- TOC entry 1970 (class 2606 OID 16677)
-- Name: siteproperties_pkey; Type: CONSTRAINT; Schema: hydromet; Owner: hydromet
--

ALTER TABLE ONLY siteproperties
    ADD CONSTRAINT siteproperties_pkey PRIMARY KEY (id);


-- Completed on 2017-04-07 10:28:45

--
-- PostgreSQL database dump complete
--

