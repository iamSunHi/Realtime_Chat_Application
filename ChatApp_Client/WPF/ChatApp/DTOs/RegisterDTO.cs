namespace ChatApp.DTOs
{
	public class RegisterDTO
	{
		public string Name { get; set; } = null!;
		public string? Email { get; set; }
		public string Username { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
