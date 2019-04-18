using LMS.Controllers;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace xUnitTests {
	public class StudentControllerTests {

		#region Utilities

		public static StudentController MakeController(Team41LMSContext db) {
			var controller = new StudentController();
			controller.UseLMSContext(db);
			return controller;
		}

		public static string AddOneStudent(Team41LMSContext db) {
			Students brian = new Students {
				UId = "u0000001",
				Major = "CS",
				FirstName = "Brian",
				LastName = "Landes",
				Dob = DateTime.Now,

			};
			db.Students.Add(brian);
			db.SaveChanges();

			var students = new List<Students>(db.Students);
			Assert.Single(students);

			return brian.UId;
		}

		private static string AddSecondStudent(Team41LMSContext db) {
			Students chloe = new Students {
				UId = "u1000001",
				Major = "CS",
				FirstName = "Chloe",
				LastName = "Josien",
				Dob = DateTime.Now,

			};
			db.Students.Add(chloe);
			db.SaveChanges();

			var students = new List<Students>(db.Students);
			Assert.Equal(2, students.Count );

			return chloe.UId;
		}

		#endregion

		[Fact]
		public void GetClassesWhenEmpty() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			var jsonResults = controller.GetMyClasses(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Empty(resultValues);
		}

		[Fact]
		public void GetClassesWhenOne() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			Courses someCourse = new Courses {
				CatalogId = "CS",

			};
			db.Courses.Add(someCourse);
			db.SaveChanges();

			var courses = new List<Courses>(db.Courses);
			Assert.Single(courses);

			Classes someClass = new Classes {
				CId = 1,
				CatalogId = "CS"
			};
			db.Classes.Add(someClass);
			db.SaveChanges();

			var classes = new List<Classes>(db.Classes);
			Assert.Single(classes);

			Enrolled Enrolled = new Enrolled {
				UId = uid,
				CId = 1
			};

			db.Enrolled.Add(Enrolled);
			db.SaveChanges();

			var enrolled = new List<Enrolled>(db.Enrolled);
			Assert.Single(enrolled);

			var jsonResults = controller.GetMyClasses(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Single(resultValues);
		}

		[Fact]
		public void GetClassesWhenSeveral() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			Courses someCourse = new Courses {
				CatalogId = "CS",

			};
			db.Courses.Add(someCourse);
			db.SaveChanges();

			var courses = new List<Courses>(db.Courses);
			Assert.Single(courses);

			int several = 200;

			for ( int i = 0; i < several; i ++ ) {
				Classes someClass = new Classes {
					CId = (uint)i + 1,
					CatalogId = "CS"
				};
				db.Classes.Add(someClass);
				db.SaveChanges();

				var classes = new List<Classes>(db.Classes);
				Assert.Equal( i+1, classes.Count);

				Enrolled Enrolled = new Enrolled {
					UId = uid,
					CId = (uint)i + 1
				};

				db.Enrolled.Add(Enrolled);
				db.SaveChanges();

				var enrolled = new List<Enrolled>(db.Enrolled);
				Assert.Equal(i + 1, enrolled.Count);
			}
			
			var jsonResults = controller.GetMyClasses(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Equal(several, resultValues.Length);
		}

		[Fact]
		public void GetClassesWhenSeveralAndTwoStudents() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string firstUid = AddOneStudent(db);
			string secondUid = AddSecondStudent(db);

			Courses someCourse = new Courses {
				CatalogId = "CS",

			};
			db.Courses.Add(someCourse);
			db.SaveChanges();

			var courses = new List<Courses>(db.Courses);
			Assert.Single(courses);

			int several = 200;

			for (int i = 0; i < several; i++) {
				Classes someClass = new Classes {
					CId = (uint)i + 1,
					CatalogId = "CS"
				};
				db.Classes.Add(someClass);
				db.SaveChanges();

				var classes = new List<Classes>(db.Classes);
				Assert.Equal(i + 1, classes.Count);

				Enrolled enrolled = new Enrolled {
					UId = firstUid,
					CId = (uint)i + 1
				};
				
				Enrolled enrolled2 = new Enrolled {
					UId = secondUid,
					CId = (uint)i + 1
				};

				db.Enrolled.Add(enrolled);
				db.SaveChanges();
				
			}
			
			var jsonResults = controller.GetMyClasses(firstUid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Equal(several, resultValues.Length);
		}

		[Fact]
		public void GetAssignmentsInClassWhenEmpty() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			var jsonResults = controller.GetAssignmentsInClass("CS", 3550, "Spring", 2019, uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Empty(resultValues);
		}

		[Fact]
		public void GetAssignmentsInClassWhenSingle() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

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

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = 101
			};
			db.AssignmentCategories.Add(assCat);

			Assignments newAssignment = new Assignments {
				AcId = 1002,
				Name = "Bonus Quiz",
				MaxPointValue = 102,
				DueDate = DateTime.Now,
				Content = "oanklaks fl lja",
			};
			db.Assignments.Add(newAssignment);
			db.SaveChanges();

			var jsonResults = controller.GetAssignmentsInClass("CS", 3550, "Spring", 2019, uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Single(resultValues);
		}

		[Fact]
		public void GetAssignmentsInClassWithSubmission() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

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

			var jsonResults = controller.GetAssignmentsInClass("CS", 3550, "Spring", 2019, uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.Single(resultValues);

			var firstResult = resultValues[0];

			uint score = Utils.GetValue<uint>(firstResult, "score");
			Assert.Equal((uint)78, score);
		}

		[Fact]
		public void EnrollWhenDoesntExist() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);
			

			var jsonResults = controller.Enroll("CS", 3550, "Spring", 2019, uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.False(Utils.ResultSuccessValue(jsonResults));
		}

		[Fact]
		public void EnrollTrivial() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

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
			
			db.SaveChanges();

			var jsonResults = controller.Enroll("CS", 3550, "Spring", 2019, uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			Assert.True(Utils.ResultSuccessValue(jsonResults));

			Assert.Single(db.Enrolled);
		}

		[Fact]
		public void EnrollDuplicate() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

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

			db.SaveChanges();

			var jsonResults = controller.Enroll("CS", 3550, "Spring", 2019, uid) as JsonResult;

			Assert.True(Utils.ResultSuccessValue(jsonResults));

			Assert.Single(db.Enrolled);

			jsonResults = controller.Enroll("CS", 3550, "Spring", 2019, uid) as JsonResult;

			Assert.False(Utils.ResultSuccessValue(jsonResults));

			Assert.Single(db.Enrolled);
		}

		[Fact]
		public void GetGPANotEnrolled() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);


			var jsonResults = controller.GetGPA( uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			float gpa = Utils.GetValue<float>(resultValues, "gpa");

			Assert.Equal(0, gpa);
		}

		[Fact]
		public void GetGPAEnrolledNoGrades() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			Enrolled newEnrolled = new Enrolled {
					UId = uid,
					CId = 101,
					Grade = "--"
			};
			db.Enrolled.Add(newEnrolled);
			db.SaveChanges();

			var jsonResults = controller.GetGPA(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			float gpa = Utils.GetValue<float>(resultValues, "gpa");

			Assert.Equal(0, gpa);
		}

		[Fact]
		public void GetGPA40() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			Enrolled newEnrolled = new Enrolled {
				UId = uid,
				CId = 101,
				Grade = "A"
			};
			db.Enrolled.Add(newEnrolled);
			db.SaveChanges();

			var jsonResults = controller.GetGPA(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			float gpa = Utils.GetValue<float>(resultValues, "gpa");

			Assert.Equal(4, gpa);
		}

		[Fact]
		public void GetGPAMultiple40() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			for ( int i = 0; i < 10; i ++ ) {
				Enrolled newEnrolled = new Enrolled {
					UId = uid,
					CId = 101 + (uint)i,
					Grade = "A"
				};
				db.Enrolled.Add(newEnrolled);
			}
			
			db.SaveChanges();

			var jsonResults = controller.GetGPA(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			float gpa = Utils.GetValue<float>(resultValues, "gpa");

			Assert.Equal(4, gpa);
		}

		[Fact]
		public void GetGPAMultiple33() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			for (int i = 0; i < 10; i++) {
				Enrolled newEnrolled = new Enrolled {
					UId = uid,
					CId = 101 + (uint)i,
					Grade = "B+"
				};
				db.Enrolled.Add(newEnrolled);
			}

			db.SaveChanges();

			var jsonResults = controller.GetGPA(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			float gpa = Utils.GetValue<float>(resultValues, "gpa");

			Assert.True( MathF.Abs( 3.3f - gpa) < 0.01f);
		}

		[Fact]
		public void GetGPAMultiple07() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

			for (int i = 0; i < 10; i++) {
				Enrolled newEnrolled = new Enrolled {
					UId = uid,
					CId = 101 + (uint)i,
					Grade = "D-"
				};
				db.Enrolled.Add(newEnrolled);
			}

			db.SaveChanges();

			var jsonResults = controller.GetGPA(uid) as JsonResult;

			dynamic resultValues = jsonResults.Value;

			float gpa = Utils.GetValue<float>(resultValues, "gpa");

			Assert.True(MathF.Abs(0.7f - gpa) < 0.01f);
		}

		[Fact]
		public void SubmitAssignmentTextTrivialFailure() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);
			
			var jsonResults = controller.SubmitAssignmentText("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, "Vikings would make boats from the bones of their enemies.") as JsonResult;

			Assert.False(Utils.ResultSuccessValue(jsonResults));
			
		}

		[Fact]
		public void SubmitAssignmentTextTrivialSuccess() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

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
			
			db.SaveChanges();

			var jsonResults = controller.SubmitAssignmentText("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, "Vikings would make boats from the bones of their enemies." ) as JsonResult;
			
			Assert.True(Utils.ResultSuccessValue(jsonResults));

			var submissions = new List<Submissions>(db.Submissions);
			var submission = submissions[0];
			Assert.Equal((uint)0, submission.Score);
			Assert.Equal(uid, submission.UId);
			Assert.Equal("Vikings would make boats from the bones of their enemies.", submission.Content);
		}


		[Fact]
		public void SubmitAssignmentTextResubmission() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			string uid = AddOneStudent(db);

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

			var jsonResults = controller.SubmitAssignmentText("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, "Vikings would make boats from the bones of their enemies.") as JsonResult;

			Assert.True(Utils.ResultSuccessValue(jsonResults));

			var submissions = new List<Submissions>(db.Submissions);
			var submission = submissions[0];
			Assert.Equal((uint)78, submission.Score);
			Assert.Equal(uid, submission.UId);
			Assert.Equal("Vikings would make boats from the bones of their enemies.", submission.Content);

		}
	}
}
