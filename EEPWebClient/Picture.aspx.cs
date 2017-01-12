using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

public partial class Picture : System.Web.UI.Page
{
    private Bitmap validateimage;
    private Graphics g;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string VNum = MakeValidateCode();
            Session["VNum"] = VNum;
            ValidateCode(VNum);
        }
    }

    public void ValidateCode(string VNum)
    {
        validateimage = new Bitmap(70, 20, PixelFormat.Format24bppRgb);
        g = Graphics.FromImage(validateimage);

        g.DrawString(VNum, new Font("Verdana", 15), new SolidBrush(Color.White), new PointF(8, 0));
        g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(110, 20), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 60, 40)), 0, 0, 120, 30);
        g.Save();
        MemoryStream ms = new MemoryStream();
        validateimage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        Response.ClearContent();
        Response.ContentType = "image/gif";
        Response.BinaryWrite(ms.ToArray());
        Response.End();
    }

    string MakeValidateCode()
    {
        char[] s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        string num = "";
        Random r = new Random();
        for (int i = 0; i < 5; i++)
        {
            num += s[r.Next(0, s.Length)].ToString();
        }
        return num;
    }
}
