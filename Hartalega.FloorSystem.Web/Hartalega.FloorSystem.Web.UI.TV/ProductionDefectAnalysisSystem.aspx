<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductionDefectAnalysisSystem.aspx.cs" Inherits="Hartalega.FloorSystem.Web.UI.TV.ProductionDefectAnalysisSystem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title></title>
    <link href="App_Themes/ReportTheme/ReportStyleSheet.css" rel="stylesheet" type="text/css" runat="server" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/TVReports.js"></script>
    <script>
        function ShrinkGrid() {
            MaxMinGrid($('#ProdDefectGridView'), $('#ProdDefectGridView tr:not(:first)'), $('#footergrid'), screen.height, $('#mainDiv'), $('body'));
        }
    </script>
</head>
<body style="table-layout: fixed; overflow: auto"> <!--onload="ShrinkGrid();"-->
    <form id="form1" runat="server">
        <asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="10" Visible="false"></asp:Label>
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="120000" Enabled="True">
        </asp:Timer>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
            </Triggers>
            <ContentTemplate>
                <asp:Timer ID="Timer2" runat="server" OnTick="Timer2_Tick" Interval="120000" Enabled="True">
                </asp:Timer>
                <div id="mainDiv" style="width: 100%; margin: 0px; padding: 0px; float: left; padding-top: 0.2%; padding-bottom: 0.2%;">
                    <div style="width: 1%; float: left; margin: 0px; padding: 0px;">
                        &nbsp;
                    </div>
                    <div id="contentDiv" style="width: 98%; float: left; margin: 0px;">
                        <asp:GridView ID="ProdDefectGridView" runat="server" AutoGenerateColumns="true"
                           Width="100%">
                            <HeaderStyle Font-Size="X-Large" />
                        </asp:GridView>
                        <div style="width: 100%" id="footergrid">
                            <table style="width: 100%">
                                    <tr style="background-color: lightgray">
                                        <td style="width: 11%">
                                            <asp:Label ID="Label1" Text="Hartalega Sdn Bhd" Font-Size="Medium" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 9%; text-align: left">
                                            <asp:Label ID="Location" Font-Size="Large" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 25%; text-align: left">
                                            <asp:Label ID="LastBatchJobRunTime" Font-Size="Medium" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 24%; text-align: left">
                                            <asp:Label ID="NextRefreshTime" Font-Size="Medium" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 14%; text-align: right">
                                            <asp:Label ID="NextRotationTime" Font-Size="Medium" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                        </div>
                    </div>
                    <div style="width: 1%; float: left; margin: 0px; padding: 0px;">
                        &nbsp;
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
