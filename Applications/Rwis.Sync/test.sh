#!/bin/bash
# build and test sync program
# starting in ./Rwis.Sync directory
cd ..
. /opt/mono/env.sh
git pull --rebase
./build.sh
read -p "press enter to continue"
rm  ./Rwis.Sync/bin/Debug/Mono.Security.dll
cp  ../Rwis.Sync.exe.config  ./Rwis.Sync/bin/Debug
read -p "Press enter to run simple web test"
mono ./Rwis.Sync/bin/Debug/Rwis.Sync.exe --test-web > test.txt
head test.txt
read -p "press enter to run real web test"
mono ./Rwis.Sync/bin/Debug/Rwis.Sync.exe --t1=lastweek
