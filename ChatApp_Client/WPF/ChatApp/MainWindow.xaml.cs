using ChatApp.DTOs;
using ChatApp.Models;
using ChatApp.Services.WebSocketServices;
using ChatApp.ViewModels;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ChatApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private UserInfoDTO _userInfo;
		private readonly ChatWebSocketHandler _chatWebSocketHandler;

		public MainWindow(UserInfoDTO userInfo)
		{
			InitializeComponent();

			_userInfo = userInfo;
			_chatWebSocketHandler = new ChatWebSocketHandler(_userInfo.Id);

			WelcomeTxt.Text = $"Welcome, {_userInfo.Name}!";
			_ = ConnectServerAsync();
		}

		private async void SendButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(MessageTxt.Text))
			{
				return;
			}

			if (_chatWebSocketHandler.ClientSocket.State == WebSocketState.Open)
			{
				var message = MessageTxt.Text;
				MessageTxt.Text = string.Empty;

				await _chatWebSocketHandler.SendMessage(message);
				AddMessage(_userInfo.Name, message, true);
			}
			else
			{
				MessageBox.Show("WebSocket is not connected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void AddMessage(string sender, string message, bool isOwner)
		{
			var messageUserControl = new MessageUserControl();
			messageUserControl.SetValue(MessageUserControl.messageVMProperty, new MessageVM
			{
				Message = new MessageEntity()
				{
					Time = DateTime.Now.ToShortTimeString(),
					SenderName = sender,
					MessageText = message,
					IsOwner = isOwner
				}
			});

			if (sender.Trim() == "admin")
			{
				messageUserControl.SetValue(MessageUserControl.IsAdminProperty, true);
			}

			this.MessageStackPanel.Children.Add(messageUserControl);
			MessageScrollViewer.ScrollToBottom();
		}

		private async Task ConnectServerAsync()
		{
			var (isSuccess, connectionStatus) = await _chatWebSocketHandler.ConnectSocketServer();
			MessageBox.Show(connectionStatus, "Connection Status", MessageBoxButton.OK, MessageBoxImage.Information);

			if (!isSuccess)
			{
				Close();
				return;
			}

			await ReceiveMessages();
		}

		private async Task ReceiveMessages()
		{
			var buffer = new byte[1024];

			while (_chatWebSocketHandler.ClientSocket.State == WebSocketState.Open)
			{
				var result = await _chatWebSocketHandler.ClientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					await _chatWebSocketHandler.ClientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
				}
				else
				{
					var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count).Split(':', 2);
					var sender = receivedMessage[0];
					var message = receivedMessage[1].Trim();
					AddMessage(sender, message, sender == _userInfo.Name);
				}
			}
		}

		private void MessageTxt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SendButton_ClickAsync(sender, e);
			}
		}
	}
}