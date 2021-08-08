using System;
namespace Micropub
{
    public record Context
    {
        public string AccessToken { get; set; }
        public Server Server { get; set; }
    }
}
