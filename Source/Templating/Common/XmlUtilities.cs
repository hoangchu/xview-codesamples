using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public static class XmlUtilities
    {
        public static XmlNamespaceManager CreateTridionNamespaceManager(XmlNameTable nameTable)
        {
            var xmlNamespaceManager = new XmlNamespaceManager(nameTable);
            xmlNamespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xmlNamespaceManager.AddNamespace("html", "http://www.w3.org/1999/xhtml");
            xmlNamespaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            xmlNamespaceManager.AddNamespace("tcm", "http://www.tridion.com/ContentManager/5.0");
            xmlNamespaceManager.AddNamespace("tridion", "http://www.tridion.com/ContentManager/5.0");
            xmlNamespaceManager.AddNamespace("tcdl", "http://www.tridion.com/ContentDelivery/5.3/TCDL");
            xmlNamespaceManager.AddNamespace("tct", "http://www.tridion.com/ContentManager/5.3/CompoundTemplate");
            xmlNamespaceManager.AddNamespace("tcmXhtml", "http://www.tridion.com/ContentManager/5.2/tcmXhtml");
            xmlNamespaceManager.AddNamespace("templatedebugging", "http://www.tridion.com/ContentManager/5.3/TemplateDebugging");
            return xmlNamespaceManager;
        }

        public static void ValidateXmlWellFormness(string xml, bool supportNamespaces = true)
        {
            using (var xmlReader = new XmlTextReader(new StringReader(xml)))
            {
                xmlReader.Namespaces = supportNamespaces;

                while (xmlReader.Read())
                {
                }
            }
        }

        public static string XsltTransfrom(string xml, string xslt, XsltArgumentList xsltParameters = null)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return XsltTransfrom(xmlDocument, xslt, xsltParameters);
        }

        public static string XsltTransfrom(XmlDocument xmlDocument, string xslt, XsltArgumentList xsltParameters = null)
        {
            if (xmlDocument == null || xmlDocument.DocumentElement == null)
            {
                return null;
            }

            var transformer = new XslCompiledTransform();
            var stringWriter = new StringWriter();

            using (var reader = new StringReader(xslt))
            using (var xmlReader = XmlReader.Create(reader))
            {
                transformer.Load(xmlReader);
                transformer.Transform(xmlDocument, xsltParameters, stringWriter);
            }

            return stringWriter.ToString();
        }
    }
}