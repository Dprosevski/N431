<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchResult.aspx.cs" Inherits="Capstone2nd.SearchResult" %>
<%@ Register TagPrefix="general" TagName="Nav" Src="Navigation.ascx" %>
<%@ Register TagPrefix="general" TagName="Footer" Src="Footer.ascx" %>


<!DOCTYPE html>
<html lang="en">

<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Searh Result Page</title>

    <!-- Bootstrap core CSS -->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom fonts for this template -->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Varela+Round" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet">

    <!-- Custom styles for this template -->
    <link href="css/grayscale.min.css" rel="stylesheet">
    <link href="css/myCss.css" rel="stylesheet">
</head>

<body>

    <!-- Navigation -->
    <general:nav id="naviation" runat="server" />

    <!-- Header -->
    <header class="masthead">
        <div class="container d-flex h-100 align-items-center">
            <div class="mx-auto text-center">
            </div>
        </div>
    </header>


	<div id="container">

		<!-- Start of Main Content Area -->

		<div id="main_panel">

			<!-- Start of Registration -->

			 <form id="Form1" runat="server">

				<!-- This panel is used to displays all the previous transactions -->
				 <asp:Panel ID="PnlTrans" CssClass="trans_panel"   runat="server">
			      
		            <!--Message-->
					<asp:Label ID="lblMessage" Height="40px" Font-Size="18px" ForeColor="Red" runat="server" />
					<br /><br />

				 </asp:Panel>
				<div class="clearthis">&nbsp;</div>
			</form>
			</div>

		<!-- End of Main Content Area -->
		<div class="clearthis">&nbsp;</div>


		<!-- Footer -->
		<general:Footer ID="footer" runat="server" />


	</div>

</body>
</html>
