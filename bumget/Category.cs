using System;

namespace bumget
{
	public class Category
	{

		private int id;
		private string name;
		private string description;

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

		}

	}
}

