<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Layout.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="iSchedule.Views.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="page-content-wrapper">
        <div class="container">
            <div class="row">
                <h1>Change Password</h1>
            </div>
                    
            <br /><br />
            <div class="row">
                <h5 style="font-weight: bold">Old Password *</h5>
                <asp:TextBox runat="server" ID="OldPass" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        
            </div>
            <br />
            <div class="row">
                <h5 style="font-weight: bold">New Password *</h5>
                <asp:TextBox runat="server" ID="NewPass" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        
            </div>
            <br />
            <div class="row">
                <h5 style="font-weight: bold">Confirm New Password *</h5>
                <asp:TextBox runat="server" ID="ConfirmNewPass" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        
            </div>
            <br />
            <div class="row">
                <div class="col-lg-6">
                    <asp:Button runat="server" ID="Update"
                        CssClass="btn btn-primary pull-right" Text="Save" OnClick="Save_Click" />
                </div>
                <div class="col-lg-8">&nbsp;
                </div>
            </div>

        </div>
    </div>
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
                    <h3>
                        <asp:Label runat="server" ID="lblModal" Text=""> </asp:Label></h3>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <%--   <asp:Button ID="btnCancel" runat="server" Text=""  OnClientClick="" />--%>
                </div>
            </div>

        </div>
    </div>
    
</asp:Content>

