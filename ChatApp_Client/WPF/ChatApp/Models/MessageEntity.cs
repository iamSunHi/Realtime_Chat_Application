using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models
{
	public class MessageEntity
	{
		public string Time { get; set; } = null!;
		public string SenderName { get; set; } = null!;
		public string MessageText { get; set; } = null!;
		public bool IsOwner { get; set; } = false;
	}
}