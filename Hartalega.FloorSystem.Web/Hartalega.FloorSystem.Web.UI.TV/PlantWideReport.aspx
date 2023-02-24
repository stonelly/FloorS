<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlantWideReport.aspx.cs" Inherits="Hartalega.FloorSystem.Web.UI.TV.PlantWideReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="App_Themes/ReportTheme/ReportStyleSheet.css" rel="stylesheet" type="text/css" runat="server" />
    <script src="JS/jquery.min.js"></script>
    <script type="text/javascript">
        var i = 1;
        $(document).ready(function () {

            var arr = $("#HiddenReportULS").val().split(',');
            setInterval(function () {
                if (i > arr.length - 1) {
                    i = 0;
                    $('#PlantFrame').attr('src', arr[i]);
                    i++;
                }
                else {
                    $('#PlantFrame').attr('src', arr[i]);
                    i++;
                }
            }, $('#<%= RefreshTimeInterval.ClientID %>').val());
          });
    </script>

</head>
<body style="overflow: hidden">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:HiddenField ID="RefreshTimeInterval" runat="server" ClientIDMode="Static" />
        <div style="width: 99%; height: 100%; position: fixed">
            <iframe id="PlantFrame" runat="server" style="width: 100%; height: 100%"></iframe>
        </div>
    </form>
</body>
</html>
