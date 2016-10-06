


drop table alarm_recipient;
DROP TABLE alarm_phone_queue;
DROP TABLE alarm_definition;
DROP TABLE alarm_list;
drop table alarm_scripts;



CREATE TABLE alarm_list
(
  list character varying(256) NOT NULL primary key
    );
  

ALTER TABLE alarm_list
  OWNER TO hydromet;



CREATE TABLE alarm_definition
(
  id integer NOT NULL primary key,
  list character varying(256) NOT NULL DEFAULT ''  references alarm_list (list)  ,
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
  list character varying(256) NOT NULL DEFAULT '' references alarm_list(list),
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
  list character varying(20) NOT NULL references alarm_list (list) ,
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

ALTER TABLE alarm_scripts
  OWNER TO hydromet;


-- test data
insert into alarm_list values ( 'test');
insert into alarm_list values ( 'test2');
insert into alarm_recipient values (1,'test',1,'5272','karl','email.@work.com');
insert into alarm_recipient values (2,'test',2,'5272','karl','email.@work.com');

insert into alarm_phone_queue values (1,'test','rob','ob',56.42,'new',now(),'',now(),5);
--insert into alarm_phone_queue values (2,'5272,5272','rob','ob',56.42,'new',now(),'',now(),1);

