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

        private string configfile;
        public string Configfile
        {
            get
            {
                return configfile;
            }
            set
            {
                configfile = value;

                if (!File.Exists(configfile))
                    throw new ArgumentException("Configfile doesn't exist.");
            }
        }

        /// <summary>
        /// the strucutre 
        /// </summary>
        /// <param name="configPath"></param>
        public ConfigXML(string configPath)
        {
            Configfile = configPath;
        }

        public bool GetChildren(string section, out string[] allKeys)
        {
            allKeys = null;
            var sectionHandler = new NameValueSectionHandler();
            try
            {
                var configFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Configfile
                };

                var appConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                var sectionProduct = appConfig.GetSection(section);
                var myParamsSectionRawXml = sectionProduct.SectionInformation.GetRawXml();
                var sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));

                if (sectionXmlDoc.DocumentElement != null)
                {
                    NameValueCollection sectionHandlerCollection = sectionHandler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
                    allKeys = sectionHandlerCollection.AllKeys;
                }

                if (allKeys == null)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetValue(string section, string key, out string value)
        {
            value = null;

            try
            {
                var configFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Configfile
                };
                var appConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                var sectionProduct = appConfig.GetSection(section);
                var myParamsSectionRawXml = sectionProduct.SectionInformation.GetRawXml();
                var sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(new StringReader(myParamsSectionRawXml));
                var sectionHandler = new NameValueSectionHandler();
                NameValueCollection sectionHandlerCollection = sectionHandler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
                value = sectionHandlerCollection[key];
                ConfigurationManager.RefreshSection("//" + section);

                if (value == null)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool SetValue(string section, string key, string value)
        {
            try
            {

                XmlDocument sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(configfile);

                string x;
                if (GetValue(section, key, out x))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(configfile);
                    string node = $"//{section}/add[@key='{key}']";
                    xmlDoc.SelectSingleNode(node).Attributes["value"].Value = value;
                    xmlDoc.Save(configfile);
                    ConfigurationManager.RefreshSection($"//{section}");
                }

                else
                {
                    var nodeRegion = sectionXmlDoc.CreateElement("add");
                    nodeRegion.SetAttribute("key", key);
                    nodeRegion.SetAttribute("value", value);
                    sectionXmlDoc.SelectSingleNode($"//{section}").AppendChild(nodeRegion);

                    sectionXmlDoc.Save(configfile);
                    ConfigurationManager.RefreshSection($"//{section}");
                }

                return true;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return false;
            }
        }

        public bool DelKey(string section, string key)
        {
            try
            {
                XmlDocument sectionXmlDoc = new XmlDocument();
                sectionXmlDoc.Load(configfile);

                string x;
                if (GetValue(section, key, out x))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(configfile);
                    string node = $"//{section}/add[@key='{key}']";
                    XmlNode xmlnode = xmlDoc.SelectSingleNode(node);
                    xmlnode.ParentNode.RemoveChild(xmlnode);
                    xmlDoc.Save(configfile);
                    ConfigurationManager.RefreshSection("//Variables");
                }

                return true;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool SetSectionNode(string name)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configfile);
                var node = xmlDoc.SelectSingleNode("//configuration");
                XmlNode newNode = xmlDoc.CreateElement(name);
                node.AppendChild(newNode);
                xmlDoc.Save(configfile);
                return true;
            }
            catch { return false; }
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

        public static string ReadXmlAttribute(string PathFile, string Myelement, string Myattribute)
        {
            string attribute = null;

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
