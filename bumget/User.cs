using System;

namespace bumget
{
	public class User
	{
		//Champs






		public static void HomePage(Profil userProfil)
		{
			Console.Clear ();
			Console.WriteLine ("************UserHomePage************** ");
			Console.WriteLine ("Bonjour," + userProfil.Name+" ,nous sommes le :"+ DateTime.Today.ToString("d"));
			Console.WriteLine ("Résumés de vos transactions : ");



		}


		public User ()
		{
		}
	}
}

