using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGames.CLI.Views
{
	internal class EmptyView : ConsoleView
	{
		public EmptyView() : base(new Dictionary<string, Action<string>>())
		{
		}

		public override string Title => String.Empty;
	}
}
