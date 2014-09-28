using System;
using System.IO;
using SQLite;
using System.Collections.Generic;
namespace bumget
{
	public class Menu
	{
		//Champs
		int IsRegistered { get; set;}
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db3"));

		public Menu () : base (){
		}


		//Methods

		public static void HomePage()
		{
			Console.Clear ();
			Console.WriteLine ("*******Bienvenue sur GSP-Transaction******)");
			Console.WriteLine ("Veuillez saisir un chiffre entre 1 et 2: ");
			Console.WriteLine ("1-Vous possédez un login");
			Console.WriteLine ("2-Nouvel uttilisateur");
			Console.Write ("Choix: ");
			int choix;

			Console.WriteLine (" ");
			bool saisicorrect = false;
			while (!saisicorrect) {
				choix= int.Parse (Console.ReadLine ());
				Console.WriteLine (" ");
				if (choix == 1) {
					saisicorrect = true;
					Menu.AuthentificationPage ();
				} else if (choix == 2) {
					saisicorrect = true;
					Menu.ProfilCreationPage ();
				} else {
					Console.Write ("Saisi incorrect, saisissez un chiffre entre 1 et 2 : ");

				}

			}

		}
		/// <summary>
		/// Menu d'authification, pourl'instant le seul login qui marche est Solo
		/// </summary>
		public static Profil AuthentificationPage()
		{
			Console.Clear ();
			bool correctLogin = false;
			bool continuer=true;
			string choix;
			string login;
			Profil userProfile=null;

			while (continuer && !correctLogin) {
				Console.Write ("Veuillez saisir votre login : ");
				login = Console.ReadLine ();
				Console.WriteLine (" ");
				var sh = db.Query<Profil> ("SELECT * FROM Profil WHERE Name = ?", login);
				if (sh.Count>0) {
					correctLogin = true;
					userProfile = sh [0];
					User.HomePage (userProfile);

				} else {
					Console.WriteLine ("Votre login est incorect");
					Console.Write ("Voulez-vous recommencer? (y/n)");
					choix = Console.ReadLine ();
					if (choix == "n") {
						continuer = false;
						Console.Clear ();
						userProfile = null;
						Menu.HomePage ();
					}

				}
				return userProfile;

			}
			return userProfile;

		}

		public static void ProfilCreationPage()
		{
			Console.Clear ();
			bool nameExist = true;
			string name=" ";
			int Choice;
			bool goodChoice=false;
			//Default Currency will be Euro and Dollars
			Devise Euro = new Devise ("€", "euros", 0.7);
			Devise Dollar = new Devise ("$", "dollars", 1);
			Devise userNewDevise = Dollar;
			string continueCategoryCreation="y";

			Console.Clear ();
			Console.WriteLine ("*****ProfilCreationPage****** ");
			//er define his namesU
			while (nameExist) {
				Console.Write ("Veuillez saisir votre Name : ");
				name = Console.ReadLine ();
				Console.WriteLine (" ");
				var sh = db.Query<Profil> ("SELECT Id FROM Profil WHERE Name = ?", name);
				if (sh.Count == 0) {
					nameExist = false;

				} else {
					Console.WriteLine ("Votre Nom est déja uttilisé veuillez en saisir un autre");
				}
			}
			//user define his surname
			Console.Write ("Veuillez saisir votre Surname : ");
			string surname = Console.ReadLine ();
			Console.WriteLine (" ");
			//User define his favourite currency
			Console.WriteLine ("Quel est votre devise favorite?");
			
			Console.WriteLine ("1-" + Euro.ToString ());
			Console.WriteLine ("2-" + Dollar.ToString ());
			Console.WriteLine ("3-Créer une nouvelle devise");

			Console.WriteLine ("Veuillez saisir votre choix : ");
			try{
			Choice = int.Parse (Console.ReadLine ());
				while (goodChoice==false) {
					switch (Choice) {
					case 1:
						Console.WriteLine ("Votre devise est l'euro");
						userNewDevise = Euro;
						goodChoice = true;
						break;
					case 2:
						Console.WriteLine ("Votre devise est le dollar canadien");
						userNewDevise = Dollar;
						goodChoice = true;
						break;
					case 3:
						userNewDevise = Menu.deviseUserCreation();
						goodChoice = true;
						break;

					default:
						Console.WriteLine ("Choix incorrect, veuillez saisir un chiffre entre 1,2 et 3");
						break;
					}
				}
			}
			catch (Exception e) {
				Console.WriteLine("{0} Exception caught.", e);
			}

			Console.WriteLine ("Votre login est :" + name);
			//User define/create his categories
			Console.WriteLine ("Quelles sont les catégories que vous souhaitez?");
			while (continueCategoryCreation=="y") {
				Menu.categoryUserCreation ();
				Console.Write ("Voulez vous continuer(y/n)?");
				continueCategoryCreation = Console.ReadLine ();
				Console.WriteLine (" ");
			}
			Profil prof = new Profil (surname, name,userNewDevise, "");
			Console.WriteLine(prof.ToString ());
			Console.WriteLine ("Appuyer sur une touche pour continuer");
			Console.ReadKey ();
			Menu.HomePage ();


		}

		//Utilitary function of Menu class
		/// <summary>
		/// Allow user to create his own devise
		/// </summary>
		/// <returns>The user devise.</returns>
		public static Devise deviseUserCreation ()
		{
			Devise userNewDevise;
			Console.Write ("Saisissez le symbole de votre devise");
			string deviseSymbol = Console.ReadLine ();
			Console.WriteLine (" ");
			Console.Write ("Saisissez la description de votre devise : ");
			string description = Console.ReadLine ();
			Console.WriteLine (" ");
			Console.Write ("Saisissez la valeur en Can$ de votre devise : ");
			int currencyRate = int.Parse (Console.ReadLine ());
			Console.WriteLine (" ");
			userNewDevise = new Devise (deviseSymbol, description, currencyRate);
			return userNewDevise;
		}


		/// <summary>
		/// Ask the user for create his own subcategorie 
		/// </summary>
		public static void categoryUserCreation()
		{
			Console.Write ("Saisissez le Nom de la catégorie : ");
			string categoryName = Console.ReadLine ();
			Console.WriteLine (" ");
			Console.Write ("Saisissez la description de votre catégorie: ");
			string categoryDescription = Console.ReadLine ();
			Category userNewCategory = new Category (categoryName, categoryDescription);
			Console.WriteLine (" ");
		}

	}


}

