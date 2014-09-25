using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Category
	{
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Category () : base() {
		}

		public Category (string name, string description)
		{
			db.CreateTable<Category>();
			Name = name;
			Description = description;
			db.Insert (this);
		}

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
		}

		public string Name {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public void Remove() {
			db.Delete (this);
		}

		public override string ToString()
		{
			return "Category with name : " + Name + " and Description : " + Description + ".";
		}
	}
}