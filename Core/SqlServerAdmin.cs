using System;
using System.Data;
namespace Reclamation.Core
{
	/// <summary>
	/// SqlServerAdmin provides SqlServer administrative functions
	/// like adding or removing users.
	/// or changing password.
	/// </summary>
	public class SqlServerAdmin
	{
    private SqlServer server;

		public SqlServerAdmin(SqlServer server)
		{
			this.server=server;
		
    }

    public DataTable Users
    {
      get
      {
        
      string sql = "select name,loginname,dbname,isntuser from master..syslogins where dbname = '"+server.DatabaseName+"'";
        return server.Table("users",sql);
      //return server.Table("sp_helpuser","sp_helpuser");
      }
    }


    /*
sp_helpuser

UserName     GroupName              LoginName                                      DefDBName                          UserID SID                                                                                                                                                                          
------------ ---------------------- ---------------------------------------------- ---------------------------------- ------ ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
dbo          db_owner               PN6200IBMLAPTOP\ktarbet                        master                             1      0x010500000000000515000000FD8C5A41C7D7EE3CCFCC1410F4030000
Fa_web       fa_readonly            fa_web                                         NorthwindTriggers                  6      0x22A91E7653F0754198E0562E5CBA5CCB
fao          public                 fao                                            master                             5      0x69288E709CB0624DA20C10F611ECE2A6
karl         fa_editor              karl                                           FaTracking                         7      0x84BB167A78B1904CA5EAE6EAE8960153
karl         fa_readonly            karl                                           FaTracking                         7      0x84BB167A78B1904CA5EAE6EAE8960153
karl         fa_admin               karl                                           FaTracking                         7      0x84BB167A78B1904CA5EAE6EAE8960153

     * */
    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public bool IsRoleMember(string roleName)
    {
      DataTable table = new DataTable();
      string user = server.Username();
      table = server.Table("sp_helpuser","sp_helpuser '"+user+"'");
      DataRow[] rows = table.Select("UserName ='"+user+"' and GroupName = '"+roleName+"'");
      return (rows.Length == 1);
    }

  
	}
}
