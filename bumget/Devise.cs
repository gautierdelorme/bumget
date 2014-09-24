using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Devise
	{
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Devise(string symbole ,string description, double valueCAN)
		{
			db.CreateTable<Devise>();
			Description = description;
			Symbole = symbole;
			ValueCAN = valueCAN;
			db.Insert (this);
			Console.WriteLine("New category with symbole : "+Symbole+".");
		}

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public string Symbole {
			get;
			set;
		}

		public double ValueCAN {
			get;
			set;
		}

		public override string ToString ()
		{
			return "Category with symbole : " + Symbole + " and Description : " + Description + " and value in CAN : "+ValueCAN+".";
		}
	}
}