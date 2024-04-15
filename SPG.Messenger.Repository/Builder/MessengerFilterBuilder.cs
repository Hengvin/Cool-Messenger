using System;
using SPG.Messenger.Domain.Interfaces;
using SPG.Messenger.Domain.Model.MessengerDomain;

namespace SPG.Messenger.Repository.Builder
{
	
    public class MessengerFilterBuilder : IMessengerFilterBuilder
    {
        public IQueryable<MessengerEntry> EntityList { get; set; }


        public MessengerFilterBuilder(IQueryable<MessengerEntry> messengerEntries)
        {
            EntityList = messengerEntries;
        }
    

        public IMessengerFilterBuilder ByTitleContains(string title)
        {
            EntityList = EntityList.Where(m => m.Title.ToLower().Contains(title.ToLower()));
            return this;
        }

        public IQueryable<MessengerEntry> Build()
        {
            return EntityList;
        }
    }
    
}

