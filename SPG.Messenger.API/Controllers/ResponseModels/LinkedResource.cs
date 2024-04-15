using System;
namespace SPG.Messenger.API.Controllers.ResponseModels
{
    public class LinkedResource<T>
    {
        public T Resource { get; set; }
        public List<LinkInfo> Links { get; set; } = new List<LinkInfo>();

        public LinkedResource(T resource)
        {
            Resource = resource;
        }
    }
}

