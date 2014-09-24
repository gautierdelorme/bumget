using System;
using System.IO;
using SQLite;

namespace bumget
{
	public class Menu
	{
		//Champs
		int IsRegistered { get; set;}
		private static SQLiteConnection db = new SQLiteConnection (Path.Combine(Directory.GetCurrentDirectory(), "bumget.db"));

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
		public static void AuthentificationPage()
		{
			Console.Clear ();
			bool correctLogin = false;
			bool continuer=true;
			string choix;
			string login;
			Console.Clear ();



			/*Gautier tu te démerde pour faire une requête vérifiant si le login est présent dans la base de donnée, si oui place le booléen correctLogin a true */
			/*var sh = db.Query<Profil> ("Select * from Profil where Name=?", login);
			foreach (var s in sh) {
				Console.WriteLine (s);
			}
			*/
			while (continuer && !correctLogin) {
				Console.Write ("Veuillez saisir votre login : ");
				login = Console.ReadLine ();
				Console.WriteLine (" ");
				if (login=="Solo") {
					correctLogin = true;
					Menu.UserHomePage ();
				} else {
					Console.WriteLine ("Votre login est incorect");
					Console.Write ("Voulez-vous recommencer? (y/n)");
					choix = Console.ReadLine ();
					if (choix == "n") {
						continuer = false;
						Console.Clear ();
						Menu.HomePage ();
					}



				}

			}

			


		}

		public static void ProfilCreationPage()
		{
			Console.Clear ();
			bool nameExist=false;
			bool continuer = true;
			string name="";

			//CHoix par défaut pour tester l'application
			int devise=1;
			string deviseSymbol;

			Console.Clear ();
			Console.WriteLine ("*****ProfilCreationPage****** ");
			while (continuer) {
				Console.Write ("Veuillez saisir votre Name : ");
				name = Console.ReadLine ();
				Console.WriteLine (" ");
				/* il faut faire une requete dans la base de donnée pour vérifier si le nom existe déja, si oui nameExist=true*/
				if (nameExist) {
					Console.WriteLine ("Votre Nom est déja uttiliser veuillez en saisir un autre");
				} else {
					continuer = false;
				}
			}
			Console.Write ("Veuillez saisir votre Surname : ");
			string surname = Console.ReadLine ();
			Console.WriteLine (" ");
			Console.Write ("Quel devise uttilisez-vous : ");
			deviseSymbol=Console.ReadLine ();
			/*Là une requete doit être faite pour récupéré le id correspondant à la devise, il serait bien aussi, d'afficher la liste des symbole des devises*/

			/*Console.WriteLine ("Votre login est :" + name);
			Profil prof = new Profil (surname, name, devise);
			Console.WriteLine(prof.ToString ());
			Console.WriteLine ("Appuyer sur une touche pour continuer");
			Console.ReadKey ();
			Console.Clear ();
			Menu.HomePage ();*/


		}

		public static void UserHomePage()
		{
			Console.Clear ();
			Console.WriteLine ("UserHomePage ");

		}
	}
}

