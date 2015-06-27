using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Summary description for UserSession
/// </summary>
public static class UserSession
{
    public static string PassKey = "kAdNB6tsP2l4sA==";
    public static string PassChars = "01234AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz56789";
    public static string CustomerName = "";
    public static int CustomerId = 0;


    public static string SessionId
    {
        set { SetCookie("sid", value); }
        get
        {

            if (string.IsNullOrEmpty(GetCookie("sid")))
            {
                Guid guidSessionId = Guid.NewGuid();
                SetCookie("sid", guidSessionId.ToString());
                return guidSessionId.ToString().ToUpper();
            }

            return GetCookie("sid");
        }
    }

    public static bool ValidateSession()
    {
        int uid = GetCurrentUserId();
        string sid = GetCurrentSessionId();
        // HttpContext.Current.Response.Write(" <br>ValidateSession - sid=" + sid + " uid=" + uid);
        // HttpContext.Current.Response.End();
        if (uid > 0 && !string.IsNullOrEmpty(sid))
            return true;

        return false;

    }   
    public static int GetCurrentCustId()
    {
        // HttpContext.Current.Res
        int id = 0;
        string uid = GetCookie("Cid");

        // HttpContext.Current.Response.Write("<br>GetCurrentUserId uid=" + uid + " <br>" + CString.IsInteger(uid).ToString());
        if (CString.IsInteger(uid))
        {
            //HttpContext.Current.Response.Write("<br>2--uid=" + uid);
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static int GetCurrentOrderId()
    {
        // HttpContext.Current.Res
        int id = 0;
        string uid = GetCookie("Oid");

        // HttpContext.Current.Response.Write("<br>GetCurrentUserId uid=" + uid + " <br>" + CString.IsInteger(uid).ToString());
        if (CString.IsInteger(uid))
        {
            //HttpContext.Current.Response.Write("<br>2--uid=" + uid);
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static int GetCurrentUserId()
    {
        // HttpContext.Current.Res
        int id = 0;
        string uid = GetCookie("uid");


        // HttpContext.Current.Response.Write("<br>GetCurrentUserId uid=" + uid + " <br>" + CString.IsInteger(uid).ToString());
        if (CString.IsInteger(uid))
        {
            //HttpContext.Current.Response.Write("<br>2--uid=" + uid);
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static int IsGuestUser()
    {
        // HttpContext.Current.Res
        int id = 0;
        string uid = GetCookie("guestUser");

        // HttpContext.Current.Response.Write("<br>GetCurrentUserId uid=" + uid + " <br>" + CString.IsInteger(uid).ToString());
        if (CString.IsInteger(uid))
        {
            //HttpContext.Current.Response.Write("<br>2--uid=" + uid);
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static string GetCurrentCustEncryptId()
    {
        string id = GetCookie("EncryptCustId");
        return id;
    }
    public static string GetCurrentUsernameWeb()
    {
        string id = GetCookie("unWeb");
        return id;
    }
    public static string GetCurrentUsername()
    {
        string id = GetCookie("un");
        return id;
    }
    public static string GetCurrentCustomerName()
    {
        string id = GetCookie("custName");
        return id;
    }
    public static string GetCurrentRoleName()
    {
        string id = GetCookie("rid");
        return id;
    }
    public static string GetGuestUser()
    {
        string id = Convert.ToString(GetCookie("guestUser"));
        return id;
    }
    public static int GetCurrentWebUserId()
    {
        
        int id = 0;
        string uid = GetCookie("Webuid");
        if (CString.IsInteger(uid))
        {
            
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static bool CheckUserLogin()
    {
        int Webuid = GetCurrentWebUserId();
        string sGuestUser = GetGuestUser();
        string sid = GetWebSessionId();


        if (Webuid > 0 && !string.IsNullOrEmpty(sid) && sGuestUser != "1")
            return true;

        return false;

    }
    public static string GetCurrentProjectName()
    {
        string id = GetCookie("pn");
        return id;
    }
    public static int GetCurrentProjectId()
    {
        // HttpContext.Current.Res
        int id = 0;
        string uid = GetCookie("pId");
        //HttpContext.Current.Response.Write("" + uid + " <br>" + CString.IsInteger(uid).ToString());

        if (CString.IsInteger(uid))
        {
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static int GetCurrentCallId()
    {
        // HttpContext.Current.Res
        int id = 0;
        string uid = GetCookie("Callid");
        //HttpContext.Current.Response.Write("" + uid + " <br>" + CString.IsInteger(uid).ToString());

        if (CString.IsInteger(uid))
        {
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static int GetCurrentSourceId()
    {
        // HttpContext.Current.Res
        int id = 0;
        string uid = GetCookie("Source");
        //HttpContext.Current.Response.Write("" + uid + " <br>" + CString.IsInteger(uid).ToString());

        if (CString.IsInteger(uid))
        {
            id = int.Parse(uid);
            if (id > 0)
                return id;
            else
                return id;
        }
        return id;
    }
    public static string GetCurrentSessionId()
    {
        // HttpContext.Current.Res
        string id = GetCookie("SID");

        // HttpContext.Current.Response.Write(" <br> GetCurrentSessionId - " + id);
        //HttpContext.Current.Response.End();

        //HttpContext.Current.Response.Write("<br>2--uid=" + uid);

        return id;
    }
    public static string GetWebSessionId()
    {
        // HttpContext.Current.Res
        string id = GetCookie("Wid");

        // HttpContext.Current.Response.Write(" <br> GetCurrentSessionId - " + id);
        //HttpContext.Current.Response.End();

        //HttpContext.Current.Response.Write("<br>2--uid=" + uid);

        return id;
    }
    public static bool AccessDenied(string roleList)
    {
        //HttpContext.Current.Response.Write("<br>mk==" + roleList);
        bool flag = false;
        string loginRole = GetCurrentRoleName();
        string[] role = roleList.Split(',');
        for (int i = 0; i < role.Length; i++)
        {
            if (string.Compare(loginRole, role[i]) == 0)
                flag = true;
        }

        if (flag)
            HttpContext.Current.Server.Transfer("Default.aspx?msg=ACCESS DENIED");
        return flag;


    }
    public static bool CreateActivityLog(string ActivityName, string dbTableName, int rowId)
    {

        if (GetCurrentUserId() <= 0 || GetCurrentSessionId() == "")
            HttpContext.Current.Server.Transfer("Login.aspx?msg=Invalid Session");
        return false;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cesDatabase"].ToString());
        con.Open();



        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.CommandText = "CreateActivityLog";
        cmd.Parameters.Add("@userId", SqlDbType.Int).Value = GetCurrentUserId();
        cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = GetCurrentProjectId();
        cmd.Parameters.Add("@SessionId", SqlDbType.VarChar, 50).Value = GetCurrentSessionId();
        cmd.Parameters.Add("@activityName", SqlDbType.VarChar, 50).Value = ActivityName;
        cmd.Parameters.Add("@dbTableName", SqlDbType.VarChar, 50).Value = dbTableName;
        cmd.Parameters.Add("@dbTableRowID", SqlDbType.Int).Value = rowId;

        SqlParameter param = new SqlParameter("@result", SqlDbType.Int);
        param.Direction = ParameterDirection.Output;

        cmd.Parameters.Add(param);
        cmd.ExecuteNonQuery();

        int result = int.Parse(cmd.Parameters["@result"].Value.ToString());
        if (result > 0)
        {
            return true;
        }
        con.Close();

        return false;

    }

    // This function is called from Login page since cookies are not set.
    public static bool CreateSessionActivityLog(int userId, string SessionId, string ActivityName, string dbTableName, int rowId)
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cesDatabase"].ToString());
        con.Open();

        // int uid = GetCurrentUserId();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.CommandText = "CreateActivityLog";
        cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
        cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = GetCurrentProjectId();
        cmd.Parameters.Add("@SessionId", SqlDbType.VarChar, 50).Value = SessionId;
        cmd.Parameters.Add("@activityName", SqlDbType.VarChar, 50).Value = ActivityName;
        cmd.Parameters.Add("@dbTableName", SqlDbType.VarChar, 50).Value = dbTableName;
        cmd.Parameters.Add("@dbTableRowID", SqlDbType.Int).Value = rowId;

        SqlParameter param = new SqlParameter("@result", SqlDbType.Int);
        param.Direction = ParameterDirection.Output;

        cmd.Parameters.Add(param);
        cmd.ExecuteNonQuery();

        int result = int.Parse(cmd.Parameters["@result"].Value.ToString());
        if (result > 0)
        {
            return true;
        }
        con.Close();

        return false;

    }
    public static bool SetCookie(string cookiename, string cookievalue)
    {
        //HttpContext.Current.Response.Write(" <br> SetCookie - cookiename=" + cookiename + " cookievalue=" + cookievalue);

        HttpContext.Current.Response.Cookies[cookiename].Value = cookievalue;
        return true;

        try
        {
            HttpCookie objCookie = new HttpCookie(cookiename);
            HttpContext.Current.Response.Cookies.Clear();
            HttpContext.Current.Response.Cookies.Add(objCookie);
            objCookie.Values.Add(cookiename, cookievalue);

            //HttpContext.Current.Response.Write("<br>--cookiename=" + cookievalue);

        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public static bool SetCookie(string cookiename, string cookievalue, int iDaysToExpire)
    {
        try
        {
            HttpCookie objCookie = new HttpCookie(cookiename);
            HttpContext.Current.Response.Cookies.Clear();
            HttpContext.Current.Response.Cookies.Add(objCookie);
            objCookie.Values.Add(cookiename, cookievalue);

            DateTime dtExpiry = DateTime.Now.AddDays(iDaysToExpire);
            HttpContext.Current.Response.Cookies[cookiename].Expires = dtExpiry;
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
    public static string GetCookie(string cookiename)
    {
        string val = "";
        if (HttpContext.Current.Request.Cookies[cookiename] != null)
            val = HttpContext.Current.Request.Cookies[cookiename].Value;
        return val;
        /*
        if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[cookiename].Value != null)
            return HttpContext.Current.Request.Cookies[cookiename].Value;
        else
            return ""; */
        string cookyval = "";
        HttpCookieCollection ck = HttpContext.Current.Request.Cookies;
        //HttpContext.Current.Response.Write("<br> ------------------------------------------------------");
        /*
        for (int i = 0; i < ck.Count; i++)
        {
            HttpCookie c = ck[i];
            HttpContext.Current.Response.Write("<br>c.Name = " + c.Name + " --- c.Value" + c.Value);

        }
        */
        HttpCookie s = ck[cookiename.Trim()];

        //HttpContext.Current.Response.Write("<br>s.Name = " + s.Name + " --- s.Value" + s.Value);
        return s[cookiename];
        /*HttpContext.Current.Response.Write("<br>s.Name = " + s.Name + " --- s.Value" + s.Value);
        HttpContext.Current.Response.Write("<br> ------------------------------------------------------");
        */

        //try
        {
            HttpCookie ck1 = HttpContext.Current.Request.Cookies[cookiename];
            if (ck1 != null && !string.IsNullOrEmpty(ck1.Value))
                cookyval = ck1.Value; // ck1[cookiename];
            // HttpContext.Current.Response.Write("<br>  GetCookie=" + cookiename + " cookyval=" + cookyval);
        }
        //catch (Exception e)
        {
            //cookyval = "";
        }
        return cookyval;
    }

    public static string AutoGeneratedPassword(string sEMail)
    {
        string sRondomPassword;


        sRondomPassword = string.Empty;
        Random oRandom = new Random();

        //  6 Characters Password...
        for (int iRunner = 0; iRunner < 6; iRunner++)
        {
            int iRandom = oRandom.Next(0, 61);
            sRondomPassword = sRondomPassword + PassChars.Substring(iRandom, 1);
        }

        //  Get PassKey from Session
        string sPassKey = PassKey;

        //  Get Encrypted Password...
        //string sCryptedPassword = CreatePasswordHash(sPassword);

        object ErrorMessage;
        UserInfo oUser = new UserInfo();

        bool UserCreated = oUser.ResetSiteUserPassword(sEMail, sRondomPassword, out ErrorMessage);
        oUser = null;

        //  Function Returning New Password
        return sRondomPassword;
    }
    public static string CreatePasswordHash(string pwd)
    {

        string saltAndPwd = String.Concat(pwd, PassKey);
        string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");

        //  Function Returning Hashed Password
        return hashedPwd;
    }

    public static bool GetUserId()
    {
        int uid = Convert.ToInt32(HttpContext.Current.Session["uid"]);

        string sGuestUser = GuestUser();

        if (uid > 0 && sGuestUser == "0")
            return true;

        return false;

    }

    public static string GuestUser()
    {
        string id = Convert.ToString(HttpContext.Current.Session["guestUser"]);
        return id;
    }

}
