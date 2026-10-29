using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Bll.Services;
public class AuthService : IAuthService
{
    private readonly IRoleDal _roleDal;
    public AuthService(IRoleDal roleDal) { _roleDal = roleDal; }
    public bool IsReceiver(User user)
    {
        var role = _roleDal.GetById(user.RoleId);
        return role != null && role.Name.Equals("Receiver", StringComparison.OrdinalIgnoreCase);
    }
}
