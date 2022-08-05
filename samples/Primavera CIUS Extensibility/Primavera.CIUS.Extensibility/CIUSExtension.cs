using Primavera.Extensibility.Base.Services;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Lithium.TaxAuthority.Portugal.Client.Rest.Models;
using System;
using System.Xml;

namespace Primavera.CIUS.Extensibility
{
    /// <summary>
    /// This class is an example that manipulates the CIUS-PT Xml and can be used to insert optional fields on the model.
    /// </summary>
    public class CIUSExtension : TaxAuthorityPublicServicesPortugal
    {
        /// <summary>
        /// This method is just a simple example that illustrates the change applied to the XML file generated in the CIUS-PT format.
        /// In this example, the optional field "EndPointID" (related to the suplier e-mail) is introduced. 
        /// </summary>
        public override void DepoisDeGerarXML(string XML, ExtensibilityEventArgs e)
        {
        //Path to the location where the file will be exported
        string filePath = "";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XML);

            XmlElement elem = xmlDocument.CreateElement("cbc", "EndpointID", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            elem.InnerText = "email@email.com";
            elem.SetAttribute("schemeID", "EM");
            XmlElement node = (XmlElement)xmlDocument.DocumentElement.GetElementsByTagName("cac:Party")[0];
            node.InsertBefore(elem, node.ChildNodes.Item(0));

            xmlDocument.Save(filePath);
        }
    }
}
