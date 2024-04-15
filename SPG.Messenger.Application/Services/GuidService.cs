using System;
using SPG.Messenger.Domain.Interfaces;

namespace SPG.Messenger.Application.Services
{
    public class GuidService : IGuidService
    {
        public Guid NewGuid() => Guid.NewGuid();
    }
}

