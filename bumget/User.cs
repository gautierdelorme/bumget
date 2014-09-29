using System;
using System.IO;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;

namespace bumget
{
	public class User
	{
		public static Profil CurrentUser;
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		/// <summary>
		/// User home page
		/// </summary>
		/// <param name="userProfil">User profil.</param>
		public static void HomePage()
		{

			Console.Clear ();

			Console.WriteLine ("*******************************");
			Console.WriteLine ("************ Home Page ********");
			Console.WriteLine ("*******************************");
			Console.WriteLine ("Hi " + CurrentUser.Login+" ! Today it's "+ DateTime.Today.ToString("d"));
			Console.WriteLine ("Your resume this month : ");
			try{
				CurrentUser.PrintResume (DateTime.Now.AddMonths(-1), DateTime.Now);
			}
			catch (SQLite.SQLiteException e) {
				Debug.WriteLine ("Exception : " + e);
				Console.WriteLine ("Sorry, your resume is not available");
			}

			Console.WriteLine ("------------------------------");
			Console.WriteLine ("------------ Menu ------------");
			Console.WriteLine ("------------------------------");
			Console.WriteLine("1 - Manage your transactions");
			Console.WriteLine("2 - Manage your budget");
			Console.WriteLine("3 - Print your resume");
			Console.WriteLine("4 - Manage your categories");
			Console.WriteLine("5 - Log out");
			Console.WriteLine("6 - Delete my account");
			User.checkHomeChoice ();

		}

		public static void checkHomeChoice()
		{
			Console.WriteLine("Enter your choice :");

			bool correctChoice=false;
			while (!correctChoice) {
				switch (Convert.ToInt32(Console.ReadLine ())) {
				case 1:
					correctChoice = true;
					User.transactionMenu ();
					break;
				case 2:
					correctChoice = true;
					User.budgetMenu ();
					break;
				case 3:
					correctChoice = true;
					User.resumeMenu ();
					break;
				case 4:
					correctChoice = true;
					User.categoriesMenu ();
					break;
				case 5:
					correctChoice = true;
					CurrentUser = null;
					Console.WriteLine ("You are disconnected. Press a key to come back to the Home Page.");
					Console.ReadKey ();
					Menu.HomePage ();
					break;
				case 6:
					correctChoice = true;
					User.DeleteAccount ();
					break;
				default:
					Console.WriteLine ("Enter the number of the action you want accomplish :");
					break;
				}
			}
		}

		public static void DeleteAccount() {
			Console.WriteLine ("Are you sure you want delete your account ? All your transactions and budget will be delete : (y/n)");
			bool correctChoice = false;
			while (!correctChoice) {
				switch (Console.ReadLine ()) {
				case "y":
					CurrentUser.Remove ();
					CurrentUser = null;
					Console.WriteLine ("Your account was deleted. Press a key to go to the Home Page");
					Console.ReadKey ();
					Menu.HomePage ();
					break;
				default:
					Console.WriteLine ("Your account was not deleted. Press a key to go to the Home Page");
					Console.ReadKey ();
					User.HomePage ();
					break;
				}
			}
		}
			
		public static void transactionMenu ()
		{
			Console.Clear ();
			Console.WriteLine ("------------------------------");
			Console.WriteLine ("------ Transaction menu ------");
			Console.WriteLine ("------------------------------");
			Console.WriteLine("1 - Add a transaction");
			Console.WriteLine("2 - Remove a transaction");
			Console.WriteLine("3 - Update a transaction");
			Console.WriteLine("4 - Print all your transactions");
			Console.WriteLine("5 - Print your transaction for a specific period");
			Console.WriteLine("6 - Back to the home page");

			Console.WriteLine("Enter your choice :");

			bool correctChoice=false;
			while (!correctChoice) {
				switch (Convert.ToInt32(Console.ReadLine ())) {
				case 1:
					correctChoice = true;
					User.AddTransaction ();
					break;
				case 2:
					correctChoice = true;
					User.RemoveTransaction ();
					break;
				case 3:
					correctChoice = true;
					User.UpdateTransaction ();
					break;
				case 4:
					correctChoice = true;
					User.PrintAllTransaction ();
					break;
				case 5:
					correctChoice = true;
					User.PrintTransactionPeriod ();
					break;
				case 6:
					correctChoice = true;
					User.HomePage ();
					break;
				default:
					Console.WriteLine ("Enter the number of the action you want accomplish :");
					break;
				}
			}
		}

