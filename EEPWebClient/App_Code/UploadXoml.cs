using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for UploadXoml
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class UploadXoml : System.Web.Services.WebService
{

    public UploadXoml()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 

        
    }

    [WebMethod]
    public string Upload(string jpgName, byte[] b, string xmlName, string xml)
    {
        try
        {
            string path = Server.MapPath("");

            jpgName = string.Format(@"{0}\{1}", path, jpgName);
            xmlName = string.Format(@"{0}\{1}", path, xmlName);

            MemoryStream stream = new MemoryStream(b);
            Bitmap image = new Bitmap(stream);
            image.Save(jpgName, ImageFormat.Jpeg);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(xmlName);

            return string.Empty;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}

