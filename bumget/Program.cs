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
			Profil MyP = new Profil("Pierre","Dupont",12,"1-2");
			MyP.Name = "Delorme";
			MyP.Surname = "Gautier";
			Console.WriteLine (MyP.ToString());

			Profil MyP2 = new Profil("Souleymane","Bagayogo",12, "2-5");
			MyP2.AddSubCategory (1);
			Console.WriteLine (MyP2.ToString());

			/*var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "users.db"));
			var table = db.Table<Profil> ();
			foreach (var s in table) {
				Console.WriteLine (s.ToString());
			}*/
		}
	}
}
