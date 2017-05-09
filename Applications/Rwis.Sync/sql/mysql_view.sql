CREATE VIEW view_seriescatalog
AS SELECT a.id AS id,
          a.parentid AS parentid,
          a.isfolder AS isfolder,
          a.sortorder AS sortorder,
          a.iconname AS iconname,
          e.description AS name,
          e.siteid AS siteid,
          f.units AS units,
          f.timeinterval AS timeinterval,
          f.statistic AS statistic,
          f.name AS parameter,
          a.tablename AS tablename,
          a.provider AS provider,
          a.connectionstring AS connectionstring,
          a.expression AS expression,
          a.notes AS notes,a.enabled AS enabled,
          (CASE 
                WHEN ((a.provider = 'HydrometDailySeries') AND (a.connectionstring LIKE 'server=GreatPlains%')) 
                    THEN 'GreatPlains' 
                WHEN ((a.provider = 'HDBSeries') AND (a.connectionstring LIKE 'server=LCHDB2%')) 
                     THEN 'LCHDB2' 
                WHEN ((a.provider = 'HDBSeries') AND (a.connectionstring LIKE 'server=UCHDB2%')) 
                     THEN 'UCHDB2' 
                WHEN ((a.provider = 'HydrometDailySeries') AND (a.connectionstring LIKE 'server=PN;%')) 
                      THEN 'PN' 
                WHEN ((a.provider = 'SFTPSeries') AND (a.connectionstring LIKE 'server=MPSFTP;%')) 
                      THEN 'MP' 
                ELSE '' END
            ) AS server,
          b.value AS t1,
          c.value AS t2,
          d.value AS count 
          FROM (
                  (
                    (
                      seriescatalog a 
                      LEFT JOIN seriesproperties b 
                      ON(((b.seriesid = a.id) AND (b.name = 't1')))) 
                      LEFT JOIN seriesproperties c 
                      ON(((c.seriesid = a.id) AND (c.name = 't2')))) 
                      LEFT JOIN seriesproperties d
                      ON(((d.seriesid = a.id) AND (d.name = 'count')))
                      LEFT JOIN sitecatalog e
                      ON(a.siteid=e.siteid)
                      LEFT JOIN parametercatalog f
                      ON (a.parameter=f.id)
                )
            WHERE a.isfolder=0;
