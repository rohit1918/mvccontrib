namespace MvcContrib.Samples.FormHelper.Models
{
	public class Role
	{
		private int _id;
		private string _name;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}


		public Role(int id, string name)
		{
			_id = id;
			_name = name;
		}
	}
}