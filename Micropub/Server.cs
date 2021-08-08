using System;
using mf;

namespace Micropub
{
    public record Server
    {
        public Uri ServerAddress { get; internal set; }
        public Uri MicropubEndpoint { get; internal set; }
        public Uri TokenEndpoint { get; internal set; }
        public Uri AuthEndpoint { get; internal set; }

        public static Server Discover(Uri address)
        {
            Server s = new Server { ServerAddress = address };
            var parser = new Parser();
            var doc = parser.Parse(address);
            if (doc.Rels.ContainsKey("micropub") && doc.Rels["micropub"] != null)
            {
                s.MicropubEndpoint = new Uri(doc.Rels["micropub"][0]);
            }

            if (doc.Rels.ContainsKey("authorization_endpoint") && doc.Rels["authorization_endpoint"] != null)
            {
                s.MicropubEndpoint = new Uri(doc.Rels["authorization_endpoint"][0]);
            }

            if (doc.Rels.ContainsKey("token_endpoint") && doc.Rels["token_endpoint"] != null)
            {
                s.MicropubEndpoint = new Uri(doc.Rels["token_endpoint"][0]);
            }

            return s;
        }
    }

    public static class ServerExtensions
    {
        public static Context Authenticate(this Server server, string access_token)
        {
            return new Context { AccessToken = access_token, Server = server };
        }
    }
}
