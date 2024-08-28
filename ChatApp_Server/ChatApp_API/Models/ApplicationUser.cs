namespace ChatApp_API.Models
{
	public class ApplicationUser
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Email { get; set; }
		public string Username { get; set; } = null!;
		public string Password { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
