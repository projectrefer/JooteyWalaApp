using System;
using System.Data;
using System.Configuration;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Text;
using System.IO;
using System.Data.SqlClient;


/// <summary>
/// Summary description for CashOnDelivery
/// </summary>
public class CashOnDelivery : Connection
{
    public CashOnDelivery()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    ErrorLogger oErrorLog = new ErrorLogger();

    public DataSet GETDesignAngles()
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataAdapter dtAdptr = new SqlDataAdapter(sqlCmd);

        DataSet dsDataSet = new DataSet();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("GetTest", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            //if (sStandardCode != "")
            //    sqlCmd.Parameters.Add("@StandardCode", SqlDbType.VarChar).Value = sStandardCode;
            //else
                //sqlCmd.Parameters.Add("@StandardCode", SqlDbType.VarChar).Value = DBNull.Value;

            dtAdptr = new SqlDataAdapter(sqlCmd);

            dsDataSet = new DataSet();
            dtAdptr.Fill(dsDataSet);
        }
        catch (Exception ex)
        {
            dtAdptr.Dispose();
            dtAdptr = null;

            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
        }
        return dsDataSet;
    }





    public void UpdateCodInCollection(string sCollectionIds, string subClassId)
    {
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("UpdateCod", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CollectionIds", SqlDbType.VarChar).Value = sCollectionIds;
            sqlCmd.Parameters.Add("@SubClassId", SqlDbType.VarChar).Value = subClassId;
            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("UpdateCodIsCollection Function IN CashOnDelivery.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd = null;
            CloseSQLConnection();
        }
    }

    public bool IsValidCodPinNumber(string PinNumber)
    {
        SqlCommand sqlCmd = null;
        bool bRetVal = false;
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("IsValidPinNumber", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@PinNumber", SqlDbType.VarChar).Value = PinNumber.Trim();
            sqlCmd.Parameters.Add("@RValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            sqlCmd.ExecuteNonQuery();
            bRetVal = Convert.ToBoolean(sqlCmd.Parameters["@RValue"].Value);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("IsValidCodPinNumber Function IN CashOnDelivery.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
            CloseSQLConnection();

        }
        return bRetVal;
    }

    public bool CheckCODNotAvial()
    {
        SqlCommand sqlCmd = null;
        bool bRetVal = false;
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("CheckCodNotAvialibility", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CUstomerId", SqlDbType.VarChar).Value = UserSession.GetCurrentCustId();
            sqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = UserSession.GetWebSessionId();
            sqlCmd.Parameters.Add("@RValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            sqlCmd.ExecuteNonQuery();
            bRetVal = Convert.ToBoolean(sqlCmd.Parameters["@RValue"].Value);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("CheckCODNotAvial Function IN CashOnDelivery.cs:" + ex.Message);
            oErrorLog.Flush();
            oErrorLog.Close();
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;
            CloseSQLConnection();
        }
        return bRetVal;
    }


    public bool UpdatePaymentOption(int sPayOption)
    {
        StringBuilder sbQuery = new StringBuilder();
        SqlCommand sqlCmd = null;
        bool bReturnValue;

        try
        {

            // 0 FOR Pay Online and 1 for Cash On delivery..

            sbQuery.Append(" UPDATE CartItems SET IsCashOnDelivery=@IsCod WHERE SessionId=@SessionId AND CustomerId=@CustomerId");
            OpenSQLConnection();
            sqlCmd = new SqlCommand(sbQuery.ToString(), oConnect);
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.Parameters.Add("@IsCod", SqlDbType.Bit).Value = sPayOption;
            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.VarChar).Value = UserSession.GetCurrentCustId();
            sqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = UserSession.GetWebSessionId();

            sqlCmd.ExecuteNonQuery();
            bReturnValue = true;
        }
        catch (Exception ex)
        {
            //  To Do with exception......
            throw (ex);
        }
        finally
        {
            sqlCmd.Dispose();
            sqlCmd = null;

            CloseSQLConnection();
        }
        return bReturnValue;
    }
    public void ConfirmCodOrders(int iOrderId)
    {
        SqlCommand sqlCmd = null;
        bool bRetVal = false;
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("ConfirmCodOrders", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = iOrderId;
            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("ConfirmCodOrders Function IN CashOnDelivery.cs:" + ex.Message);
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

    public SqlDataReader GetCodCustomerAdress(string sCustomerId)
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("GetCustomerById", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CustId", SqlDbType.Int).Value = sCustomerId;
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetCodCustomerAdress: Function IN CashOnDelivery.cs:" + ex.Message);
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

    public SqlDataReader GetBillingAndShippingId(string sCustomerId)
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("Select AddressId,ShippingAddressId from Customer Where CustomerId=@CustomerId", oConnect);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = sCustomerId;
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

    public void UpdateCodOrderAddress(int sCustomerId, int shippingAddressId, int BillingAddId,
                            string sBillFirstName, string sBillLastName, string sBillAddress,
                            string sBillCity, string sBillState, string sBillCOuntry,
                            string sBillPinNo, string sBillOfficePhone, string sBillHomePhone, string sBillMobileNo,
                            string sOrderId
                            )
    {
        SqlCommand sqlCmd = new SqlCommand();
        try
        {
            OpenSQLConnection();
            sqlCmd = new SqlCommand("[UpdateOrderAddress]", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = sCustomerId;
           // sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserSession.GetCurrentUserId();
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
            sqlCmd.Parameters.Add("@OrderId", SqlDbType.VarChar).Value = sOrderId;




            sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            oErrorLog.Open();
            oErrorLog.Log("UpdateCodOrderAddress in CashOnDelivery.cs" + ex.Message);
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
    public SqlDataReader GetCodCustomerAdressAdmin(string sCustomerId)
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("[GetCustomerByIdAdmin]", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@CustId", SqlDbType.Int).Value = sCustomerId;
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetCodCustomerAdress: Function IN CashOnDelivery.cs:" + ex.Message);
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
    public SqlDataReader GetOrderSource()
    {
        SqlDataReader dr = null;
        SqlCommand sqlCmd = new SqlCommand();
        try
        {

            OpenSQLConnection();
            sqlCmd = new SqlCommand("Source_GetSourceList", oConnect);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            oErrorLog.Open();
            oErrorLog.Log("GetOrderSource: Function IN CashOnDelivery.cs:" + ex.Message);
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
}
