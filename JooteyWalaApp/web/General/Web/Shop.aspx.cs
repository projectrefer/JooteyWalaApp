using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class web_General_Web_Shop : System.Web.UI.Page
{
    CashOnDelivery Co = new CashOnDelivery();

    protected void Page_Load(object sender, EventArgs e)
    {
        GETDesignAngles();
    }

    public void GETDesignAngles()
    {
        //StandardDesignInfo objInfo = new StandardDesignInfo();

        DataSet dsInfo = Co.GETDesignAngles();
        if (dsInfo.Tables[0].Rows.Count > 0)
        {
            RepDetails.DataSource = dsInfo;
            RepDetails.DataBind();


           
        }
        //if (dsInfo.Tables[1].Rows.Count > 0)
        //{
        //    rptIcons.DataSource = dsInfo.Tables[1];
        //    rptIcons.DataBind();
        //}
    }
}