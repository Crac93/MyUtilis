using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConfigFilesLib
{
    /// <summary>
    /// Configurate Xml 
    /// </summary>
    public class ConfigXML
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="AllKeys"></param>
        /// <param name="Config"></param>
        /// <returns></returns>

        public static int GetKeys(string Section, out string[] AllKeys, string Config)
        {
            AllKeys = null;
            try

            {

                var configFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Config
                };

                var appConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                var sectionProduct = appConfig.GetSection(Section);
                var myParamsSectionRawXml = sectionProduct.SectionInformation.GetRawXml();
                var sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
                var sectionHandler = new NameValueSectionHandler();

                var sectionHandlerCollection = sectionHandler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
                AllKeys = sectionHandlerCollection.AllKeys;

                if (AllKeys == null)
                    return -1;

                return 0;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int GetValue(string section, string key, out string value, string config)
        {
            value = null;

            try
            {
                var configFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = config
                };
                var appConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                var sectionProduct = appConfig.GetSection(section);
                var myParamsSectionRawXml = sectionProduct.SectionInformation.GetRawXml();
                var sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
                var sectionHandler = new NameValueSectionHandler();
                var sectionHandlerCollection = sectionHandler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
                value = sectionHandlerCollection[key];
                ConfigurationManager.RefreshSection("//" + section);
                if (value == null)
                    return -1;
                return 0;
            }
            catch
            {
                return -1;
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="config"></param>
        /// <returns></returns>

        public static int SetValue(string section, string key, string value, string config)
        {
            try
            {
                var sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(config);

                string x;
                if (GetValue(section, key, out x, config) == 0)
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(config);
                    var node = "//" + section + "/add[@key='" + key + "']";
                    xmlDoc.SelectSingleNode(node).Attributes["value"].Value = value;
                    xmlDoc.Save(config);
                    ConfigurationManager.RefreshSection("//Variables");
                }

                else
                {
                    var nodeRegion = sectionXmlDoc.CreateElement("add");
                    nodeRegion.SetAttribute("key", key);
                    nodeRegion.SetAttribute("value", value);
                    sectionXmlDoc.SelectSingleNode("//" + section).AppendChild(nodeRegion);
                    sectionXmlDoc.Save(config);
                    ConfigurationManager.RefreshSection("//Variables");
                }

                return 0;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int DelKey(string section, string key, string config)
        {
            try
            {
                var sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(config);

                string x;
                if (GetValue(section, key, out x, config) != 0) return 0;
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(config);
                var node = "//" + section + "/add[@key='" + key + "']";
                var xmlnode = xmlDoc.SelectSingleNode(node);
                xmlnode.ParentNode.RemoveChild(xmlnode);
                xmlDoc.Save(config);
                ConfigurationManager.RefreshSection("//Variables");

                return 0;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return -1;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="XMLfile"></param>
        /// <param name="RootValue"></param>
        public static void CreateXMLfile(string XMLfile, string RootValue)
        {
            var xd = new XmlDocument();
            var root = xd.AppendChild(xd.CreateElement(RootValue));
            var child = root.AppendChild(xd.CreateElement(""));
            var childAtt = child.Attributes.Append(xd.CreateAttribute("Attribute"));
            childAtt.InnerText = "My innertext";
            child.InnerText = "Node Innertext";
            xd.Save(XMLfile);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="XMLDocument"></param>
        /// <param name="XMLParentNodeValue"></param>
        /// <param name="XMLChildNodeValue"></param>
        public static void AppendChildtoXMLElement(XmlDocument XMLDocument, XmlNode XMLParentNodeValue, String XMLChildNodeValue)
        {
            XmlNode Child = XMLParentNodeValue.AppendChild(XMLDocument.CreateElement(XMLChildNodeValue));
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="PathFile"></param>
        /// <param name="Myelement"></param>
        /// <param name="Myattribute"></param>
        /// <returns></returns>

        public static String ReadXmlAttribute(String PathFile, String Myelement, String Myattribute)
        {
            String attribute = null;

            if (!System.IO.File.Exists(PathFile))
            {
                XmlReader xmlReader = XmlReader.Create(PathFile);
                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == Myelement))
                    {
                        if (xmlReader.HasAttributes)
                            attribute = xmlReader.GetAttribute(Myattribute).ToString();
                    }
                }

            }
            return (attribute);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PathFile"></param>
        /// <param name="element"></param>
        /// <returns></returns>

        public static string ReadXmlElementValue(string PathFile, string element)
        {
            string MyelementValue = null;

            if (!System.IO.File.Exists(PathFile))
            {
                XmlReader xmlReader = XmlReader.Create(PathFile);

                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == element))
                    {
                        if (xmlReader.HasValue)

                            MyelementValue = xmlReader.Value.ToString();
                    }
                }
            }
            return (MyelementValue);
        }

    }
}
