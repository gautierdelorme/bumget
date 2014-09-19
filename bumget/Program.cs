using System;
using System.IO;
using bumget;
using SQLite;

namespace bumget
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Profil MyP = new Profil("Pierre","Dupont",12);
			MyP.Name = "Delorme";
			MyP.Surname = "Gautier";
			Console.WriteLine (MyP.ToString());

			/*var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "users.db"));
			var table = db.Table<Profil> ();
			foreach (var s in table) {
				Console.WriteLine (s.ToString());
			}*/
		}
	}
}
