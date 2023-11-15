using System;

public class User
{
	public string UserName { get; set; }

	public string Password { get; set; }

	public string Email { get; set; }


	public User(string username, string password, string email)
	{
		UserName = username;

		Password = password;

		Email = email;
	}
}
