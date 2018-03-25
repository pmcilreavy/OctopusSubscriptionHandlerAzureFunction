using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace OctopusSubscriptionHandler.Utility
{
    public static class Extensions
    {
        public static IPAddress GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse((((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress));
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return IPAddress.Parse(prop.Address);
            }

            return null;
        }
    }
}