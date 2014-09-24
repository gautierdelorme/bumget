using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Transact
	{
		private int ownerId;
		private int subCategoryId;
		private string description;
		private DateTime date;
		private double amount;
		private bool expense;

		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Transact () : base() {
		}

		public Transact (int subCategoryIdT,string descriptionT,DateTime dateT,double amountT,int ownerIdT, bool expenseT)
		{
			db.CreateTable<Transact>();
			SubCategoryId = subCategoryIdT;
			Description = descriptionT;
			Date = dateT;
			Amount = amountT;
			OwnerId = ownerIdT;
			Expense = expenseT;
			db.Insert (this);
			Console.WriteLine("I'm here !! I'm "+description+".");

		}

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
		}

		public int OwnerId
		{
			get{
				return ownerId;
			}
			set{
				ownerId = value;
			}

		}

		public bool Expense
		{
			get{
				return expense;
			}
			set{
				expense = value;
			}

		}

		public int SubCategoryId
		{
			get {
				return subCategoryId;
			}
			set{
				subCategoryId = value;
			}
		}

		public string Description
		{
			get{
				return description;
			}
			set{
				description = value;
			}
		}

		public DateTime Date
		{
			get{
				return date;
			}
			set{
				date = value;
			}
		}

		public double Amount
		{
			get{
				return amount;
			}
			set{
				amount = value;
			}
		}

		public override string ToString()
		{
			return "User :" + OwnerId + ", Montant: " + Amount + "CAN$, Date: " + Date.ToString () + ", Category : " + SubCategoryId + ", Description : " + Description + ", Expense = "+Expense;
		}

		public void Synchronize()
		{
			db.Execute("UPDATE Transact SET OwnerId = ?, SubCategoryId = ?, Description = ?, Date = ?, Amount = ?, Expense = ? WHERE Id = ?",OwnerId,SubCategoryId,Description,Date,Amount,Expense,Id);
		}

	}
}