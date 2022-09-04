# Micropub dotnet

## Usage
```
    Micropub.Client client = Micropub.Client.Discover(new Uri("http://sample.org"));
    client.Authentication = access_token;
    client.Inspect();

    var postLocation = client.Post(post);

    var mediaLocation = client.PostMedia(file);
    var updates = new UpdateModel();

    updates.Add("photo", new []{mediaLocation.ToString()});
    client.PostUpdate(postLocation, updates);
```
