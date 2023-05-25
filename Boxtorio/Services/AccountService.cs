using AutoMapper;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Microsoft.EntityFrameworkCore;

namespace Boxtorio.Services
{
	public class AccountService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		public AccountService(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}

		public async Task<Guid> CreateAccount(CreateAccountModel model)
		{

			switch (model.Role)
			{
				case "Admin":
					var admin = _mapper.Map<CreateAccountModel, Admin>(model);
					await _context.Admins.AddAsync(admin);
					break;
				case "Worker":
					var worker = _mapper.Map<CreateAccountModel, Worker>(model);
					await _context.Workers.AddAsync(worker);
					break;
				default:
					throw new ArgumentException("Invalid account role");
			}
			await _context.SaveChangesAsync();
			return _context.Accounts.First(x => x.Email == model.Email).Id;
		}

		public async Task<bool> CheckAccountExist(string email)
		{
			return await _context.Accounts.AnyAsync(x => x.Email.ToLower() == email.ToLower());
		}

		public async Task Delete(Guid id)
		{
			var dbUser = await GetAccountById(id);
			if (dbUser != null)
			{
				_context.Accounts.Remove(dbUser);
				await _context.SaveChangesAsync();
			}
		}

		private async Task<Account> GetAccountById(Guid id)
		{
			var user = await _context.Accounts.FindAsync(id);
			if (user == null)
				throw new Exception("user not found");

			return user;
		}

		public async Task<AccountModel> GetAccount(Guid id)
		{
			var user = await GetAccountById(id);
			return _mapper.Map<AccountModel>(user);
		}

		public async Task<T> GetAccount<T>(Guid id) where T : Account
		{
			var account = await _context.Set<T>().FindAsync(id);
			if (account == null)
				throw new Exception("Account not found");
			return account;
		}
	}
}