using ChatApp.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		public Login()
		{
			InitializeComponent();

			UsernameTextBox.Text = "Enter your username. . .";
			PasswordBox.Password = "Enter your password. . .";
		}

		private async void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			LoginBtn.Content = "Logging in...";
			LoginBtn.IsEnabled = false;

			string username = UsernameTextBox.Text;
			string password = PasswordBox.Password;

			try
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri("https://localhost:9999");
					var response = await client.PostAsJsonAsync("/api/auth/login", new { username, password });

					if (response.IsSuccessStatusCode)
					{
						var userInfo = await response.Content.ReadFromJsonAsync<UserInfoDTO>();

						if (userInfo is null)
						{
							throw new HttpRequestException("Failed to login. Please try again later.");
						}

						MainWindow mainWindow = new MainWindow(userInfo);
						mainWindow.Show();

						Close();
					}
					else
					{
						MessageBox.Show("Username or password is incorrect!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				LoginBtn.Content = "Login";
				LoginBtn.IsEnabled = true;
			}
		}

		private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (UsernameTextBox.Text == "Enter your username. . .")
				UsernameTextBox.Text = "";
		}

		private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(UsernameTextBox.Text))
				UsernameTextBox.Text = "Enter your username. . .";
		}

		private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (PasswordBox.Password == "Enter your password. . .")
				PasswordBox.Password = "";
		}

		private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(PasswordBox.Password))
				PasswordBox.Password = "Enter your password. . .";
		}
	}
}
