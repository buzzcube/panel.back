using Microsoft.EntityFrameworkCore;

namespace panel.back.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly DataContext _dataContext;

        public UserService(ILogger<UserService> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }
        public panel.back.Models.User ActivateUser(int id)
        {
            throw new NotImplementedException();
        }

        public panel.back.Models.User CreateUser(panel.back.Models.User user)
        {
            throw new NotImplementedException();
        }

        public panel.back.Models.User DeactivateUser(int id)
        {
            throw new NotImplementedException();
        }

        public panel.back.Models.User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public panel.back.Models.User GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public panel.back.Models.User GetUserByInvitationId(string invitationId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            var list = _dataContext.Users.Select(
                s => new User
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    InvitationId = s.InvitationId,
                    Created = s.Created,
                    IsActive = s.IsActive,
                    Status = s.Status
                }
            ).ToListAsync().Result;

            if (list.Count < 0)
            {
                return new List<User>();
            }
            else
            {
                return list;
            }
        }
    }
}
