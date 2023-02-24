<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RejectGloveSummary.aspx.cs" Inherits="Hartalega.FloorSystem.Web.UI.TV.RejectGloveSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quick summary Online Rejection</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,chrome=1" />
    <script src="JS/jquery.min.js"></script>

    <script src="JS/jquery-ui-1.8.24.min.js"></script>
    <link href="styles/jquery-ui.css" rel="stylesheet" />
    <link href="styles/rejectGlove.css" rel="stylesheet" />
    <script src="JS/rejectGlove.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="padding-left: 200px;">
            </div>

            <div class="Divfrst">
                <div class="divDate">
                    <label>Start Date</label>
                    &nbsp; &nbsp;<asp:TextBox ID="txtfrom" runat="server" MaxLength="10"></asp:TextBox>
                </div>
                <div class="divDate">
                    <label>End Date</label>
                    &nbsp; &nbsp;<asp:TextBox ID="txtTo" runat="server" MaxLength="10"></asp:TextBox>
                </div>
                <div class="divbutton">
                    <asp:Button runat="server" ID="btnvwReport" Text="View Report" OnClick="btnvwReport_Click" OnClientClick="return validateSub()" />
                    &nbsp;&nbsp;
                     <asp:Button runat="server" ID="btnexport" Text="Export to Excel" OnClick="btnexport_Click" />
                </div>
            </div>
            <div>
                <div class="divDate">
                    <label>Plant</label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlPlant" runat="server"></asp:DropDownList>
                </div>
                <div class="divDate">
                    <label>Shift</label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlShift" runat="server">
                        <asp:ListItem Text="7:00 - 7:00" Value="0"></asp:ListItem>
                        <asp:ListItem Text="12:00 - 12:00" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="error_field" id="validation_msg">
            </div>
            <div class="Divfrst">
                <asp:Panel ID="pnlReport" runat="server" Visible="false">
                    <hr />
                    <h1>Quick summary Online Rejection</h1>
                    <div class="divDate">
                        <b>Start Date: &nbsp;<asp:Label runat="server" ID="lblstartDate" />
                        </b>
                    </div>
                    <div class="divDate">
                        <b>End Date: &nbsp;<asp:Label runat="server" ID="lblendate" />
                        </b>
                    </div>
                    <div class="divDate">
                        <b>Plant: &nbsp;<asp:Label runat="server" ID="lblPlant" />
                        </b>
                    </div>
                    <div class="divDate">
                        <b>Shift: &nbsp;<asp:Label runat="server" ID="lblShift" />
                        </b>
                    </div>
                    <br />
                    <br />
                    <asp:GridView runat="server" ID="gvRejectSummary" CssClass="mGrid">
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdnfrom" />
        <asp:HiddenField runat="server" ID="hdnTo" />
        <asp:HiddenField runat="server" ID="hdnPlant" />
    </form>
</body>
</html>
