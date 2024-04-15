using System;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Domain.Model.UserDomain;

namespace SPG.Messenger.Domain.Interfaces
{
    public interface IWriteableMessengerRepository
    {
        int Create(MessengerEntry entity);

        // IMessengerEntryUpdateBuilder UpdateBuilder(MessengerEntry entity);

        // void Delete(int id);
    }
}

