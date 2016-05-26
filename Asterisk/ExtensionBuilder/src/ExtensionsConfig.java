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
			writer.println("[hydromet_groups]");

			for (int i = 0; i < list.getGroupNames().length; i++) {
				writer.println("exten => " + list.getGroupNames()[i] + ",1,NoOp()");
				if (i == 0) {
					writer.println("same  => n,Set(${var}=wicews_gh_confirmed)\n" +
							"same  => n,Set(DB(hydromet/${var})=0)");
				}
				for (int j = 0; j < list.GetPhoneNumbers(list.getGroupNames()[i]).length; j++) {
					writer.println("same  => n,Originate(SIP/pn/" + list.GetPhoneNumbers(list.getGroupNames()[i])[j] +
							",exten,hydromet_alarm,alarmmsg,1,10)\n" +
							"same  => n,GotoIf($[${DB(hydromet/${var})} = 1]?end)\n"+
							"same  => n(end),Hangup()");
				}
			}
			writer.println();
			writer.println("[hydromet_alarm]\n"+
					"exten => alarmmsg,1,NoOp()\n"+
					"same  => n,Wait(2)\n"+
					"same  => n,Playback(hydromet/hydrometintro)\n"+
					"same  => n,Playback(hydromet/wicews_gh)\n"+
					"same  => n,Playback(hydromet/value)\n"+
					"same  => n,SayAlpha(${DB(hydromet/wicews_gh)})\n"+
					"same  => n,Read(ackDigit,hydromet/press1,1,,2,5)\n"+
					"same  => n,Gotoif($[\"${ackDigit}\" = \"1\"]?ackalert)\n"+
					"same  => n,Wait(5)\n"+
					"same  => n(ackalert),NoOp()\n"+
					"same  => n,Read(ackDigit,hydromet/acknowledging,1,,2,5)\n"+
					"same  => n,Gotoif($[\"${ackDigit}\" = \"1\"]?confirmed)\n"+
					"same  => n,Goto(unconfirmed)\n"+
					"same  => n(confirmed),NoOp()\n"+
					"same  => n,Playback(vm-goodbye)\n"+
					"same  => n,Set(DB(hydromet/${var})=1)\n"+
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
