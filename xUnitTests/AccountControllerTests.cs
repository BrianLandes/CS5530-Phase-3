using LMS.Controllers;
using LMS.Models.LMSModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace xUnitTests {
	public class AccountControllerTests {

		#region Utilities

		private static AccountController MakeController(Team41LMSContext db) {
			AccountController controller = new AccountController(null, null, null, null);
			controller.UseLMSContext(db);
			return controller;
		}
		
		public static void AddOneProfessor(Team41LMSContext db) {
			var controller = MakeController(db);
			controller.CreateNewUser("Brian", "Landes", new DateTime(), "CS", "Professor");

		}

		#endregion

		// Create a new administrator
		[Fact]
		public void CreateNewAdministrator() {
			var db = Utils.MakeMockDatabase();

			AccountController controller = new AccountController(null, null, null, null);

			controller.UseLMSContext(db);

			controller.CreateNewUser("Brian", "Landes", new DateTime(), "CS", "Administrator");
			var admins = new List<Administrators>(db.Administrators);

			Assert.Single(admins);
			var onlyAdmin = admins[0];
			Assert.Equal("Brian", onlyAdmin.FirstName);
			Assert.Equal("Landes", onlyAdmin.LastName);

		}

		// Create a new professor
		[Fact]
		public void CreateNewProfessor() {
			var db = Utils.MakeMockDatabase();

			AddOneProfessor(db);

			var professors = new List<Professors>(db.Professors);

			Assert.Single(professors);
			var onlyProfessor = professors[0];
			Assert.Equal("Brian", onlyProfessor.FirstName);
			Assert.Equal("Landes", onlyProfessor.LastName);
		}
		

		// Create a new student
		[Fact]
		public void CreateNewStudent() {
			var db = Utils.MakeMockDatabase();

			AccountController controller = new AccountController(null, null, null, null);

			controller.UseLMSContext(db);

			controller.CreateNewUser("Brian", "Landes", new DateTime(), "CS", "Student");
			var students = new List<Students>(db.Students);

			Assert.Single(students);
			var onlyStudent = students[0];
			Assert.Equal("Brian", onlyStudent.FirstName);
			Assert.Equal("Landes", onlyStudent.LastName);
		}

		// Create two new students -> different ids
		[Fact]
		public void CreateTwoDifferentStudents() {
			var db = Utils.MakeMockDatabase();

			AccountController controller = new AccountController(null, null, null, null);

			controller.UseLMSContext(db);

			string idOne = controller.CreateNewUser("Brian", "Landes",
				new DateTime(), "CS", "Student");

			string idTwo = controller.CreateNewUser("Chloe", "Josien",
				new DateTime(), "CS", "Student");

			var students = new List<Students>(db.Students);

			Assert.Equal(2, students.Count);

			Assert.NotEqual(idOne, idTwo);
		}

		// Create a bunch of students -> different ids
		[Fact]
		public void CreateManyDifferentStudents() {
			var db = Utils.MakeMockDatabase();

			AccountController controller = new AccountController(null, null, null, null);

			controller.UseLMSContext(db);

			List<string> usedIds = new List<string>();

			for( int i = 0; i < 200; i ++ ) {
				string uid = controller.CreateNewUser("Brian" + i, "Landes",
					new DateTime(), "CS", "Student");

				Assert.DoesNotContain(uid, usedIds);
				usedIds.Add(uid);
				Assert.Equal(i+1, usedIds.Count);

				var students = new List<Students>(db.Students);
				Assert.Equal(usedIds.Count, students.Count);
			}
			
		}
		
	}
}
