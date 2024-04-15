using System;
using SPG.Messenger.Domain.Interfaces;
using SPG.Messenger.Domain.Model.UserDomain;

namespace SPG.Messenger.Repository.Builder
{
	public class UserFilterBuilder : IUserFilterBuilder
	{
        public IQueryable<User> EntityList { get; set; }


		public UserFilterBuilder(IQueryable<User> users)
		{
            EntityList = users;
		}

        public IQueryable<User> Build()
        {
            return EntityList;
        }

        public IUserFilterBuilder ByEmailContains(string email)
        {
            EntityList = EntityList.Where(u => u.Email.ToLower().Contains(email.ToLower()));
            return this;
        }

        public IUserFilterBuilder ByFirstNameContains(string firstName)
        {
            EntityList = EntityList.Where(u => u.Profile.FirstName.ToLower().Contains(firstName.ToLower()));
            return this;
        }

        public IUserFilterBuilder ByLastNameContains(string lastName)
        {
            EntityList = EntityList.Where(u => u.Profile.LastName.ToLower().Contains(lastName.ToLower()));
            return this;
        }
    }
}

