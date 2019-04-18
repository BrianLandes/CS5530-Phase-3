using LMS.Controllers;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace xUnitTests {
	public class AdministratorControllerTests {

		#region Utilities

		private static AdministratorController MakeController(Team41LMSContext db) {
			var controller = new AdministratorController();
			controller.UseLMSContext(db);
			return controller;
		}

		#endregion

		// GetCourses
		[Fact]
		public void GetCoursesWhenEmpty() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			var jsonResults = controller.GetCourses("CS") as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Empty(resultValues);
		}

		// GetProfessors

		// CreateCourse successfully
		[Fact]
		public void CreateOneCourse() {
			var db = Utils.MakeMockDatabase();
			var controller = MakeController(db);

			var jsonResult = controller.CreateCourse("CS", 5530, "Probabilitities") as JsonResult;
			Assert.True( Utils.ResultSuccessValue(jsonResult) );
		}

		// CreateCourse try to add two of the same
		[Fact]
		public void CreateTwoDuplicateCourses() {
			var db = Utils.MakeMockDatabase();
			var controller = MakeController(db);

			var jsonResultA = controller.CreateCourse("CS", 5530, "Probabilitities") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResultA));

			var jsonResultB = controller.CreateCourse("CS", 5530, "Probabilitities") as JsonResult;
			Assert.False(Utils.ResultSuccessValue(jsonResultB));
		}

		// CreateClass

		[Fact]
		public void CreateClassTrivial() {
			var db = Utils.MakeMockDatabase();
			var controller = MakeController(db);

			var jsonResult = controller.CreateCourse("CS", 4400, "Probabilitities") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateClass( "CS", 4400, "Spring", 2019, DateTime.Now, DateTime.Now, "On the moon", "Professor Landes") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));
		}


		[Fact]
		public void CreateClassSameCourseSemester() {
			var db = Utils.MakeMockDatabase();
			var controller = MakeController(db);

			var jsonResult = controller.CreateCourse("CS", 4400, "Probabilitities") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateClass("CS", 4400, "Spring", 2019,
				DateTime.Now, DateTime.Now, "On the moon", "Professor Landes") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateClass("CS", 4400, "Spring", 2019,
				DateTime.UnixEpoch, DateTime.UnixEpoch, "On the moon", "Professor Landes") as JsonResult;
			Assert.False(Utils.ResultSuccessValue(jsonResult));
		}

		[Fact]
		public void CreateClassDifferentCourseSemester() {
			var db = Utils.MakeMockDatabase();
			var controller = MakeController(db);

			var jsonResult = controller.CreateCourse("CS", 4400, "Probabilitities") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateClass("CS", 4400, "Spring", 2019,
				DateTime.Now, DateTime.Now, "On the moon", "Professor Landes") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateClass("CS", 4400, "Spring", 1999,
				DateTime.UnixEpoch, DateTime.UnixEpoch, "On the moon", "Professor Landes") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));
		}

		[Fact]
		public void CreateClassOverlappingTimeLocation() {
			var db = Utils.MakeMockDatabase();
			var controller = MakeController(db);

			var jsonResult = controller.CreateCourse("CS", 4400, "Probabilitities") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateCourse("CS", 1300, "Algorithmatics") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateClass("CS", 4400, "Spring", 2019,
				new DateTime(2019,5,21, 6,00,00), new DateTime(2019, 5, 21, 7, 00, 00), "On the moon", "Professor Landes") as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			jsonResult = controller.CreateClass("CS", 1300, "Spring", 2019,
				new DateTime(2019, 5, 21, 5, 00, 00), new DateTime(2019, 5, 21, 6, 30, 00), "On the moon", "Professor Landes") as JsonResult;
			Assert.False(Utils.ResultSuccessValue(jsonResult));
		}
	}
}
