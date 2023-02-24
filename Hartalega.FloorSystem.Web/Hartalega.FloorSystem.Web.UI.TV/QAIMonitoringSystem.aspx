<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QAIMonitoring.aspx.cs" Inherits="Hartalega.FloorSystem.Web.UI.TV.QAIMonitoringSystem" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,chrome=1" />
    <link href="App_Themes/ReportTheme/ReportStyleSheet.css" rel="stylesheet" type="text/css" runat="server" />
    <link href="styles/QAIMonitoring.css" rel="stylesheet" type="text/css" runat="server" />
    <style>
        #QAIGridView tr td{
            text-align:center;line-height:0.65;
        }

        #QAIGridView tr td:nth-child(n+3){
            font-size: 13px;
        }

    </style>
    <script src="JS/jquery.min.js"></script>
<%--    <script src="JS/TVReports.js"></script>--%>
    <script>

        //function ShrinkTable() {
        //    MaxMinGrid($('#QAIGridView'), $('#QAIGridView tr:not(:first)'), $('#footergrid'), screen.height, $('#mainDiv'), $('body'));
        //}
    </script>
</head>
<body" style="table-layout: fixed; overflow: hidden">
    <form id="form1" runat="server">
        <div style="width: 100%;">
            <asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="10" Visible="false"></asp:Label>
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="120000" Enabled="true">
            </asp:Timer>
            <asp:Timer ID="Timer2" runat="server" OnTick="Timer2_Tick" Enabled="true">
            </asp:Timer>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                </Triggers>
                <ContentTemplate>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="mainDiv" style="width: 100%; margin: 0px; padding: 0px; float: left; padding-top: 0.2%; padding-bottom: 0.2%">
                                <div style="width: 0%; float: left; margin: 0px; padding: 0px;">
                                    &nbsp;
                                </div>
                                <div id="contentDiv" style="width: 100%; float: left; margin: 0px;">
                                    <asp:GridView ID="QAIGridView" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="True" PagerSettings-Visible="false" Width="100%">
                                        <HeaderStyle Font-Size="Larger" />
                                            <Columns>
                                                <asp:BoundField DataField="Plant" HeaderText="" ItemStyle-Width="35px" />
                                                <asp:BoundField DataField="Line" HeaderText="" ItemStyle-Width="35px" />
                                                <asp:BoundField DataField="H00" HeaderText="H00" />
                                                <asp:BoundField DataField="H01" HeaderText="H01" />
                                                <asp:BoundField DataField="H02" HeaderText="H02" />
                                                <asp:BoundField DataField="H03" HeaderText="H03" />
                                                <asp:BoundField DataField="H04" HeaderText="H04" />
                                                <asp:BoundField DataField="H05" HeaderText="H05" />
                                                <asp:BoundField DataField="H06" HeaderText="H06" />
                                                <asp:BoundField DataField="H07" HeaderText="H07" />
                                                <asp:BoundField DataField="H08" HeaderText="H08" />
                                                <asp:BoundField DataField="H09" HeaderText="H09" />
                                                <asp:BoundField DataField="H10" HeaderText="H10" />
                                                <asp:BoundField DataField="H11" HeaderText="H11" />
                                                <asp:BoundField DataField="H12" HeaderText="H12" />
                                                <asp:BoundField DataField="H13" HeaderText="H13" />
                                                <asp:BoundField DataField="H14" HeaderText="H14" />
                                                <asp:BoundField DataField="H15" HeaderText="H15" />
                                                <asp:BoundField DataField="H16" HeaderText="H16" />
                                                <asp:BoundField DataField="H17" HeaderText="H17" />
                                                <asp:BoundField DataField="H18" HeaderText="H18" />
                                                <asp:BoundField DataField="H19" HeaderText="H19" />
                                                <asp:BoundField DataField="H20" HeaderText="H20" />
                                                <asp:BoundField DataField="H21" HeaderText="H21" />
                                                <asp:BoundField DataField="H22" HeaderText="H22" />
                                                <asp:BoundField DataField="H23" HeaderText="H23" />
                                            </Columns>
                                        <PagerSettings Visible="False" />
                                    </asp:GridView> 
                                   
                                        <div style="width: 100%" id="footergrid">
                                        <table style="width: 100%">
                                            <tr style="background-color: lightgray">
                                                <td style="padding-right: 60px">
                                                    <asp:Label ID="Label1" Text="Hartalega Sdn Bhd" Font-Size="Small" runat="server"></asp:Label>
                                                </td>                                               
                                                <td style="padding-right: 80px">
                                                    <asp:Label ID="LastBatchJobRunTime" Font-Size="Small" runat="server"></asp:Label>
                                                </td>
                                                 <td style="padding-right: 100px">
                                                    <asp:Label ID="NextRefreshTime" Font-Size="Small" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 20%; text-align: right">
                                                    <asp:Label ID="NextRotationTime" Font-Size="Small" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div style="width: 0%; float: left; margin: 0px; padding: 0px;">
                                    &nbsp;
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
