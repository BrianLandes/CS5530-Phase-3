using LMS.Controllers;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Xunit;

namespace xUnitTests {
	public class CommonControllerTests {

		#region Utilities

		private static CommonController MakeController(Team41LMSContext db) {
			CommonController controller = new CommonController();
			controller.UseLMSContext(db);
			return controller;
		}

		public static string AddOneProfessor(Team41LMSContext db) {
			var departments = new List<Departments>(db.Departments);
			var newUser = new Professors {
				FirstName = "Brian",
				LastName = "Landes",
				UId = "u000006",
				WorksIn = departments[0].Name,
				WorksInNavigation = departments[0]
			};
			db.Professors.Add(newUser);
			db.SaveChanges();
			return "u000006";
		}
		
		public static string AddOneStudent(Team41LMSContext db) {
			var departments = new List<Departments>(db.Departments);
			var newUser = new Students {
				FirstName = "Brian",
				LastName = "Landes",
				UId = "u000006",
				Major = departments[0].Name,
				MajorNavigation = departments[0]
			};
			db.Students.Add(newUser);
			db.SaveChanges();
			return "u000006";
		}

		public static string AddOneAdmin(Team41LMSContext db) {
			var newUser = new Administrators {
				FirstName = "Brian",
				LastName = "Landes",
				UId = "u000006"
			};
			db.Administrators.Add(newUser);
			db.SaveChanges();
			return "u000006";
		}

		#endregion

		[Fact]
		public void GetUserStudent() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = AddOneStudent(db);

			var jsonResult = controller.GetUser(uid) as JsonResult;
			dynamic resultValue = jsonResult.Value;
			Assert.NotNull(resultValue);
			
			string firstName = Utils.GetValue<string>(resultValue, "fname");
			Assert.Equal("Brian", firstName);
			string lastName = Utils.GetValue<string>(resultValue, "lname");
			Assert.Equal("Landes", lastName);
		}

		[Fact]
		public void GetUserProfessor() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = AddOneProfessor(db);

			var jsonResult = controller.GetUser(uid) as JsonResult;
			dynamic resultValue = jsonResult.Value;
			Assert.NotNull(resultValue);

			string firstName = Utils.GetValue<string>(resultValue, "fname");
			Assert.Equal("Brian", firstName);
			string lastName = Utils.GetValue<string>(resultValue, "lname");
			Assert.Equal("Landes", lastName);
		}

		[Fact]
		public void GetUserAdministrator() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = AddOneAdmin(db);

			var jsonResult = controller.GetUser(uid) as JsonResult;
			dynamic resultValue = jsonResult.Value;
			Assert.NotNull(resultValue);

			string firstName = Utils.GetValue<string>(resultValue, "fname");
			Assert.Equal("Brian", firstName);
			string lastName = Utils.GetValue<string>(resultValue, "lname");
			Assert.Equal("Landes", lastName);
		}

		[Fact]
		public void GetUserDoesntExist() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			
			var jsonResult = controller.GetUser("some garbage") as JsonResult;
			Assert.False( Utils.ResultSuccessValue(jsonResult) );
		}

		[Fact]
		public void GetDepartment() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			var jsonResult = controller.GetDepartments() as JsonResult;
			dynamic resultValue = jsonResult.Value as dynamic;
			Assert.Single(resultValue);
		}

		[Fact]
		public void GetCatalogSingle() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			var jsonResult = controller.GetCatalog() as JsonResult;
			dynamic resultValue = jsonResult.Value as dynamic;
			Assert.Single(resultValue);
			var csCatalog = resultValue[0];
			
		}

		[Fact]
		public void GetCatalogWithOneCourse() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			Courses newCourse = new Courses() {
				Listing = "CS",
				Number = "101",
				Name = "Calculushies"
			};
			db.Courses.Add(newCourse);
			int rowsAffected = db.SaveChanges();

			var jsonResult = controller.GetCatalog() as JsonResult;
			dynamic resultValue = jsonResult.Value as dynamic;
			Assert.Single(resultValue);
			var csCatalog = resultValue[0];
			Assert.NotNull(csCatalog);
			dynamic courses = Utils.GetValue<dynamic>(csCatalog, "courses");
			Assert.NotNull(courses);
			Assert.Single(courses);
		}


		[Fact]
		public void GetAssignmentContentsTrivialSuccess() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			uint cid = ProfessorControllerTests.AddCourseAndClass(db);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			string assignmentContents = "Just leave your head wide open " +
				"My love comes in doses" +
				"So if you're nervous you shouldn't be " +
				"I'll take away your panic";

			Assignments newAssignment = new Assignments {
				AId = 999,
				AcId = 1002,
				Name = "Bonus Quiz",
				MaxPointValue = 102,
				DueDate = DateTime.Now,
				Content = assignmentContents,
			};
			db.Assignments.Add(newAssignment);

			db.SaveChanges();

			var contentResult = controller.GetAssignmentContents("CS", 3550, "Spring", 2019, "Quizzes", "Bonus Quiz") as ContentResult;
			Assert.Equal(assignmentContents, contentResult.Content);
		}
	}
}
