// See https://aka.ms/new-console-template for more information
//DEV NOTES: Is there a template for an async main?
using Microsoft.Extensions.DependencyInjection;
using WarGames.CLI;

var provider = new Startup().Start();
var menu = provider.GetService<MenuController>();
if (menu == null)
	throw new Exception("Menu could not start");

while(menu.ShowMenu)
{
	menu.GetNextAction();
	menu.DoAction(string.Empty);
}

