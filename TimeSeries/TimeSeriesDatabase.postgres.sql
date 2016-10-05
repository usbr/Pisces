


drop table alarm_recipient;
DROP TABLE alarm_phone_queue;
DROP TABLE alarm_definition;
DROP TABLE alarm_group_names;
drop table alarm_scripts;



CREATE TABLE alarm_group_names
(
  alarm_group character varying(256) NOT NULL primary key,
  name character varying(256) NOT NULL DEFAULT ''
  );
  

ALTER TABLE alarm_group_names
  OWNER TO hydromet;



CREATE TABLE alarm_definition
(
  id integer NOT NULL primary key,
  alarm_group character varying(256) NOT NULL DEFAULT ''  references alarm_group_names (alarm_group)  ,
  siteid character varying(256) NOT NULL references sitecatalog (siteid),
  parameter character varying(256) NOT NULL DEFAULT '',
  alarm_condition character varying(256) NOT NULL DEFAULT '',
  clear_condition character varying(256) NOT NULL DEFAULT '',
  message character varying(256) NOT NULL DEFAULT '',
  priority int not null default 10
);


ALTER TABLE alarm_definition
  OWNER TO hydromet;


CREATE TABLE alarm_phone_queue
(
  id integer NOT NULL primary key,
  phone_numbers character varying(256) NOT NULL DEFAULT '',
  siteid character varying(256) NOT NULL references sitecatalog (siteid),
  parameter character varying(256) NOT NULL DEFAULT '',
  value double precision NOT NULL,
  status character varying(256) NOT NULL DEFAULT 'new' check (status in ('new','unconfirmed','confirmed','cleared') ),
  status_time timestamp without time zone NOT NULL,
  confirmed_by character varying(256) NOT NULL DEFAULT '',
  event_time timestamp without time zone NOT NULL,
  priority int not null default 10
  );
  

ALTER TABLE alarm_phone_queue
  OWNER TO hydromet;




  CREATE TABLE alarm_recipient
(
  id integer NOT NULL primary key,
  alarm_group character varying(20) NOT NULL references alarm_group_names (alarm_group) ,
  call_order integer NOT NULL DEFAULT 0,
  phone character varying(20) NOT NULL DEFAULT '',
  name character varying(30) NOT NULL DEFAULT '',
  email character varying(30) NOT NULL DEFAULT ''
  );

ALTER TABLE alarm_recipient
  OWNER TO hydromet;

CREATE Table alarm_scripts
(
 id integer NOT NULL primary key,
 text character varying(2048) NOT NULL DEFAULT '',
 filename character varying(2048) NOT NULL DEFAULT ''
);



-- test data
insert into alarm_group_names values ( 'test','test to office ');
insert into alarm_group_names values ( 'test2','test to home ');
insert into alarm_phone_queue values (1,'test','abei','ob',56.42,'new',now(),'',now());

