<%@ Page Language="C#" UnobtrusiveValidationMode="None" AutoEventWireup="true" CodeBehind="CRUDTable.aspx.cs" Inherits="WebApplicationCRUD.CRUDTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="~/css/foundation.min.css"/>
    <title></title>
    <script type="text/javascript">
        function grade() {
            var nilai = document.getElementById("TxtNilai").value;

            if (nilai <= 40) {
                document.getElementById("TxtGrade").value = "E";
            } else if (nilai > 40 && nilai <= 55) {
                document.getElementById("TxtGrade").value = "D";
            } else if (nilai > 55 && nilai <= 60) {
                document.getElementById("TxtGrade").value = "C";
            } else if (nilai > 60 && nilai < 75) {
                document.getElementById("TxtGrade").value = "B";
            } else {
                document.getElementById("TxtGrade").value = "A";
            }
        }
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <div>
        
        <h2>CRUD SEDERHANA</h2>
            
        <label>ID:</label>
        <asp:TextBox ID="TxtID" runat="server" Enabled="False"></asp:TextBox><br />

        <label>Subject:</label>
        <asp:TextBox ID="TxtSubjectName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="TxtSubjectName" ErrorMessage="Subjek harus diisi" 
            ForeColor="#FF3300" ValidationGroup="crud" Display="Dynamic"></asp:RequiredFieldValidator><br />

        <label>Nilai:</label>
        <asp:TextBox ID="TxtNilai" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
            ControlToValidate="TxtNilai" ErrorMessage="Nilai harus diisi" 
            ForeColor="#FF3300" ValidationGroup="crud" Display="Dynamic"></asp:RequiredFieldValidator>
        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ControlToValidate="TxtNilai" ErrorMessage="Harus berupa angka" 
            ForeColor="#FF3300" ValidationExpression="^[0-9]*$" ValidationGroup="crud" Display="Dynamic"></asp:RegularExpressionValidator><br />

        <label>Grade:</label>
        <asp:TextBox ID="TxtGrade" runat="server"></asp:TextBox><br />

        <p>
            <asp:Button ID="btnSimpan" runat="server" onclick="btnSimpan_Click" 
                        ValidationGroup="crud" Text="Insert" class="button"/>
            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click"
                        Text="Update" ValidationGroup="crud" class="button" />
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click"                   
                        OnClientClick="return confirm('Hapus?')"
                        Text="Delete" ValidationGroup="crud" class="button"/> 
            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click"                   
                        Text="Batal" class="button"/> 
        </p>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="SubjectID" OnRowCommand="OnRowCommand" ShowHeaderWhenEmpty="True"                    
                AllowSorting="True" AllowPaging="True" onsorting="GridView1_Sorting" 
                PageSize="2" onpageindexchanging="GridView1_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="SubjectID" HeaderText="SubjectID" 
                        SortExpression="SubjectID" Visible="False" />                
                    <asp:BoundField DataField="SubjectName" HeaderText="Subject" SortExpression="SubjectName" />
                    <asp:BoundField DataField="Marks" HeaderText="Nilai" SortExpression="Marks" />
                    <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                    <%--
                    <asp:ButtonField Text="Pilih"  CommandArgument='<%# Eval("SubjectId") %>' CommandName="cmPilih" ItemStyle-Width="30" />
                    <asp:ButtonField Text="Hapus" CommandName="cmHapus" ItemStyle-Width="30" />
                    --%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("SubjectId") %>' runat="server" />
                            <asp:LinkButton ID="LinkButton1" CommandName="cmPilih" CommandArgument='<%# ((GridViewRow) Container).RowIndex +"-"+ Eval("SubjectId")%>' runat="server" Text="Pilih"  />  
                            <asp:LinkButton CommandName="cmHapus" OnClientClick= "return confirm('Hapus data?')"  CommandArgument='<%# Eval("SubjectId") %>' runat="server" Text="Hapus"  />  
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Label ID="LbData" runat="server" />
        
        </div>   
    </form>
</body>
</html>