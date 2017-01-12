using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infolight.EasilyReportTools.DataCenter
{
    internal class BinarySerialize
    {
        #region Old Function
        //string strFile = @"c:\eRptTemp.dat";

        //public byte[] Serialize(object objClass)
        //{
        //    byte[] buffer = null;
        //    StreamWriter sw = null;
        //    //using (FileStream fs = new FileStream(strFile, FileMode.Create))
        //    using (MemoryStream fs = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(fs, objClass);
        //        sw = new StreamWriter(fs);
        //        //buffer = new byte[fs.Length];
        //        //fs.Write(buffer, 0, Convert.ToInt32(fs.Length));
        //        buffer = fs.ToArray();
        //    }
        //    return buffer;
        //}

        //public object FreeDeSerialize(byte[] buffer)
        //{
        //    object objClass = null;
        //    //using (FileStream fs = new FileStream(strFile, FileMode.Open))
        //    using (MemoryStream fs = new MemoryStream(buffer))
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        objClass = formatter.Deserialize(fs);
        //    }
        //    return objClass;
        //}
        #endregion

        public byte[] Serialize(object objClass)
        {
            byte[] buffer = null;
            buffer = XmlConverter.ConvertFrom(objClass);
            return buffer;
        }

        public object DeSerialize(byte[] buffer)
        {
            object objClass = null;
            objClass = XmlConverter.ConvertTo(buffer);
            return objClass;
        } 
    }
}
