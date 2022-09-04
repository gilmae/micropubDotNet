using System.Collections.Generic;
namespace Micropub;
public record UpdateModel {
    public Dictionary<string, object[]> Add { get; set; } = new Dictionary<string, object[]> ();

    public List<string> Delete { get; set; } = new List<string> ();

    public Dictionary<string, object[]> Replace { get; set; } = new Dictionary<string, object[]> ();

}