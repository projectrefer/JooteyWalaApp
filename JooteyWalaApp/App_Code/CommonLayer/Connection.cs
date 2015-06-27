using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//  My Usings
using System.Data.SqlClient;

/// <summary>
/// Summary description for Connector
/// Use this object to manage Database connection related Operations such as open / close database connection.
/// </summary>

public class Connection
{
    #region Variables
    protected SqlConnection oConnect;
    static string sConString;
    static int commandTimeout = 5000;
    #endregion

    #region Constructor

    //  Default constructor
    public Connection()
    {
        //  Get Connection string from web.config file
        sConString = ConfigurationManager.ConnectionStrings["PorticoDb"].ToString();
    }
    #endregion

    #region User Functions

    public int CommandTimeout
    {
        set { commandTimeout = value; }
        get { return commandTimeout; }
    }

    //  OpenSQLConnection Function
    public void OpenSQLConnection()
    {
        oConnect = new SqlConnection(sConString);
        oConnect.Open();
    }

    //  CloseSQLConnection Function
    public void CloseSQLConnection()
    {
        oConnect.Close();
    }

    //  GetConnectionObject Use this function if required...
    public SqlConnection GetConnectionObject()
    {
        SqlConnection oConObject = new SqlConnection(sConString);
        oConObject.Open();

        return oConObject;
    }

    public void CloseConnectionObject(ref SqlConnection oCon)
    {
        //SqlConnection con = new SqlConnection();
        oCon.Close();
    }
    #endregion



    #region Previously Used Connection

    public SqlConnection GetConnection()
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["porticodb"].ToString());
        cn.Open();
        return cn;
    }
    public void CloseConnection(ref SqlConnection cn)
    {
        cn.Close();
    }

    #endregion
}
