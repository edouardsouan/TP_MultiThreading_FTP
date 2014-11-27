<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
	<link rel="stylesheet" href="Content/css/style.css">
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
    		<input type="text" class="form-control col-md-3" id="localPath" placeholder="Ex: C:\Users\Guest\MesDocs">
    		<button class="btn btn-info">...</button>
  			</div>
		</div>
	</div>

	<!-- Import des Scripts-->
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/js/bootstrap.min.js"></script>
	<script src="//code.jquery.com/jquery-1.11.0.min.js"></script>
</body>

