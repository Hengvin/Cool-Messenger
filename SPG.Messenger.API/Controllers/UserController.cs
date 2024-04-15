using System;
using Microsoft.AspNetCore.Mvc;
using SPG.Messenger.API.Controllers.ResponseModels;
using SPG.Messenger.Domain.Dtos;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Interfaces;

namespace SPG.Messenger.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IWritableUserService _writableUserService;
        private readonly IReadOnlyUserService _readOnlyUserService;

        public UsersController(IWritableUserService writableUserService, IReadOnlyUserService readOnlyUserService)
        {
            _writableUserService = writableUserService;
            _readOnlyUserService = readOnlyUserService;
        }



        // HATEAOS Helper

        private LinkedResource<UserDto> MapToLinkedResource(UserDto user)
        {
            var links = new List<LinkInfo>
            {
                new LinkInfo(Url.Link(nameof(GetSingle), new { id = user.Id })!, "self", "GET"),
                new LinkInfo(Url.Link(nameof(Update), new { id = user.Id })!, "update_user", "PUT"),
                new LinkInfo(Url.Link(nameof(Delete), new { id = user.Id })!, "delete_user", "DELETE")
            };

            return new LinkedResource<UserDto>(user){ Links = links };
        }



        // GET ALL USERS


        [HttpGet()]
        public IActionResult GetAll()
        {
            var users = _readOnlyUserService.GetAll();
            var linkedUsers = users.Select(MapToLinkedResource).ToList();

            // Status 200
            return Ok(linkedUsers);
        }




        // GET SINGLE USER


        [HttpGet("{id}", Name = nameof(GetSingle))]
        public IActionResult GetSingle(int id)
        {
            try
            {
                var user = _readOnlyUserService.GetSingle(id);
                var linkedResource = MapToLinkedResource(user);

                // Status 200
                return Ok(linkedResource);
            }
            catch (UserServiceReadException ex)
            {
                return NotFound(ex.Message);
            }
        }




        // FILTER USERS BY EMAIL

        [HttpGet("filter")]
        public IActionResult FilterByEmail([FromQuery] string email)
        {
            try
            {
                var filteredUsers = _readOnlyUserService.FilterByEmail(email);
                var linkedUsers = filteredUsers.Select(MapToLinkedResource).ToList();

                // Status 200
                return Ok(linkedUsers);
            }
            catch (Exception ex)
            {
                // Status 500
                return StatusCode(500, ex.Message);
            }
        }





        // REGISTER USER


        [HttpPost]
        public IActionResult Register(RegisterUserCommand command)
        {
            try
            {
                var userDto = _writableUserService.Register(command);
                var linkedResource = MapToLinkedResource(userDto);

                // Status 201 and Http Location Header
                return CreatedAtAction(nameof(GetSingle), new { id = userDto.Id }, linkedResource);
            }
            catch (UserServiceValidationException ex)
            {
                // Status 400
                return BadRequest(ex.Message);
            }
            catch (UserServiceCreateException ex)
            {
                // Status 500
                return StatusCode(500, ex.Message);
            }
        }




        // UPDATE USER

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                // Status 400
                return BadRequest("Mismatched user ID in the request");
            }
                
            try
            {
                var userDto = _writableUserService.Update(command);
                var linkedResource = MapToLinkedResource(userDto);

                // Status 200
                return Ok(linkedResource);
            }
            catch (UserServiceUpdateException ex)
            {
                // Status 404
                return NotFound(ex.Message);
            }
        }




        // DELETE USER

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _writableUserService.Delete(id);

                // Status 204
                return NoContent();
            }
            catch (UserServiceDeleteException ex)
            {
                // Status 404
                return NotFound(ex.Message);
            }
        }
    }
}

