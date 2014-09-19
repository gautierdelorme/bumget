using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Profil
	{
		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			set;
		}
		private string nom;
		public string Nom {
			get {
				return nom;
			}
			set {
				nom = value;
				var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "users.db"));
				db.Execute("UPDATE Profil SET Nom = ? WHERE Id = ?",Nom,Id);
			}
		}

		public string Prenom {
			get;
			set;
		}

		public int Devise {
			get;
			set;
		}

		/*public List<uint> SousCategorie {
			get;
			set;
		}*/

		public Profil (string prenom, string nom, int devise) {
			Nom = nom;
			Prenom = prenom;
			Devise = devise;
			var db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "users.db"));
			db.CreateTable<Profil>();
			db.Insert (this);
			Console.WriteLine("Je suis créé ! Je suis "+Prenom+" "+Nom+".");
		}

		public override string ToString() {
			return "Je suis "+Prenom+" "+Nom+".";
		}

		public void Remove() {
		}

		public string PrintTransaction() {
			return "";
		}

		public string PrintResume() {
			return "";
		}

		public void AddTransaction() {
		}

		public void AddSousCategorie() {
		}

		public void RemoveSousCategorie() {
		}
	}
}

