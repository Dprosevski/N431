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

            <form id="form1" runat="server">

                <div id="registration_info">
                    <h1>Search Programs</h1>
                    <br />
                    <asp:Label ID="lblMessage" Height="40px" Font-Size="18px" ForeColor="Red" runat="server" />
                    <h2>Please enter the information below:</h2>
                </div>


                <!-- Search by name-->
                <asp:Label ID="lblName" runat="server">Name Contains:</asp:Label>
                <asp:TextBox ID="name" MaxLength="50" Width="300" Height="20" Text="" runat="server" Font-Size="12"></asp:TextBox>
                <br /><hr />


                <!-- Search by field -->
                <asp:Label runat="server" Text="Search by field's name" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="fieldList"  AppendDataBoundItems="False" AutoPostBack="true" 
                    runat="server"></asp:DropDownList>
                <br /><hr />


                <!-- Search by eligible grade -->
                <asp:Label runat="server" Text="Search byn eligible grade" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="gradeList"  AppendDataBoundItems="False" AutoPostBack="true" 
                    runat="server"></asp:DropDownList>
                <br /><hr />


                <!-- Search by residential -->
                <asp:Label runat="server" Text="Search by residential" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="residentialList"  AppendDataBoundItems="False" 
                    AutoPostBack="true" runat="server"></asp:DropDownList>
                <br />
                <hr />


                <!-- Search by cost -->
                <asp:Label runat="server" Text="Search by cost" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="costList"  AppendDataBoundItems="False" 
                    AutoPostBack="true" runat="server"></asp:DropDownList>
                <br />
                <hr />


                <!-- Search by stipend -->
                <asp:Label runat="server" Text="Search by stipend" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="stipendList"  AppendDataBoundItems="False" 
                    AutoPostBack="true" runat="server"></asp:DropDownList>
                <br />
                <hr />


                <!-- Search by duration -->
                <asp:Label runat="server" Text="Search by duration" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="durationList"  AppendDataBoundItems="False" 
                    AutoPostBack="true" runat="server"></asp:DropDownList>
                <br />
                <hr />


                <!-- Search by season -->
                <asp:Label runat="server" Text="Search by season" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="seasonList"  AppendDataBoundItems="False" 
                    AutoPostBack="true" runat="server"></asp:DropDownList>
                <br />
                <hr />


                 <!-- Search by service area -->
                <asp:Label runat="server" Text="Search by service area" ForeColor="Black" Font-Size="10pt"></asp:Label><br />
                <asp:DropDownList ID="areaList"  AppendDataBoundItems="False" 
                    AutoPostBack="true" runat="server"></asp:DropDownList>
                <br />
                <hr />


                <!-- Search by start date -->
                <asp:Label ID="lblStartDate" runat="server">Search by Program's Start Date:</asp:Label>
                <asp:TextBox ID="startDate" TextMode="Date" runat="server" Font-Size="12"></asp:TextBox>                


                <!-- Search by application dead line -->
                <asp:Label ID="lblAppDeadline" runat="server">Search by Application Deadline:</asp:Label>
                <asp:TextBox ID="appDeadline" TextMode="Date" runat="server" Font-Size="12"></asp:TextBox><hr />


                <!-- Search by University Affiliation -->
                <asp:Label ID="lblAffiliation" runat="server">Search by University Affiliation:</asp:Label>
                <asp:TextBox ID="affiliation" runat="server" Font-Size="12"></asp:TextBox><hr />


                <!-- Search by Restrictions  -->
                <asp:Label ID="lblRestrictions" runat="server">Search by Restrictions :</asp:Label>
                <asp:TextBox ID="restrictions" runat="server" Font-Size="12"></asp:TextBox><hr />


                <!-- Search by State  -->
                <asp:Label ID="lblState" runat="server">Search by State :</asp:Label>
                <asp:DropDownList ID="stateList"  AppendDataBoundItems="False" AutoPostBack="true" runat="server" OnSelectedIndexChanged="stateList_SelectedIndexChanged"></asp:DropDownList><hr />


                <!-- Search by County -->
                <asp:Label ID="lblCounty" runat="server">Search by County:</asp:Label>
                <asp:DropDownList ID="countyList"  AppendDataBoundItems="False" AutoPostBack="true" runat="server" OnSelectedIndexChanged="countyList_SelectedIndexChanged"></asp:DropDownList><hr />


                <!-- Search by City -->
                <asp:Label ID="lblCity" runat="server">Search by City:</asp:Label>
                <asp:DropDownList ID="cityList"  AppendDataBoundItems="False" AutoPostBack="true" runat="server"></asp:DropDownList><hr />


                <!--Button for Submitting the Form-->
                <asp:Button CssClass="btn btn-primary" ID="btnSubmit" runat="server" Text="Search Programs" UseSubmitBehavior="true" OnClick="BtnSubmit_Click" />

            </form>
            
            <div class="clearthis">&nbsp;</div>
        </div>
    </div>




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
