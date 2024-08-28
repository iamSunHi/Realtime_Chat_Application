using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
	public class MessageVM
	{
		public MessageEntity Message { get; set; }

		public MessageVM()
		{
			Message = new MessageEntity
			{
				Time = DateTime.Now.ToShortTimeString(),
				SenderName = "Sun Hi",
				MessageText = "Hi, is anyone online?",
				IsOwner = false
			};
		}
	}
}
