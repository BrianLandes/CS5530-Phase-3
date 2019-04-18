
using LMS.Controllers;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Xunit;

namespace xUnitTests {
	public class ProfessorControllerTests {

		#region Utilities

		private static ProfessorController MakeController(Team41LMSContext db) {
			ProfessorController controller = new ProfessorController();
			controller.UseLMSContext(db);
			return controller;
		}
		
		#endregion

		[Fact]
		public void GradeSubmission() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);
			
			Courses newCourse = new Courses {
				CatalogId = "12345",
				Listing = "CS",
				Name = "Algorithms",
				Number = "3550"
			};
			db.Courses.Add(newCourse);

			Classes newClass = new Classes {
				CId = 101,
				CatalogId = "12345",
				SemesterYear = 2019,
				SemesterSeason = "Spring",
				Location = "On the moon",
				StartTime = new TimeSpan(),
				EndTime = new TimeSpan()
			};
			db.Classes.Add(newClass);

			Enrolled newEnrolled = new Enrolled {
				UId = uid,
				CId = 101,
				Grade = "--"
			};
			db.Enrolled.Add(newEnrolled);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = 101
			};
			db.AssignmentCategories.Add(assCat);

			Assignments newAssignment = new Assignments {
				AId = 999,
				AcId = 1002,
				Name = "Bonus Quiz",
				MaxPointValue = 102,
				DueDate = DateTime.Now,
				Content = "oanklaks fl lja",
			};
			db.Assignments.Add(newAssignment);

			Submissions newSubmission = new Submissions {
				UId = uid,
				AId = 999,
				Content = "This is some serious content",
				Time = DateTime.Now,
				Score = 78,
			};
			db.Submissions.Add(newSubmission);

			db.SaveChanges();

			var jsonResult = controller.GradeSubmission("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, 79) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));
		}
		

	}
}
