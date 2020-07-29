<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Layout.Master" AutoEventWireup="true" CodeBehind="Schedules.aspx.cs" Inherits="iSchedule.Views.Entries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="page-content-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-8 col-lg-offset-3">
                    <h1>View Schedules</h1>
                    <p>Displays all schedules</p>
                    <br />
                    <div class="row">
                        <div class="col-lg-8">
                            <asp:RadioButtonList runat="server" ID="radEvents" RepeatDirection="Horizontal" CssClass="marginRadio" AutoPostBack="true" OnSelectedIndexChanged="radEvents_SelectedIndexChanged">
                                <asp:ListItem Text="Show Past Events" Value="True" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Show Upcoming Events" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-4">
                            <asp:Button CssClass="btn btn-danger" runat="server" ID="Purge"
                                Text="Purge" OnClientClick="$('#divConfirmAll').modal('show'); return false;" />
                            <asp:Button CssClass="btn btn-default" runat="server" ID="ExportToCSV"
                                Text="Export To CSV using comma" OnClick="ExportToCsv_click" />
                        </div>
                    </div>
                    
                    <div class="row" runat="server" id="PurgeSel">
                        <div class="col-lg-offset-4 col-lg-2">
                            <%--<asp:Button CssClass="btn btn-danger" runat="server" ID="PurgeSelec"
                                Text="Purge Selected Entries" OnClientClick="$('#divConfirm').modal('show');return false;" />--%>
                        </div>
                    </div>

                    <div class="row" runat="server" id="ExportDiv">
                        <div class="col-lg-offset-4 col-lg-2">
                            <%--<asp:Button CssClass="btn btn-default" runat="server" ID="ExportToCSV"
                                Text="Export To CSV using comma" OnClick="ExportToCsv_click" />--%>
                        </div>

                    </div>

                    <div class="row" runat="server" id="PurgeDiv">
                        <div class="col-lg-offset-4 col-lg-2">
                            <%--<asp:Button CssClass="btn btn-danger" runat="server" ID="Purge"
                                Text="Purge" OnClientClick="$('#divConfirmAll').modal('show'); return false;" />--%>
                        </div>
                    </div>
                </div>
            </div>
            
            <p>&nbsp;</p>

            <div>
                <div id="IsValidDiv" runat="server" class="row">
                    <div class="col-lg-offset-5 col-lg-2">
      
                        <asp:DropDownList CssClass="form-control" ID="ddlValidity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlValidity_SelectedIndexChanged">
                            <asp:ListItem Text="Select All" Value="Select All" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Valid" Value="Valid"></asp:ListItem>
                            <asp:ListItem Text="InValid" Value="Invalid"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div><br />
            <div>
                <div id="Div1" runat="server" class="row">
                    <div class="col-lg-offset-5 col-lg-2">
      
                        <asp:DropDownList CssClass="form-control" ID="ddlIsSent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsSent_SelectedIndexChanged">
                            <asp:ListItem Text="Select All" Value="Select All" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Sent" Value="True"></asp:ListItem>
                            <asp:ListItem Text="Not Sent" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div><br />
            <div class="row" runat="server" id="LoadedDiv">
                <div class="col-lg-12" style="overflow: auto;">
                    <div class="table-responsive">
                        <asp:GridView ID="EntriesGV" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                            AutoGenerateColumns="false"
                            EmptyDataText="There are no data records to display.">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="Tick" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SchedulesId" HeaderText="SchedulesId" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" />
                                <%--     <asp:TemplateField HeaderText="Convert To Winner">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Pick Entry" CssClass="btn btn-default" ID="ConvertWinner" OnClick="ConvertWinner_Click" Visible='<%# (Convert.ToBoolean(Eval("IsValid")) && Convert.ToBoolean(Eval("ExcludePastWinner"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Created On">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="CreatedOn" Text='<%# (Convert.ToDateTime(Eval("CreatedOn"))).ToString("dd MMM yyyy HH:mm:ss") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />                                
                                <asp:BoundField DataField="IsValid" HeaderText="Is Valid" />
                                <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                <asp:TemplateField HeaderText="Event Date">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="EventDate" Text='<%# (Convert.ToDateTime(Eval("EventDate"))).ToString("dd MMM yyyy HH:mm:ss") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Custom1" HeaderText="Custom 1" />
                                <asp:BoundField DataField="Custom2" HeaderText="Custom 2" />
                                <asp:BoundField DataField="Custom3" HeaderText="Custom 3" />
                                <asp:BoundField DataField="IsSent" HeaderText="Is Sent" />
                                <asp:TemplateField HeaderText="Sent On">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="SentOn" Text='<%# Eval("SentOn")==DBNull.Value ?"": (Convert.ToDateTime(Eval("SentOn"))).ToString("dd MMM yyyy HH:mm:ss") %>'></asp:Label>
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

            <p>&nbsp;</p>
            <div class="row" runat="server" id="PagingDiv">
                <div class="col-lg-12">
                    <div style="text-align: center">
                        <asp:Button CssClass="btn btn-default" runat="server" ID="FirstPage"
                            Text="First Page" OnClick="FirstPage_Click" />

                        <asp:Button CssClass="btn btn-default" runat="server" ID="PreviousPage"
                            Text="<" OnClick="PreviousPage_Click" />

                        <asp:TextBox runat="server" ID="CurrentPage" Style="width: 4%" TextMode="Number" Text="1"></asp:TextBox>

                        <span class="label label-default">/
                            <asp:Label runat="server" ID="lblTotalPages"></asp:Label></span>

                        <asp:Button CssClass="btn btn-default" runat="server" ID="Go"
                            Text="GO" />

                        <asp:Button CssClass="btn btn-default" runat="server" ID="NextPage"
                            Text=">" OnClick="NextPage_Click" />

                        <asp:Button CssClass="btn btn-default" runat="server" ID="LastPage"
                            Text="Last Page" OnClick="LastPage_Click" />

                        <span class="label label-default">No Of Total Records :
                            <asp:Label runat="server" ID="lblTotal"></asp:Label></span>
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
    <div id="divPopUpNoCancel" class="modal fade" aria-hidden="false" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <%--    <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title"></h4>
                            </div>--%>
                <div class="modal-body">
                    <h3>
                        <asp:Label runat="server" ID="lblModalNoCancel" Text=""> </asp:Label></h3>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <%--   <asp:Button ID="btnCancel" runat="server" Text=""  OnClientClick="" />--%>
                </div>
            </div>

        </div>
    </div>
    <!-- Modal -->
    <div id="divConfirm" class="modal fade" aria-hidden="false" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <%--    <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title"></h4>
                            </div>--%>
                <div class="modal-body">
                    <h3>
                        <asp:Label runat="server" ID="lblConfirm" Text="Are you sure to delete schedule(s)?"> </asp:Label></h3>
                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-danger" runat="server" ID="PurgeSelected"
                        Text="Confirm" OnClick="PurgeSelected_Click" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <%--   <asp:Button ID="btnCancel" runat="server" Text=""  OnClientClick="" />--%>
                </div>
            </div>

        </div>
    </div>

    <div id="divConfirmAll" class="modal fade" aria-hidden="false" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <%--    <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title"></h4>
                            </div>--%>
                <div class="modal-body">
                    <h3>
                        <asp:Label runat="server" ID="lblConfirmAll" Text="Are you sure to delete schedule(s)?"> </asp:Label></h3>
                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-danger" runat="server" ID="PurgeAll"
                        Text="Confirm" OnClick="Purge_Click" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Cancel</button>
                    <%--   <asp:Button ID="btnCancel" runat="server" Text=""  OnClientClick="" />--%>
                </div>
            </div>

        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FootHolder" runat="server">
</asp:Content>

