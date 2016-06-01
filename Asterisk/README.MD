
Here are some examples of asterisk CLI commands used:

asterisk -x database put family key value

asterisk -x "database put app_user rob_fb 124"

asterisk -x database del family key

asterisk -x "database show"

asterisk -x "dialplan reload"

asterisk -x "channel originate /boia_emm@hydromet_groups extension"

   same  => n,SayAlpha(${alarm_value})
   
   same =>n,Set(DB(hydromet/${alarm_confirmed})=1)   ; set database value within extension.
