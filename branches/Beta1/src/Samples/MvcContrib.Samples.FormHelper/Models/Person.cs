namespace MvcContrib.Samples.FormHelper.Models
{
	public class Person
	{
		private string _name;
		private int _id;
		private int _roleId;
		private Gender _gender;

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public int RoleId
		{
			get { return _roleId; }
			set { _roleId = value; }
		}

		public bool IsDeveloper
		{
			get { return _roleId == 2; }
		}

		public Gender Gender
		{
			get { return _gender;  }
			set { _gender = value;}
		}
	}
}