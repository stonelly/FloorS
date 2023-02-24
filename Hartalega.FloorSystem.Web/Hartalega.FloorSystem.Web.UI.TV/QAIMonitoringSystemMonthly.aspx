<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QAIMonitoringSystemMonthly.aspx.cs" Inherits="Hartalega.FloorSystem.Web.UI.TV.QAIMonitoringSystemMonthly" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,chrome=1" />
    <link href="App_Themes/ReportTheme/ReportStyleSheet.css" rel="stylesheet" type="text/css" runat="server" />
    <link href="styles/QAIMonitoring.css" rel="stylesheet" type="text/css" runat="server" />
    <link href="styles/jquery-ui.css" rel="stylesheet" />
    <style>
        #QAIGridView tr td{
            text-align:center;line-height:0.65;
        }

        #QAIGridView tr td:nth-child(n+3){
            font-size: 13px;
        }

    </style>
    <script src="JS/jquery.min.js"></script>
    <script src="JS/jquery-ui-1.8.24.min.js"></script>
    <!--<script src="JS/TVReports.js"></script>-->
    <script>
        //function ShrinkTable() {
        //    //MaxMinGrid($('#QAIGridView'), $('#QAIGridView tr:not(:first)'), $('#footergrid'), screen.height, $('#mainDiv'), $('body'));      
        //    SetDatePicker();
        //}

        function SetDatePicker() {
            $('#txtDate').datepicker({
                changeMonth: true,
                changeYear: true,
                showOn: "button",
                buttonImage: "/images/calendar.png",
                buttonImageOnly: true,
                dateFormat: 'dd/mm/yy',
                maxDate: new Date(),
                beforeShow: function () {
                    $(".ui-datepicker").css('font-size', 12)
                },
            });
        }
    </script>
</head>
<body style="table-layout: fixed; overflow: hidden">
    <form id="form1" runat="server">
        <div style="width: 100%;">
            <asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="10" Visible="false"></asp:Label>
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="mainDiv" style="width: 100%; margin: 0px; padding: 0px; float: left; padding-top: 0.2%; padding-bottom: 0.2%">
                        <div style="width: auto; float: left; margin: 0px; padding: 0px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHeader" runat="server" Text="Date"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:Label ID="lblHeader2" runat="server" Text="Line"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlLine" runat="server" DataMember="LineNumber" DataValueField="LineNumber"></asp:DropDownList></td>
                                    <td>
                                        <asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="contentDiv" style="width: 100%; float: left; margin: 0px;">                            
                            <asp:GridView ID="QAIGridView" runat="server" AutoGenerateColumns="false"
                                AllowPaging="true" PagerSettings-Visible="false" Width="100%">
                                <HeaderStyle Font-Size="Larger" />
                                <Columns>
                                                <asp:BoundField DataField="Line" HeaderText="Line" ItemStyle-Width="35px" />
                                                <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-Width="35px" />
                                                <asp:BoundField DataField="00" HeaderText="00" />
                                                <asp:BoundField DataField="01" HeaderText="01" />
                                                <asp:BoundField DataField="02" HeaderText="02" />
                                                <asp:BoundField DataField="03" HeaderText="03" />
                                                <asp:BoundField DataField="04" HeaderText="04" />
                                                <asp:BoundField DataField="05" HeaderText="05" />
                                                <asp:BoundField DataField="06" HeaderText="06" />
                                                <asp:BoundField DataField="07" HeaderText="07" />
                                                <asp:BoundField DataField="08" HeaderText="08" />
                                                <asp:BoundField DataField="09" HeaderText="09" />
                                                <asp:BoundField DataField="10" HeaderText="10" />
                                                <asp:BoundField DataField="11" HeaderText="11" />
                                                <asp:BoundField DataField="12" HeaderText="12" />
                                                <asp:BoundField DataField="13" HeaderText="13" />
                                                <asp:BoundField DataField="14" HeaderText="14" />
                                                <asp:BoundField DataField="15" HeaderText="15" />
                                                <asp:BoundField DataField="16" HeaderText="16" />
                                                <asp:BoundField DataField="17" HeaderText="17" />
                                                <asp:BoundField DataField="18" HeaderText="18" />
                                                <asp:BoundField DataField="19" HeaderText="19" />
                                                <asp:BoundField DataField="20" HeaderText="20" />
                                                <asp:BoundField DataField="21" HeaderText="21" />
                                                <asp:BoundField DataField="22" HeaderText="22" />
                                                <asp:BoundField DataField="23" HeaderText="23" />
                                            </Columns>
                                        <PagerSettings Visible="False" />
                            </asp:GridView>
                            <div style="width: 100%" id="footergrid" hidden="">
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
        </div>

    </form>
</body>
</html>
