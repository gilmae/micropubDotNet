using System;
using System.IO;
using System.Threading.Tasks;
using mf;
using RestSharp;
using RestSharp.Authenticators;

namespace Micropub {
    public class Client {
        public Uri MicropubEndpoint { get;  set; }
        public Uri MediaEndpoint { get;  set; }
        public string Authentication { get; set; }

        public static Client Discover (Uri address) {
            Client c = new Client ();

            var parser = new Parser ();
            var doc = parser.Parse (address);
            if (doc.Rels.ContainsKey ("micropub") && doc.Rels["micropub"] != null) {
                c.MicropubEndpoint = new Uri (doc.Rels["micropub"][0]);
            }

            return c;
        }

        public void Inspect () {
            var rest = new RestClient (MicropubEndpoint);

            var request = new RestRequest ("/micropub")
                .AddQueryParameter ("q", "config");
            request.AddHeader ("Authorization", $"Bearer {Authentication}");

            var response = rest.Get (request);

            if (response.IsSuccessful) {
                var capabilities = System.Text.Json.JsonSerializer.Deserialize<Capabilities> (response.Content);
                MediaEndpoint = new Uri (capabilities.MediaEndpoint);
            }
        }

    }
}