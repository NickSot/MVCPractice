<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RepoManager
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%:ViewData["Encr"]%></h2>

    <h3>
        Create directory
    </h3>
    <br />
    <form action="/RepoManager" method="post">
        Directory name: <input type="text" name="folderName" value="" />
    <br />
    <br />
        <input type="submit" name="" value="Submit" />
    </form>


    <h3>
        Upload file
    </h3>
    <br />
    <form action="/RepoManager" method="post" enctype="multipart/form-data">
        <input type="file" name="file" value=""/>
    <br />
    <br />
        <input type="submit" name="" value="Submit" />
    <br />
    <br />
    </form>

    <h3>
        Folders
    </h3>
    <br />

    <% 
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Model.folderPath);

        foreach(var directory in dir.GetDirectories()){%>
            <%:Html.ActionLink(directory.Name, "EnterDir", new {folderName = directory.Name, folderPath = directory.FullName}, null )%>
        <%}  
       
    %>

    <br />
    <%

    System.IO.FileInfo[] files = dir.GetFiles();
    
    %>   

    <h3>
        Download
    </h3>

    <% foreach (var file in files) { %>
        <%: Html.ActionLink(file.Name, "DownloadFile", "RepoManager", new { fileName = file.Name, filePath = file.FullName}, null)%>
    <br />
    <% }%>
    
    <h3>Encryption</h3>
        
    <% foreach (var file in files) { %>
        <%: Html.ActionLink(file.Name, "Encrypt", "RepoManager", new {fileName = file.Name}, null)%>
    <br />
    <% }%>
    <br />
    <h3>Decryption</h3>
    <br />
    <%    
    foreach (var file in files) { %>
        <%: Html.ActionLink(file.Name, "Decrypt", "RepoManager", new {fileName = file.Name}, null)%>
    <br />
    <%}%>

</asp:Content>
