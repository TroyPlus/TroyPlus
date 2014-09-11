using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;


namespace Troy.Utilities.CrossCutting
{
    public class ModeltoSAPXmlConvertor
    {
        public static string ConvertModelToXMLString(Object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream aStream = new MemoryStream();
            StreamReader streamReader = null;
            string xmlOutput = "";
            try
            {
                serializer.Serialize(aStream, obj);
                aStream.Position = 0;
                streamReader = new StreamReader(aStream);
                xmlOutput = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
            }
            return xmlOutput;
        }
    }
}