// BAGAYOGO Souleymane
// FOURCADE Pierre-Ange
// DELORME Gautier

using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Transact
	{
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Transact () : base() {
		}

		public Transact (int subCategoryId,string description,DateTime date,double amount,int ownerId, bool expense)
		{
			db.CreateTable<Transact>();
			SubCategoryId = subCategoryId;
			Description = description;
			Date = date;
			Amount = amount;
			OwnerId = ownerId;
			Expense = expense;
			db.Insert (this);
		}

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
		}

		public int OwnerId {
			get;
			set;
		}

		public bool Expense {
			get;
			set;
		}

		public int SubCategoryId {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public DateTime Date {
			get;
			set;
		}

		public double Amount {
			get;
			set;
		}

		public void Remove() {
			db.Delete (this);
		}

		public override string ToString()
		{
			return "("+Id+") User :" + OwnerId + ", Montant: " + Amount + "CAN$, Date: " + Date.ToString () + ", Category : " + SubCategoryId + ", Description : " + Description + ", Expense = "+Expense;
		}

		public void Synchronize()
		{
			db.Execute("UPDATE Transact SET OwnerId = ?, SubCategoryId = ?, Description = ?, Date = ?, Amount = ?, Expense = ? WHERE Id = ?",OwnerId,SubCategoryId,Description,Date,Amount,Expense,Id);
		}
	}
}