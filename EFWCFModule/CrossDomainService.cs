using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Xml;

namespace EFWCFModule
{
    [ServiceContract]
    public interface ICrossDomain
    {
        [OperationContract]
        [WebGet(UriTemplate = "clientaccesspolicy.xml")]
        Message ProvidePolicyFile();
    }

    public class CrossDomainService : ICrossDomain
    {
        public Message ProvidePolicyFile()
        {
            XmlReader xmlReader = MakeXml();
            return Message.CreateMessage(MessageVersion.None, "", xmlReader);
        }

        private XmlReader MakeXml()
        {
            TextReader reader = new StringReader(
                @"<?xml version='1.0' encoding='utf-8'?>
                <access-policy>
                  <cross-domain-access>
                    <policy>
                      <allow-from http-request-headers='*'>
                        <domain uri='*'/>
                      </allow-from>
                      <grant-to> 
                        <resource path='/' include-subpaths='true'/>
                      </grant-to>
                    </policy>
                  </cross-domain-access>
                </access-policy>");
            return XmlReader.Create(reader);
        }
    }
}
