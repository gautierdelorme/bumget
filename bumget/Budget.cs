using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Budget
	{
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Budget () : base() {
		}

		public Budget (int subCategoryId, int profilId, double value)
		{
			db.CreateTable<Budget>();
			SubCategoryId = subCategoryId;
			ProfilId = profilId;
			Value = value;
			db.Insert (this);
		}

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
		}

		// 0 for mensual budget
		public int SubCategoryId {
			get;
			set;
		}

		public int ProfilId {
			get;
			set;
		}

		public double Value {
			get;
			set;
		}

		public void Remove() {
			db.Delete (this);
		}

		public static void Update(double value, int id, int categoryId) {
			db.Execute ("UPDATE Budget SET Value = ? WHERE ProfilId = ? and SubCategoryId = ?", value, id, categoryId);
		}
	}
}

