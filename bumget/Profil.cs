﻿using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using bumget;

namespace bumget
{
	public class Profil
	{

		private string name;
		private string firstName;
		private int currency;
		private string subCategories;

		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Profil () : base() {
		}

		public Profil (string firstName, string name, int currency, string subCategories) {
			db.CreateTable<Profil>();
			Name = name;
			FirstName = firstName;
			Currency = currency;
			SubCategories = subCategories;
			db.Insert (this);
			Console.WriteLine("I'm here !! I'm "+FirstName+" "+Name+".");
		}

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
		}

		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
			
		public string FirstName {
			get {
				return firstName;
			}
			set {
				firstName = value;
			}
		}

		public int Currency {
			get {
				return currency;
			}
			set {
				currency = value;
			}
		}
			
		public string SubCategories {
			get {
				return subCategories;
			}
			set {
				subCategories = value;
			}
		}
			
		public override string ToString() {
			return "I'm "+FirstName+" "+Name+". I want "+Currency+" and SubCategories = "+SubCategories+".";
		}

		public void Remove() {
			db.Delete (this);
		}

		public void PrintTransact() {
			var transacts = db.Query<Transact> ("SELECT * FROM Transact WHERE OwnerId = ?",Id);
			Console.Write ("****************************************************\nMy transactions\n****************************************************\n**\n");
			foreach (Transact t in transacts) {
				Console.WriteLine ("** "+t);
			}
			Console.WriteLine ("**");
			Console.WriteLine ("****************************************************");
		}

		public void PrintTransact(DateTime begin, DateTime end) {
			var transacts = db.Query<Transact> ("SELECT * FROM Transact WHERE Date <= ? AND Date >= ? AND OwnerId = ?",end,begin,Id);
			Console.Write ("****************************************************\nMy transactions between "+begin.Date+" and "+end.Date+"\n****************************************************\n**\n");
			foreach (Transact t in transacts) {
				Console.WriteLine ("** "+t);
			}
			Console.WriteLine ("**");
			Console.WriteLine ("****************************************************");
		}

		public void PrintResume() {
			var expenses = db.Query<Transact> ("SELECT * FROM Transact WHERE OwnerId = ? AND Expense = 1", Id);
			var earnings = db.Query<Transact> ("SELECT * FROM Transact WHERE OwnerId = ? AND Expense = 0", Id);
			Console.Write ("****************************************************\nMy resume\n****************************************************\n**\n");
			double totalExpenses = 0;
			double totalEarnings = 0;
			foreach (var e in expenses) {
				totalExpenses += e.Amount;
			}
			foreach (var e in earnings) {
				totalEarnings += e.Amount;
			}
			Console.WriteLine ("** Total of earnings : " + totalEarnings +"("+Math.Round(100*totalEarnings/(totalExpenses+totalEarnings), 
				MidpointRounding.AwayFromZero)+"%)");
			Console.WriteLine ("** Total of expenses : " + totalExpenses+"("+Math.Round(100*totalExpenses/(totalExpenses+totalEarnings),MidpointRounding.AwayFromZero)+"%)\n**");
			Console.WriteLine ("****************************************************");
		}

		public void PrintResume(DateTime begin, DateTime end) {
			var expenses = db.Query<Transact> ("SELECT * FROM Transact WHERE Date <= ? AND Date >= ? AND OwnerId = ? AND Expense = 1",end, begin, Id);
			var earnings = db.Query<Transact> ("SELECT * FROM Transact WHERE Date <= ? AND Date >= ? AND OwnerId = ? AND Expense = 0",end, begin, Id);
			Console.Write ("****************************************************\nMy resume between "+begin.Date+" and "+end.Date+"\n****************************************************\n**\n");
			double totalExpenses = 0;
			double totalEarnings = 0;
			foreach (var e in expenses) {
				totalExpenses += e.Amount;
			}
			foreach (var e in earnings) {
				totalEarnings += e.Amount;
			}
			Console.WriteLine ("** Total of earnings : " + totalEarnings +" ("+Math.Round(100*totalEarnings/(totalExpenses+totalEarnings), 
				MidpointRounding.AwayFromZero)+"%)");
			Console.WriteLine ("** Total of expenses : " + totalExpenses+" ("+Math.Round(100*totalExpenses/(totalExpenses+totalEarnings),MidpointRounding.AwayFromZero)+"%)\n**");
			Console.WriteLine ("****************************************************");
		}

		public void AddTransact(int subCategoryIdT,string descriptionT,DateTime dateT,double amountT, bool expenseT) {
			new Transact (subCategoryIdT,descriptionT,dateT,amountT,Id,expenseT);
		}

		public void AddSubCategory(int idCategory) {
			SubCategories = SubCategories + "-" + idCategory.ToString();
			db.Execute("UPDATE Profil SET SubCategories = ? WHERE Id = ?",SubCategories,Id);
		}

		public void RemoveSubCategory(int idCategory) {
			List<string> listSubCategoriesnew = new List<string> (SubCategories.Split (new char[] { '-' }));
			listSubCategoriesnew.Remove (idCategory.ToString());
			SubCategories = string.Join("-", listSubCategoriesnew);
			db.Execute("UPDATE Profil SET SubCategories = ? WHERE Id = ?",SubCategories,Id);

		}

		public void Synchronize()
		{
			db.Execute("UPDATE Profil SET Name = ?, FirstName = ?, Currency = ?, SubCategories = ? WHERE Id = ?",Name,FirstName,Currency,SubCategories,Id);
		}
	}
}