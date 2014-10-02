// BAGAYOGO Souleymane
// FOURCADE Pierre-Ange
// DELORME Gautier

using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Devise
	{
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Devise () : base() {
		}

		public Devise(string symbole ,string description, double valueCAN)
		{
			db.CreateTable<Devise>();
			Description = description;
			Symbole = symbole;
			ValueCAN = valueCAN;
			db.Insert (this);
		}

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
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

		public void Remove() {
			db.Delete (this);
		}

		public override string ToString ()
		{
			return "Devise with symbole : " + Symbole + " and Description : " + Description + " and value in CAN : "+ValueCAN+".";
		}
	}
}