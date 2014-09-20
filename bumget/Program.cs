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
			//Tests de Category
			Console.WriteLine ("Tests de Category");
			Category cate = new Category ();
			cate.Description = "SoloBg";
			Console.WriteLine (cate.ToString ());
			cate.Id = 12;
			Console.WriteLine (cate.ToString ());
			cate.Name = "Solo";
			Console.WriteLine (cate.ToString ());
			Category cate1 = new Category (13, "Kebab", "Salade,tomate,oignons");
			Console.WriteLine (cate1.ToString ());
			//Tets de devise
			Console.WriteLine ("Tests de Devise");
			Devise dollarUS = new Devise ();
			Console.WriteLine (dollarUS.ToString ());
			dollarUS.Description = "dollar américains";
			Console.WriteLine (dollarUS.ToString ());
			dollarUS.Id = 2;
			Console.WriteLine (dollarUS.ToString ());
			dollarUS.Symbole = "$";
			Console.WriteLine (dollarUS.ToString ());
			Devise euro = new Devise (3,"euro européens" ,"eu");
			Console.WriteLine (euro.ToString ());

		}
	}
}
