using System.Security.Claims;

namespace ChatsFrontend.Logic.SavingData.UserData
{
    public interface IUserDataReader
    {
        public Task<IEnumerable<Claim>> ReadUserDataAsync();
    }
}
