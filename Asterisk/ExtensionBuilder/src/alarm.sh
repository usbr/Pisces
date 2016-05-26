#!/bin/bash
#  initiate an hydromet alarm in asterisk

EXT=boia_emm
ALARM_DEF=ark_fb
ALARM_VALUE=3212.55

echo $EXT
echo $ALARM_DEF
echo $ALARM_VALUE

asterisk -x "database put hydromet  alarm_definition ${ALARM_DEF}"
asterisk -x "database put hydromet  alarm_value  '${ALARM_VALUE}'"
asterisk -x "channel originate local/${EXT}@hydromet_groups extension"

