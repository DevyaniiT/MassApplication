<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dynamic_Form.aspx.cs" Inherits="MassApplication.WebForms.Dynamic_Form" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dynamic Report</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="row">
            <CR:CrystalReportViewer ID="Dynamic_Report" runat="server" AutoDataBind="false" ToolPanelView="None" ToolPanelWidth="200px" HasCrystalLogo="false" HasToggleGroupTreeButton="false" BestFitPage="True" HasToggleParameterPanelButton="false" />
        </div>

    </form>
</body>
</html>
