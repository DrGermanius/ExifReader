<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExifReader.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ExifReader</title>

     <link rel="stylesheet" href="~/styles/style.css" type="text/css" runat="server"  />



</head>
<body>
    <form id="form1" runat="server">


        <asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="panel1" runat="server">
            <ContentTemplate>



                <div id="imageContainer" class="imageContainer" runat="server"></div>


                <div id="mainPanel">
                    <asp:Image ID="mainImage" runat="server" Height="300" Width="300" />

                    <ul>
                        <li>
                            <h1>Image Info</h1>
                            </li>
                        <li>
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="Label3" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="Label4" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="Label5" runat="server"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="Label6" runat="server"></asp:Label>
                        </li>
                    </ul>



                    <div enableviewstate="true" id="map" class="map"></div>


                </div>

                <div class="descriptionPanel">
                    <asp:Label ID="DescriptionLabel" runat="server" Font-Bold="True" Font-Size="X-Large" Text="User description"></asp:Label>

                    <asp:TextBox ID="DescriptionContainer" runat="server" Enabled="False" Height="53px" TextMode="MultiLine" Width="257px"></asp:TextBox>

                    <asp:Button ID="DescriptionButton" runat="server" OnClick="DescriptionButton_Click" Text="Edit" Height="58px" Width="74px" />

                </div>



            </ContentTemplate>
        </asp:UpdatePanel>




        <footer>
            <asp:FileUpload ID="FileUpload1" runat="server" />

            <asp:Button ID="Button2"
                runat="server"
                Text="Upload"
                Type="Button"
                UseSubmitBehavior="False"
                Font-Bold="true"
                ForeColor="DodgerBlue"
                Height="22px"
                Width="104px" OnClick="Button2_Click" />

        </footer>
    </form>

                <script>
                    function initMap(posLat, posLng) {
                        var latit = parseFloat(posLat);
                        var longit = parseFloat(posLng);
                        var position = { lat: latit, lng: longit };
                        var map = new google.maps.Map(
                            document.getElementById('map'), { zoom: 10, center: position });
                        var marker = new google.maps.Marker({ position: position, map: map });
                    }
    </script>

    <script async="async" defer="defer"
        src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBtHESjO4XIzxKID7xR2ij3ymvZz400rGI&callback=initMap">
    </script>


</body>
</html>
