# Micropub dotnet

## Usage
```
    Micropub.Client client = Micropub.Client.Discover(new Uri("http://sample.org"));
    client.Authentication = access_token;
    client.Inspect();

    var mediaLocation = client.PostMedia(file);
    var postLocation = client.Post(post);

    Micropub.Client client = Client.Authenticate(host, username, password)
```
