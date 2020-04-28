<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Capstone2nd.Login" %>
<%@ Register TagPrefix="general" TagName="Nav" Src="Navigation.ascx" %>
<%@ Register TagPrefix="general" TagName="Footer" Src="Footer.ascx" %>

<!DOCTYPE html>
<html lang="en">

<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Login Page</title>

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
    <general:nav id="naviation" runat="server" />

    <!-- Header -->
    <header class="masthead">
        <div class="container d-flex h-100 align-items-center">
            <div class="mx-auto text-center">
            </div>
        </div>
    </header>

    <div class="row">
        <div class="col-xl-2 col-lg-2">
        </div>

        <div class="col-xl-8 col-lg-8">
            <form id="form1" runat="server">

                <br />
                <!--Text Box for user Name-->
                <div class="input-group">
                    <div class="input-group-prepend">
                        <asp:Label ID="lbluser" CssClass="input-group-text" runat="server" Text="Username" ForeColor="Black" Font-Size="12pt"></asp:Label>
                        <asp:TextBox ID="userName" CssClass="form-control" runat="server" TextMode="SingleLine" BackColor="White" onfocus="this.select()"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqUsername" runat="server" ForeColor="Red" ErrorMessage="Username is required."
                            Display="Dynamic" ControlToValidate="userName">*Required</asp:RequiredFieldValidator>
                    </div>
                </div>


                <!--Text Box for Password-->
                <div class="input-group">
                    <div class="input-group-prepend">
                        <asp:Label ID="lblPassword" CssClass="input-group-text" runat="server" Text="Password" ForeColor="Black" Font-Size="12pt"></asp:Label>
                        <asp:TextBox ID="password" CssClass="form-control" TextMode="Password" BackColor="White" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqPass" runat="server" ErrorMessage="Password is required"
                            Display="dynamic" ControlToValidate="password">*Required</asp:RequiredFieldValidator>
                    </div>
                </div>

                <!--Link to forgot pwd page-->
                <asp:HyperLink ID="forgotID" runat="server" NavigateUrl="~/ForgetPwd.aspx" Visible="true">Forgot password?</asp:HyperLink><br />

                <!--Link to register page-->
                <asp:HyperLink ID="registerID" runat="server" NavigateUrl="~/Register.aspx" Visible="true">Don't have an account yet?</asp:HyperLink><br />

                <!--Button for Submitting the Form-->
                <asp:Label ID="lblSubmit" runat="server" ForeColor="Blue" Font-Size="18pt"></asp:Label>
                <asp:Button ID="BtnSubmit" CssClass="btn btn-primary" runat="server" Text="Submit" UseSubmitBehavior="true" OnClick="BtnSubmit_Click" />

                <!--Message-->
                <asp:Label ID="lblMessage" Height="40px" Font-Size="18px" ForeColor="Red" runat="server" />
            </form>
        </div>

        <div class="col-xl-2 col-lg-2">
        </div>

    </div>
    <!-- Footer -->
    <general:footer id="footer" runat="server" />

</body>

</html>


</body>

</html>