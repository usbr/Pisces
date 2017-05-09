
 -- triggers to enforce RWIS data standards
 

DELIMITER $$
-- before inserting new id
DROP TRIGGER IF EXISTS before_insert_seriescatalog$$
CREATE TRIGGER before_insert_seriescatalog
    BEFORE INSERT ON seriescatalog FOR EACH ROW
    BEGIN
        -- condition to check time interval 
        IF NEW.timeinterval not in ('Daily', 'Monthly') THEN
            -- hack to solve absence of SIGNAL/prepared statements in triggers
            UPDATE `RWIS Error: invalid time iterval` SET x=1;
        END IF;
        
         -- condition to check time interval 
        IF NEW.parameter  not in (select id from  parametercatalog) THEN
            -- hack to solve absence of SIGNAL/prepared statements in triggers
            UPDATE `RWIS Error: invalid parameter code` SET x=1;
        END IF;
        
        -- condition to check site codes
        IF NEW.siteid  not in (select siteid from  sitecatalog) THEN
            -- hack to solve absence of SIGNAL/prepared statements in triggers
            UPDATE `RWIS Error: invalid site code` SET x=1;
        END IF;
    END$$
DELIMITER ;


DELIMITER $$
-- before inserting new id
DROP TRIGGER IF EXISTS before_delete_seriescatalog$$
CREATE TRIGGER before_delete_seriescatalog
    BEFORE DELETE ON seriescatalog FOR EACH ROW
    BEGIN
        -- condition to check if folder 
        IF OLD.isfolder = 1 THEN
            -- hack to solve absence of SIGNAL/prepared statements in triggers
            UPDATE `RWIS Error: folder deletions not allowed` SET x=1;
        END IF;
        
        
    END$$
DELIMITER ;

