using System;
using System.IO;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace bumget
{
	public class Menu
	{
		//Champs
		int IsRegistered { get; set;}
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Menu () : base (){
		}

		public static void Init() {
			try {
				db.Query<Profil> ("SELECT * FROM Profil");
			}
			catch (SQLite.SQLiteException e){
				Debug.WriteLine ("Exception : " + e);

				db.CreateTable<Profil> ();
				db.CreateTable<Transact> ();
				db.CreateTable<Budget> ();
				db.CreateTable<Category> ();
				new Category ("Restaurant", "Restaurant");
				new Category ("Grocery", "Grocery");
				new Category ("Rental", "Rental");
				new Category ("Equipment", "Household");
				new Category ("Costs", "Education costs");
				new Category ("Supplies", "Education Supplies");
				new Category ("Sport", "Sport");
				new Category ("Nightlife", "Nightlife");
				new Category ("Bus", "Bus transport");
				new Category ("Fuel", "Fuel transport");

				db.CreateTable<Devise> ();
				new Devise ("€", "euros", 0.7);
				new Devise ("CAD", "canadian dollar", 1);
				new Devise ("$", "american dollar", 0.9);
				new Devise ("JPY", "japanese yen", 98);
			}
		}


		//Methods
		/// <summary>
		/// HomePage seen when the program is launched.
		/// </summary>
		public static void HomePage()
		{
			Console.Clear ();
			Console.WriteLine ("*****************************************");
			Console.WriteLine ("******* Welcome in GSP-Transaction ******");
			Console.WriteLine ("*****************************************");
			Console.WriteLine ("Please select a number between 1 and 2: ");
			Console.WriteLine ("1 - Log in");
			Console.WriteLine ("2 - Sign up");
			Console.WriteLine ("Choice : ");
			bool correctchoice = false;
			while (!correctchoice) {
				try {
					switch (Convert.ToInt32 (Console.ReadLine ())) {
					case 1 :
						correctchoice = true;
						Menu.AuthentificationPage ();
						break;
					case 2 :
						correctchoice = true;
						Menu.ProfilCreationPage ();
						break;
					default :
						Console.WriteLine ("Incorrect choice, select a number between 1 and 2 : ");
						break;
					}
				}
				catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Incorrect choice, select a number between 1 and 2 : ");
				}
			}
		}

		/// <summary>
		/// Allow new user to login
		/// </summary>
		public static void AuthentificationPage()
		{
			Console.Clear ();
			Console.WriteLine ("*******************************");
			Console.WriteLine ("**** Authentification Page ****");
			Console.WriteLine ("*******************************");

			bool correctLogin = false;
			bool keepGoing=true;

			while (keepGoing && !correctLogin) {
				Console.WriteLine ("Please enter your login : ");
				string login = Console.ReadLine ();
				Console.WriteLine ("Please enter your password : ");
				string password = Console.ReadLine ();
				try {
					Profil userProfile = db.Query<Profil> ("SELECT * FROM Profil WHERE Login = ? and Password = ?", login, password).First();
					userProfile.Currency = db.Get<Devise>(userProfile.CurrencyId);
					correctLogin = true;
					User.CurrentUser = userProfile;
					User.HomePage ();
				}
				catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Incorrect informations");
					Console.WriteLine ("Try again ? (y/n)");
					switch (Console.ReadLine ()) {
					case "n":
						keepGoing = false;
						Menu.HomePage ();
						break;
					case "y":
						break;
					default:
						keepGoing = false;
						Menu.HomePage ();
						break;
					}
				}
			}
		}
		/// <summary>
		/// Allow new user to create  a new Profile
		/// </summary>
		public static void ProfilCreationPage()
		{
			Console.Clear ();
			Console.WriteLine ("*******************************");
			Console.WriteLine ("**** Profil Creation Page *****");
			Console.WriteLine ("*******************************");

			string login = Menu.userLoginCreation ();
			string password = Menu.userPasswordCreation ();
			Devise userCurrency = Menu.userFavouriteCurrencyCreation ();

			Profil currentProfil = new Profil (login, password, userCurrency, "");

			Menu.userCategoryCreation (currentProfil);
			Console.WriteLine ("\n****************************************");
			Console.WriteLine ("Profil saved. You will be able to log in.\nPlease push a key to continue...");
			Console.WriteLine ("******************************************");
			Console.ReadKey ();
			Menu.HomePage ();
		}

		//Utilitary function for Menu class

		/// <summary>
		/// Allow user to create his login
		/// </summary>
		/// <returns>user's login</returns>
		public static string userLoginCreation()
		{
			Console.WriteLine ("Please enter a login : ");
			string login = "";
			while (true) {
				try {
					login = Console.ReadLine ();
				}
				catch (Exception e) {
					Debug.WriteLine("Exception : "+e);
					Console.WriteLine ("Please enter a correct login : ");
				}
				try {
					db.Query<Profil> ("SELECT * FROM Profil WHERE Login = ?", login).First ();
					Console.WriteLine ("Please enter an other login : ");
				} catch (Exception e) {
					Debug.WriteLine("Exception : "+e);
					return login;
				}
			}
		}

		/// <summary>
		/// Allow user to create his own password
		/// </summary>
		/// <returns>a string which contain user's password</returns>
		public static string userPasswordCreation()
		{
			Console.WriteLine ("Please enter your password : ");
			while (true) {
				try {
					return Console.ReadLine ();
				}
				catch (Exception e) {
					Debug.WriteLine("Exception : "+e);
					Console.WriteLine ("Please enter a correct password : ");
				}
			}
		}

		/// <summary>
		/// Ask the user for create his own subcategorie 
		/// </summary>
		public static void userCategoryCreation(Profil currentProfil)
		{
			Console.WriteLine ("Please select in what categories are you interested : ");
			List<Category> categories = db.Query<Category> ("SELECT * FROM Category");
			foreach (Category c  in categories) {
				Console.WriteLine ("(ID " + c.Id +") : "+ c.Name +"("+c.Description+")");
			}

			Console.WriteLine ("Enter categories IDs separated with '-' : ");
			List<string> listIds = new List<string> (Console.ReadLine ().Split (new char[] { '-' }));

			foreach (string id in listIds) {
				try {
					currentProfil.AddSubCategory(db.Get<Category> (Convert.ToInt32(id)));
				} catch (Exception e) {
					Debug.WriteLine("Exception : "+e);
					Console.WriteLine ("The last ID was not correct. Precedents IDs were saved. You will be able to manage your categories later.");
				}
			}
		}

		/// <summary>
		/// Allow user to create his own devise
		/// </summary>
		/// <returns>The user devise.</returns>
		public static Devise userDeviseCreation ()
		{
			Console.WriteLine ("Enter the devise symbol : ");
			string deviseSymbol = Console.ReadLine ();

			Console.WriteLine ("Enter the devise description : ");
			string description = Console.ReadLine ();

			bool correctChoice = false;
			double currencyRate = 0;

			while (!correctChoice) {
				Console.WriteLine ("Enter the devise CAD value : ");
				try {
					currencyRate = Convert.ToDouble (Console.ReadLine ());
					correctChoice = true;
				} catch (Exception e) {
					Debug.WriteLine ("Exception : " + e);
					Console.WriteLine ("Please enter a correct value (use a '.' instead of ',') : ");
					correctChoice = false;
				}
			}
			return new Devise (deviseSymbol, description, currencyRate);
		}

		/// <summary>
		/// Allow user to choice or create his favourite devise
		/// </summary>
		/// <returns>The favourite currency creation.</returns>
		public static Devise userFavouriteCurrencyCreation()
		{
			Console.WriteLine ("Devise available : ");

			List<Devise> devises = db.Query<Devise> ("SELECT * FROM Devise");
			foreach (Devise d  in devises) {
				Console.WriteLine ("(ID " + d.Id +") : "+ d.Description +"("+d.Symbole+")");
			}
			Console.WriteLine ("Enter the devise ID or '0' to create a new devise : ");
			int id;
			while (true) {
				try {
					id = Convert.ToInt32 (Console.ReadLine ());
					switch (id) {
					case 0:
						return Menu.userDeviseCreation ();
					default:
						return db.Get<Devise> (id);
					}
				} catch (Exception e) {
					Debug.WriteLine("Exception : "+e);
					Console.WriteLine ("Please enter an existing ID : ");
					break;
				}
			}
			return null;
		}
	}
}

