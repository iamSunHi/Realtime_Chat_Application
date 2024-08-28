using ChatApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp
{
	/// <summary>
	/// Interaction logic for MessageUserControl.xaml
	/// </summary>
	public partial class MessageUserControl : UserControl
	{
		public MessageUserControl()
		{
			InitializeComponent();

			this.DataContext = this;
		}

		public MessageVM MessageVM
		{
			get { return (MessageVM)GetValue(messageVMProperty); }
			set { SetValue(messageVMProperty, value); }
		}

		// Using a DependencyProperty as the backing store for messageVM.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty messageVMProperty =
			DependencyProperty.Register("MessageVM", typeof(MessageVM), typeof(MessageUserControl), new PropertyMetadata(new MessageVM()));

	}
}
