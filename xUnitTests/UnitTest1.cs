using LMS.Controllers;
using LMS.Models.LMSModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace xUnitTests {
	public class UnitTest1 {
		[Fact]
		public void CreateNewUser() {
			var db = Utils.MakeMockDatabase();

			AccountController controller = new AccountController(null,null,null,null);

			controller.UseLMSContext(db);

			controller.CreateNewUser("Brian", "Landes", new DateTime(), "CS", "Administrator");
			var admins = new List<Administrators>( db.Administrators);

			Debug.WriteLine(admins);

			//var query = from admin in db.Administrators
			//			select admin;

		}
	}
}
