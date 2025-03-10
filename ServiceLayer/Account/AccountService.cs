using AutoMapper;
using Repository.Accounts;
using Repository.Data;
using Repository.Entities;
using ServiceLayer.Models;

namespace ServiceLayer.Account;

public class AccountService(IAccountRepository accountRepository, IMapper mapper) : IAccountService
{
    public async Task<AccountDTO?> LoginAsync(string email, string password)
    {
        var systemAccount = await accountRepository.GetAccountByEmailAsync(email);
        if (systemAccount == null)
        {
            return null;
        }
        if (systemAccount.AccountPassword != password)
        {
            return null;
        }
        var accountDto = mapper.Map<AccountDTO>(systemAccount);
        return accountDto;
    }

    public async Task<AccountDTO?> GetAcountByEmailAsync(string email)
    {
        var systemAccount = await accountRepository.GetAccountByEmailAsync(email);
        var accountDto = mapper.Map<AccountDTO>(systemAccount);
        return accountDto;
    }

    public async Task<AccountDTO?> GetAcountByIdAsync(int accountId)
    {
        var systemAccount = await accountRepository.GetAccountByIdAsync(accountId);
        var accountDto = mapper.Map<AccountDTO>(systemAccount);
        return accountDto;
    }

    public async Task<AccountDTO?> CreateNewAccountAsync(AccountDTO accountDto)
    {
        var systemAccount = mapper.Map<SystemAccount>(accountDto);
        var addedAccount = await accountRepository.CreateAsync(systemAccount);
        var accountDtoToReturn = mapper.Map<AccountDTO>(addedAccount);
        return accountDtoToReturn;
    }

    public async Task<int?> UpdateAccountAsync(AccountDTO accountDto)
    {
        var systemAccount = mapper.Map<SystemAccount>(accountDto);
        var updateAccount = await accountRepository.GetAccountByIdAsync(accountDto.AccountId);
        if (updateAccount == null)
            return null;
        if (systemAccount.AccountEmail != updateAccount.AccountEmail)
        {
            var existedAccount = await accountRepository.GetAccountByEmailAsync(
                systemAccount.AccountEmail
            );
            if (existedAccount != null)
                return null;
        }
        var effectedRow = await accountRepository.UpdateAsync(systemAccount);
        return effectedRow;
    }

    public async Task<int?> DeleteAccountAsync(int accountId)
    {
        var systemAccount = await accountRepository.GetAccountByIdAsync(accountId);
        var effectedRow = await accountRepository.DeleteAsync(systemAccount);
        return effectedRow;
    }

    public async Task<IEnumerable<AccountDTO>> ListAllAccounts()
    {
        var accounts = await accountRepository.ListAllAsync();
        var accountDtos = mapper.Map<IEnumerable<AccountDTO>>(accounts);
        return accountDtos;
    }

    public async Task<int?> UpdateProfile(AccountDTO account)
    {
        var repoAccount = await accountRepository.GetAccountByIdAsync(account.AccountId);
        if (repoAccount == null)
            return null;
        var accountDtoUpdated = mapper.Map<AccountDTO>(repoAccount);

        if (account.AccountRole == default)
        {
            account.AccountRole = accountDtoUpdated.AccountRole;
        }
        var updateAccount = mapper.Map<SystemAccount>(account);
        var effected = await accountRepository.UpdateAsync(updateAccount);
        return effected;
    }

    public async Task<PaginatedList<AccountDTO>> ListAccountsWithPaginationAndFiltering(
        string? searchString,
        string? sortOrder,
        int? pageNumber,
        int? pageSize
    )
    {
        pageNumber ??= 1;
        pageSize ??= 8;
        var paginatedListEntity = await accountRepository.GetAccountsQuery(
            (int)pageNumber,
            (int)pageSize,
            searchString,
            sortOrder
        );
        var accountDtos = mapper.Map<PaginatedList<AccountDTO>>(paginatedListEntity);
        accountDtos = new PaginatedList<AccountDTO>(
            accountDtos,
            paginatedListEntity.TotalPages,
            paginatedListEntity.TotalElements,
            paginatedListEntity.PageIndex
        );
        return accountDtos;
    }
}
