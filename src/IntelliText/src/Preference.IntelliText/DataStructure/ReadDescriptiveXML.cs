/**
 * @file ReadDescriptiveXML.cs
 *
 * @brief This file contains the class ReadDescriptiveXML. Used to read the Descriptive XML
 *
 * @author Cesar Augusto Guzman Alvarez
 * Contact: cguzman@preference.es
 *
 */
namespace Preference.IntelliText.DataStructure.DescriptiveXML
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Xml;

    public class ReadDescriptiveXML
    {

        public XmlDocument XmlDoc { get; set; }

        public ReadDescriptiveXML(string xml)
        {
            // read the xml
            XmlDoc = new XmlDocument();
            XmlDoc.Load(xml);
        }

        public string ConvertToJSON(string jsonPath)
        {

            string xmlString = "";

            if (jsonPath != null)
            {
                xmlString = JsonConvert.SerializeXmlNode(XmlDoc);

                using StreamWriter writer = new(jsonPath);
                writer.WriteLine(xmlString);

            }

            return xmlString;

        }

    }
}
