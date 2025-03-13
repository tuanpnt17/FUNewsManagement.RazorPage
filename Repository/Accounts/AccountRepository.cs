using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;

namespace Repository.Accounts
{
    public class AccountRepository(FuNewsDbContext context) : IAccountRepository
    {
        public Task<SystemAccount?> GetAccountByEmailAsync(string email)
        {
            return context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<PaginatedList<SystemAccount>> GetAccountsQuery(
            int pageNumber,
            int pageSize,
            string? searchString,
            string? sortOrder
        )
        {
            var systemAccounts = context.SystemAccounts.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                systemAccounts = systemAccounts.Where(x =>
                    x.AccountEmail.ToLower().Contains(searchString)
                    || x.AccountName.ToLower().Contains(searchString)
                );
            }

            systemAccounts = sortOrder switch
            {
                "name" => systemAccounts.OrderBy(a => a.AccountName),
                "name_desc" => systemAccounts.OrderByDescending(a => a.AccountName),
                "email" => systemAccounts.OrderBy(a => a.AccountEmail),
                "email_desc" => systemAccounts.OrderByDescending(a => a.AccountEmail),
                "role" => systemAccounts.OrderBy(a => a.AccountRole),
                "role_desc" => systemAccounts.OrderByDescending(a => a.AccountRole),
                _ => systemAccounts.OrderByDescending(a => a.AccountId),
            };

            var result = await PaginatedList<SystemAccount>.CreateAsync(
                systemAccounts.AsNoTracking(),
                pageNumber,
                pageSize
            );
            return result;
        }

        public Task<SystemAccount?> GetAccountByIdAsync(int id)
        {
            return context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId == id);
        }

        public async Task<IEnumerable<SystemAccount>> ListAllAsync()
        {
            return await context.SystemAccounts.ToListAsync();
        }

        public async Task<SystemAccount> CreateAsync(SystemAccount account)
        {
            account.AccountPassword = "@1";
            var lastedAccount = await context
                .SystemAccounts.OrderByDescending(a => a.AccountId)
                .FirstOrDefaultAsync();
            if (lastedAccount != null)
            {
                account.AccountId = (short)(lastedAccount.AccountId + 1);
            }
            else
            {
                account.AccountId = 1;
            }
            var addedAccount = await context.SystemAccounts.AddAsync(account);
            await context.SaveChangesAsync();
            return addedAccount.Entity;
        }

        public async Task<int?> UpdateAsync(SystemAccount account)
        {
            var systemAccount = await GetAccountByIdAsync(account.AccountId);
            if (systemAccount == null)
            {
                return null;
            }

            // Update fields that are different
            if (
                systemAccount.AccountName != account.AccountName
                && !string.IsNullOrEmpty(account.AccountName)
            )
            {
                systemAccount.AccountName = account.AccountName;
            }
            if (
                systemAccount.AccountEmail != account.AccountEmail
                && !string.IsNullOrEmpty(account.AccountEmail)
            )
            {
                var existed = await GetAccountByEmailAsync(account.AccountEmail);
                if (existed != null && existed.AccountId != account.AccountId)
                {
                    return null;
                }
                systemAccount.AccountEmail = account.AccountEmail;
            }
            if (systemAccount.AccountRole != account.AccountRole)
            {
                systemAccount.AccountRole = account.AccountRole;
            }
            if (
                systemAccount.AccountPassword != account.AccountPassword
                && !string.IsNullOrEmpty(account.AccountPassword)
            )
            {
                systemAccount.AccountPassword = account.AccountPassword;
            }

            var effectedRow = await context.SaveChangesAsync();
            return effectedRow;
        }

        public async Task<int?> DeleteAsync(SystemAccount? account)
        {
            if (account == null)
                return null;
            var systemAccount = await GetAccountByIdAsync(account.AccountId);
            if (systemAccount == null)
            {
                return null;
            }
            await Task.Run(() => context.SystemAccounts.Remove(systemAccount));
            var effectedRow = await context.SaveChangesAsync();
            return effectedRow;
        }
    }
}
