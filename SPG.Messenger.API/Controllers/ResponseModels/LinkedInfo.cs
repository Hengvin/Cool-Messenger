﻿using System;
namespace SPG.Messenger.API.Controllers.ResponseModels
{
    public class LinkInfo
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public LinkInfo(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}

