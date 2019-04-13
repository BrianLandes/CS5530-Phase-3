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

		private static StudentController MakeController(Team41LMSContext db) {
			var controller = new StudentController();
			controller.UseLMSContext(db);
			return controller;
		}

		private static string AddOneStudent(Team41LMSContext db) {
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
	}
}
