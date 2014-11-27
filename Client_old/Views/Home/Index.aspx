<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
	<link rel="stylesheet" href="../../Content/css/style.css">
	<title><%: ViewData["Title"]%></title>
</head>
<body>
	<div class="container-fluid">
		<div class="header">
			<nav class="navbar">
				<h1 class="text-muted col-md-2 col-md-offset-5">AwesomeFTP</h1>
			</nav>
		</div>
		<div class="jumbotron">
			<div class="form-group">
	    		<label for="localPath">Local Path</label>
	    		<input type="file" class="form-control"/>
  			</div>
		</div>

		<% using (Html.BeginForm())
	       { %>
	        <label for="UserName">User Name:</label>
	        <br />
	        <%= Html.TextBox("UserName") %>
	    <%}%>
	</div>

	<!-- Import des Scripts-->
	<script src="http://code.jquery.com/jquery-1.11.0.min.js"></script>
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js"></script>
</body>

