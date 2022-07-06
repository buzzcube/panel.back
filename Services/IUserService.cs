namespace panel.back.Services
{
    public interface IUserService
    {
        List<panel.back.Models.User> GetUsers();
        panel.back.Models.User GetUserById(int id);
        panel.back.Models.User GetUserByEmail(string email);
        panel.back.Models.User GetUserByInvitationId(string invitationId);
        panel.back.Models.User CreateUser(panel.back.Models.User user);
        panel.back.Models.User DeactivateUser(int id);
        panel.back.Models.User ActivateUser(int id);
    }
}
