<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Layout.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="iSchedule.Views.SettingsView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="page-content-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div id="settingDiv" runat="server">
                <div class="col-lg-8 col-lg-offset-3">
                    <h1>Settings</h1>
                    <div class="row">
                    </div>
                    
                    <div class="row">
                        <div class="col-lg-4">
                            <asp:TextBox ID="sendTime" runat="server" CssClass="form-control timepicker" placeholder="Time to Send"></asp:TextBox>
                        </div>
                        <div class="col-lg-8">&nbsp;
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-4">
                            <asp:TextBox ID="areaMsgTemplate" CssClass="form-control" textmode="MultiLine" Columns="60" Rows="8" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-8">&nbsp;
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-4">
                            <asp:Label Text="Note: You can use {custom1},{custom2},{custom3} to personalize your message if you have uploaded personalised columns in your list." 
                                runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-8">&nbsp;
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-4">
                            <asp:Button runat="server" ID="Save"
                                CssClass="btn btn-primary pull-right" Text="Save" OnClick="Save_Click" />
                        </div>
                        <div class="col-lg-8">&nbsp;
                        </div>
                    </div>
                </div>
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
    
    <!-- Modal -->
    <%-- <div id="divConfirm" class="modal fade" aria-hidden="false" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-body">
                    <h3>
                        <asp:Label runat="server" ID="lblConfirm" Text="Are you sure to delete entry(s)?"> </asp:Label></h3>
                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-danger" runat="server" ID="PurgeSelected"
                        Text="Confirm" OnClick="PurgeSelected_Click" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                </div>
            </div>

        </div>
    </div>--%>

    <%--  <div id="divConfirmAll" class="modal fade" aria-hidden="false" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
              
                <div class="modal-body">
                    <h3>
                        <asp:Label runat="server" ID="lblConfirmAll" Text="Are you sure to delete entry(s)?"> </asp:Label></h3>
                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-danger" runat="server" ID="PurgeAll"
                        Text="Confirm" OnClick="Purge_Click" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                </div>
            </div>

        </div>
    </div>--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FootHolder" runat="server">
    <script>
        $(document).ready(function () {
            
        });
    </script>
</asp:Content>

