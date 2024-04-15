using System;
using Microsoft.Extensions.Logging;
using SPG.Messenger.Domain.Dtos;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Interfaces;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Repository;

namespace SPG.Messenger.Application.Services
{
    public class UserService : IReadOnlyUserService, IWritableUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWritableUserRepository _writableUserRepository;
        private readonly IWriteableMessengerRepository _writeableMessengerRepository;
        private readonly IGuidService _guidService;

        public UserService(
            ILogger<UserService> logger,
            IReadOnlyUserRepository readOnlyUserRepository,
            IWritableUserRepository writableUserRepository,
            IWriteableMessengerRepository writeableMessengerRepository,
            IGuidService guidService
        )
        {
            _logger = logger;
            _readOnlyUserRepository = readOnlyUserRepository;
            _writableUserRepository = writableUserRepository;
            _writeableMessengerRepository = writeableMessengerRepository;
            _guidService = guidService;
        }




        // GET ALL USERS


        public List<UserDto> GetAll()
        {

            var users = _readOnlyUserRepository.GetAll().ToList();
            var userDtos = users.Select(u => u.ToDto()).ToList();

            //_logger.LogInformation("Successfully retrieved all users.");
            return userDtos;

        }




        // GET SINGLE USER

        public UserDto GetSingle(int id)
        {
            //_logger.LogDebug($"Attempting to retrieve user with ID: {id}");

            try
            {
                User user = _readOnlyUserRepository.GetSingle(id);
                //_logger.LogInformation($"Successfully retrieved user with ID: {id}");
                return user.ToDto();
            }
            catch (RepositoryReadException ex)
            {
                //_logger.LogError($"Error retrieving user with ID: {id}. Exception: {ex.Message}");
                throw new UserServiceReadException($"Failed to retrieve user with ID: {id}", ex);
            }
        }




        // FILTER BY EMAIL

        public List<UserDto> FilterByEmail(string email)
        {
           // _logger.LogDebug($"Starting email filter with criterion: {email}");

            var filteredUsers = _readOnlyUserRepository
                .FilterBuilder
                .ByEmailContains(email)
                .Build()
                .ToList();

            var userDtos = filteredUsers.Select(u => u.ToDto()).ToList();

            //_logger.LogInformation($"Filter by email '{email}' returned {userDtos.Count} users.");

            return userDtos;
        }



        // CREATE USER

        public UserDto Register(RegisterUserCommand command)
        {
            //_logger.LogDebug($"Attempting to register user with email: {command.Email}");

            // Assert
            if (_readOnlyUserRepository.ExistsByEmail(command.Email))
            {
                throw new UserServiceValidationException($"User registration failed: Email {command.Email} already in use.");
            }

            // Create a User
            User newUser = User.FromDto(command);
            newUser.Guid = _guidService.NewGuid();

            try
            {
                _writableUserRepository.Create(newUser);
                //_logger.LogInformation($"Successfully registered user with email: {command.Email}");

                // Create a MessengerEntry if participants are specified
                if (command.ParticipantIds.Count > 0)
                {
                    var participants = command.ParticipantIds
                        .Select(_readOnlyUserRepository.GetSingle)
                        .ToHashSet();

                    MessengerEntry messenger = new(newUser, participants, command.MessengerTitle, command.MessengerDescription);
                    _writeableMessengerRepository.Create(messenger);
                }

                
                return newUser.ToDto();
            }
            catch (RepositoryCreateException ex)
            {
                //_logger.LogError($"User registration failed for email: {command.Email}. Exception: {ex.Message}");
                throw new UserServiceCreateException($"Failed to register user with email: {command.Email}", ex);
            }
        }



        // UPDATE USER

        public UserDto Update(UpdateUserCommand command)
        {
            //_logger.LogDebug($"Attempting to update user with ID: {command.Id}");
            User user;

            try
            {
                user = _readOnlyUserRepository.GetSingle(command.Id);
            }
            catch (RepositoryReadException ex)
            {
                //_logger.LogError($"Error retrieving user with ID: {command.Id} for update. Exception: {ex.Message}");
                throw new UserServiceUpdateException($"Failed to retrieve user for update with ID: {command.Id}", ex);
            }

            try
            {
                _writableUserRepository
                    .UpdateBuilder(user)
                    .WithFirstName(command.FirstName)
                    .WithLastName(command.LastName)
                    .Save();

                //_logger.LogInformation($"Successfully updated user with ID: {command.Id}");
                return user.ToDto();
            }
            catch (RepositoryUpdateException ex)
            {
                //_logger.LogError($"User update failed for ID: {command.Id}. Exception: {ex.Message}");
                throw new UserServiceUpdateException($"Failed to update user with ID: {command.Id}", ex);
            }
        }



        // DELETE USER

        public void Delete(int id)
        {
            //_logger.LogDebug($"Attempting to delete user with ID: {id}");

            try
            {
                _writableUserRepository.Delete(id);
                //_logger.LogInformation($"Successfully deleted user with ID: {id}.");
            }
            catch (RepositoryReadException ex)
            {
                //_logger.LogError($"Error retrieving user with ID: {id} for deletion. Exception: {ex.Message}");
                throw new UserServiceDeleteException($"Failed to delete user with ID: {id}.", ex);
            }
            catch (RepositoryDeleteException ex)
            {
                //_logger.LogError($"Deletion failed for user with ID: {id}. Exception: {ex.Message}");
                throw new UserServiceDeleteException($"Failed to delete user with ID: {id}.", ex);
            }
        }

    }
}

