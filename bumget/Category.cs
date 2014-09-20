using System;

namespace bumget
{
	public class Category
	{
		//Champs
		private int id;
		private string name;
		private string description;

		//Méthodes
		public int Id
		{
			get{
				return id;
			}
			set{
				id = value;
			}
		}

		public string Name
		{
			get{
				return name;
			}
			set{
				name = value;
			}
		}

		public string Description
		{
			get{
				return description;
			}
			set{
				description = value;
			}
		}

		public Category ()
		{
			Id = 0;
			Name = "NA";
			Description = "Categorie inexistante";

		}
		public Category(int id, string name, string description)
		{
			Id = id;
			Name = name;
			Description = description;
		}

		public override string ToString ()
		{
			return "id: " + Id.ToString () + ",Name: " + Name + ",Description: " + Description;
		}


	}
}

