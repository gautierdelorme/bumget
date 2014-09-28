using System;

namespace bumget
{
	public class User
	{
		//Champs
		public static Profil Profile;




		/// <summary>
		/// User home page
		/// </summary>
		/// <param name="userProfil">User profil.</param>
		public static void HomePage(Profil userProfil)
		{

			Console.Clear ();



			Console.WriteLine ("************UserHomePage************** ");
			Console.WriteLine ("Bonjour," + userProfil.Login+" ,nous sommes le :"+ DateTime.Today.ToString("d"));
			Console.WriteLine ("Résumés de vos transactions : ");
			Console.WriteLine ("*******************************");
			try{
				userProfil.PrintResume ();
			}
			catch (SQLite.SQLiteException e) {
				Console.WriteLine ("Vous n'avez pas encore de transaction");
			}

			Console.WriteLine ("*******************************");
			Console.WriteLine ("Appuyez sur une touche pour continuer...");
			Console.ReadKey();
			Console.WriteLine ("------Gestion de votre Profil------");
			Console.WriteLine("1-Ajouter une transaction");
			Console.WriteLine("2-Ajouter un budget");
			Console.WriteLine("3-Afficher ses transactions sur une période donnée");
			Console.WriteLine ("4-Modifier ses sous-categories");
			User.checkHomeChoice ();

		}

		/// <summary>
		/// Allow th euser to add a new transaction
		/// </summary>
		public static void transactionMenu ()
		{
		
		}
		/// <summary>
		/// Allow the user to modify his budget
		/// </summary>
		public static void budgetMenu ()
		{

		}
		/// <summary>
		/// allow the user to print transactions by specified dates
		/// </summary>
		public static void printCustomTransactionPage ()
		{

		}
		/// <summary>
		/// Allow the user to modify his subCategory
		/// </summary>
		public static void modifySubCategoryPage ()
		{

		}

		//Utility method for user pages

		public static void checkHomeChoice()
		{
			int choice;
			bool correctChoice=false;

			while (!correctChoice) {
				Console.Write("Veuillez saisir votre choix : ");
				choice = int.Parse (Console.ReadLine ());
				switch (choice) {
				case 1:
					User.transactionMenu ();
					correctChoice = true;
					break;
				case 2:
					User.budgetMenu ();
					correctChoice = true;
					break;
				case 3:
					User.printCustomTransactionPage ();
					correctChoice = true;
					break;
				case 4:
					User.modifySubCategoryPage ();
					correctChoice = true;
					break;
				default:
					Console.WriteLine ("Saisi incorrect");
					break;
				}


				}



		}



	}
}

