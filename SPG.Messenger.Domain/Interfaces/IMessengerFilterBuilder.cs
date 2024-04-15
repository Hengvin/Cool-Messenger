using System;
using SPG.Messenger.Domain.Model.MessengerDomain;

namespace SPG.Messenger.Domain.Interfaces
{
	public interface IMessengerFilterBuilder
	{
        IMessengerFilterBuilder ByTitleContains(string title);

        IQueryable<MessengerEntry> Build();
    }
}

