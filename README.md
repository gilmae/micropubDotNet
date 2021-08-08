# Micropub dotnet

## Usage
```
    Micropub.Server server = Micropub.Server.Discover(new Uri("http://sample.org"));
    var context = server.Authenticate(access_token);
    var client = new Micropub.Client(context);

    var mediaLocation = client.PostMedia(file);
    var postLocation = client.Post(post);
```
