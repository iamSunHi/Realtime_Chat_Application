using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// Interaction logic for Message.xaml
	/// </summary>
	public partial class Message : UserControl
	{
		public MessageVM MessageVM { get; set; }

		public Message()
		{
			InitializeComponent();

			MessageVM = new()
			{
				Time = DateTime.Now.ToShortTimeString(),
				SenderName = "Sun Hi",
				MessageText = "Hi, is anyone online?",
				IsOwner = false
			};

			this.DataContext = MessageVM;
		}
	}
}
