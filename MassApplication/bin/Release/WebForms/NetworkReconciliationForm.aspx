<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NetworkReconciliationForm.aspx.cs" Inherits="MassApplication.WebForms.NetworkReconciliationForm" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
<script type="text/javascript">
    function getQueryParams(params, url) {
        let href = url;
        //this expression is to get the query strings
        let reg = new RegExp('[?&]' + params + '=([^&#]*)', 'i');
        let queryString = reg.exec(href);
        return queryString ? queryString[1] : null;
    }
    var results = getQueryParams('duration', window.location);
    var type = getQueryParams('type', window.location);
    
    document.title = decodeURI('Network Wise ' + type + ' Reconciliation: ' + results);
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <CR:CrystalReportViewer ID="Network_Reconciliation" runat="server" AutoDataBind="false" ToolPanelView="None" ToolPanelWidth="200px" HasCrystalLogo="false" HasToggleGroupTreeButton="false" BestFitPage="True" HasToggleParameterPanelButton="false"/>
        </div>
    </form>
</body>
</html>
