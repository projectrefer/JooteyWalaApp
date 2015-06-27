using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;



using System.Data.SqlClient;
using System.IO;
using System.Text;



/// <summary>
/// Summary description for UserInfo
/// </summary>
public class UserInfo : Connection
{
    public UserInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    StringBuilder sbMsg = new StringBuilder();
    string sRemoteAddress = HttpContext.Current.Request.ServerVariables["remote_addr"].ToString();
    public string Message
    {
        set { sbMsg.Append(value); }
        get { return sbMsg.ToString(); }
    }
    ErrorLogger oErrorLog = new ErrorLogger();

    public void SetWriteReview()
    {
        UserSession.SetCookie("WriteReview", "1");
    }
    public void ReSetWriteReview()
    {
        UserSession.SetCookie("WriteReview", "");
    }

    public void SetIsDeliveryAddress()
    {
        UserSession.SetCookie("DelieveryAdd", "1");
    }
    public void ReSetIsDeliveryAddress()
    {
        UserSession.SetCookie("DelieveryAdd", "");
    }
    public void SetTrackOrder()
    {
        UserSession.SetCookie("TrackOrder", "1");
    }
    public void ReSetTrackOrder()
    {
        UserSession.SetCookie("TrackOrder", "");
    }

    public void SetAccount()
    {
        UserSession.SetCookie("Account", "1");
    }
    public void ReSetAccount()
    {
        UserSession.SetCookie("Account", "");
    }


    public void SetReferral()
    {
        UserSession.SetCookie("Referral", "1");
    }
    public void ReSetReferral()
    {
        UserSession.SetCookie("Referral", "");
    }
    public bool CheckIsReferral()
    {
        string s = UserSession.GetCookie("Referral");

        if (!string.IsNullOrEmpty(s) && s.CompareTo("1") == 0)
        {
            return true;
        }
        return false;
    }

    public bool CheckSetIsDeliveryAddress()
    {
        string s = UserSession.GetCookie("DelieveryAdd");

        if (!string.IsNullOrEmpty(s) && s.CompareTo("1") == 0)
        {
            return true;
        }
        return false;
    }

    public bool CheckIsWriteReview()
    {
        string s = UserSession.GetCookie("WriteReview");

        if (!string.IsNullOrEmpty(s) && s.CompareTo("1") == 0)
        {
            return true;
        }
        return false;
    }

    public bool CheckTrackOrder()
    {
        string s = UserSession.GetCookie("TrackOrder");

        if (!string.IsNullOrEmpty(s) && s.CompareTo("1") == 0)
        {
            return true;
        }
        return false;
    }
    public bool CheckAccount()
    {
        string s = UserSession.GetCookie("Account");

        if (!string.IsNullOrEmpty(s) && s.CompareTo("1") == 0)
        {
            return true;
        }
        return false;
    }
    public int CreateNewUserAccount(string sEmailAddress, string Password, string FirstName, string LastName, bool bIsPromotion)
    {
        int iUserId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("CreateWebUserLogin_v2", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = sEmailAddress;
            sqlCmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;

            sqlCmd.Parameters.Add("@ShFirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCmd.Parameters.Add("@ShLastName", SqlDbType.VarChar).Value = LastName;

            sqlCmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
            sqlCmd.Parameters.Add("@IsActiveForPromotion", SqlDbType.VarChar).Value = bIsPromotion;
            sqlCmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sRemoteAddress;


            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
            pUserId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserId);



            SqlParameter pUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
            pUserName.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserName);

            SqlParameter pCustomerId = new SqlParameter("@CustomerId", SqlDbType.Int);
            pCustomerId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCustomerId);

