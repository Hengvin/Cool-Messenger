using System;
using SPG.Messenger.Domain.Interfaces;

namespace SPG.Messenger.Application.Mock
{
    public class GuidServiceMock : IGuidService
    {
        public Guid NewGuid() => new Guid("497edb1f-21ed-4327-983c-5a7e83529f3c");
    }
    
}

