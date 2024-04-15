using System;
using Microsoft.EntityFrameworkCore;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Interfaces;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Infrastructure;

namespace SPG.Messenger.Repository.Builder
{
	public class UserUpdateBuilder : IUserUpdateBuilder
	{
        private readonly MessengerContext _context;
        private readonly User _entity;


		public UserUpdateBuilder(MessengerContext context, User entity)
		{
            _context = context;
            _entity = entity;
		}

        public IUserUpdateBuilder WithFirstName(string firstName)
        {
            _entity.Profile.FirstName = firstName;
            return this;
        }

        public IUserUpdateBuilder WithLastName(string lastName)
        {
            _entity.Profile.LastName = lastName;
            return this;
        }

        public int Save()
        {
            try
            {
                _context.Update(_entity);
                return _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw RepositoryUpdateException.FromDbError(ex, nameof(User));
            }
            catch (DbUpdateException ex)
            {
                throw RepositoryUpdateException.FromDbError(ex, nameof(User));
            }
        }
    }
}

