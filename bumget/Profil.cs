using System;
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

		/// <summary>
		/// Initializes a new instance of the <see cref="bumget.Profil"/> class.
		/// </summary>
		/// <param name="firstName">First name.</param>
		/// <param name="name">Name.</param>
		/// <param name="name">Password.</param>
		/// <param name="currency">Currency.</param>
		/// <param name="subCategories">Sub categories (a string with Id of subcategories like that "1-2-3").</param>
		public Profil (string login, string password, Devise currency, string subCategories) {
			db.CreateTable<Profil>();
			Login = login;
			Password = password;
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

		public string Login {
			get;
			set;
		}

		public string Password {
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
			return "I'm "+Login+". I want "+Currency+" and SubCategories = "+SubCategories+".";
		}

		public void Remove() {
			foreach (Transact t in getAllTransacts()) {
				db.Delete (t);
			}
			foreach (Budget b in getAllBudget()) {
				db.Delete (b);
			}
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
			if (totalEarnings + totalExpenses > 0) {
				Console.WriteLine ("** Total of earnings in ("+Currency.Symbole+"): " + totalEarnings +" ("+Math.Round(100*totalEarnings/(totalExpenses+totalEarnings), 
				MidpointRounding.AwayFromZero)+"%)");
				Console.WriteLine ("** Total of expenses in ("+Currency.Symbole+"): " + totalExpenses+" ("+Math.Round(100*totalExpenses/(totalExpenses+totalEarnings),MidpointRounding.AwayFromZero)+"%)\n**");
			} else {
				Console.WriteLine ("** Nothing to print...\n**");
			}
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
			if (totalEarnings + totalExpenses > 0) {
				Console.WriteLine ("** Total of earnings in (" + Currency.Symbole + "): " + totalEarnings + " (" + Math.Round (100 * totalEarnings / (totalExpenses + totalEarnings), 
					MidpointRounding.AwayFromZero) + "%)");
				Console.WriteLine ("** Total of expenses in (" + Currency.Symbole + "): " + totalExpenses + " (" + Math.Round (100 * totalExpenses / (totalExpenses + totalEarnings), MidpointRounding.AwayFromZero) + "%)\n**");
			} else {
				Console.WriteLine ("** Nothing to print...\n**");
			}
			Console.WriteLine ("****************************************************");
		}

		public void AddTransact(Category subCategoryT,string descriptionT,DateTime dateT,double amountT, bool expenseT) {
			new Transact (subCategoryT.Id,descriptionT,dateT,amountT/Currency.ValueCAN,Id,expenseT);

			if (isOutLimitBudget())
				Console.WriteLine("You exceed the limits for the month !");
			if (isOutLimitBudget(subCategoryT))
				Console.WriteLine("You exceed the limits for the '"+subCategoryT.Name+"' category !");
		}

		public void RemoveTransact(Transact transaction) {
			db.Delete (transaction);
		}

		private void ManageBudget(int Cat) {
			bool correctChoice = false;
			while (!correctChoice) {
				switch (Console.ReadLine ()) {
				case "y":
					Console.WriteLine ("Enter the budget in "+Currency.Symbole+" : ");
					try {
						if (getBudgetByCategoryId(Cat) == null)
							new Budget (Cat, Id, Convert.ToDouble(Console.ReadLine ())/Currency.ValueCAN);
						else
							Budget.Update(Convert.ToDouble(Console.ReadLine ())/Currency.ValueCAN,Id,Cat);
					}
					catch (System.FormatException e) {
						Debug.WriteLine ("Exception : " + e);
						Console.WriteLine ("Please do not use a '.' but a ','. Try again ? (y/n)");
						break;
					}
					Console.WriteLine ("Budget saved. You will be able to modify it later.");
					correctChoice = true;
					break;
				case "n":
					Console.WriteLine ("No budget saved. You will be able to specify it later.");
					correctChoice = true;
					break;
				default:
					Console.WriteLine ("Enter 'y' or 'n' : ");
					break;
				}
			}
		}

		public void CreateBudget() {
			Console.WriteLine ("Do you want to create a budget for the month ? (y/n)");
			ManageBudget (0);
		}

		public void CreateBudget(Category Cat) {
			Console.WriteLine ("Do you want to create a budget for the '"+Cat.Name+"' category ? (y/n)");
			ManageBudget (Cat.Id);
		}

		public void UpdateBudget(Category Cat) {
			Console.WriteLine ("Do you want to update the budget for the '"+Cat.Name+"' category ? (y/n)");
			ManageBudget (Cat.Id);
		}

		public void UpdateBudget() {
			Console.WriteLine ("Do you want to update the budget for the month ? (y/n)");
			ManageBudget (0);
		}

		public void UpdateTransactById(int TransactId) {
			Transact myTransact = db.Get<Transact> (TransactId);
			Console.WriteLine ("Enter the new amount : ");
			bool correctChoice = false;
			while (!correctChoice) {
				try {
					myTransact.Amount = Convert.ToDouble (Console.ReadLine ())/Currency.ValueCAN;
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct amount : ");
				}
			}
			myTransact.Synchronize ();
		}

		public void AddSubCategory(Category Cat) {
			if (SubCategories.Length <= 0) {
				SubCategories = Cat.Id.ToString ();
			} else {
				SubCategories = SubCategories + "-" + Cat.Id.ToString ();
			}
			Synchronize ();
			Console.WriteLine ("Do you want to create a budget for the '"+Cat.Name+"' category ? (y/n)");
			ManageBudget (Cat.Id);
		}

		public void RemoveSubCategory(Category Cat) {
			List<string> listSubCategoriesnew = new List<string> (SubCategories.Split (new char[] { '-' }));
			listSubCategoriesnew.Remove (Cat.Id.ToString());
			SubCategories = string.Join("-", listSubCategoriesnew);
			removeBudgetBySubCategoryId (Cat.Id);
			Synchronize ();
		}

		public void PrintCategories () {
			Console.WriteLine ("List of your categories : ");
			List<string> categories = new List<string> (SubCategories.Split (new char[] { '-' }));
			foreach (string c  in categories) {
				Category Cat = db.Get<Category> (Convert.ToInt32(c));
				Console.WriteLine ("(ID " + Cat.Id +") : "+ Cat.Name +"("+Cat.Description+")");
			}
		}

		public void PrintNotYoursCategories () {
			Console.WriteLine ("List of categories you can add : ");
			List<string> categories = new List<string> (SubCategories.Split (new char[] { '-' }));
			List<Category> categoriesId = db.Query<Category> ("SELECT * FROM Category"); 
			foreach (Category Cat  in categoriesId) {
				if (!categories.Contains(Convert.ToString(Cat.Id))) {
					Console.WriteLine ("(ID " + Cat.Id +") : "+ Cat.Name +"("+Cat.Description+")");
				}
			}
		}

		public List<int> getNotYoursCategoriesId () {
			List<string> categories = new List<string> (SubCategories.Split (new char[] { '-' }));
			List<Category> categoriesObject = db.Query<Category> ("SELECT * FROM Category"); 
			List<int> categoriesId = new List<int> ();
			foreach (Category Cat  in categoriesObject) {
				if (!categories.Contains (Convert.ToString (Cat.Id))) {
					categoriesId.Add (Cat.Id);
				}
			}
			return categoriesId;
		}

		public List<int> getCategoriesId () {
			List<string> categories = new List<string> (SubCategories.Split (new char[] { '-' }));
			List<int> categoriesId = new List<int> ();
			foreach (string c  in categories) {
				categoriesId.Add(Convert.ToInt32(c));
			}
			return categoriesId;
		}
			
		public void PrintBudget(Category Cat) {
			Budget MyBudget = getBudgetByCategoryId (Cat.Id);
			Console.WriteLine ("****************************************************");
			if (MyBudget != null) {
				Console.WriteLine ("** Budget for '" + Cat.Name + "' category : " + MyBudget.Value * Currency.ValueCAN + " " + Currency.Symbole);
				Console.WriteLine ("** Total expenses for this category this month : " + getTransactsValuesThisMonth (Cat) * Currency.ValueCAN + " " + Currency.Symbole + " (" + Math.Round (100 * getTransactsValuesThisMonth (Cat) / MyBudget.Value, 
					MidpointRounding.AwayFromZero) + "% of the budget)");
				if (isOutLimitBudget (Cat))
					Console.WriteLine ("** You exceed the limits for this category !");
			}
			else
				Console.WriteLine ("** No budget.");
			Console.WriteLine ("****************************************************");
		}

		public void PrintBudget() {
			Budget MyBudget = getMensualBudget ();
			Console.WriteLine ("****************************************************");
			if (MyBudget != null) {
				Console.WriteLine ("** Budget for the month : " + MyBudget.Value * Currency.ValueCAN + " " + Currency.Symbole);
				Console.WriteLine ("** Total expenses for the month : " + getTransactsValuesThisMonth() * Currency.ValueCAN + " " + Currency.Symbole+" ("+Math.Round(100*getTransactsValuesThisMonth()/MyBudget.Value, 
					MidpointRounding.AwayFromZero)+"% of the budget)");
				if (isOutLimitBudget ())
					Console.WriteLine ("** You exceed the limits for the month !");
			}
			else
				Console.WriteLine ("** No budget.");
			Console.WriteLine ("****************************************************");
		}

		public double getBudgetValue() {
			Budget MyBudget = getMensualBudget ();
			if (MyBudget != null)
				return MyBudget.Value;
			else
				return 0;
		}

		public double getBudgetValue(Category Cat) {
			Budget MyBudget = getBudgetByCategoryId (Cat.Id);
			if (MyBudget != null)
				return MyBudget.Value;
			else
				return 0;
		}

		public double getTransactsValuesThisMonth(Category Cat) {
			List<Transact> transacts = getAllExpensesBySubCategoryThisMonth (Cat.Id);
			double total = 0;
			foreach (Transact t in transacts) {
				total += t.Amount;
			}
			return total;
		}

		public double getTransactsValuesThisMonth() {
			List<Transact> transacts = getAllExpensesThisMonth ();
			double total = 0;
			foreach (Transact t in transacts) {
				total += t.Amount;
			}
			return total;
		}

		public bool isOutLimitBudget () {
			return (getTransactsValuesThisMonth () > getBudgetValue ()) && (getMensualBudget () != null);
		}

		public bool isOutLimitBudget (Category Cat) {
			return (getTransactsValuesThisMonth (Cat) > getBudgetValue (Cat)) && (getBudgetByCategoryId (Cat.Id) != null);
		}

		#endregion

		#region Database SQL Methods

		public List<Category> getAllCategories() {
			try {
				List<string> listCategoriesId = new List<string> (SubCategories.Split (new char[] { '-' }));
				List<Category> listCategories = new List<Category>();
				foreach (string c in listCategoriesId) {
					listCategories.Add(db.Get<Category>(Convert.ToInt32(c)));
				}
				return listCategories;
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

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

		public List<Transact> getTransactBySubCategory(int myCategory) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE SubCategoryId = ? and OwnerId = ? ORDER BY SubCategoryId, Date", myCategory, Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getAllExpensesBySubCategory(int myCategory) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Expense = 1 and OwnerId = ? and SubCategoryId = ? ORDER BY SubCategoryId, Date", Id, myCategory);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getAllExpensesBySubCategoryThisMonth(int myCategory) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Expense = 1 and OwnerId = ? and SubCategoryId = ? and Date >= date('now', '-1 month') ORDER BY SubCategoryId, Date", Id, myCategory);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Transact> getAllExpensesThisMonth() {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Expense = 1 and OwnerId = ? and Date >= date('now', '-1 month') ORDER BY SubCategoryId, Date", Id);
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

		public List<Transact> getAllEarningsBySubCategory(int myCategory) {
			try {
				return db.Query<Transact> ("SELECT * FROM Transact WHERE Expense = 0 and OwnerId = ? and SubCategoryId = ? ORDER BY SubCategoryId, Date", Id, myCategory);
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
			Transact t = db.Query<Transact> ("SELECT * FROM Transact WHERE OwnerId = ? AND Id = ?", Id, id).First();
			t.Remove();
		}

		public void removeBudgetBySubCategoryId(int id) {
			try {
				Budget t = db.Query<Budget> ("SELECT * FROM Budget WHERE ProfilId = ? AND SubCategoryId = ?", Id, id).First();
				t.Remove();
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
			}
		}

		public Budget getBudgetByCategoryId(int id) {
			try {
				return db.Query<Budget> ("SELECT * FROM Budget WHERE ProfilId = ? AND SubCategoryId = ?", Id, id).First();
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public Budget getMensualBudget() {
			try {
				return db.Query<Budget> ("SELECT * FROM Budget WHERE ProfilId = ? AND SubCategoryId = 0", Id).First();
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public List<Budget> getAllBudget() {
			try {
				return db.Query<Budget> ("SELECT * FROM Budget WHERE ProfilId = ?", Id);
			}
			catch (System.InvalidOperationException e){
				Debug.WriteLine ("Exception : " + e);
				return null;
			}
		}

		public void Synchronize()
		{
			db.Execute("UPDATE Profil SET Login = ?, CurrencyId = ?, SubCategories = ?, Password = ? WHERE Id = ?",Login,Currency.Id,SubCategories,Password,Id);
		}

		#endregion
	}
}