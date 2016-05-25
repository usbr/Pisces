/*
 * 
 */



import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import javax.swing.table.AbstractTableModel;

 

public class CsvFile extends AbstractTableModel {

    /**
	 * 
	 */
	private static final long serialVersionUID = -6204022418061224795L;
	ArrayList<String> columnNames;
    ArrayList<String> items;


    public CsvFile(String fileName) {

     ReadFile(fileName);
     }


    /**
     * private constructor used for filtering.
     */
private CsvFile()
{
      items  = new ArrayList<String>();
      columnNames = new ArrayList<String>();
}


    /**
     * filters CsvFile (based on in memory contents)
     * and returns a new CsvFile
     */
    public CsvFile filter(String columnName, Object columnValue, CsvFileFilter filter )
    {
        CsvFile rval = new CsvFile();
        for( String s: columnNames ) {
            rval.columnNames.add(s);
        }


        String strColumnValue = columnValue.toString();

        int idx = FindColumnIndex(columnName);
        if( idx <0 )
        {
            String msg = "Error: Column '"+columnName+"' not found";
            Logger.WriteLine(msg);
            throw new IndexOutOfBoundsException(msg);
        }

        for( String item: items)
        {
          String[] a = DecodeCsv(item);
        //  if( a.length < idx)
          //{
           //   System.out.println("bug");
          //}
        //  System.out.println("a.length ="+a.length+ "  idx = "+idx);
           String val = a[idx];
           if( filter == CsvFileFilter.Equal &&  val.compareToIgnoreCase(strColumnValue)== 0 )
           {
              rval.items.add(item);
           }
           else if(filter == CsvFileFilter.NotEqual &&  val.compareToIgnoreCase(strColumnValue)!= 0)
           {
              rval.items.add(item);
           }
           else if (filter == CsvFileFilter.BeginsWith &&  val.startsWith(strColumnValue) )
           {
               rval.items.add(item);
           }
        }

        return rval;
    }

    public String[] getDistinctValues(String columnName)
    {
    	
    	ArrayList<String> rval = new ArrayList<String>();
    	
    	int idx = FindColumnIndex(columnName);
    	
    	
    	for(int i=0; i<items.size(); i++)
        {
    	String[] csv = DecodeCsv(items.get(i));
    	 String a = csv[idx];
    	 if( !rval.contains(a))
    	   rval.add(csv[idx]);	
        }
    	return (String[])  rval.toArray(new String[0]);
    }
    
    public static String[] DecodeCsv(String csvText)
    {
        //TODO csv parse
     //return null;
    //Prepare the regexes we'll use

Pattern pCSVmain = Pattern.compile(
    "  \\G(?:^|,)                                      \n"+
    "  (?:\n"+
    "     # Either a double-quoted field...\n"+
    "     \" # field's opening quote\n"+
    "      (  (?> [^\"]*+ ) (?> \"\" [^\"]*+ )*+  )\n"+
    "     \" # field's closing quote\n"+
    "   # ... or ...\n"+
    "   |\n"+
    "     # ... some non-quote/non-comma text ...\n"+
    "     ( [^\",]*+ )\n"+
    "  )\n",
    Pattern.COMMENTS);
Pattern pCSVquote = Pattern.compile("\"\"");
// Now create Matcher objects, with dummy text, that we'll use later.
Matcher mCSVmain  = pCSVmain.matcher("");
Matcher mCSVquote = pCSVquote.matcher("");

     mCSVmain.reset(csvText); // Tie the target text to the mCSVmain object
     ArrayList<String> rval = new ArrayList<String>();
while ( mCSVmain.find() )
{
    String field; // We'll fill this in with $1 or $2 . . .
    String second = mCSVmain.group(2);
    if ( second != null )
        field = second;
    else {
        // If $1, must replace paired double-quotes with one double quote
        mCSVquote.reset(mCSVmain.group(1));
        field = mCSVquote.replaceAll("\"");
    }
    
    // We can now work with field . . .
    rval.add(field);
    //System.out.println("Field [" + field + "]");
}
   return (String[])  rval.toArray(new String[0]);

    }

    public String getStringAt(int rowIndex, String columnName)
    {
        
        return getValueAt(rowIndex,columnName).toString();
    }
    
    public Object getValueAt(int rowIndex, String columnName)
    {
        int idx = FindColumnIndex(columnName);

       if( idx ==-1)
       {
           Logger.WriteLine("Warning column '"+columnName+"' not found");
           return "";
       }

        return getValueAt(rowIndex,idx);
    }

    public Object getValueAt(int rowIndex, int columnIndex)
    {
      String[] csv = DecodeCsv(items.get(rowIndex));
      if( columnIndex >= csv.length)
      {
          Logger.WriteLine("out of bounds");
      }
      return csv[columnIndex];
    }

     public int GetIntegerAt(int rowIndex, String columnName)
    {
        String s = getValueAt(rowIndex,columnName).toString();
        int rval = Integer.parseInt(s);
        return rval;
    }
    public int GetIntegerAt(int rowIndex, int columnIndex)
    {
        String s = getValueAt(rowIndex,columnIndex).toString();
        int rval = Integer.parseInt(s);
        return rval;
    }


    public int getColumnCount()
    {
        return columnNames.size();
    }
    public int getRowCount()
    {
        return items.size();
    }

    private int FindColumnIndex(String columnName) {
        //int idx = columnNames..indexOf(columnName,
        int idx = -1;
        for (int i = 0; i < columnNames.size(); i++) {
            String s = columnNames.get(i).trim();
            if (s.compareToIgnoreCase(columnName) == 0) {
                idx = i;
                break;
            }
        }
        return idx;
    }

    private void ReadFile(String filename)
    {
       
        items  = new ArrayList<String>();
        columnNames = new ArrayList<String>();

        try {
            FileReader f = new FileReader(filename);
            BufferedReader in = new BufferedReader(f);
            boolean first = true;
             String str;
             while(( str = in.readLine()) != null)
             {

//                 if( str.trim().length() > maxLength )
  //                   maxLength = str.trim().length();
                 if(first )
                 {
                     String[] cn = DecodeCsv(str);
                     for(String n : cn)
                        columnNames.add(n.trim());
                   first = false;
                 }
                 else
                 {
                     items.add(str);
                 }

             }
             in.close();
    //     Logger.WriteLine("max length = "+maxLength);
       } catch (IOException e) {

           e.printStackTrace();
           System.out.println("Error");
        }

    }




}
