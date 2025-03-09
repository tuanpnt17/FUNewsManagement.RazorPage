using Repository.Data;
using Repository.Entities;

namespace Repository.Accounts;

public interface IAccountRepository : IGenericRepository<SystemAccount>
{
    Task<SystemAccount?> GetAccountByIdAsync(int id);
    Task<SystemAccount?> GetAccountByEmailAsync(string email);
}