            SqlParameter pCustomerName = new SqlParameter("@CustName", SqlDbType.VarChar, 50);
            pCustomerName.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCustomerName);

            SqlParameter pEncryptCustId = new SqlParameter("@EncryptCustId", SqlDbType.VarChar, 50);
            pEncryptCustId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pEncryptCustId);

            SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            pErrorMessage.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pErrorMessage);

            sqlCmd.ExecuteNonQuery();
            iUserId = Convert.ToInt32(pUserId.Value);
            if (iUserId > 0)
            {

                string sessionId = UserSession.GetWebSessionId();
                if (string.IsNullOrEmpty(sessionId))
                {
                    Guid guidSessionId = Guid.NewGuid();
                    sessionId = guidSessionId.ToString();
                }

                UserSession.SetCookie("Webuid", sqlCmd.Parameters["@userId"].Value.ToString());
                UserSession.SetCookie("sid ", sessionId.ToString());
                UserSession.SetCookie("un", sqlCmd.Parameters["@UserName"].Value.ToString());
                UserSession.SetCookie("Cid", sqlCmd.Parameters["@CustomerId"].Value.ToString());
                UserSession.SetCookie("EncryptCustId", sqlCmd.Parameters["@EncryptCustId"].Value.ToString());

                UserSession.SetCookie("guestUser", "0");
                UserSession.SetCookie("custName", sqlCmd.Parameters["@CustName"].Value.ToString());
                //HttpContext.Current.Session["custName"] = Convert.ToString(sqlCmd.Parameters["@CustName"].Value);
                UserSession.SetCookie("Source", "2");
            }
            if (pErrorMessage.Value != "")
                Message = Convert.ToString(pErrorMessage.Value);

        }
        catch (Exception ex)
        {
            Message = ex.Message.ToString();
            oErrorLog.Open();
            oErrorLog.Log("CreateNewUserAccount Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return iUserId;

    }


    public int DoUserLogin(string Email, string Password)
    {
        int userId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("DoWebLogin_bbm_v1", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("Email", SqlDbType.VarChar).Value = Email;
            sqlCmd.Parameters.Add("Password", SqlDbType.VarChar).Value = Password;


            SqlParameter paramMuserId = new SqlParameter("@userId", SqlDbType.Int);
            paramMuserId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramMuserId);

            SqlParameter paramMCustId = new SqlParameter("@CustomerId", SqlDbType.BigInt);
            paramMCustId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramMCustId);


            SqlParameter paramUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
            paramUserName.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramUserName);

            SqlParameter paramCustomer = new SqlParameter("@CustName", SqlDbType.VarChar, 100);
            paramCustomer.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramCustomer);

            SqlParameter paramEncryptCusterId = new SqlParameter("@EncryptCusterId", SqlDbType.VarChar, 100);
            paramEncryptCusterId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramEncryptCusterId);

            sqlCmd.ExecuteNonQuery();
            if (sqlCmd.Parameters["@userId"] != null)
            {
                userId = int.Parse(sqlCmd.Parameters["@userId"].Value.ToString());

                if (userId > 0)
                {
                    string sessionId = UserSession.GetWebSessionId();
                    if (string.IsNullOrEmpty(sessionId))
                    {
                        Guid guidSessionId = Guid.NewGuid();
                        sessionId = guidSessionId.ToString();
                    }

                    UserSession.CustomerId = Convert.ToInt32(sqlCmd.Parameters["@CustomerId"].Value.ToString());

                    UserSession.SetCookie("Webuid", sqlCmd.Parameters["@userId"].Value.ToString());
                    UserSession.SetCookie("sid ", sessionId.ToString());

                    UserSession.SetCookie("Cid", sqlCmd.Parameters["@CustomerId"].Value.ToString());
                    UserSession.SetCookie("EncryptCustId", sqlCmd.Parameters["@EncryptCusterId"].Value.ToString());

                    HttpContext.Current.Session["CustomerId"] = UserSession.CustomerId;

                    UserSession.SetCookie("unWeb", sqlCmd.Parameters["@UserName"].Value.ToString());
                    UserSession.SetCookie("Source", "2");
                    UserSession.SetCookie("guestUser", "0");
                    UserSession.SetCookie("custName", sqlCmd.Parameters["@CustName"].Value.ToString());

                    UserSession.CustomerName = sqlCmd.Parameters["@CustName"].Value.ToString();

                    return userId;

                }
            }
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("DoUserLogin Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return userId;
    }

    public int CreateGuestUser(string sEmailAddress)
    {
        int iUserId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();

            //int CustId = GetCustId();
            sqlCmd = new SqlCommand("NewCreateWebUserLogin", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            //this.CustomerId = CustId;
            //sqlCmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = CustId;

            sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = sEmailAddress;
            sqlCmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sRemoteAddress;
            sqlCmd.Parameters.Add("@IsGuestUser", SqlDbType.VarChar).Value = 1;

            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
            pUserId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserId);


            SqlParameter pCustomerId = new SqlParameter("@CustomerId", SqlDbType.Int);
            pCustomerId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCustomerId);


            sqlCmd.ExecuteNonQuery();
            iUserId = Convert.ToInt32(pUserId.Value);
            if (iUserId > 0)
            {

                string sessionId = UserSession.GetWebSessionId();
                if (string.IsNullOrEmpty(sessionId))
                {
                    Guid guidSessionId = Guid.NewGuid();
                    sessionId = guidSessionId.ToString();
                }

                UserSession.SetCookie("Webuid", sqlCmd.Parameters["@userId"].Value.ToString());
                UserSession.SetCookie("sid ", sessionId.ToString());
                UserSession.SetCookie("Cid", sqlCmd.Parameters["@CustomerId"].Value.ToString());

                HttpContext.Current.Session["CustomerId"] = sqlCmd.Parameters["@CustomerId"].Value.ToString();

                UserSession.SetCookie("guestUser", "1");
                UserSession.SetCookie("Source", "2");
            }

        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("CreateGuestUser Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return iUserId;

    }
    public SqlDataReader GetCustomerAdress()
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("GetCustomerById", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CustId", SqlDbType.Int).Value = UserSession.GetCurrentCustId();
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetCustomerAdress: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }

    public SqlDataReader GetCustomerShippingAddress()
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("NewGetCustomerShippAddress", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = UserSession.GetCurrentCustId();
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetCustomerShippingAddress: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }

    public void UpdateUserAccount(int sCustomerId, int BillingAddId, int ShippingAddId,
                            string sBillFirstName, string sBillLastName, string sBillAddress,
                            string sBillCity, string sBillState, string sBillCOuntry,
                            string sBillPinNo, string sBillOfficePhone, string sBillHomePhone, string sBillMobileNo

                            , string sShippFirstName, string sShippLastName,
                            string sShippAddress, string sShippCity, string sShippState,
                            string sShippCOuntry, string sShippPinNo, string sShippOfficePhone,
                            string sShippHomePhone, string sShippMobileNo)
    {
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("[UpdateuserAddress]", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = UserSession.GetCurrentCustId();
            sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserSession.GetCurrentWebUserId();
            sqlCmd.Parameters.Add("@BillingAddId", SqlDbType.Int).Value = BillingAddId;
            sqlCmd.Parameters.Add("@ShipAddId", SqlDbType.Int).Value = ShippingAddId;

            sqlCmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = sBillFirstName;
            sqlCmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = sBillLastName;


            //cmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar).Value = this.POffPhone.ToString();
            //cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar).Value = this.PHomePhone.ToString();
            //cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = this.PMobile.ToString();

            sqlCmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar).Value = sBillOfficePhone;
            sqlCmd.Parameters.Add("@HomePhone", SqlDbType.VarChar).Value = sBillHomePhone;
            sqlCmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = sBillMobileNo;

            //sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = this.EmailAddress.ToString();

            //sqlCmd.Parameters.Add("@IsEmailOptOut", SqlDbType.Bit).Value = this.IsCallOpt;
            sqlCmd.Parameters.Add("@SourceId", SqlDbType.Int).Value = 2;
            sqlCmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = sBillAddress;
            sqlCmd.Parameters.Add("@PinNo", SqlDbType.VarChar).Value = sBillPinNo;
            sqlCmd.Parameters.Add("@City", SqlDbType.VarChar).Value = sBillCity;
            sqlCmd.Parameters.Add("@State", SqlDbType.VarChar).Value = sBillState;

            //sqlCmd.Parameters.Add("@ContactPhone", SqlDbType.VarChar).Value = this.ContactNumber.ToString();
            //sqlCmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar).Value = this.ContactPerson.ToString();

            sqlCmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = sBillCOuntry;

            //sqlCmd.Parameters.Add("@DeliveryNote", SqlDbType.VarChar).Value = this.DeliveryComment.ToString();


            sqlCmd.Parameters.Add("@ShFirstName", SqlDbType.VarChar).Value = sShippFirstName;
            sqlCmd.Parameters.Add("@ShLastName", SqlDbType.VarChar).Value = sShippLastName;



            sqlCmd.Parameters.Add("@ShAddress", SqlDbType.VarChar).Value = sShippAddress;

            sqlCmd.Parameters.Add("@ShPinNo", SqlDbType.VarChar).Value = sShippPinNo;
            sqlCmd.Parameters.Add("@ShipMobile", SqlDbType.VarChar).Value = sShippMobileNo;
            sqlCmd.Parameters.Add("@ShCity", SqlDbType.VarChar).Value = sShippCity;
            sqlCmd.Parameters.Add("@ShState", SqlDbType.VarChar).Value = sShippState;
            sqlCmd.Parameters.Add("@ShCountry", SqlDbType.VarChar).Value = sShippCOuntry;


            //if (!string.IsNullOrEmpty(this.Password.ToString()))
            //{
            //    sqlCmd.Parameters.Add("@PassWord", SqlDbType.VarChar).Value = this.Password.ToString();
            //}

            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UpdateUserAccount in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
    }


    public void UpdateBillingAddress(int sCustomerId, int shippingAddressId, int BillingAddId,
                            string sBillFirstName, string sBillLastName, string sBillAddress,
                            string sBillCity, string sBillState, string sBillCOuntry,
                            string sBillPinNo, string sBillOfficePhone, string sBillHomePhone, string sBillMobileNo
                            )
    {
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("[UpdateuserAddress]", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = UserSession.GetCurrentCustId();
            sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserSession.GetCurrentWebUserId();
            sqlCmd.Parameters.Add("@BillingAddId", SqlDbType.Int).Value = BillingAddId;
            sqlCmd.Parameters.Add("@ShipAddId", SqlDbType.Int).Value = shippingAddressId;


            sqlCmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = sBillFirstName;
            sqlCmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = sBillLastName;


            sqlCmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar).Value = sBillOfficePhone;
            sqlCmd.Parameters.Add("@HomePhone", SqlDbType.VarChar).Value = sBillHomePhone;
            sqlCmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = sBillMobileNo;


            sqlCmd.Parameters.Add("@SourceId", SqlDbType.Int).Value = 2;
            sqlCmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = sBillAddress;
            sqlCmd.Parameters.Add("@PinNo", SqlDbType.VarChar).Value = sBillPinNo;
            sqlCmd.Parameters.Add("@City", SqlDbType.VarChar).Value = sBillCity;
            sqlCmd.Parameters.Add("@State", SqlDbType.VarChar).Value = sBillState;


            sqlCmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = sBillCOuntry;




            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            oErrorLog.Open();
            oErrorLog.Log("UpdateBillingAddress in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
    }

    public void UpdateCustomer(int sCustomerCource, int BillingAddId, int ShippingAddId,
                             string sBillFirstName, string sBillLastName, string sBillAddress,
                             string sBillCity, string sBillState, string sBillCOuntry,
                             string sBillPinNo, string sBillOfficePhone, string sBillHomePhone, string sBillMobileNo
                        )
    {
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("[UpdateCustomer]", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = UserSession.GetCurrentCustId();
            //sqlCmd.Parameters.Add("@BillingAddId", SqlDbType.Int).Value = BillingAddId;
            //sqlCmd.Parameters.Add("@ShipAddId", SqlDbType.Int).Value = ShippingAddId;

            sqlCmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = sBillFirstName;
            sqlCmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = sBillLastName;
            sqlCmd.Parameters.Add("@OfficePhone", SqlDbType.VarChar).Value = sBillOfficePhone;
            sqlCmd.Parameters.Add("@HomePhone", SqlDbType.VarChar).Value = sBillHomePhone;
            sqlCmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = sBillMobileNo;

            if (sCustomerCource == 2)
            {
                //sqlCmd.Parameters.Add("@AddressId", SqlDbType.Int).Value = ShippingAddId;
                sqlCmd.Parameters.Add("@AddressId", SqlDbType.Int).Value = BillingAddId;
            }
            else
            {
                //sqlCmd.Parameters.Add("@AddressId", SqlDbType.Int).Value = BillingAddId;
                sqlCmd.Parameters.Add("@AddressId", SqlDbType.Int).Value = ShippingAddId;
            }
            //sqlCmd.Parameters.Add("@SourceId", SqlDbType.Int).Value = 1;
            sqlCmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = sBillAddress;
            sqlCmd.Parameters.Add("@PinNo", SqlDbType.VarChar).Value = sBillPinNo;
            sqlCmd.Parameters.Add("@City", SqlDbType.VarChar).Value = sBillCity;
            sqlCmd.Parameters.Add("@State", SqlDbType.VarChar).Value = sBillState;

            sqlCmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = sBillCOuntry;
            sqlCmd.Parameters.Add("@ContactPhone", SqlDbType.VarChar).Value = DBNull.Value;
            sqlCmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar).Value = DBNull.Value;

            sqlCmd.Parameters.Add("@DeliveryNote", SqlDbType.VarChar).Value = DBNull.Value;
            sqlCmd.Parameters.Add("@PageName", SqlDbType.VarChar).Value = "OMS";

            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UpdateCustomer in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
    }


    public int GetSourceByCustomerId()
    {
        int scustSourceId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("Select SourceId From Customer Where CustomerId=@CustomerId", oConnect);
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = UserSession.GetCurrentCustId();
            object obj = sqlCmd.ExecuteScalar();
            if (obj != null)
            {
                scustSourceId = int.Parse(obj.ToString());
            }

        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetSourceByCustomerId in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return scustSourceId;
    }


    public SqlDataReader GetBillingAndShippingId()
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("Select AddressId,ShippingAddressId from Customer Where CustomerId=@CustomerId", oConnect);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = UserSession.GetCurrentCustId();
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetBillingAndShippingId: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }


    public SqlDataReader GetUserDetails()
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("GetUserDetailByUserId", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserSession.GetCurrentWebUserId();
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetUserDetails: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }


    #region User Order List


    public int CheckGuestUser(string sCustomerId)
    {
        int iUserId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();


            sqlCmd = new SqlCommand("CHeckGuestUser", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.VarChar).Value = sCustomerId;


            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
            pUserId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserId);


            /* SqlParameter pCustomerId = new SqlParameter("@CustomerId", SqlDbType.Int);
             pCustomerId.Direction = ParameterDirection.Output;
             sqlCmd.Parameters.Add(pCustomerId);*/


            sqlCmd.ExecuteNonQuery();
            iUserId = Convert.ToInt32(pUserId.Value);
            if (iUserId > 0)
            {

                string sessionId = UserSession.GetWebSessionId();
                if (string.IsNullOrEmpty(sessionId))
                {
                    Guid guidSessionId = Guid.NewGuid();
                    sessionId = guidSessionId.ToString();
                }

                UserSession.SetCookie("Webuid", sqlCmd.Parameters["@userId"].Value.ToString());
                UserSession.SetCookie("sid ", sessionId.ToString());
                //UserSession.SetCookie("Cid", sqlCmd.Parameters["@CustomerId"].Value.ToString());
                UserSession.SetCookie("Cid", sCustomerId);

                HttpContext.Current.Session["CustomerId"] = sqlCmd.Parameters["@CustomerId"].Value.ToString();

                UserSession.SetCookie("guestUser", "1");
                UserSession.SetCookie("Source", "2");
            }

        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("CheckGuestUser Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return iUserId;

    }


    public SqlDataReader UserOrderList()
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("SELECT O.OrderNo,O.ORDERID,O.ORDERDATE,O.BILLAMOUNT,S.DESCRIPTION,O.TrackingNo,O.DespatchDate FROM ORDERS O INNER JOIN  ORDERSTATUS S ON O.STATUSID=S.STATUSID INNER JOIN CUSTOMER C ON O.CUSTOMERID=C.CUSTOMERID where C.CustomerId=@CustomerId and O.PaymentStatus IN(1,2,3,4,8) ORDER BY O.ORDERDATE DESC", oConnect);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = UserSession.GetCurrentCustId();
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UserOrderList: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }

    #endregion



    #region Product Review and Rating


    public DataSet RatingDetails()
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataAdapter dtAdptr = new SqlDataAdapter(sqlCmd);

        DataSet dsDataSet = new DataSet();

        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand(" SELECT Rate FROM RatingDetails ", oConnect);
            sqlCmd.CommandType = CommandType.Text;
            dtAdptr = new SqlDataAdapter(sqlCmd);

            dsDataSet = new DataSet();
            dtAdptr.Fill(dsDataSet);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("RatingDetails Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            dtAdptr.Dispose();
            dtAdptr = null;

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return dsDataSet;
    }
    public int UpdateRatingDetails(string iRate, string sStandardCode, string sUserNickName, string sReviewTitle,
        string sUserReview, string sUserLocation, string sIsRecommend)
    {
        int iRateId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("UpdateRatingDetails", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;


            if (!string.IsNullOrEmpty(sStandardCode))
                sqlCmd.Parameters.Add("@StandardCode", SqlDbType.VarChar).Value = sStandardCode;
            else
                sqlCmd.Parameters.Add("@StandardCode", SqlDbType.VarChar).Value = DBNull.Value;


            sqlCmd.Parameters.Add("@Rate", SqlDbType.Int).Value = iRate;
            sqlCmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserSession.GetCurrentWebUserId();
            sqlCmd.Parameters.Add("@UserNickName", SqlDbType.VarChar).Value = sUserNickName;
            sqlCmd.Parameters.Add("@ReviewTitle", SqlDbType.VarChar).Value = sReviewTitle;
            sqlCmd.Parameters.Add("@UserReview", SqlDbType.VarChar).Value = sUserReview;
            sqlCmd.Parameters.Add("@UserLocation", SqlDbType.VarChar).Value = sUserLocation;
            sqlCmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sRemoteAddress;
            sqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = UserSession.GetWebSessionId();

            bool IsRecommend = false;
            if (sIsRecommend == "1")
                IsRecommend = true;

            sqlCmd.Parameters.Add("@IsRecommendProduct", SqlDbType.Bit).Value = IsRecommend;

            SqlParameter pRateId = new SqlParameter("@RateId", SqlDbType.Int);
            pRateId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pRateId);

            sqlCmd.ExecuteNonQuery();


            if (pRateId.Value != "")
                iRateId = Convert.ToInt32(pRateId.Value);
            SetWriteReview();

        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UpdateRatingDetails in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return iRateId;
    }

    public void UpdateStandardDesignReview(bool IsSubmitOrCancel,string standardcode)
    {
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();

            sqlCmd = new SqlCommand("UpdateDesignReview", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@StandardCode", SqlDbType.VarChar).Value = standardcode;

            sqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = UserSession.GetWebSessionId();

            sqlCmd.Parameters.Add("@IsSubmitOrCancel", SqlDbType.Bit).Value = IsSubmitOrCancel;

            sqlCmd.ExecuteNonQuery();
            ReSetWriteReview();
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UpdateStandardDesignReview in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
            CloseSQLConnection();
        }
    }

    public SqlDataReader GetDesignImage(string sdesignCode)
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        StringBuilder sbQuery = new StringBuilder();
        try
        {

            OpenSQLConnection();

            sbQuery.Append(" SELECT S.Thumbnail,Ic.CollectionName,S.Description,S.StandardCode	 FROM StandardDesign S ");
            sbQuery.Append(" INNER JOIN RatingDetails AS Rtd ON Rtd.StandardCode=S.StandardCode ");
            sbQuery.Append(" INNER JOIN ItemCOllection AS Ic ON Ic.ItemCollectionId=S.CollectionId  WHERE Rtd.SessionId=@SessionId AND Rtd.StandardCode=@StandardCode ");

            sqlCmd = new SqlCommand(sbQuery.ToString(), oConnect);

            sqlCmd.CommandType = CommandType.Text;

            string ss = UserSession.GetCurrentSessionId();
            sqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = UserSession.GetWebSessionId();
            sqlCmd.Parameters.Add("@StandardCode", SqlDbType.VarChar).Value = sdesignCode;
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetDesignImage: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }
    public void UpdaterateCustomer()
    {
        OpenSQLConnection();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = oConnect;
        cmd.CommandText = "UpdateRateuserId";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@SessionId", SqlDbType.VarChar, 100).Value = UserSession.GetWebSessionId();
        cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 100).Value = UserSession.GetCurrentWebUserId();
        cmd.ExecuteNonQuery();
        CloseSQLConnection();
    }

    public DataSet GetReviewList()
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataAdapter dtAdptr = new SqlDataAdapter(sqlCmd);
        DataSet dsDataSet = new DataSet();
        StringBuilder sb = new StringBuilder();

        try
        {
            OpenSQLConnection();
            sb.Append(" SELECT * FROM RatingDetails ");
            sqlCmd = new SqlCommand(sb.ToString(), oConnect);
            sqlCmd.CommandType = CommandType.Text;
            dtAdptr = new SqlDataAdapter(sqlCmd);
            dsDataSet = new DataSet();
            dtAdptr.Fill(dsDataSet);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetReviewList Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            dtAdptr.Dispose();
            dtAdptr = null;

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return dsDataSet;
    }
    public void UpdateActiveReview(string sActiveIds, string sDeActiveIds)
    {
        if (sActiveIds != "")
        {
            StringBuilder sbActiveIds = new StringBuilder();
            SqlCommand cmd = new SqlCommand();
            try
            {
                OpenSQLConnection();

                sbActiveIds.Append(" UPDATE RatingDetails SET IsActiveOnWebsite=1 WHERE Id IN (" + sActiveIds + ")");
                cmd = new SqlCommand(sbActiveIds.ToString(), oConnect);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                CloseSQLConnection();
            }
            catch (Exception ex)
            {
                oErrorLog.Open();
                oErrorLog.Log("UpdateActiveReview Function In UserInfo:" + ex.Message);
                oErrorLog.Flush();
                oErrorLog.Close();
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                sbActiveIds = null;
                CloseSQLConnection();
            }

        }

        if (sDeActiveIds != "")
        {
            StringBuilder sbDeActiveIds = new StringBuilder();
            SqlCommand cmdDeactive = new SqlCommand();
            try
            {
                OpenSQLConnection();

                sbDeActiveIds.Append(" UPDATE RatingDetails SET IsActiveOnWebsite=0 WHERE Id IN (" + sDeActiveIds + ")");
                cmdDeactive = new SqlCommand(sbDeActiveIds.ToString(), oConnect);
                cmdDeactive.CommandType = CommandType.Text;
                cmdDeactive.ExecuteNonQuery();
                CloseSQLConnection();
            }
            catch (Exception ex)
            {
                oErrorLog.Open();
                oErrorLog.Log("UpdateActiveReview Function In UserInfo:" + ex.Message);
                oErrorLog.Flush();
                oErrorLog.Close();
            }
            finally
            {
                cmdDeactive.Dispose();
                cmdDeactive = null;
                sbDeActiveIds = null;

                CloseSQLConnection();
            }
        }

    }
    #endregion



    #region Create Newsletter

    public int UpdateNewsLetter(string sEMailAdress)
    {
        int iCatelogId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("CreateCatalogue", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;


            if (!string.IsNullOrEmpty(sEMailAdress))
                sqlCmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = sEMailAdress;
            else
                sqlCmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = DBNull.Value;

            sqlCmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sRemoteAddress;

            SqlParameter pCatalog = new SqlParameter("@CatalogueId", SqlDbType.Int);
            pCatalog.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCatalog);

            sqlCmd.ExecuteNonQuery();


            if (pCatalog.Value != "")
                iCatelogId = Convert.ToInt32(pCatalog.Value);

        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UpdateNewsLetter in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return iCatelogId;
    }



    #endregion



    #region Reset password


    public bool ResetSiteUserPassword(string Email, string UserPassword, out object ErrorMessage)
    {
        bool retVal = false;
        try
        {
            OpenSQLConnection();

            SqlCommand sqlCmd = new SqlCommand("ReSetpassword", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = Email;
            sqlCmd.Parameters.Add("@Password", SqlDbType.VarChar, 100).Value = UserPassword;

            SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            pErrorMessage.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pErrorMessage);

            sqlCmd.ExecuteNonQuery();            
            ErrorMessage = pErrorMessage.Value.ToString();

            if (string.Compare(ErrorMessage.ToString(), "Success", true) == 0)
            {
                retVal = true;
            }
            ErrorMessage = "";
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            CloseSQLConnection();
        }        
        return retVal;
    }


    #endregion


    #region Review


    public SqlDataReader GetRatingBySession(string standardcode)
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        StringBuilder sbQuery = new StringBuilder();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("RatingDetail", oConnect);

            sqlCmd.CommandType = CommandType.StoredProcedure;

            string ss = UserSession.GetCurrentSessionId();
            sqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = UserSession.GetWebSessionId();
            sqlCmd.Parameters.Add("@StandardCode", SqlDbType.VarChar).Value = standardcode;
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetRatingBySession: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }

    #endregion

    #region Save Share With Friend

    public void Debug(SqlCommand cmd)
    {
        HttpContext.Current.Response.Write("<br>CommandText= EXEC " + cmd.CommandText);
        foreach (SqlParameter p in cmd.Parameters)
        {
            if (p.ParameterName != null)
                HttpContext.Current.Response.Write("\n," + p.ParameterName);
            if (p.Value != null)
                HttpContext.Current.Response.Write("='" + p.Value.ToString() + "'");
        }
    }

    public string SaveRecordSendToFriend(string EmailTo, string FromEmail, string FirstName, string LastName,
        string Message, string sDesignCodeId, string sUserIpAddress, string sFarwordLink)
    {

        OpenSQLConnection();
        try
        {
            SqlCommand cmd = new SqlCommand("InsertEmailtoFriend", oConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EmailTo", SqlDbType.VarChar).Value = EmailTo;
            cmd.Parameters.Add("@EmailFrom", SqlDbType.VarChar).Value = FromEmail;
            cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;
            cmd.Parameters.Add("@Message", SqlDbType.VarChar).Value = Message;
            cmd.Parameters.Add("@DesignCodeId", SqlDbType.VarChar).Value = sDesignCodeId;

            cmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sUserIpAddress;

            cmd.Parameters.Add("@ForwardLink", SqlDbType.VarChar).Value = sFarwordLink;


            SqlParameter paramId = new SqlParameter("@Id", SqlDbType.Int);
            paramId.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(paramId);
            cmd.ExecuteNonQuery();
            //HttpContext.Current.Response.Write("Di" + sDesignCodeId);
            //Debug(cmd);
            //HttpContext.Current.Response.End();

            string Id = string.Empty;
            if (paramId.Value != null)
            {
                Id = paramId.Value.ToString();
            }
            return Id;
        }
        catch (Exception objException)
        {
            throw (objException);
        }
        finally
        {
            CloseSQLConnection();
        }


    }


    #endregion

    #region Save Offer EMail

    public int SaveOfferUsersEmail(string sEmail)
    {
        int iResultId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("SaveOfferUsers", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(sEmail))
                sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = sEmail;
            else
                sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = DBNull.Value;

            sqlCmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sRemoteAddress;

            // sqlCmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = DBNull.Value;


            SqlParameter sId = new SqlParameter("@Id", SqlDbType.Int);
            sId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(sId);

            SqlParameter paramErrorMsg = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 300);
            paramErrorMsg.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramErrorMsg);

            sqlCmd.ExecuteNonQuery();
            if (paramErrorMsg.Value != null)
            {
                Message = paramErrorMsg.Value.ToString();
            }
            if (sId.Value != null)
            {
                iResultId = Convert.ToInt32(sId.Value);
            }
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("SaveOfferUsersEmail in UserInfo.cs" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
            CloseSQLConnection();
        }
        return iResultId;
    }

    #endregion

    public SqlDataReader UserOrderStatus(string OrderNo)
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("SELECT O.OrderNo,O.ORDERID,O.ORDERDATE,O.BILLAMOUNT,S.DESCRIPTION,O.TrackingNo,O.DespatchDate FROM ORDERS O INNER JOIN  ORDERSTATUS S ON O.STATUSID=S.STATUSID INNER JOIN CUSTOMER C ON O.CUSTOMERID=C.CUSTOMERID where O.OrderNo=@OrderNo and O.PaymentStatus IN(1,2,3,4,8) ORDER BY O.ORDERDATE DESC", oConnect);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = OrderNo;
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UserOrderList: Function IN UserInfo.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dr;
    }


    public int UpdateUserAccount(string sEmailAddress, string Password, string FirstName, string LastName, bool bIsPromotion)
    {
        int iUserId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("[UpdateUserPersonaldata]", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = sEmailAddress;
            sqlCmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;

            sqlCmd.Parameters.Add("@ShFirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCmd.Parameters.Add("@ShLastName", SqlDbType.VarChar).Value = LastName;

            if (Password != "")
                sqlCmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;

            sqlCmd.Parameters.Add("@IsActiveForPromotion", SqlDbType.VarChar).Value = bIsPromotion;
            sqlCmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sRemoteAddress;


            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
            pUserId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserId);



            SqlParameter pUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
            pUserName.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserName);

            SqlParameter pCustomerId = new SqlParameter("@CustomerId", SqlDbType.Int);
            pCustomerId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCustomerId);

            SqlParameter pCustomerName = new SqlParameter("@CustName", SqlDbType.VarChar, 50);
            pCustomerName.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCustomerName);



            SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            pErrorMessage.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pErrorMessage);

            sqlCmd.ExecuteNonQuery();
            iUserId = Convert.ToInt32(pUserId.Value);
            if (iUserId > 0)
            {

                string sessionId = UserSession.GetWebSessionId();
                if (string.IsNullOrEmpty(sessionId))
                {
                    Guid guidSessionId = Guid.NewGuid();
                    sessionId = guidSessionId.ToString();
                }

                UserSession.SetCookie("Webuid", sqlCmd.Parameters["@userId"].Value.ToString());
                UserSession.SetCookie("sid ", sessionId.ToString());
                UserSession.SetCookie("un", sqlCmd.Parameters["@UserName"].Value.ToString());
                UserSession.SetCookie("Cid", sqlCmd.Parameters["@CustomerId"].Value.ToString());
                UserSession.SetCookie("guestUser", "0");
                UserSession.SetCookie("custName", sqlCmd.Parameters["@CustName"].Value.ToString());
                //HttpContext.Current.Session["custName"] = Convert.ToString(sqlCmd.Parameters["@CustName"].Value);
                UserSession.SetCookie("Source", "2");
            }
            if (pErrorMessage.Value != "")
                Message = Convert.ToString(pErrorMessage.Value);

        }
        catch (Exception ex)
        {
            Message = ex.Message.ToString();
            oErrorLog.Open();
            oErrorLog.Log("UpdateUserAccount Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return iUserId;

    }

    public int CreateNewUserFbGmailAccount(string sEmailAddress, string Password, string FirstName, string LastName, bool bIsPromotion, string UserType)
    {
        int iUserId = 0;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("CreateFbGmWebUserLogin_bbm_v1", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = sEmailAddress;
            sqlCmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;

            sqlCmd.Parameters.Add("@ShFirstName", SqlDbType.VarChar).Value = FirstName;
            sqlCmd.Parameters.Add("@ShLastName", SqlDbType.VarChar).Value = LastName;

            sqlCmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
            sqlCmd.Parameters.Add("@IsActiveForPromotion", SqlDbType.VarChar).Value = bIsPromotion;
            sqlCmd.Parameters.Add("@UserIpAddress", SqlDbType.VarChar).Value = sRemoteAddress;
            sqlCmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = UserType;


            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
            pUserId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserId);



            SqlParameter pUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 100);
            pUserName.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pUserName);

            SqlParameter pCustomerId = new SqlParameter("@CustomerId", SqlDbType.Int);
            pCustomerId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCustomerId);

            SqlParameter pCustomerName = new SqlParameter("@CustName", SqlDbType.VarChar, 50);
            pCustomerName.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pCustomerName);


            SqlParameter paramEncryptCusterId = new SqlParameter("@EncryptCustId", SqlDbType.VarChar, 100);
            paramEncryptCusterId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramEncryptCusterId);

            SqlParameter pErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            pErrorMessage.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(pErrorMessage);

            sqlCmd.ExecuteNonQuery();
            iUserId = Convert.ToInt32(pUserId.Value);
            if (iUserId > 0)
            {
                string sessionId = UserSession.GetWebSessionId();
                if (string.IsNullOrEmpty(sessionId))
                {
                    Guid guidSessionId = Guid.NewGuid();
                    sessionId = guidSessionId.ToString();
                }

                UserSession.SetCookie("Webuid", sqlCmd.Parameters["@userId"].Value.ToString());
                UserSession.SetCookie("sid ", sessionId.ToString());
                UserSession.SetCookie("un", sqlCmd.Parameters["@UserName"].Value.ToString());
                UserSession.SetCookie("Cid", sqlCmd.Parameters["@CustomerId"].Value.ToString());
                UserSession.SetCookie("EncryptCustId", sqlCmd.Parameters["@EncryptCustId"].Value.ToString());
                UserSession.SetCookie("guestUser", "0");
                UserSession.SetCookie("custName", sqlCmd.Parameters["@CustName"].Value.ToString());
                UserSession.SetCookie("Source", "2");
                UserSession.SetCookie("UserType", "facebook");

            }
            if (pErrorMessage.Value != "")
                Message = Convert.ToString(pErrorMessage.Value);

        }
        catch (Exception ex)
        {
            Message = ex.Message.ToString();
            oErrorLog.Open();
            oErrorLog.Log("CreateNewUserAccount Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return iUserId;

    }
    public DataSet GetEmailByEncryptCustId(string sEncryptedId)
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataAdapter dtAdptr = new SqlDataAdapter(sqlCmd);

        DataSet dsDataSet = new DataSet();

        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("GetUserDetailByEncryptId", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@EncryptCustomerId", SqlDbType.VarChar).Value = sEncryptedId;
            dtAdptr = new SqlDataAdapter(sqlCmd);

            dsDataSet = new DataSet();
            dtAdptr.Fill(dsDataSet);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetEmailByEncryptCustId Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            dtAdptr.Dispose();
            dtAdptr = null;

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return dsDataSet;
    }
    public string Referral_GetCustomerEncryptedId(int iUserId)
    {
        string sEncryptedId = "";
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("[Referral_GetEncryptCustomerId]", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;


            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = iUserId;

            SqlParameter paramEncryptCusterId = new SqlParameter("@EncryptCusterId", SqlDbType.VarChar, 100);
            paramEncryptCusterId.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(paramEncryptCusterId);


            sqlCmd.ExecuteNonQuery();
            if (sqlCmd.Parameters["@EncryptCusterId"] != null)
            {
                sEncryptedId = Convert.ToString((sqlCmd.Parameters["@EncryptCusterId"].Value));

                if (sEncryptedId != "")
                {


                    UserSession.SetCookie("EncryptCustId", sqlCmd.Parameters["@EncryptCusterId"].Value.ToString());

                    return sEncryptedId;

                }
            }
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("Referral_GetCustomerEncryptedId.cs Function In UserInfo:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return sEncryptedId;
    }

}
