#!/bin/bash
# update data from regional databases
. /opt/mono/env.sh
cd /data/apps/Pisces
mono /data/apps/Pisces/Rwis.Sync/bin/Debug/Rwis.Sync.exe --t1=lastyear > rwis_sync.log
cp rwis_sync.log `date +%A`
