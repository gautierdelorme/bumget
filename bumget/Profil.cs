using System;
using System.IO;
using SQLite;
using bumget;

namespace bumget
{
	public class Profil
	{
		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
		}

		private string name;
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
				var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
				db.Execute("UPDATE Profil SET Name = ? WHERE Id = ?",Name,Id);
			}
		}

		private string surname;
		public string Surname {
			get {
				return surname;
			}
			set {
				surname = value;
				var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
				db.Execute("UPDATE Profil SET Surname = ? WHERE Id = ?",Surname,Id);
			}
		}

		private int currency;
		public int Currency {
			get {
				return currency;
			}
			set {
				currency = value;
				var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
				db.Execute("UPDATE Profil SET Currency = ? WHERE Id = ?",Currency,Id);
			}
		}

		private string subCategories;
		public string SubCategories {
			get {
				return subCategories;
			}
			set {
				subCategories = value;
				var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
				db.Execute("UPDATE Profil SET SubCategories = ? WHERE Id = ?",SubCategories,Id);
			}
		}

		public Profil() : base() {}

		public Profil (string surname, string name, int currency, string subCategories) {
			var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
			db.CreateTable<Profil>();
			Name = name;
			Surname = surname;
			Currency = currency;
			SubCategories = subCategories;
			db.Insert (this);
			Console.WriteLine("I'm here !! I'm "+Surname+" "+Name+".");
		}

		public override string ToString() {
			return "I'm "+Surname+" "+Name+". I want "+Currency+" and SubCategories = "+SubCategories+".";
		}

		public void Remove() {
			var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
			db.Delete (this);
		}

		public string PrintTransaction(Transaction transaction) {
			return "";
			//return transaction.ToString;
		}

		public string PrintResume() {
			return "";
		}

		public void AddTransaction() {
		}

		public void AddSubCategory(int idCategory) {
			// A MODIFIER
			var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
			Profil User = db.Get<Profil>(Id);
			Console.WriteLine (User.ToString ());
			//db.Execute("UPDATE Profil SET SubCategories = ? WHERE Id = ?",User.SubCategories + "-" + idCategory.ToString(),Id);
		}

		public void RemoveSubCategory() {
		}
	}
}