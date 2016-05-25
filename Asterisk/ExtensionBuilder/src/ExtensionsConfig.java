import java.io.PrintWriter;

public class ExtensionsConfig {

	public static void main(String[] args) {

		
		CallOutLists c = new CallOutLists();
		
		System.out.println("there are "+c.getGroupNames().length+ " groups");
		
		String group1 = c.getGroupNames()[0];
		System.out.println("numbers for group "+group1);
		
		
		String[] numbers = c.GetPhoneNumbers(group1);
			for (int i = 0; i < numbers.length; i++) {
				System.out.println(numbers[i]);
			}
		
		
		try{
			PrintWriter writer = new PrintWriter("extensions.conf", "UTF-8");

			//start of the extensions.conf
			writer.println("[LocalSets]\n" + 
					"exten => 200,1,Playback(tt-weasels)\n" +
					"same  => n,Hangup()\n" +
					"exten => 210,1,DIAL(SIP/210)\n" +
					"same  => n,Hangup()\n" +
					"exten => 211,1,DIAL(SIP/211)\n" +
					"same  => n,Hangup()\n" +
					"exten => 212,1,DIAL(SIP/212)\n" +
					"same  => n,Hangup()\n" +
					"exten => 213,1,DIAL(SIP/213)\n" +
					"same  => n,Hangup()\n");

			writer.println("[rob]\n" +
					"exten => msg1,1,NoOp(originates a call to rob)\n" +
					"same  => n,Originate(SIP/pn/6213,exten,almmsg,msg1,1,10)\n" +
					"same  => n,GotoIf($[${ORIGINATE_STATUS} = SUCCESS]?end)\n" +
					"same  => n(end),Hangup()\n");

			writer.println("[dectalk]\n" +
					"exten => robalarm,1,NoOp(calls karl and rob)\n" +
					"same  => n,Originate(SIP/pn/6213,exten,almmsg,msg1,1,10)\n" +
					"same  => n,GotoIf($[${ORIGINATE_STATUS} = SUCCESS]?end)\n" +
					"same  => n,Originate(SIP/pn/5272,exten,almmsg,msg1,1,10)\n" +
					"same  => n,GotoIf($[${ORIGINATE_STATUS} = SUCCESS]?end)\n" +
					"same  => n(end),Hangup()\n" +
					"exten => billalarm,1,NoOp(calls bill)\n" +
					"same  => n,Originate(SIP/pn/5278,exten,almmsg,msg1,1,10)\n" +
					"same  => n,GotoIf($[${ORIGINATE_STATUS} = SUCCESS]?end)\n" +
					"same  => n(end),Hangup()\n");

			writer.println("[almmsg]\n" +
					"exten => msg1,1,NoOp(first test alarm message)\n" +
					"same  => n,Wait(2)\n" +
					"same  => n,Playback(hydromet/hydrometintro)\n" +
					"same  => n,Playback(hydromet/wicews_gh)\n" +
					"same  => n,Playback(hydromet/value)\n" +
					"same  => n,SayAlpha(${DB(app_user/rob_fb)})\n" +
					"same  => n,Read(ackDigit,hydromet/press1,1,,2,5)\n" +
					"same  => n,Gotoif($[\"${ackDigit}\" = \"1\"]?ackalert)\n" +
					"same  => n,Wait(5)\n" +
					"\n" +
					"same  => n(ackalert),NoOp(Hydromet alert acknowledgement)\n" +
					"same  => n,Read(ackDigit,hydromet/acknowledging,1,,2,5)\n" +
					"same  => n,Gotoif($[\"${ackDigit}\" = \"1\"]?confirmed)\n" +
					"same  => n,Goto(unconfirmed)\n" +
					"\n" +
					"same  => n(confirmed),NoOp(Hydromet alarm confirmed)\n" +
					"same  => n,Playback(vm-goodbye)\n" +
					"same  => n,Hangup()\n" +
					"\n" +
					"same  => n(unconfirmed),NoOp(Hydromet alarm not confirmed)\n" +
					"same  => n,Playback(tt-weasels)\n" +
					"same  => n,Hangup()\n");

					writer.close();
					System.out.println("updated extensions.conf");

		}catch(Exception e){
			System.out.println("An Error Occured creating the file: " + e);
		}
	}
}
