using System;
using SPG.Messenger.Domain.Model.MessengerDomain;

namespace SPG.Messenger.Domain.Interfaces
{
    public interface IReadOnlyMessengerRepository
    {
        IMessengerFilterBuilder FilterBuilder { get; set; }

        IQueryable<MessengerEntry> GetAll();
        MessengerEntry GetSingle(int id);
    }
}

