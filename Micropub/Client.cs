using System;
using System.IO;
using System.Threading.Tasks;
using mf;
using RestSharp;
using RestSharp.Authenticators;

namespace Micropub {
    public class Client {
        public Uri MicropubEndpoint { get; set; }
        public Uri MediaEndpoint { get; set; }
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

        public async Task<Uri> Post (Microformat item) {
            RestClient client = new RestClient (MicropubEndpoint);
            client.Authenticator = new JwtAuthenticator (Authentication);

            var request = new RestRequest ("", Method.Post);
            request.AddJsonBody (item);

            var response = await client.ExecutePostAsync (request);

            if (response.IsSuccessful) {

                foreach (var h in response.Headers) {
                    if (h.Name.ToLower () == "location") {
                        return new Uri (h.Value.ToString ());
                    }
                }

            }
            return null;
        }

        public async Task<Uri> PostMedia (string path) {
            if (!File.Exists (path)) {
                return null;
            }
            RestClient client = new RestClient (MediaEndpoint);
            client.Authenticator = new JwtAuthenticator (Authentication);

            var mediaPostRequest = new RestRequest ("", Method.Post);

            mediaPostRequest.AddFile ("file", path);
            mediaPostRequest.AddHeader ("Content-Type", "multipart/form-data");
            mediaPostRequest.AddHeader ("Accept", "application/json");
            var response = await client.ExecutePostAsync (mediaPostRequest);

            if (response.IsSuccessful) {

                foreach (var h in response.Headers) {
                    if (h.Name.ToLower () == "location") {
                        return new Uri (h.Value.ToString ());
                    }
                }

            }
            return null;

        }

        public async Task<bool> PostUpdate (Uri itemUri, UpdateModel updates) {
            RestClient client = new RestClient (MicropubEndpoint);
            client.Authenticator = new JwtAuthenticator (Authentication);

            var request = new RestRequest ("", Method.Post);
            request.AddJsonBody (new {
                action = "update",
                    url = itemUri.ToString (),
                    add = updates.Add,
                    replace = updates.Replace,
                    delete = updates.Delete
            });

            var response = await client.ExecutePostAsync (request);
            return response.IsSuccessful;
        }

        public async Task<Microformat> Get (Uri url) {
            RestClient client = new RestClient (MicropubEndpoint);
            client.Authenticator = new JwtAuthenticator (Authentication);

            var request = new RestRequest ("", Method.Get);
            request.AddQueryParameter ("q", "source");
            request.AddQueryParameter ("url", url.ToString ());

            var response = await client.ExecuteAsync<Microformat> (request);

            if (response.IsSuccessful) {

                return response.Data;

            }
            return null;
        }
    }
}