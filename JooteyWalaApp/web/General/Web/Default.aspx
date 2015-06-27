<%@ Page Title="" Language="C#" MasterPageFile="~/web/General/Master/JooteyWala.master"
    AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="web_General_Web_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <div>
        <asp:Repeater ID="RepDetails" runat="server">
            <HeaderTemplate>
                <table style="border: 1px solid #df5015; width: 500px" cellpadding="0">
                    <tr style="background-color: #df5015; color: White">
                        <td colspan="2">
                            <b>Comments</b>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="background-color: #EBEFF0">
                    <td>
                        <table style="background-color: #EBEFF0; border-top: 1px dotted #df5015; width: 500px">
                            <tr>
                                <td>
                                    Subject:
                                    <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("Subject") %>' Font-Bold="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblComment" runat="server" Text='<%#Eval("Comment") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="background-color: #EBEFF0; border-top: 1px dotted #df5015; border-bottom: 1px solid #df5015;
                            width: 500px">
                            <tr>
                                <td>
                                    Post By:
                                    <asp:Label ID="lblUser" runat="server" Font-Bold="true" Text='<%#Eval("UserName") %>' />
                                </td>
                                <td>
                                    Created Date:<asp:Label ID="lblDate" runat="server" Font-Bold="true" Text='<%#Eval("PostedDate") %>' />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
