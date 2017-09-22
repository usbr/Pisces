using System;

namespace Reclamation.TimeSeries.Hec
{

/*//http://www.iqdotdt.com/svn/usf/trunk/hecdss/dss/
 * 
  DATA CVALS/ '1YEAR', '1MON',
     * '1WEEK',  '1DAY',    '12HOUR',  '8HOUR',
     * '6HOUR',  '4HOUR',   '3HOUR',   '2HOUR',
     * '1HOUR',  '30MIN',   '20MIN',   '15MIN',
     * '10MIN',  '5MIN',    '4MIN',    '3MIN',
     * '2MIN',   '1MIN',
     * 'IR-CENTURY', 'IR-DECADE ', 'IR-YEAR  ',
     * 'IR-MONTH  ', 'IR-DAY    '/

*/
  /// <summary>
  /// Hec DSS files are indexed using a 6 part (A-F) path
  /// A  River or project name , etc.
  /// B   location.   
  /// C  data paramter 
  /// D starting date military format  01JAN1988
  /// E Time interval  (see examples above)
  /// F User defined (scenario or study name) 'OBS', 'baseline1'
  /// </summary>
  public class HecDssPath
  {
    string[] parts;
    string path;

    public HecDssPath(string a, string b, string c, string d, string e, string f)
    {
        parts = new string[6];
        parts[0] = a;
        parts[1] = b;
        parts[2] = c;
        parts[3] = d;
        parts[4] = e;
        parts[5] = f;
    }

    public bool IsValid
    {
        get
        {
            for (int i = 0; i < parts.Length; i++)
            {
              if( parts[i].IndexOfAny(new char[]{','}) >=0)
                  return false;
            }

            return true;
        }
    }
    public HecDssPath(string path)
    {
      this.path = path;
      string tmp = path.Trim();
      if (tmp.Length == 0)
          throw new ArgumentException("DSS Path is empty");
      if( tmp[tmp.Length-1]=='/')
      {// trim trailing '/'
          tmp  = tmp.Substring(0,tmp.Length-1);
      }
      if( tmp[0] == '/')
      {
        tmp = tmp.Substring(1);
      }

      parts = tmp.ToUpper().Split(new char[] {'/'});
        if( parts.Length != 6)
            throw new ArgumentException("DSS Path had "+parts.Length +" parts. It requires 6");
    }
    public string CondensedName { get { return "/"+A+"/"+B+"/"+C+"//"+E+"/"+F+"/"; }}
    public string Name { get { return path;}}
      /// <summary>
      /// Group
      /// </summary>
    public string A	{get { return parts[0];}}
      /// <summary>
      /// Location
      /// </summary>
    public string B	{get { return parts[1];}}
      /// <summary>
      /// Parameter
      /// </summary>
    public string C	{get { return parts[2];}}
      /// <summary>
      /// Block Start Date
      /// </summary>
    public string D	{get { return parts[3];}}
      /// <summary>
      /// Time interval or Block Length
      /// </summary>
    public string E	{get { return parts[4];}}
      /// <summary>
      /// Descriptor
      /// </summary>
    public string F	{get { return parts[5];}}

    public override string ToString()
    {
        return CondensedName;
    }
  }
}

