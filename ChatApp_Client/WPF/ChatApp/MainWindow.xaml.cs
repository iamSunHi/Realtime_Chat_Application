using ChatApp.Models;
using ChatApp.ViewModels;
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
		public MainWindow()
		{
			InitializeComponent();

			var messageUserControl = new MessageUserControl();
			messageUserControl.SetValue(MessageUserControl.messageVMProperty, new MessageVM
			{
				Message = new MessageEntity()
				{
					Time = DateTime.Now.ToShortTimeString(),
					SenderName = "Nhat Huy",
					MessageText = "Hi, is anyone online?",
					IsOwner = true
				}
			});

			this.MessageStackPanel.Children.Add(messageUserControl);

			var messageUserControl_1 = new MessageUserControl();
			messageUserControl_1.SetValue(MessageUserControl.messageVMProperty, new MessageVM
			{
				Message = new MessageEntity()
				{
					Time = DateTime.Now.ToShortTimeString(),
					SenderName = "Sun Hi",
					MessageText = "I'm here!",
					IsOwner = false
				}
			});

			this.MessageStackPanel.Children.Add(messageUserControl_1);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}
}