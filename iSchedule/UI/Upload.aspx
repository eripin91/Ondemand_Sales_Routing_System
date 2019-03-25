<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Layout.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="iSchedule.Views.UploadView" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="page-content-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-8 col-lg-offset-3">
                    <h1>Upload Entries</h1>
                    <a href="~\Content\csv_template">Download Template</a><br /><br />
                    File format: CSV<br />
                    Columns: MobileNo | EventDate | Custom1 | Custom2 | Custom3 <br /><br />
                    MobileNo must have country code<br />
                    EventDate must be in DD MMM YYYY format i.e 05 May 2019<br />
                    Custom1,Custom2,Custom3 are optional columns<br /><br />
                    IMPORTANT: Current schedules will be purged and replaced<br /><br />
                    <asp:FileUpload ID="UploadF" runat="server" />
                    <h5 style="font-weight: bold" runat="server" id="FileSpecs" visible="false"></h5>
                    <div class="row">
                        <div class="col-lg-offset-4 col-lg-2">
                            <asp:Button runat="server" ID="Filter"
                                CssClass="btn btn-primary" Text="Submit" OnClick="Submit_Click" />
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


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FootHolder" runat="server">
     <script>
         $(document).ready(function () {
             jQuery('.datetimepicker').datetimepicker();
         });
    </script>
</asp:Content>