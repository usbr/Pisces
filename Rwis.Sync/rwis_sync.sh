#!/bin/bash
# update data from regional databases
. /opt/mono/env.sh
TMPDIR=/data/apps/Pisces/tmp
export TMPDIR
rm -fr /data/apps/Pisces/tmp
mkdir -p /data/apps/Pisces/tmp
cd /data/apps/Pisces
mono /data/apps/Pisces/Rwis.Sync/bin/Debug/Rwis.Sync.exe --t1=lastyear > rwis_sync.log
cp rwis_sync.log `date +%A`
