using Repository.Data;
using Repository.Entities;

namespace Repository.Accounts;

public interface IAccountRepository : IGenericRepository<SystemAccount>
{
    Task<SystemAccount?> GetAccountByIdAsync(int id);
    Task<SystemAccount?> GetAccountByEmailAsync(string email);

    Task<PaginatedList<SystemAccount>> GetAccountsQuery(
        int pageNumber,
        int pageSize,
        string? searchString,
        string? sortOrder
    );
}
