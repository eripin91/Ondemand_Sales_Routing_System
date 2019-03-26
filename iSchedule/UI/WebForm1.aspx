<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="iSchedule.UI.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <br />
            <br />
            Key: <asp:Label ID="lblKey" runat="server" Text=""></asp:Label><br />
            IV: <asp:Label ID="lblIV" runat="server" Text=""></asp:Label><br />
            Decrypted string: <asp:Label ID="lblDecrypted" runat="server"></asp:Label>
            
            <br />
            <br />
            <br />
            <asp:TextBox ID="txtEncrypted" runat="server" Height="20px" Width="633px"></asp:TextBox>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <br />
            <br />
            <asp:Label ID="lblEncryptString" runat="server" Text="Label"></asp:Label>
            <br />
            
        </div>
    </form>
</body>
</html>
