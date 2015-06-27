using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for CString
/// </summary>
public class CString
{
    public CString()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string Left(string param, int length)
    {
        //we start at 0 since we want to get the characters starting from the
        //left and with the specified lenght and assign it to a variable
        string result = param.Substring(0, length);
        //return the result of the operation
        return result;
    }
    public static string Right(string param, int length)
    {
        //start at the index based on the lenght of the sting minus
        //the specified lenght and assign it a variable
        string result = param.Substring(param.Length - length, length);
        //return the result of the operation
        return result;
    }

    public static string Mid(string param, int startIndex, int length)
    {
        //start at the specified index in the string ang get N number of
        //characters depending on the lenght and assign it to a variable
        string result = param.Substring(startIndex, length);
        //return the result of the operation
        return result;
    }

    public static string Mid(string param, int startIndex)
    {
        //start at the specified index and return all characters after it
        //and assign it to a variable
        string result = param.Substring(startIndex);
        //return the result of the operation
        return result;
    }

    private static Regex _isNumber = new Regex(@"^\d+$");

    public static bool IsInteger(string theValue)
    {
        Match m = _isNumber.Match(theValue);
        return m.Success;
    } //IsInteger

    public static bool IsDecimal(string theValue)
    {
        try
        {
            Convert.ToDouble(theValue);
            return true;
        }
        catch
        {
            return false;
        }
    } //IsDecimal


    public static bool IsDate(string Expression)
    {
        if (Expression != null)
        {
            if (Expression is DateTime)
            {
                return true;
            }
            if (Expression is string)
            {
                try
                {

                    DateTime time1 = DateTime.Parse(Expression); /// DateType.FromString((string)Expression);
                    return true;
                }
                catch (Exception)
                {
                }
            }
        }
        return false;
    }

    //Convert String to ProperCase?
    public static string PCase(string strParam)
    {
        String strProper = strParam.Substring(0, 1).ToUpper();
        strParam = strParam.Substring(1).ToLower();
        String strPrev = "";

        for (int iIndex = 0; iIndex < strParam.Length; iIndex++)
        {
            if (iIndex > 1)
            {
                strPrev = strParam.Substring(iIndex - 1, 1);
            }
            if (strPrev.Equals(" ") ||
                strPrev.Equals("\t") ||
                strPrev.Equals("\n") ||
                strPrev.Equals("."))
            {
                strProper += strParam.Substring(iIndex, 1).ToUpper();
            }
            else
            {
                strProper += strParam.Substring(iIndex, 1);
            }
        }
        return strProper;
    }
}
