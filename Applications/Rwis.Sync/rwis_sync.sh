#!/bin/bash
# update data from regional databases
. /opt/mono/env.sh
export TMPDIR=/data/apps/Pisces/tmp
mkdir -p /data/apps/Pisces/Rwis.Sync/log
cd /data/apps/Pisces/Rwis.Sync
find /data/apps/Pisces/Rwis.Sync/log -name rwis_sync.log -mtime 14 -exec rm {} \;
mono /data/apps/Pisces/Rwis.Sync/bin/Debug/Rwis.Sync.exe --update=all --t1=lastyear 2>&1 | tee /data/apps/Pisces/Rwis.Sync/log/rwis_sync.log`date +%m%d%Y`
