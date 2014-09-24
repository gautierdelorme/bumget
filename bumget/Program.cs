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
			Devise Euro = new Devise ("€", "euros", 0.7);
			Devise Dollar = new Devise ("$", "dollars", 1);
			Profil MyP = new Profil("Pierre","Dupont",Dollar,"");
			MyP.Name = "Delorme";
			MyP.FirstName = "Gautier";
			MyP.Synchronize ();
			Console.WriteLine (MyP);

			Profil MyP2 = new Profil("Souleymane","Bagayogo",Euro, "");
			Console.WriteLine (MyP2);
			MyP2.AddTransact (1,"Beers",DateTime.Now,12.06,true);
			MyP2.AddTransact (1,"Beers",new DateTime (1994, 03, 04),12.06,true);
			MyP2.AddTransact (1,"Beers",new DateTime (1994, 11, 21),21.06,false);
			MyP2.AddTransact (1,"Beers",new DateTime (1881, 06, 16),54.06,true);
			MyP2.AddTransact (1,"Beers",new DateTime (1994, 11, 21),98.06,false);
			MyP2.AddSubCategory (new Category("Vacances", "Dépenses en vacances"));
			Console.WriteLine (MyP2);

			MyP2.RemoveSubCategory (2);

			/*Console.WriteLine (MyP2.ToString());

			MyP2.PrintTransact ();
			MyP2.PrintTransact (new DateTime (1992, 06, 16), new DateTime (2000, 06, 16));

			MyP2.PrintResume ();
			MyP2.PrintResume (new DateTime (1992, 06, 16), new DateTime (2000, 06, 16));*/

		}
	}
}