		public static void AddTransaction() {
			CurrentUser.PrintCategories ();
			Console.WriteLine ("Enter the ID category of the transaction :");
			bool correctChoice = false;
			int CatId = 0;
			while (!correctChoice) {
				try {
					CatId = Convert.ToInt32 (Console.ReadLine ());
					if (CurrentUser.getCategoriesId().Contains(CatId)) {
						correctChoice = true;
					} else {
						Console.WriteLine ("Enter a correct ID :");
					}
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Enter a correct ID :");
				}
			}
			Console.WriteLine ("Enter the transaction's description :");
			string CatDes = Console.ReadLine ();
			Console.WriteLine ("Enter the transaction's date :");
			DateTime CatDate = DateTime.Now;
			correctChoice = false;
			while (!correctChoice) {
				try {
					CatDate = Convert.ToDateTime (Console.ReadLine ());
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct date format DD/MM/YYYY : ");
				}
			}
			Console.WriteLine ("Enter the transaction's amount :");
			double CatAmount = Convert.ToDouble (Console.ReadLine ());
			Console.WriteLine ("Is it an expense or gain ? (e/g) :");
			bool CatExpense = true;
			correctChoice = false;
			while (!correctChoice) {
				switch (Console.ReadLine ()) {
				case "e":
					correctChoice = true;
					break;
				case "g":
					correctChoice = true;
					CatExpense = false;
					break;
				default:
					Console.WriteLine ("Please enter 'e' or 'g' :");
					break;
				}
			}
			CurrentUser.AddTransact (db.Get<Category>(CatId), CatDes, CatDate, CatAmount, CatExpense);
			Console.WriteLine ("Transaction saved ! Press a key to come back to the transaction menu.");
			Console.ReadKey ();
			User.transactionMenu ();
		}

		public static void RemoveTransaction() {
			CurrentUser.PrintTransacts ();
			Console.WriteLine ("Enter the ID of the transaction you want to remove : ");
			bool correctChoice = false;
			while (!correctChoice) {
				try {
					CurrentUser.removeTransacById (Convert.ToInt32 (Console.ReadLine ()));
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct ID : ");
				}
			}
			Console.WriteLine ("Transaction removed ! Press a key to come back to the transaction menu.");
			Console.ReadKey ();
			User.transactionMenu ();
		}

		public static void UpdateTransaction() {
			CurrentUser.PrintTransacts ();
			Console.WriteLine ("Enter the ID of the transaction you want to update : ");
			bool correctChoice = false;
			while (!correctChoice) {
				try {
					CurrentUser.UpdateTransactById (Convert.ToInt32 (Console.ReadLine ()));
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct ID : ");
				}
			}
			Console.WriteLine ("Transaction updated ! Press a key to come back to the transaction menu.");
			Console.ReadKey ();
			User.transactionMenu ();
		}

		public static void PrintAllTransaction() {
			CurrentUser.PrintTransacts ();
			Console.WriteLine ("Press a key to come back to the transaction menu.");
			Console.ReadKey ();
			User.transactionMenu ();
		}

		public static void PrintTransactionPeriod () {
			Console.WriteLine ("Enter the period first date :");
			DateTime firstDate = DateTime.Now;
			bool correctChoice = false;
			while (!correctChoice) {
				try {
					firstDate = Convert.ToDateTime (Console.ReadLine ());
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct date format DD/MM/YYYY : ");
				}
			}
			Console.WriteLine ("Enter the period last date :");
			DateTime lastDate = DateTime.Now;
			correctChoice = false;
			while (!correctChoice) {
				try {
					lastDate = Convert.ToDateTime (Console.ReadLine ());
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct date format DD/MM/YYYY : ");
				}
			}
			CurrentUser.PrintTransacts (firstDate, lastDate);
			Console.WriteLine ("Press a key to come back to the transaction menu.");
			Console.ReadKey ();
			User.transactionMenu ();
		}

