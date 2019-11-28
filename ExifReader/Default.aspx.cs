using ExifLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExifReader
{
    public partial class Default : System.Web.UI.Page
    {
        const string existingFormat = ".jpg";

        protected void Page_Load(object sender, EventArgs e)
        {

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            ShowImages();


        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string extension = System.IO.Path.GetExtension(FileUpload1.FileName);


                if (extension == ".png" || extension == ".PNG")
                {
                    string path = Server.MapPath("Images\\");
                    FileUpload1.SaveAs(path + Path.ChangeExtension(FileUpload1.FileName, "jpg"));
                    Label1.Text = "Saved";


                    ShowImages();
                    Response.Redirect(Request.RawUrl);

                }

                if (extension == ".jpg" || extension == ".JPG")
                {

                    string path = Server.MapPath("Images\\");
                    FileUpload1.SaveAs(path + FileUpload1.FileName);

                    ShowImages();
                    Response.Redirect(Request.RawUrl);



                }

            }

        }


        protected void OnImageClick(object sender, EventArgs e)
        {
            ImageButton imageButton = sender as ImageButton;
            string imageUrl = imageButton.ImageUrl;
            ClearFields();
            GetExif(Server.MapPath(imageUrl));
            mainImage.ImageUrl = ConvertPath(imageUrl); // create correct path for mainImage and set image

            var desc = GetDescriptionsFromJson();
            if (desc.TryGetValue(mainImage.ImageUrl, out string result))
            {
                DescriptionContainer.Text = result;
            }


        }

        private void ShowMap(String image)
        {
            try
            {
                var reader = new ExifLib.ExifReader(image);
                reader.GetTagValue(ExifTags.GPSLatitude, out double[] latit);
                reader.GetTagValue(ExifTags.GPSLongitude, out double[] longit);

                double latitreal = latit[0] + latit[1] / 60f + latit[2] / 3600f;
                double longitreal = longit[0] + longit[1] / 60f + longit[2] / 3600f;

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "onload",
                    "initMap(" + latitreal.ToString() + "," + longitreal.ToString() + ");", true);
            }
            catch (System.NullReferenceException)
            {

            }

        }

        private void ClearFields()
        {
            DescriptionContainer.Text = String.Empty;
            Label1.Text = String.Empty;
            Label2.Text = String.Empty;
            Label3.Text = String.Empty;
            Label4.Text = String.Empty;
            Label5.Text = String.Empty;
            Label6.Text = String.Empty;
        }

        private string ConvertPath(string path)
        {
            string RelativePath = path.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
            return RelativePath;
        }

        private string EncodeCompression(int copmression)
        {
            switch (copmression)
            {
                case 1: return "Uncompressed";
                case 2: return "CCITT 1D";
                case 3: return "T4/Group 3 Fax";
                case 4: return "T6/Group 4 Fax";
                case 5: return "LZW";
                case 6: return "JPEG (old-style)";
                case 7: return "JPEG";
                case 8: return "Adobe Deflate";
                case 9: return "JBIG B&W";
                case 10: return "JBIG Color";
                case 99: return "JPEG";
                case 262: return "Kodak 262";
                case 32766: return "Next";
                case 32767: return "Sony ARW Compressed";
                case 32769: return "Packed RAW";
                case 32770: return "Samsung SRW Compressed";
                case 32771: return "CCIRLEW";
                case 32772: return "Samsung SRW Compressed 2";
                case 32773: return "PackBits";
                case 32809: return "Thunderscan";
                case 32867: return "Kodak KDC Compressed";
                case 32895: return "IT8CTPAD";
                case 32896: return "IT8LW";
                case 32897: return "IT8MP";
                case 32898: return "IT8BL";
                case 32908: return "PixarFilm";
                case 32909: return "PixarLog";
                case 32946: return "Deflate";
                case 32947: return "DCS";
                case 33003: return "Aperio JPEG 2000 YCbCr";
                case 33005: return "Aperio JPEG 2000 RGB";
                case 34661: return "JBIG";
                case 34676: return "SGILog";
                case 34677: return "SGILog24";
                case 34712: return "JPEG 2000";
                case 34713: return "Nikon NEF Compressed";
                case 34715: return "JBIG2 TIFF FX";
                case 34718: return "Microsoft Document Imaging (MDI) Binary Level Codec";
                case 34719: return "Microsoft Document Imaging (MDI) Progressive Transform Codec";
                case 34720: return "Microsoft Document Imaging (MDI) Vector";
                case 34887: return "ESRI Lerc";
                case 34892: return "Lossy JPEG";
                case 34925: return "LZMA2";
                case 34927: return "WebP";
                case 34933: return "PNG";
                case 34934: return "JPEG XR";
                case 65000: return "Kodak DCR Compressed";
                case 65535: return "Pentax PEF Compressed";
                default: return "";
            }
        }

        private List<string> GetImages()
        {
            string path = Server.MapPath("~/images/");
            var files = Directory.GetFiles(path);

            List<string> imagesList = new List<string>();
            foreach (string file in files)
            {
                if (Regex.IsMatch(file, @".jpg"))
                    imagesList.Add(file);
            }

            imagesList.Sort();
            return imagesList;
        }

        private void ShowImages()
        {
            List<string> imagesList = GetImages();
            imageContainer.Controls.Clear();
            foreach (string s in imagesList)
            {
                ImageButton img = new ImageButton();
                img.ImageUrl = ConvertPath(s);
                img.Click += new ImageClickEventHandler(OnImageClick);

                imageContainer.Controls.Add(img);
            }
        }

        private void GetExif(string image)
        {

            try
            {
                var reader = new ExifLib.ExifReader(image);


                reader.GetTagValue(ExifTags.ExifVersion, out byte[] version);
                reader.GetTagValue(ExifTags.DateTimeOriginal, out DateTime date);
                reader.GetTagValue(ExifTags.Model, out string model);
                reader.GetTagValue(ExifTags.Make, out string maker);
                reader.GetTagValue(ExifTags.ExposureTime, out double exptime);
                reader.GetTagValue(ExifTags.Compression, out UInt16 compression);



                Label1.Text = String.Concat("ExposureTime: ", exptime.ToString());
                Label2.Text = "ExifVersion: "; if (version != null) Label2.Text += System.Text.Encoding.UTF8.GetString(version);
                Label3.Text = String.Concat("Data: ", String.Format("{0:dddd, MMMM d, yyyy}", date));
                Label4.Text = String.Concat("Model: ", model);
                Label5.Text = String.Concat("Maker: ", maker);
                Label6.Text = String.Concat("Compression:", EncodeCompression(compression));
                ShowMap(image);
            }
            catch (ExifLib.ExifLibException)
            {
                Label1.Text = "No EXIF info";
            }


        }

        private Dictionary<string, string> GetDescriptionsFromJson()
        {
            var desc = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Server.MapPath("/images/comments.json")));
            return desc;
        }

        private void SaveDescriptions(Dictionary<string, string> desc)
        {
            File.WriteAllText(Server.MapPath("/images/comments.json"), JsonConvert.SerializeObject(desc));

        }


        protected void DescriptionButton_Click(object sender, EventArgs e)
        {
            if (DescriptionContainer.Enabled)
            {

                var descriptions = GetDescriptionsFromJson();
                if (descriptions.TryGetValue(mainImage.ImageUrl, out string result))
                {
                    descriptions.Remove(mainImage.ImageUrl);
                    descriptions.Add(mainImage.ImageUrl, DescriptionContainer.Text);
                }
                else
                {
                    descriptions.Add(mainImage.ImageUrl, DescriptionContainer.Text);
                }

                SaveDescriptions(descriptions);


                DescriptionContainer.Enabled = false;
                DescriptionButton.Text = "EDIT";
                GetExif(Server.MapPath(mainImage.ImageUrl));

            }
            else
            {
                DescriptionContainer.Enabled = true;
                DescriptionButton.Text = "SAVE";
                GetExif(Server.MapPath(mainImage.ImageUrl));
            }

        }
    }
}
