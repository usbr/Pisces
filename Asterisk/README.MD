
lrgs5*CLI> database put app_user rob_fb 123
lrgs5*CLI> database del app_user rob_fb


asterisk -x "database put app_user rob_fb 124"

 asterisk -x "dialplan reload"

asterisk -x "channel originate local/msg2@vartest extension"
same  => n,SayNumber(${DB(app_user/rob_fb)})

same =>n,Set(DB(hydromet/karl_fb)=1)   ; set database value within extension.
