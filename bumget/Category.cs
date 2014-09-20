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

		public float Name
		{
			get{
				return name;
			}
			set{
				name = value;
			}
		}

		public float Description
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

		}

	}
}

