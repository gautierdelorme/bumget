﻿using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using bumget;
using System.Linq;
using System.Diagnostics;

namespace bumget
{
	public class Profil
	{
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Profil () : base() {
		}

		public Profil (string firstName, string name, Devise currency, string subCategories) {
			db.CreateTable<Profil>();
			Name = name;
			FirstName = firstName;
			Currency = currency;
			CurrencyId = currency.Id;
			SubCategories = subCategories;
			db.Insert (this);
		}

		#region Properties

		[PrimaryKey, AutoIncrement]
		public int Id {
			get;
			private set;
		}

		public string Name {
			get;
			set;
		}
			
		public string FirstName {
			get;
			set;
		}

		[Ignore]
		public Devise Currency {
			get;
			set;
		}

		public int CurrencyId {
			get;
			set;
		}
			
		public string SubCategories {
			get;
			set;
		}

		#endregion

		#region Methods
			
		public override string ToString() {
			return "I'm "+FirstName+" "+Name+". I want "+Currency+" and SubCategories = "+SubCategories+".";
		}

		public void Remove() {
			db.Delete (this);
		}

		public void PrintTransact(Transact t) {
			Console.Write (t);
		}

		public void PrintTransacts() {
			var transacts = getAllTransacts();
			Console.Write ("****************************************************\nMy transactions\n****************************************************\n**\n");
			foreach (Transact t in transacts) {
				string s = "";
				Category cat = db.Get<Category> (t.SubCategoryId);
				if (t.Expense)
					s = "-";
				else
					s = "+";
				Console.WriteLine ("** ("+cat.Name+") Transaction n°"+t.Id+" : "+t.Date.Date+" : "+s+t.Amount*Currency.ValueCAN+Currency.Symbole+" : "+t.Description);
			}
			Console.WriteLine ("**");
			Console.WriteLine ("****************************************************");
		}

		public void PrintTransacts(DateTime begin, DateTime end) {
			var transacts = getTransacts (begin, end);
			Console.Write ("****************************************************\nMy transactions between "+begin.ToShortDateString()+" and "+end.ToShortDateString()+"\n****************************************************\n**\n");
			foreach (Transact t in transacts) {
				string s = "";
				Category cat = db.Get<Category> (t.SubCategoryId);
				if (t.Expense)
					s = "-";
				else
					s = "+";
				Console.WriteLine ("** ("+cat.Name+") Transaction °"+t.Id+" : "+t.Date.Date+" : "+s+t.Amount*Currency.ValueCAN+Currency.Symbole+" : "+t.Description);
			}
			Console.WriteLine ("**");
			Console.WriteLine ("****************************************************");
		}

		public void PrintResume() {
			var expenses = getAllExpenses();
			var earnings = getAllEarnings();
			Console.Write ("****************************************************\nMy resume\n****************************************************\n**\n");
			double totalExpenses = 0;
			double totalEarnings = 0;
			foreach (var e in expenses) {
				totalExpenses += e.Amount*Currency.ValueCAN;
			}
			foreach (var e in earnings) {
				totalEarnings += e.Amount*Currency.ValueCAN;
			}
			Console.WriteLine ("** Total of earnings in ("+Currency.Symbole+"): " + totalEarnings +" ("+Math.Round(100*totalEarnings/(totalExpenses+totalEarnings), 
				MidpointRounding.AwayFromZero)+"%)");
			Console.WriteLine ("** Total of expenses in ("+Currency.Symbole+"): " + totalExpenses+" ("+Math.Round(100*totalExpenses/(totalExpenses+totalEarnings),MidpointRounding.AwayFromZero)+"%)\n**");
			Console.WriteLine ("****************************************************");
		}

		public void PrintResume(DateTime begin, DateTime end) {
			var expenses = getAllExpenses(begin, end);
			var earnings = getAllEarnings(begin, end);
			Console.Write ("****************************************************\nMy resume between "+begin.ToShortDateString()+" and "+end.ToShortDateString()+"\n****************************************************\n**\n");
			double totalExpenses = 0;
			double totalEarnings = 0;
			foreach (var e in expenses) {
				totalExpenses += e.Amount*Currency.ValueCAN;
			}
			foreach (var e in earnings) {
				totalEarnings += e.Amount*Currency.ValueCAN;
			}
			Console.WriteLine ("** Total of earnings in ("+Currency.Symbole+"): " + totalEarnings +" ("+Math.Round(100*totalEarnings/(totalExpenses+totalEarnings), 
				MidpointRounding.AwayFromZero)+"%)");
			Console.WriteLine ("** Total of expenses in ("+Currency.Symbole+"): " + totalExpenses+" ("+Math.Round(100*totalExpenses/(totalExpenses+totalEarnings),MidpointRounding.AwayFromZero)+"%)\n**");
			Console.WriteLine ("****************************************************");
		}

		public void AddTransact(int subCategoryIdT,string descriptionT,DateTime dateT,double amountT, bool expenseT) {
			new Transact (subCategoryIdT,descriptionT,dateT,amountT/Currency.ValueCAN,Id,expenseT);
		}

		public void RemoveTransact(Transact transaction) {
			db.Delete (transaction);
		}

		public void AddSubCategory(Category Cat) {
			SubCategories = SubCategories + "-" + Cat.Id.ToString();
			Synchronize ();
		}

		public void RemoveSubCategory(Category Cat) {
			List<string> listSubCategoriesnew = new List<string> (SubCategories.Split (new char[] { '-' }));
			listSubCategoriesnew.Remove (Cat.Id.ToString());
			SubCategories = string.Join("-", listSubCategoriesnew);
			Synchronize ();
		}

		#endregion

		#region Database SQL Methods

		public List<Transact> getAllTransacts() {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE OwnerId = ? ORDER BY SubCategoryId, Date", Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getTransacts(DateTime begin, DateTime end) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Date <= ? AND Date >= ? AND OwnerId = ? ORDER BY SubCategoryId, Date",end,begin,Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public Transact getTransactById(int id) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE OwnerId = ? and Id = ? ORDER BY SubCategoryId, Date", Id, id).First();
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getTransactByDescription(string myDescription) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Description = ? and OwnerId = ? ORDER BY SubCategoryId, Date", myDescription, Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getTransactBySubCategory(string myCategory) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE SubCategoryId = ? and OwnerId = ? ORDER BY SubCategoryId, Date", myCategory, Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getAllExpenses() {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Expense = 1 and OwnerId = ? ORDER BY SubCategoryId, Date", Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getAllEarnings() {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Expense = 0 and OwnerId = ? ORDER BY SubCategoryId, Date", Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getAllExpenses(DateTime begin, DateTime end) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Date <= ? AND Date >= ? AND OwnerId = ? AND Expense = 1 ORDER BY SubCategoryId, Date",end, begin, Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getAllEarnings(DateTime begin, DateTime end) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Date <= ? AND Date >= ? AND OwnerId = ? AND Expense = 0 ORDER BY SubCategoryId, Date",end, begin, Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public void removeTransacById(int id) {
			try {
				Transact t = db.Query<Transact> ("SELECT * FROM Transact WHERE OwnerId = ? AND Id = ?", Id, id).First();
				t.Remove();
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
			}
		}

		public void Synchronize()
		{
			db.Execute("UPDATE Profil SET Name = ?, FirstName = ?, CurrencyId = ?, SubCategories = ? WHERE Id = ?",Name,FirstName,Currency.Id,SubCategories,Id);
		}

		#endregion
	}
}