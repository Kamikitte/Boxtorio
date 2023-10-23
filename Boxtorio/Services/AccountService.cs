using AutoMapper;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Microsoft.EntityFrameworkCore;

namespace Boxtorio.Services;

public sealed class AccountService
{
	private readonly IMapper mapper;
	private readonly DataContext context;
	public AccountService(IMapper mapper, DataContext context)
	{
		this.mapper = mapper;
		this.context = context;
	}

	public async Task CreateAccount(CreateAccountModel model)
	{

		switch (model.Role)
		{
			case "Admin":
				var admin = mapper.Map<CreateAccountModel, Admin>(model);
				await context.Admins.AddAsync(admin);
				break;
			case "Worker":
				var worker = mapper.Map<CreateAccountModel, Worker>(model);
				await context.Workers.AddAsync(worker);
				break;
			default:
				throw new ArgumentException("Invalid account role");
		}
		await context.SaveChangesAsync();
    }

	public async Task<bool> CheckAccountExist(string email)
	{
		return await context.Accounts.AnyAsync(x => string.Equals(x.Email, email, StringComparison.CurrentCultureIgnoreCase));
	}

	public async Task Delete(Guid id)
	{
		var dbUser = await GetAccountById(id);
        context.Accounts.Remove(dbUser);
        await context.SaveChangesAsync();
    }

	private async Task<Account> GetAccountById(Guid id)
	{
		var user = await context.Accounts.FindAsync(id);
		if (user == null)
        {
            throw new ArgumentException("user not found");
        }

        return user;
	}

	public async Task<AccountModel> GetAccount(Guid id)
	{
		var user = await GetAccountById(id);
		return mapper.Map<AccountModel>(user);
	}

	public async Task<T> GetAccount<T>(Guid id) where T : Account
	{
		var account = await context.Set<T>().FindAsync(id);
		if (account == null)
        {
            throw new ArgumentException("Account not found");
        }

        return account;
    }

	public Task<IEnumerable<T>> GetAccounts<T>() where T : Account
	{
		return Task.FromResult<IEnumerable<T>>(context.Set<T>());
	}
}
