import java.io.PrintWriter;

public class ExtensionsConfig {

	public static void main(String[] args) {


		CallOutLists list = new CallOutLists();

		System.out.println("there are "+list.getGroupNames().length+ " groups");

		String group1 = list.getGroupNames()[0];
		System.out.println("numbers for group "+group1);


		String[] numbers = list.GetPhoneNumbers(group1);
		for (int i = 0; i < numbers.length; i++) {
			System.out.println(numbers[i]);
		}


		try{
			PrintWriter writer = new PrintWriter("extensions.conf", "UTF-8");

			//start of the extensions.conf
			writer.println(";  inputs: (volatile temporary variable) -- set by bash script\n"+
					";    DB(hydromet/alarm_definition) (boii_ob)   i.e. wave file\n"+
					";    DB(hydromet/alarm_value)      ( 44.5)  degF\n"+
					"\n;  internal asterisk variables\n"+
					";   ${alarm_definition}    =  boii_ob \n"+
					";   ${alarm_confirmed}  =  boii_ob_confirmed   (set to zero to start, set to one when confirmed)\n"+
					";   ${alarm_value} = 44.5 \n");
					writer.println("[hydromet_groups]");

					for (int i = 0; i < list.getGroupNames().length; i++) {
						writer.println("exten => " + list.getGroupNames()[i] + ",1,NoOp()");

						for (int j = 0; j < list.GetPhoneNumbers(list.getGroupNames()[i]).length; j++) {
							writer.println("same  => n,Originate(SIP/pn/" + list.GetPhoneNumbers(list.getGroupNames()[i])[j] +
									",exten,hydromet_alarm,alarmmsg,1,10)\n" +
									"same  => n,GotoIf($[${DB(hydromet/${alarm_comfirmed})}=1]?end)");
						}
						writer.println("same  => n(end),Hangup()");
					}
					writer.println();
					writer.println("[hydromet_alarm]\n"+
							"exten => alarmmsg,1,NoOp()\n"+
							"same  => n,Set(alarm_definition=${DB(hydromet/alarm_definition)})" +
							"same  => n,Verbose(${DB_DELETE(hydromet/alarm_definition)})" +
							"same  => n,Set(alarm_confirmed=${DB(hydromet/alarm_confirmed)})" +
							"same  => n,Set(DB(hydromet/alarm_comfirmed)=0)" +
							"same  => n,Set(alarm_value=${DB(hydromet/alarm_value)})" +
							"same  => n,Wait(2)\n"+
							"same  => n,Playback(hydromet/hydrometintro)\n"+
							"same  => n,Playback(hydromet/${alarm_definition}))\n"+
							"same  => n,Playback(hydromet/value)\n"+
							"same  => n,SayAlpha(${alarm_value})\n"+
							"same  => n,Read(ackDigit,hydromet/press1,1,,2,5)\n"+
							"same  => n,Gotoif($[\"${ackDigit}\" = \"1\"]?ackalert)\n"+
							"same  => n,Wait(5)\n"+
							"same  => n(ackalert),NoOp()\n"+
							"same  => n,Read(ackDigit,hydromet/acknowledging,1,,2,5)\n"+
							"same  => n,Gotoif($[\"${ackDigit}\" = \"1\"]?confirmed)\n"+
							"same  => n,Goto(unconfirmed)\n"+
							"same  => n(confirmed),NoOp()\n"+
							"same  => n,Playback(vm-goodbye)\n"+
							"same  => n,Set(DB(hydromet/${alarm_comfirmed})=1)\n"+
							"same  => n,Hangup()\n"+
							"same  => n(unconfirmed),NoOp()\n"+
							"same  => n,Playback(vm-goodbye)");

					writer.close();
					System.out.println("updated extensions.conf");

		}catch(Exception e){
			System.out.println("An Error Occured creating the file: " + e);
		}
	}
}
