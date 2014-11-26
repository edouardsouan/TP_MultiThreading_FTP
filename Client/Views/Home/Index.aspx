<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
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
    		<input type="text" class="form-control" id="localPath" placeholder="EX: C:\Users\Guest\MesDocs">
    		<asp:Button id="btnFileLoader"
    			Text="..."
    			Onclick="#"/>
  			</div>
		</div>
	</div>

	<!-- Import des Scripts-->
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js"></script>
</body>

