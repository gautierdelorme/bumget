using System;

namespace bumget
{
	public class Devise
	{
		//Champ
		private int id;
		private string description;
		private string symbole;

		//Méthodes
		public int Id 
		{
			get {
				return id;
			}
			set {
				id = value;
			}
		}
		public string Description 
		{
			get {
				return description;
			}
			set {
				description = value;
			}
		}
		public string Symbole
		{
			get{
				return symbole;
			}
			set{
				symbole = value;
			}

		}





		public Devise ()
		{
			Id = 0;
			Description = "NA";
			Symbole = "NA";
		}
		public Devise(int id, string symbole)
		{
			Id = id;
			Description = "NA";
			Symbole = symbole;
		}
		public Devise(int id,string description ,string symbole)
		{
			Id = id;
			Description = description;
			Symbole = symbole;
		}

		/// <summary>
		/// Convert the value m to it's value in dollar CAN
		/// </summary>
		/// <returns>Le montant en CAN$</returns>
		/// <param name="t">convert rates from the currency to dollar CAN </param>
		/// <param name="m">Amount to convert</param>
		public float ConversionCAN(float t,float m)
		{
			return m * t;
		}
		/// <summary>
		/// Convert the value m in dollar CAN to it's value in the selected Currency
		/// </summary>
		/// <returns>Value in the selected currency</returns>
		/// <param name="t">convert rates from dollar CAN to the selected Currency</param>
		/// <param name="m">amount to convert</param>
		public float ConversionCurrency(float t, float m)
		{
			return m * t;
		}

		public override string ToString ()
		{
			return "id: " + Id.ToString ()+ "," + Description + ",Symbole:" + Symbole;
		}
	}
}

