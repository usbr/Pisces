
public class CallOutLists {

	
		CsvFile csv;
	
	public CallOutLists()
	{
		 csv = new CsvFile("callout_list.csv");
	}
	
	public String[] getGroupNames()
	{
		return csv.getDistinctValues("group");
	}

	public String[]  GetPhoneNumbers(String group)
	{
		return GetFromGroup(group,"phone");
	}
	
	public String[]  GetNames(String group)
	{
		return GetFromGroup(group,"name");
	}
	
	
	/*
	 * Get a column of data for a specific group
	 */
	/**
	 * @param group
	 * @param columnName
	 * @return
	 */
	private String[] GetFromGroup(String group, String columnName)
	{
		CsvFile a = csv.filter("group", group, CsvFileFilter.Equal);
		String[] rval = new String[a.getRowCount()];
		
		for (int i = 0; i < a.getRowCount(); i++) {
			rval[i] = a.getStringAt(i, columnName);
		}
		
		return rval;
	}
	
}
