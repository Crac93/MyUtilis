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

                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Config
                };

                var AppConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

                var SectionProduct = AppConfig.GetSection(Section);

                string myParamsSectionRawXml = SectionProduct.SectionInformation.GetRawXml();
                XmlDocument sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
                NameValueSectionHandler SectionHandler = new NameValueSectionHandler();

                NameValueCollection SectionHandlerCollection = SectionHandler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
                AllKeys = SectionHandlerCollection.AllKeys;

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
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="Config"></param>
        /// <returns></returns>
        public static int GetValue(string Section, string Key, out string Value, string Config)
        {
            Value = null;

            try
            {
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Config
                };
                var AppConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                var SectionProduct = AppConfig.GetSection(Section);
                string myParamsSectionRawXml = SectionProduct.SectionInformation.GetRawXml();
                XmlDocument sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
                NameValueSectionHandler SectionHandler = new NameValueSectionHandler();
                NameValueCollection SectionHandlerCollection = SectionHandler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
                Value = SectionHandlerCollection[Key];
                ConfigurationManager.RefreshSection("//" + Section);
                if (Value == null)
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
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="config"></param>
        /// <returns></returns>

        public static int SetValue(string Section, string Key, string Value, string config)
        {
            try
            {
                XmlDocument sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(config);

                string x;
                if (GetValue(Section, Key, out x, config) == 0)
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(config);
                    string node = "//" + Section + "/add[@key='" + Key + "']";
                    xmlDoc.SelectSingleNode(node).Attributes["value"].Value = Value;
                    xmlDoc.Save(config);
                    ConfigurationManager.RefreshSection("//Variables");
                }

                else
                {
                    var nodeRegion = sectionXmlDoc.CreateElement("add");
                    nodeRegion.SetAttribute("key", Key);
                    nodeRegion.SetAttribute("value", Value);
                    sectionXmlDoc.SelectSingleNode("//" + Section).AppendChild(nodeRegion);
                    sectionXmlDoc.Save(config);
                    ConfigurationManager.RefreshSection("//Variables");
                }

                return 0;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int DelKey(string Section, string Key, string config)
        {
            try
            {
                XmlDocument sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(config);

                string x;
                if (GetValue(Section, Key, out x, config) == 0)
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(config);
                    string node = "//" + Section + "/add[@key='" + Key + "']";
                    XmlNode xmlnode = xmlDoc.SelectSingleNode(node);
                    xmlnode.ParentNode.RemoveChild(xmlnode);
                    xmlDoc.Save(config);
                    ConfigurationManager.RefreshSection("//Variables");
                }

                return 0;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
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
            XmlDocument XD = new XmlDocument();
            XmlNode Root = XD.AppendChild(XD.CreateElement(RootValue));
            XmlNode Child = Root.AppendChild(XD.CreateElement(""));
            XmlAttribute ChildAtt = Child.Attributes.Append(XD.CreateAttribute("Attribute"));
            ChildAtt.InnerText = "My innertext";
            Child.InnerText = "Node Innertext";
            XD.Save(XMLfile);

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
