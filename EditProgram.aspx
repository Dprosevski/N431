<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProgram.aspx.cs" Inherits="Capstone2nd.EditProgram" %>
<%@ Register TagPrefix="general" TagName="Nav" Src="Navigation.ascx" %>

<!DOCTYPE html>
<html lang="en">

<head runat="server">

  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <meta name="description" content="">
  <meta name="author" content="">

  <title>Edit Program</title>

  
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

<body id="page-top">

    
	    <!-- Navigation -->
    <general:Nav ID="naviation" runat="server" />

    
		<!-- Header -->
    <header class="masthead">
        <div class="container d-flex h-100 align-items-center">
            <div class="mx-auto text-center">
            </div>
        </div>
    </header>

    <form id="form1" runat="server">
            
        <!--Error Message-->
        <asp:Label id="lblMessage" Height="40px" Font-Size="18px" ForeColor="Red" runat="server" /><br />
        
        <asp:Label ID="ProgDescription" runat="server" Text="Choose Which Program You Wish To Edit:" ForeColor="Black" Font-Size="14pt"></asp:Label><br />
        <asp:DropDownList ID="ProgList" runat="server">
        </asp:DropDownList><br />

        <!--Button for Submitting the Form-->
        <asp:Label ID="lblSubmit" runat="server" ForeColor="Blue" Font-Size="18pt"></asp:Label><br />
        <asp:Button ID="BtnSubmit" runat="server" Text="Submit" OnClick="BtnSubmit_Click"/>
</form>

    
		<!-- Footer -->
    <footer class="bg-black small text-center text-white-50">
        <div class="container">
            Copyright &copy; Your Website 2019
        </div>
    </footer>

    
		<!-- Bootstrap core JavaScript -->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    
		<!-- Plugin JavaScript -->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    
		<!-- Custom scripts for this template -->
    <script src="js/grayscale.min.js"></script>
</body>
</html>
