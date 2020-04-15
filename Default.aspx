<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Capstone2nd.Default" %>
<%@ Register TagPrefix="general" TagName="Nav" Src="Navigation.ascx" %>
<%@ Register TagPrefix="general" TagName="Footer" Src="Footer.ascx" %>

<!DOCTYPE html>
<html lang="en">

<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Home Page</title>

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

    <!-- About Section -->
    <section id="about" class="about-section text-center">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 mx-auto">
                    <form action="Default.aspx">
                        <asp:Label ID="lblHello" runat="server" Text="Hello" Font-Size="30" ForeColor="White"></asp:Label>
                    </form>
                    <h2 class="text-white mb-4"></h2>
                </div>
            </div>
            <img src="img/ipad.png" class="img-fluid" alt="">
        </div>
    </section>














    <!-- Start of Main Content Area -->

    <div id="main_content">

        <!-- Start of Registration -->

        <div id="registration">

            <form id="Form2" runat="server">

                <div id="registration_info">
                    <h1>Search Programs</h1>
                    <br />
                    <asp:Label ID="lblMessage" Height="40px" Font-Size="18px" ForeColor="Red" runat="server" />
                    <h2>Please enter the information below:</h2>
                </div>



                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblName" runat="server">Name Contains:</asp:Label>
                        </td>
                        <td>
                                <asp:TextBox ID="name" MaxLength="50" Width="300" Height="20" Text="" runat="server" Font-Size="12"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <!--Button for Submitting the Form-->
                <hr />
                <asp:Button CssClass="btn btn-primary" ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true" OnClick="BtnSubmit_Click" />



            </form>

            <div class="clearthis">&nbsp;</div>
        </div>
    </div>
	<!-- End of Main Content Area -->
    <div class="clearthis">&nbsp;</div>

















    <!-- Login Section -->
    <section id="signup" class="signup-section">
        <div class="container">
            <div class="row">
                <div class="col-md-10 col-lg-8 mx-auto text-center">

                    <i class="far fa-paper-plane fa-2x mb-2 text-white"></i>
                    <h2 class="text-white mb-5">To create an account click on button bellow!</h2>
                    <a class="btn btn-primary mx-auto" href="Register.aspx">Register</a>

                </div>
            </div>
        </div>
    </section>

    <!-- Contact Section -->
    <section class="contact-section bg-black">
        <div class="container">

            <div class="row">

                <div class="col-md-4 mb-3 mb-md-0">
                    <div class="card py-4 h-100">
                        <div class="card-body text-center">
                            <i class="fas fa-map-marked-alt text-primary mb-2"></i>
                            <h4 class="text-uppercase m-0">Address</h4>
                            <hr class="my-4">
                            <div class="small text-black-50">4923 Market Street, Orlando FL</div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 mb-3 mb-md-0">
                    <div class="card py-4 h-100">
                        <div class="card-body text-center">
                            <i class="fas fa-envelope text-primary mb-2"></i>
                            <h4 class="text-uppercase m-0">Email</h4>
                            <hr class="my-4">
                            <div class="small text-black-50">
                                <a href="#">hello@yourdomain.com</a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 mb-3 mb-md-0">
                    <div class="card py-4 h-100">
                        <div class="card-body text-center">
                            <i class="fas fa-mobile-alt text-primary mb-2"></i>
                            <h4 class="text-uppercase m-0">Phone</h4>
                            <hr class="my-4">
                            <div class="small text-black-50">+1 (555) 902-8832</div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="social d-flex justify-content-center">
                <a href="#" class="mx-2">
                    <i class="fab fa-twitter"></i>
                </a>
                <a href="#" class="mx-2">
                    <i class="fab fa-facebook-f"></i>
                </a>
                <a href="#" class="mx-2">
                    <i class="fab fa-github"></i>
                </a>
            </div>

        </div>
    </section>

    <!-- Footer -->
    <general:footer id="footer" runat="server" />

</body>

</html>
