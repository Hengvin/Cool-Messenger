using System;
using SPG.Messenger.Domain.Model.UserDomain;

namespace SPG.Messenger.Domain.Interfaces
{
    public interface IUserFilterBuilder
    {
        IUserFilterBuilder ByFirstNameContains(string firstName);

        IUserFilterBuilder ByLastNameContains(string lastName);

        IUserFilterBuilder ByEmailContains(string email);

        IQueryable<User> Build();
    }
}

