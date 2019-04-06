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

	}
}
