<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Layout.Master" AutoEventWireup="true" CodeBehind="AdminSettings.aspx.cs" Inherits="iSchedule.Views.AdminSettingsView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="page-content-wrapper">
        <div class="container">
            <div class="row">
                <div class="col-lg-4">
                    <h1>Setting</h1><br />
                </div>
            </div>
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#divAddPopUp">
              Add
            </button>
            <br /><br /><br />
            <asp:HiddenField runat="server" ID="hdnEntryID" />
            <div class="row" runat="server" id="LoadedDiv">
                <div class="col-lg-12" style="overflow: auto;">
                    <div class="table-responsive">
                        <asp:GridView ID="UsersGV" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                            AutoGenerateColumns="false"
                            EmptyDataText="There are no data records to display.">
                            <Columns>
                                <asp:BoundField DataField="SettingsId" HeaderText="SettingsId" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                <%--     <asp:TemplateField HeaderText="Convert To Winner">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Pick Entry" CssClass="btn btn-default" ID="ConvertWinner" OnClick="ConvertWinner_Click" Visible='<%# (Convert.ToBoolean(Eval("IsValid")) && Convert.ToBoolean(Eval("ExcludePastWinner"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="AppId" HeaderText="App Id" />
                                <asp:BoundField DataField="AppSecret" HeaderText="App Secret" />
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Delete" CssClass="btn btn-default" ID="Delete"
                                            OnClick="Delete_Click" OnClientClick="return confirm('Are you sure you want to proceed?');"
                                             />
                                    </ItemTemplate>
                                </asp:TemplateField>
                              <%--  <asp:TemplateField HeaderText="File Link">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="FileLink" Target="_blank" NavigateUrl='<%# Eval("FileLink") %>' Text='<%# (Eval("FileLink")).ToString() == "" ? "" : "View"  %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                               <%-- <asp:TemplateField HeaderText="Verified">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Verify" CssClass="btn btn-default" ID="Verified"
                                            OnClick="Verified_Click" Visible='<%# (Eval("FileLink").ToString() != ""  && Convert.ToBoolean(Eval("IsValid")) == true  && Convert.ToBoolean(Eval("IsVerified")) == false) %>' />
                                        <asp:Label Visible='<%# Convert.ToBoolean(Eval("IsVerified")) == true %>' Text="Receipt Verfied" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rejected">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Reject" CssClass="btn btn-default" ID="InEligible"
                                            OnClick="InEligible_Click" Visible='<%# (Eval("FileLink").ToString() != "" && Convert.ToBoolean(Eval("IsValid")) == true && Convert.ToBoolean(Eval("IsVerified")) == false) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Resend">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Resend" CssClass="btn btn-default" ID="Resend"
                                            OnClick="Resend_Click" Visible='<%# (Eval("FileLink").ToString() != "" && Convert.ToBoolean(Eval("IsValid")) == true && Convert.ToBoolean(Eval("IsVerified")) == false) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
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

    <div id="divAddPopUp" class="modal fade" aria-hidden="false" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <%--    <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title"></h4>
                            </div>--%>
                <div class="modal-body">
                    <h5 style="font-weight: bold">App Id *</h5>
                    <asp:TextBox runat="server" ID="txtAppId" CssClass="form-control"></asp:TextBox>
                    <p>
                        &nbsp;               
                    </p>
                    <h5 style="font-weight: bold">App Secret *</h5>
                    <asp:TextBox runat="server" ID="txtAppSecret" CssClass="form-control"></asp:TextBox>
                    <p>
                        &nbsp;               
                    </p>
                </div>
                <div class="modal-footer">   
                    <asp:button CssClass="btn btn-primary" runat="server" Text="Add" onclick="Add_Click"></asp:button>
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <%--   <asp:Button ID="btnCancel" runat="server" Text=""  OnClientClick="" />--%>
                </div>
            </div>

        </div>
    </div>

    <%--<div id="divUserPopUp" class="modal fade" aria-hidden="false" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                    <%--<div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h3 class="modal-title"></h3>
                            </div>
                <div class="modal-body">
                    <h4 style="font-weight: bold">Update User</h4>
                    <h5>App Id: </h5>
                        <asp:TextBox runat="server" ID="txtAppId" CssClass="form-control" Text=""></asp:TextBox>
                    <h5>App Secret: </h5>
                        <asp:TextBox runat="server" ID="txtAppSecret" CssClass="form-control" Text=""></asp:TextBox>
                    <br />  
                    <asp:Button runat="server" Text="Update" CssClass="btn btn-primary" ID="Update"
                                            OnClick="Update_Click" OnClientClick="return confirm('Are you sure you want to proceed?');"
                                             />              
                    <h5>
                            <asp:Label runat="server" ID="lblError" CssClass="alert-warning" Text=""> </asp:Label>
                            <asp:Label runat="server" ID="lblSuccess" CssClass="alert-success" Text=""> </asp:Label>
                        </h5>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    <%--   <asp:Button ID="btnCancel" runat="server" Text=""  OnClientClick="" />
                </div>
            </div>

        </div>
    </div>--%>
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

