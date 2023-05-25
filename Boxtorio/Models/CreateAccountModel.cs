using System.ComponentModel.DataAnnotations;

namespace Boxtorio.Models
{
	public class CreateAccountModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare(nameof(Password))]
		public string RetryPassword { get; set; }
		[Required]
		public string Role { get; set; }

		public CreateAccountModel(string name, string email, string password, string retryPassword, string role)
		{
			Name = name;
			Email = email;
			Password = password;
			RetryPassword = retryPassword;
			Role = role;
		}
	}
}