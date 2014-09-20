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

		/*public List<int> SousCategorie {
			get;
			set;
		}*/

		public Profil (string surname, string name, int currency) {
			var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));
			db.CreateTable<Profil>();
			Name = name;
			Surname = surname;
			Currency = currency;
			db.Insert (this);
			Console.WriteLine("I'm here !! I'm "+Surname+" "+Name+".");
		}

		public override string ToString() {
			return "I'm "+Surname+" "+Name+".";
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

		public void AddSubCategory() {
		}

		public void RemoveSubCategory() {
		}
	}
}

