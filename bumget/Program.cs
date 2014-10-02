// BAGAYOGO Souleymane
// FOURCADE Pierre-Ange
// DELORME Gautier

using System;
using System.IO;
using bumget;
using SQLite;
using System.Collections.Generic;

namespace bumget
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Menu.Init ();
			Menu.HomePage ();
		}
	}
}