		/// <summary>
		/// Allow the user to modify his budget
		/// </summary>
		public static void budgetMenu ()
		{
			Console.Clear ();
			Console.WriteLine ("------------------------------");
			Console.WriteLine ("-------- Budget menu ---------");
			Console.WriteLine ("------------------------------");
			Console.WriteLine("1 - Add a mensual budget");
			Console.WriteLine("2 - Update your mensual budget");
			Console.WriteLine("3 - Add a budget to a category");
			Console.WriteLine("4 - Update the budget from a category");
			Console.WriteLine("5 - Print your mensual budget");
			Console.WriteLine("6 - Print the budget from a category");
			Console.WriteLine("7 - Back to the home page");

			Console.WriteLine("Enter your choice :");

			bool correctChoice=false;
			while (!correctChoice) {
				switch (Convert.ToInt32(Console.ReadLine ())) {
				case 1:
					correctChoice = true;
					User.AddMensualBudgetMenu ();
					break;
				case 2:
					correctChoice = true;
					User.UpdateMensualBudgetMenu ();
					break;
				case 3:
					correctChoice = true;
					User.AddCategoryBudgetMenu ();
					break;
				case 4:
					correctChoice = true;
					User.UpdateCategoryBudgetMenu ();
					break;
				case 5:
					correctChoice = true;
					User.PrintMensualBudget ();
					break;
				case 6:
					correctChoice = true;
					User.PrintCategoryBudget ();
					break;
				case 7:
					correctChoice = true;
					User.HomePage ();
					break;
				default:
					Console.WriteLine ("Enter the number of the action you want accomplish :");
					break;
				}
			}
		}

		public static void AddMensualBudgetMenu () {
			CurrentUser.CreateBudget ();
			Console.WriteLine ("Press a key to come back to the budget menu.");
			Console.ReadKey ();
			User.budgetMenu ();

		}

		public static void UpdateMensualBudgetMenu () {
			CurrentUser.UpdateBudget ();
			Console.WriteLine ("Press a key to come back to the budget menu.");
			Console.ReadKey ();
			User.budgetMenu ();

		}

		public static void AddCategoryBudgetMenu () {
			CurrentUser.PrintCategories ();
			Console.WriteLine ("Enter the ID category of the budget :");
			bool correctChoice = false;
			int CatId = 0;
			while (!correctChoice) {
				try {
					CatId = Convert.ToInt32 (Console.ReadLine ());
					if (CurrentUser.getCategoriesId().Contains(CatId)) {
						CurrentUser.CreateBudget (db.Get<Category>(CatId));
						correctChoice = true;
					} else {
						Console.WriteLine ("Enter a correct ID :");
					}
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Enter a correct ID :");
				}
			}
			Console.WriteLine ("Press a key to come back to the budget menu.");
			Console.ReadKey ();
			User.budgetMenu ();

		}

		public static void UpdateCategoryBudgetMenu () {
			CurrentUser.PrintCategories ();
			Console.WriteLine ("Enter the ID category of the budget :");
			bool correctChoice = false;
			int CatId = 0;
			while (!correctChoice) {
				try {
					CatId = Convert.ToInt32 (Console.ReadLine ());
					if (CurrentUser.getCategoriesId().Contains(CatId)) {
						CurrentUser.UpdateBudget (db.Get<Category>(CatId));
						correctChoice = true;
					} else {
						Console.WriteLine ("Enter a correct ID :");
					}
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Enter a correct ID :");
				}
			}
			Console.WriteLine ("Press a key to come back to the budget menu.");
			Console.ReadKey ();
			User.budgetMenu ();
		}

		public static void PrintMensualBudget () {
			CurrentUser.PrintBudget ();
			Console.WriteLine ("Press a key to come back to the budget menu.");
			Console.ReadKey ();
			User.budgetMenu ();
		}

		public static void PrintCategoryBudget () {
			CurrentUser.PrintCategories ();
			Console.WriteLine ("Enter the ID category of the budget :");
			bool correctChoice = false;
			int CatId = 0;
			while (!correctChoice) {
				try {
					CatId = Convert.ToInt32 (Console.ReadLine ());
					if (CurrentUser.getCategoriesId().Contains(CatId)) {
						CurrentUser.PrintBudget (db.Get<Category>(CatId));
						correctChoice = true;
					} else {
						Console.WriteLine ("Enter a correct ID :");
					}
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Enter a correct ID :");
				}
			}
			Console.WriteLine ("Press a key to come back to the budget menu.");
			Console.ReadKey ();
			User.budgetMenu ();
		}


