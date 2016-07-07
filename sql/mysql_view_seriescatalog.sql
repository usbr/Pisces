CREATE ALGORITHM=UNDEFINED DEFINER=`admin`@`%` SQL SECURITY DEFINER 

VIEW `timeseries`.`view_seriescatalog` 
AS select `a`.`id` AS `id`,`a`.`parentid` AS `parentid`,`a`.`isfolder` AS `isfolder`,
          `a`.`sortorder` AS `sortorder`,`a`.`iconname` AS `iconname`,`a`.`name` AS `name`,
          `a`.`siteid` AS `siteid`,`a`.`units` AS `units`,`a`.`timeinterval` AS `timeinterval`,
          `a`.`parameter` AS `parameter`,`a`.`tablename` AS `tablename`,`a`.`provider` AS `provider`,
          `a`.`connectionstring` AS `connectionstring`,`a`.`expression` AS `expression`,
          `a`.`notes` AS `notes`,`a`.`enabled` AS `enabled`,
           (
             case 
                when ((`a`.`provider` = 'HydrometDailySeries') 
                     and (`a`.`connectionstring` like 'server=GreatPlains%')) then 'GreatPlains' 
                when ((`a`.`provider` = 'HDBSeries') and (`a`.`connectionstring` like 'server=LCHDB2%')) 
                     then 'LCHDB2' 
                when ((`a`.`provider` = 'HDBSeries') and (`a`.`connectionstring` like 'server=UCHDB2%')) 
                     then 'UCHDB2' 
		        when ((`a`.`provider` = 'HydrometDailySeries') and (`a`.`connectionstring` like 'server=PN;%')) 
                      then 'PN' else '' end)
		   AS `server`,`b`.`value` AS `t1`,`c`.`value` AS `t2`,`d`.`value` AS `count` 
           from (((`timeseries`.`seriescatalog` `a` 
                left join `timeseries`.`seriesproperties` `b` 
                      on(((`b`.`seriesid` = `a`.`id`) and (`b`.`name` = 't1')))) 
                left join `timeseries`.`seriesproperties` `c` on(((`c`.`seriesid` = `a`.`id`) and (`c`.`name` = 't2')))) left join `timeseries`.`seriesproperties` `d`
                      on(((`d`.`seriesid` = `a`.`id`) and (`d`.`name` = 'count'))));
