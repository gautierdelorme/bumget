using System;


namespace bumget
{
	public class Transaction
	{
		//Champs

		//On verra après pour l'autoincrémentaiton dans la base de donnée
		private int id;

		private int ownerId;
		private int subcategoryId;
		private string subcategoryName;
		private string transactionDescription;
		private DateTime transactionDate;
		private float transactionAmount;


		//Méthodes
		/// <summary>
		/// Constructeur de la transaction, reste à trouver un moyen pour attribuer le transaction id
		/// </summary>
		public Transaction (int owner,int souscategorieId,string description,DateTime date,float montant)
		{
			this.ownerId = owner;
			this.subcategoryId = souscategorieId;
			this.transactionDescription = description;
			this.transactionDate = date;
			this.transactionAmount = montant;

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


		public int SubcategoryId
		{
			get {
				return subcategoryId;
			}
			set{
				subcategoryId = value;
			}
		}

		public string SubcategoryName
		{
			get{
				return subcategoryName;
			}
			set{
				subcategoryName = value;

			}

		}
		public int Id
		{
			get{
				return id;
			}
			set{
				id = value;
			}

		}

		public string TransactionDescription
		{
			get{
				return transactionDescription;
			}
			set{
				transactionDescription = value;
			}
		}

		public DateTime TransactionDate
		{
			get{
				return transactionDate;
			}
			set{
				transactionDate = value;
			}
		}

		public float TransactionAmount
		{
			get{
				return transactionAmount;
		}
			set{
				transactionAmount = value;
			}
		}

		//Méthode to string pour afficher une transactio
		public override string ToString()
		{
			return "Utilisateur :" + OwnerId + "," + "Montant: " + TransactionAmount + "CAN$," + "Date: " + TransactionDate.ToString () + ",Category : " + SubcategoryName + ",Description : " + TransactionDescription;
		}

	}

}