		/// <summary>
		/// allow the user to print transactions by specified dates
		/// </summary>
		public static void resumeMenu ()
		{
			Console.Clear ();
			Console.WriteLine ("------------------------------");
			Console.WriteLine ("-------- Resume menu ---------");
			Console.WriteLine ("------------------------------");
			Console.WriteLine("1 - Print your complete resume");
			Console.WriteLine("2 - Print your resume for a specific period");
			Console.WriteLine("3 - Back to the home page");

			Console.WriteLine("Enter your choice :");

			bool correctChoice=false;
			while (!correctChoice) {
				switch (Convert.ToInt32(Console.ReadLine ())) {
				case 1:
					correctChoice = true;
					User.PrintResumeMenu ();
					break;
				case 2:
					correctChoice = true;
					User.PrintSpecificResumeMenu ();
					break;
				case 3:
					correctChoice = true;
					User.HomePage ();
					break;
				default:
					Console.WriteLine ("Enter the number of the action you want accomplish :");
					break;
				}
			}
		}

		public static void PrintResumeMenu () {
			Console.WriteLine ("Your total resume :");
			try {
				CurrentUser.PrintResume ();
			}
			catch (SQLite.SQLiteException e) {
				Debug.WriteLine ("Exception : " + e);
				Console.WriteLine ("Sorry, your resume is not available");
			}
			Console.WriteLine ("Press a key to come back to the resume menu.");
			Console.ReadKey ();
			User.resumeMenu ();
		}

		public static void PrintSpecificResumeMenu () {
			Console.WriteLine ("Your specific resume :");
			Console.WriteLine ("Enter the period first date :");
			DateTime firstDate = DateTime.Now;
			bool correctChoice = false;
			while (!correctChoice) {
				try {
					firstDate = Convert.ToDateTime (Console.ReadLine ());
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct date format DD/MM/YYYY : ");
				}
			}
			Console.WriteLine ("Enter the period last date :");
			DateTime lastDate = DateTime.Now;
			correctChoice = false;
			while (!correctChoice) {
				try {
					lastDate = Convert.ToDateTime (Console.ReadLine ());
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct date format DD/MM/YYYY : ");
				}
			}
			CurrentUser.PrintResume (firstDate, lastDate);
			Console.WriteLine ("Press a key to come back to the resume menu.");
			Console.ReadKey ();
			User.resumeMenu ();
		}

		/// <summary>
		/// Allow the user to modify his subCategory
		/// </summary>
		public static void categoriesMenu ()
		{
			Console.Clear ();
			Console.WriteLine ("------------------------------");
			Console.WriteLine ("------ Categories menu -------");
			Console.WriteLine ("------------------------------");
			Console.WriteLine("1 - Add a category");
			Console.WriteLine("2 - Remove a category");
			Console.WriteLine("3 - Back to the home page");

			Console.WriteLine("Enter your choice :");

			bool correctChoice=false;
			while (!correctChoice) {
				switch (Convert.ToInt32(Console.ReadLine ())) {
				case 1:
					correctChoice = true;
					User.AddCategoryMenu ();
					break;
				case 2:
					correctChoice = true;
					User.RemoveCategoryMenu ();
					break;
				case 3:
					correctChoice = true;
					User.HomePage ();
					break;
				default:
					Console.WriteLine ("Enter the number of the action you want accomplish :");
					break;
				}
			}
		}

		public static void AddCategoryMenu () {
			CurrentUser.PrintNotYoursCategories ();
			Console.WriteLine ("Enter category ID :");

			bool correctChoice = false;
			int CatId = 0;
			while (!correctChoice) {
				try {
					CatId = Convert.ToInt32 (Console.ReadLine ());
					if (CurrentUser.getNotYoursCategoriesId().Contains(CatId)) {
						CurrentUser.AddSubCategory (db.Get<Category>(CatId));
						correctChoice = true;
					} else {
						Console.WriteLine ("Enter a correct ID :");
					}
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Enter a correct ID :");
				}
			}
			Console.WriteLine ("Press a key to come back to the categories menu.");
			Console.ReadKey ();
			User.categoriesMenu ();
		}

		public static void RemoveCategoryMenu () {
			CurrentUser.PrintCategories ();
			Console.WriteLine ("Enter the ID category you want to remove :");
			bool correctChoice = false;
			int CatId = 0;
			while (!correctChoice) {
				try {
					CatId = Convert.ToInt32 (Console.ReadLine ());
					if (CurrentUser.getCategoriesId().Contains(CatId)) {
						CurrentUser.RemoveSubCategory (db.Get<Category>(CatId));
						correctChoice = true;
					} else {
						Console.WriteLine ("Enter a correct ID :");
					}
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Enter a correct ID :");
				}
			}
			Console.WriteLine ("Category removed !\nPress a key to come back to the categories menu.");
			Console.ReadKey ();
			User.categoriesMenu ();
		}
	}
}

