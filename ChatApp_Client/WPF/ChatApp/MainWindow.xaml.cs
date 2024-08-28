using ChatApp.Models;
using ChatApp.Services.WebSocketServices;
using ChatApp.ViewModels;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ChatWebSocketHandler _chatWebSocketHandler = new ChatWebSocketHandler();

		public MainWindow()
		{
			InitializeComponent();

			_ = ConnectServerAsync();
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

			this.MessageStackPanel.Children.Add(messageUserControl);
		}

		private async Task ConnectServerAsync()
		{
			var connectionStatus = await _chatWebSocketHandler.ConnectSocketServer();
			MessageBox.Show(connectionStatus, "Connection Status", MessageBoxButton.OK, MessageBoxImage.Information);
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
					var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
					AddMessage("Client", message, false);
				}
			}
		}

		private async void SendButton_ClickAsync(object sender, RoutedEventArgs e)
		{
			if (_chatWebSocketHandler.ClientSocket.State == WebSocketState.Open)
			{
				var message = MessageTxt.Text;
				MessageTxt.Text = string.Empty;

				await _chatWebSocketHandler.SendMessage(message);

				AddMessage("You", message, true);
			}
			else
			{
				MessageBox.Show("WebSocket is not connected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}