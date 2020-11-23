<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="iSchedule.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <meta content="" name="description" />
    <meta content="" name="author" />
    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href='../Content/bootstrap.css' type='text/css' rel='stylesheet' />
    <link href='../Content/style.css' type='text/css' rel='stylesheet' />
    <link rel="stylesheet" type="text/css" href="//fonts.googleapis.com/css?family=Roboto:400,700" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.2/css/font-awesome.min.css" />
    <link href='../Content/General.css' type='text/css' rel='stylesheet' />
</head>
<body class="gray-bg">
    <div class="container">
        <div class="wrapper">
            <form runat="server" method="post" name="Login_Form" class="form-signin">

                <hr class="colorgraph" />
                <p>
                    &nbsp;
                </p>
                <h3 class="form-signin-heading">SMSDome iSchedule Portal</h3>

                <div class="form-group">
                    <asp:TextBox runat="server" ID="UserName" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox runat="server" ID="PassWord" CssClass="form-control" TextMode="Password"></asp:TextBox>
                </div>

                <asp:Button runat="server" CssClass="btn btn-lg btn-primary btn-block" OnClick="Login_Click" Text="Login" />

                <p>
                    <small><strong>Copyright</strong> &copy;
                    <asp:Label runat="server" ID="lblDT"></asp:Label><strong>SMSDOME iSchedule v&nbsp;<span runat="server" id="Version"></span></strong></small>
                </p>


                <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>


                <!-- Modal -->
                <div id="divPopUp" class="modal fade" aria-hidden="false" role="dialog">
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                        <%--    <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title"></h4>
                            </div>--%>
                            <div class="modal-body">
                                <h3><asp:Label runat="server"  ID="lblModal" Text=""> </asp:Label></h3>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                                <%--   <asp:Button ID="btnCancel" runat="server" Text=""  OnClientClick="" />--%>
                            </div>
                        </div>

                    </div>
                </div>

                <!-- BEGIN CORE PLUGINS -->
                <!--Reference Scripts-->
                <script type='text/javascript' src='../Scripts/refs/jquery-1.10.2.min.js'></script>
                <script type='text/javascript' src='../Scripts/refs/jquery.validate.min.js'></script>
                <script type='text/javascript' src='../Scripts/refs/bootstrap.min.js'></script>


                <script type='text/javascript' src='../Scripts/refs/moment.js'></script>

                <%--<script type='text/javascript' src='../Scripts/refs/angular.min.js'></script>
<script type='text/javascript' src='../Scripts/refs/angular-sanitize.js'></script>
<script type='text/javascript' src='../Scripts/refs/ui-bootstrap-tpls-1.3.3.min.js'></script>
<script type='text/javascript' src='../Scripts/refs/angular-moment.js'></script>
<script type='text/javascript' src='../Scripts/refs/datetime-picker.js'></script>
<script type='text/javascript' src='../Scripts/refs/bootbox.min.js'></script>
<script type='text/javascript' src='../Scripts/refs/ngBootbox.js'></script>--%>
                <!--End Of Reference Scripts-->
                <!--Page Related-->
                <%--<script type='text/javascript' src='../Scripts/BC/app.js'></script>
<script type='text/javascript' src='../Scripts/BC/Factory.js'></script>
<script type='text/javascript' src='../Scripts/BC/Login.js'></script>--%>
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

            </form>

        </div>
    </div>

</body>

</html>

