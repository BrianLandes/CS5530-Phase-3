
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
		
		public static uint AddCourseAndClass(Team41LMSContext db) {

			uint cid = 101;

			Courses newCourse = new Courses {
				CatalogId = "12345",
				Listing = "CS",
				Name = "Algorithms",
				Number = "3550"
			};
			db.Courses.Add(newCourse);

			Classes newClass = new Classes {
				CId = cid,
				CatalogId = "12345",
				SemesterYear = 2019,
				SemesterSeason = "Spring",
				Location = "On the moon",
				StartTime = new TimeSpan(),
				EndTime = new TimeSpan()
			};
			db.Classes.Add(newClass);
			db.SaveChanges();

			return cid;
		}

		public static void Enroll( string uid, uint cid, Team41LMSContext db) {
			Enrolled newEnrolled = new Enrolled {
				UId = uid,
				CId = cid,
				Grade = "--"
			};
			db.Enrolled.Add(newEnrolled);
			db.SaveChanges();
		}

		public static void AddAssignmentAndCategory(uint cid, Team41LMSContext db) {

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = cid
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
		}

		public void AssertGradeEquals(string uid, string grade, Team41LMSContext db) {
			var studentController = StudentControllerTests.MakeController(db);
			var gradeJsonResult = studentController.GetMyClasses(uid) as JsonResult;
			var onlyClass = Utils.JsonResultValueAtIndex(gradeJsonResult, 0);
			var letterGrade = Utils.GetValue<string>(onlyClass, "grade");
			Assert.Equal(grade, letterGrade);
		}

		#endregion

		[Fact]
		public void GradeSubmission() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AddAssignmentAndCategory(cid, db);

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


		[Fact]
		public void GradeSubmissionTrivialFailure() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AddAssignmentAndCategory(cid, db);

			// no submission

			db.SaveChanges();

			var jsonResult = controller.GradeSubmission("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, 79) as JsonResult;
			Assert.False(Utils.ResultSuccessValue(jsonResult));
		}


		[Fact]
		public void GradeSubmissionUpdateGradeOneSumbissionA() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 100,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			Assignments newAssignment = new Assignments {
				AId = 999,
				AcId = 1002,
				Name = "Bonus Quiz",
				MaxPointValue = 100,
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

			AssertGradeEquals(uid, "--", db);
			
			var jsonResult = controller.GradeSubmission("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, 100) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			AssertGradeEquals(uid, "A", db);
		}


		[Fact]
		public void GradeSubmissionUpdateGradeOneSumbissionB() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 100,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			Assignments newAssignment = new Assignments {
				AId = 999,
				AcId = 1002,
				Name = "Bonus Quiz",
				MaxPointValue = 100,
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

			AssertGradeEquals(uid, "--", db);

			var jsonResult = controller.GradeSubmission("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, 85) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			AssertGradeEquals(uid, "B", db);
		}

		[Fact]
		public void GradeSubmissionUpdateGradeOneSumbissionD() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 100,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			Assignments newAssignment = new Assignments {
				AId = 999,
				AcId = 1002,
				Name = "Bonus Quiz",
				MaxPointValue = 100,
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

			AssertGradeEquals(uid, "--", db);

			var jsonResult = controller.GradeSubmission("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, 65) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			AssertGradeEquals(uid, "D", db);
		}

		[Fact]
		public void GradeSubmissionUpdateGradeTwoAssignmentsA() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 100,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);
			{
				Assignments newAssignment = new Assignments {
					AId = 999,
					AcId = 1002,
					Name = "Bonus Quiz",
					MaxPointValue = 100,
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
			}
			db.SaveChanges();

			AssertGradeEquals(uid, "--", db);

			var jsonResult = controller.GradeSubmission("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz", uid, 100) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			AssertGradeEquals(uid, "A", db);

			{
				Assignments newAssignment = new Assignments {
					AId = 9991,
					AcId = 1002,
					Name = "Bonus Quiz 2",
					MaxPointValue = 100,
					DueDate = DateTime.Now,
					Content = "oanklaks fl lja",
				};
				db.Assignments.Add(newAssignment);

				Submissions newSubmission = new Submissions {
					UId = uid,
					AId = 9991,
					Content = "This is some serious content",
					Time = DateTime.Now,
					Score = 78,
				};
				db.Submissions.Add(newSubmission);
			}
			db.SaveChanges();

			jsonResult = controller.GradeSubmission("CS", 3550, "Spring", 2019,
				"Quizzes", "Bonus Quiz 2", uid, 70) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));

			AssertGradeEquals(uid, "B", db);
		}

		[Fact]
		public void GradeSubmissionEachGradeRegrading() {
			var db = Utils.MakeMockDatabase();

			var professorController = MakeController(db);
			var commonController = CommonControllerTests.MakeController(db);
			var studentController = StudentControllerTests.MakeController(db);
			var accountController = AccountControllerTests.MakeController(db);
			var adminController = AdministratorControllerTests.MakeController(db);

			// The following several commands are just setup
			string uid = accountController.CreateNewUser("Brian", "Landes", 
				new DateTime(), "CS", "Student");

			Utils.AssertExecutedSuccessfully( 
				adminController.CreateCourse("CS", 1286, "Game Playing") );
			Utils.AssertExecutedSuccessfully(
				adminController.CreateClass("CS", 1286, "Spring", 2019,
				DateTime.Now, DateTime.Now, "On the moon", "Professor Landes"));
			Utils.AssertExecutedSuccessfully(
				studentController.Enroll("CS", 1286, "Spring", 2019, uid));

			int categoryWeightA = 100;
			Utils.AssertExecutedSuccessfully(
				professorController.CreateAssignmentCategory("CS", 1286, "Spring", 2019,
				"Practical Jokes", categoryWeightA));
			int assignmentMaxPointsA = 100;
			Utils.AssertExecutedSuccessfully(
				professorController.CreateAssignment("CS", 1286, "Spring", 2019, 
				"Practical Jokes", "Use Watergun", assignmentMaxPointsA, DateTime.Now, 
				"Students are hereby commanded to do unto others as they would do unto themselves if they had a water gun." ) );

			Utils.AssertExecutedSuccessfully(
			studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 
				"I did unto others that which I would have done unto myself if I had had a water gun."));

			// try a value for each letter grade

			// ---- This is the actual command being tested -----
			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 0));
			AssertGradeEquals(uid, "E", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 59));
			AssertGradeEquals(uid, "E", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 62));
			AssertGradeEquals(uid, "D-", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 65));
			AssertGradeEquals(uid, "D", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 68));
			AssertGradeEquals(uid, "D+", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 71));
			AssertGradeEquals(uid, "C-", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 74));
			AssertGradeEquals(uid, "C", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 77));
			AssertGradeEquals(uid, "C+", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 81));
			AssertGradeEquals(uid, "B-", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 85));
			AssertGradeEquals(uid, "B", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 88));
			AssertGradeEquals(uid, "B+", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 91));
			AssertGradeEquals(uid, "A-", db);

			Utils.AssertExecutedSuccessfully(
				professorController.GradeSubmission("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid, 95));
			AssertGradeEquals(uid, "A", db);
			
		}

		[Fact]
		public void GradeSubmissionAssignmentsWithDifferentPointValues() {
			var db = Utils.MakeMockDatabase();

			var professorController = MakeController(db);
			var commonController = CommonControllerTests.MakeController(db);
			var studentController = StudentControllerTests.MakeController(db);
			var accountController = AccountControllerTests.MakeController(db);
			var adminController = AdministratorControllerTests.MakeController(db);

			// The following several commands are just setup
			string uid = accountController.CreateNewUser("Brian", "Landes",
				new DateTime(), "CS", "Student");

			Utils.AssertExecutedSuccessfully(
				adminController.CreateCourse("CS", 1286, "Game Playing"));
			Utils.AssertExecutedSuccessfully(
				adminController.CreateClass("CS", 1286, "Spring", 2019,
				DateTime.Now, DateTime.Now, "On the moon", "Professor Landes"));
			Utils.AssertExecutedSuccessfully(
				studentController.Enroll("CS", 1286, "Spring", 2019, uid));

			int categoryWeightA = 100;
			Utils.AssertExecutedSuccessfully(
				professorController.CreateAssignmentCategory("CS", 1286, "Spring", 2019,
				"Practical Jokes", categoryWeightA));
			int assignmentMaxPointsZ = 100;
			Utils.AssertExecutedSuccessfully(
				professorController.CreateAssignment("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", assignmentMaxPointsZ, DateTime.Now,
				"Students are hereby commanded to do unto others as they would do unto themselves if they had a water gun."));

			Utils.AssertExecutedSuccessfully(
			studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Use Watergun", uid,
				"I did unto others that which I would have done unto myself if I had had a water gun."));

			int assignmentMaxPointsY = 60;
			Utils.AssertExecutedSuccessfully(
				professorController.CreateAssignment("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Plant Fake Evidence", assignmentMaxPointsY, DateTime.Now,
				"Do whatever it takes to win the case."));

			Utils.AssertExecutedSuccessfully(
			studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Plant Fake Evidence", uid,
				"Planted fingerprint on the wedding ring."));

			int assignmentMaxPointsX = 340;
			Utils.AssertExecutedSuccessfully(
				professorController.CreateAssignment("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Guerrilla Yard Work", assignmentMaxPointsX, DateTime.Now,
				"Do someones yard work without them knowing."));

			Utils.AssertExecutedSuccessfully(
			studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
				"Practical Jokes", "Guerrilla Yard Work", uid,
				"I'll take the zero."));

			{
				int submissionScoreZ = 0;
				int submissionScoreY = 60;
				int submissionScoreX = 340;

				// ---- This is the actual command being tested -----
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Use Watergun", uid, submissionScoreZ));
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Plant Fake Evidence", uid, submissionScoreY));
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Guerrilla Yard Work", uid, submissionScoreX));

				// grade score should be:
				// earnedPercentageForCategoryA = submissionScores (for each Z, Y, and X) / assignmentMaxPoints (for each Z, Y, and X)
				// pointsEarnedInCategoryA = categoryWeightA * earnedPercentageForCategoryA
				// possiblePointsInCategoryA = categoryWeightA
				// score = pointsEarnedInCategoryA / possiblePointsInCategoryA * 100

				double earnedAssignmentPoints = submissionScoreZ
					+ submissionScoreY
					+ submissionScoreX;

				double possibleAssignmentPoints = assignmentMaxPointsZ
					+ assignmentMaxPointsY
					+ assignmentMaxPointsX;

				double pointsEarnedInCategoryA =
					earnedAssignmentPoints / possibleAssignmentPoints * categoryWeightA;

				double score = pointsEarnedInCategoryA / categoryWeightA * 100f;

				string letterGrade = professorController.GetLetterGrade(score);

				AssertGradeEquals(uid, letterGrade, db);
			}

			{
				int submissionScoreZ = 100;
				int submissionScoreY = 60;
				int submissionScoreX = 340;

				// ---- This is the actual command being tested -----
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Use Watergun", uid, submissionScoreZ));
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Plant Fake Evidence", uid, submissionScoreY));
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Guerrilla Yard Work", uid, submissionScoreX));

				// grade score should be:
				// earnedPercentageForCategoryA = submissionScores (for each Z, Y, and X) / assignmentMaxPoints (for each Z, Y, and X)
				// pointsEarnedInCategoryA = categoryWeightA * earnedPercentageForCategoryA
				// possiblePointsInCategoryA = categoryWeightA
				// score = pointsEarnedInCategoryA / possiblePointsInCategoryA * 100

				double earnedAssignmentPoints = submissionScoreZ
					+ submissionScoreY
					+ submissionScoreX;

				double possibleAssignmentPoints = assignmentMaxPointsZ
					+ assignmentMaxPointsY
					+ assignmentMaxPointsX;

				double pointsEarnedInCategoryA =
					earnedAssignmentPoints / possibleAssignmentPoints * categoryWeightA;

				double score = pointsEarnedInCategoryA / categoryWeightA * 100f;

				string letterGrade = professorController.GetLetterGrade(score);

				AssertGradeEquals(uid, letterGrade, db);
			}

			{
				int submissionScoreZ = 80;
				int submissionScoreY = 20;
				int submissionScoreX = 140;

				// ---- This is the actual command being tested -----
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Use Watergun", uid, submissionScoreZ));
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Plant Fake Evidence", uid, submissionScoreY));
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					"Practical Jokes", "Guerrilla Yard Work", uid, submissionScoreX));

				// grade score should be:
				// earnedPercentageForCategoryA = submissionScores (for each Z, Y, and X) / assignmentMaxPoints (for each Z, Y, and X)
				// pointsEarnedInCategoryA = categoryWeightA * earnedPercentageForCategoryA
				// possiblePointsInCategoryA = categoryWeightA
				// score = pointsEarnedInCategoryA / possiblePointsInCategoryA * 100

				double earnedAssignmentPoints = submissionScoreZ
					+ submissionScoreY
					+ submissionScoreX;

				double possibleAssignmentPoints = assignmentMaxPointsZ
					+ assignmentMaxPointsY
					+ assignmentMaxPointsX;

				double pointsEarnedInCategoryA =
					earnedAssignmentPoints / possibleAssignmentPoints * categoryWeightA;

				double score = pointsEarnedInCategoryA / categoryWeightA * 100f;

				string letterGrade = professorController.GetLetterGrade(score);

				AssertGradeEquals(uid, letterGrade, db);
			}
		}


		[Fact]
		public void GradeSubmissionWeightedAssignmentCategories() {
			var db = Utils.MakeMockDatabase();

			var professorController = MakeController(db);
			var commonController = CommonControllerTests.MakeController(db);
			var studentController = StudentControllerTests.MakeController(db);
			var accountController = AccountControllerTests.MakeController(db);
			var adminController = AdministratorControllerTests.MakeController(db);

			// The following several commands are just setup
			string uid = accountController.CreateNewUser("Brian", "Landes",
				new DateTime(), "CS", "Student");

			Utils.AssertExecutedSuccessfully(
				adminController.CreateCourse("CS", 1286, "Game Playing"));
			Utils.AssertExecutedSuccessfully(
				adminController.CreateClass("CS", 1286, "Spring", 2019,
				DateTime.Now, DateTime.Now, "On the moon", "Professor Landes"));
			Utils.AssertExecutedSuccessfully(
				studentController.Enroll("CS", 1286, "Spring", 2019, uid));

			string[] categoryNames = new string[] { "Sucker Punching", "Basket Weaving", "Telling Dirty Jokes" };
			string categoryName;
			int[] categoryWeights = new int[] { 10, 100, 500 };
			
			for( int i = 0; i < categoryWeights.Length; i ++ ) {
				categoryName = categoryNames[i];
				int categoryWeight = categoryWeights[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignmentCategory("CS", 1286, "Spring", 2019,
					categoryName, categoryWeight));
			}

			categoryName = categoryNames[0];
			int[] assignmentWeightsA = new int[] { 200, 12, 4 };
			int[] submissionScoresA = new int[] { 180, 12, 3 };

			for (int i = 0; i < assignmentWeightsA.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsA[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));

				Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, uid,
					"Planted fingerprint on the wedding ring."));

				int submissionScore = submissionScoresA[i];
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, uid, submissionScore));
			}

			categoryName = categoryNames[1];
			int[] assignmentWeightsB = new int[] { 20, 120, 40, 10 };
			int[] submissionScoresB = new int[] { 18, 120, 0, 9 };

			for (int i = 0; i < assignmentWeightsB.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsB[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));

				Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, uid,
					"Planted fingerprint on the wedding ring."));

				int submissionScore = submissionScoresB[i];
				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, uid, submissionScore));
			}
			categoryName = categoryNames[2];
			int[] assignmentWeightsC = new int[] { 2, 1 };
			int[] submissionScoresC = new int[] { 1, 1 };

			for (int i = 0; i < assignmentWeightsC.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsC[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));

				Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, "Plant Fake Evidence " + i, uid,
					"Planted fingerprint on the wedding ring."));
				
				int submissionScore = submissionScoresC[i];
				if (submissionScore != 0) {
					Utils.AssertExecutedSuccessfully(
						professorController.GradeSubmission("CS", 1286, "Spring", 2019,
						categoryName, "Plant Fake Evidence " + i, uid, submissionScore));
				}
				
			}

			// grade score should be:
			// earnedPercentageForCategoryA = submissionScores (for each Z, Y, and X) / assignmentMaxPoints (for each Z, Y, and X)
			// pointsEarnedInCategoryA = categoryWeightA * earnedPercentageForCategoryA
			// possiblePoints = categoryWeightA + categoryWeightB + categoryWeightC
			// score = (pointsEarnedInCategoryA + pointsEarnedInCategoryB + ... )/ possiblePoints * 100

			double totalCategoryWeights = 0;
			double pointsEarnedInCategoryA = 0;
			double pointsEarnedInCategoryB = 0;
			double pointsEarnedInCategoryC = 0;

			{
				double earnedAssignmentPointsA = 0;
				double possibleAssignmentPointsA = 0;

				for (int i = 0; i < assignmentWeightsA.Length; i++) {
					earnedAssignmentPointsA += submissionScoresA[i];
					possibleAssignmentPointsA += assignmentWeightsA[i];
				}

				pointsEarnedInCategoryA =
					earnedAssignmentPointsA / possibleAssignmentPointsA * categoryWeights[0];

				totalCategoryWeights += categoryWeights[0];
			}
			{
				double earnedAssignmentPointsB = 0;
				double possibleAssignmentPointsB = 0;

				for (int i = 0; i < assignmentWeightsB.Length; i++) {
					earnedAssignmentPointsB += submissionScoresB[i];
					possibleAssignmentPointsB += assignmentWeightsB[i];
				}

				pointsEarnedInCategoryB =
					earnedAssignmentPointsB / possibleAssignmentPointsB * categoryWeights[1];

				totalCategoryWeights += categoryWeights[1];
			}
			{
				double earnedAssignmentPointsC = 0;
				double possibleAssignmentPointsC = 0;

				for (int i = 0; i < assignmentWeightsC.Length; i++) {
					earnedAssignmentPointsC += submissionScoresC[i];
					possibleAssignmentPointsC += assignmentWeightsC[i];
				}

				pointsEarnedInCategoryC =
					earnedAssignmentPointsC / possibleAssignmentPointsC * categoryWeights[2];

				totalCategoryWeights += categoryWeights[2];
			}

			double score = (pointsEarnedInCategoryA + pointsEarnedInCategoryB + pointsEarnedInCategoryC)
				/ totalCategoryWeights * 100f;

			string letterGrade = professorController.GetLetterGrade(score);

			AssertGradeEquals(uid, letterGrade, db);

		}

		[Fact]
		public void GradeSubmissionWeightedAssignmentCategoriesWithMissingSubmissions() {
			var db = Utils.MakeMockDatabase();

			var professorController = MakeController(db);
			var commonController = CommonControllerTests.MakeController(db);
			var studentController = StudentControllerTests.MakeController(db);
			var accountController = AccountControllerTests.MakeController(db);
			var adminController = AdministratorControllerTests.MakeController(db);

			// The following several commands are just setup
			string uid = accountController.CreateNewUser("Brian", "Landes",
				new DateTime(), "CS", "Student");

			Utils.AssertExecutedSuccessfully(
				adminController.CreateCourse("CS", 1286, "Game Playing"));
			Utils.AssertExecutedSuccessfully(
				adminController.CreateClass("CS", 1286, "Spring", 2019,
				DateTime.Now, DateTime.Now, "On the moon", "Professor Landes"));
			Utils.AssertExecutedSuccessfully(
				studentController.Enroll("CS", 1286, "Spring", 2019, uid));

			string[] categoryNames = new string[] { "Sucker Punching", "Basket Weaving", "Telling Dirty Jokes" };
			string categoryName;
			int[] categoryWeights = new int[] { 200, 10, 500 };

			for (int i = 0; i < categoryWeights.Length; i++) {
				categoryName = categoryNames[i];
				int categoryWeight = categoryWeights[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignmentCategory("CS", 1286, "Spring", 2019,
					categoryName, categoryWeight));
			}

			categoryName = categoryNames[0];
			int[] assignmentWeightsA = new int[] { 200, 12, 400 };
			int[] submissionScoresA = new int[] { 0, 12, 390 };

			for (int i = 0; i < assignmentWeightsA.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsA[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));

				int submissionScore = submissionScoresA[i];
				if (submissionScore != 0) {
					Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid,
					"Planted fingerprint on the wedding ring."));

				
					Utils.AssertExecutedSuccessfully(
						professorController.GradeSubmission("CS", 1286, "Spring", 2019,
						categoryName, categoryName + i, uid, submissionScore));
				}
			}

			categoryName = categoryNames[1];
			int[] assignmentWeightsB = new int[] { 20, 120, 400, 10 };
			int[] submissionScoresB = new int[] { 0, 0, 0, 0 };

			for (int i = 0; i < assignmentWeightsB.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsB[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));
				int submissionScore = submissionScoresB[i];
				if (submissionScore != 0) {
					Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid,
					"Planted fingerprint on the wedding ring."));

				
					Utils.AssertExecutedSuccessfully(
						professorController.GradeSubmission("CS", 1286, "Spring", 2019,
						categoryName, categoryName + i, uid, submissionScore));
				}
			}
			categoryName = categoryNames[2];
			int[] assignmentWeightsC = new int[] { 2, 1 };
			int[] submissionScoresC = new int[] { 2, 1 };

			for (int i = 0; i < assignmentWeightsC.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsC[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));

				int submissionScore = submissionScoresC[i];
				if (submissionScore != 0) {
					Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid,
					"Planted fingerprint on the wedding ring."));

				
					Utils.AssertExecutedSuccessfully(
						professorController.GradeSubmission("CS", 1286, "Spring", 2019,
						categoryName, categoryName + i, uid, submissionScore));
				}

			}

			// grade score should be:
			// earnedPercentageForCategoryA = submissionScores (for each Z, Y, and X) / assignmentMaxPoints (for each Z, Y, and X)
			// pointsEarnedInCategoryA = categoryWeightA * earnedPercentageForCategoryA
			// possiblePoints = categoryWeightA + categoryWeightB + categoryWeightC
			// score = (pointsEarnedInCategoryA + pointsEarnedInCategoryB + ... )/ possiblePoints * 100

			double totalCategoryWeights = 0;
			double pointsEarnedInCategoryA = 0;
			double pointsEarnedInCategoryB = 0;
			double pointsEarnedInCategoryC = 0;

			{
				double earnedAssignmentPointsA = 0;
				double possibleAssignmentPointsA = 0;

				for (int i = 0; i < assignmentWeightsA.Length; i++) {
					earnedAssignmentPointsA += submissionScoresA[i];
					possibleAssignmentPointsA += assignmentWeightsA[i];
				}

				pointsEarnedInCategoryA =
					earnedAssignmentPointsA / possibleAssignmentPointsA * categoryWeights[0];

				totalCategoryWeights += categoryWeights[0];
			}
			{
				double earnedAssignmentPointsB = 0;
				double possibleAssignmentPointsB = 0;

				for (int i = 0; i < assignmentWeightsB.Length; i++) {
					earnedAssignmentPointsB += submissionScoresB[i];
					possibleAssignmentPointsB += assignmentWeightsB[i];
				}

				pointsEarnedInCategoryB =
					earnedAssignmentPointsB / possibleAssignmentPointsB * categoryWeights[1];

				totalCategoryWeights += categoryWeights[1];
			}
			{
				double earnedAssignmentPointsC = 0;
				double possibleAssignmentPointsC = 0;

				for (int i = 0; i < assignmentWeightsC.Length; i++) {
					earnedAssignmentPointsC += submissionScoresC[i];
					possibleAssignmentPointsC += assignmentWeightsC[i];
				}

				pointsEarnedInCategoryC =
					earnedAssignmentPointsC / possibleAssignmentPointsC * categoryWeights[2];

				totalCategoryWeights += categoryWeights[2];
			}

			double score = (pointsEarnedInCategoryA + pointsEarnedInCategoryB + pointsEarnedInCategoryC)
				/ totalCategoryWeights * 100f;

			string letterGrade = professorController.GetLetterGrade(score);

			AssertGradeEquals(uid, letterGrade, db);

		}


		[Fact]
		public void GradeSubmissionWeightedAssignmentCategoriesWithMissingSubmissionsAndOtherStudents() {
			var db = Utils.MakeMockDatabase();

			var professorController = MakeController(db);
			var commonController = CommonControllerTests.MakeController(db);
			var studentController = StudentControllerTests.MakeController(db);
			var accountController = AccountControllerTests.MakeController(db);
			var adminController = AdministratorControllerTests.MakeController(db);

			// The following several commands are just setup
			string uid = accountController.CreateNewUser("Brian", "Landes",
				new DateTime(), "CS", "Student");
			string uid2 = accountController.CreateNewUser("Chloe", "Josien",
				new DateTime(), "CS", "Student");

			Utils.AssertExecutedSuccessfully(
				adminController.CreateCourse("CS", 1286, "Game Playing"));
			Utils.AssertExecutedSuccessfully(
				adminController.CreateClass("CS", 1286, "Spring", 2019,
				DateTime.Now, DateTime.Now, "On the moon", "Professor Landes"));
			Utils.AssertExecutedSuccessfully(
				studentController.Enroll("CS", 1286, "Spring", 2019, uid));
			Utils.AssertExecutedSuccessfully(
				studentController.Enroll("CS", 1286, "Spring", 2019, uid2));

			string[] categoryNames = new string[] { "Sucker Punching", "Basket Weaving", "Telling Dirty Jokes" };
			string categoryName;
			int[] categoryWeights = new int[] { 200, 10, 500 };

			for (int i = 0; i < categoryWeights.Length; i++) {
				categoryName = categoryNames[i];
				int categoryWeight = categoryWeights[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignmentCategory("CS", 1286, "Spring", 2019,
					categoryName, categoryWeight));
			}

			categoryName = categoryNames[0];
			int[] assignmentWeightsA = new int[] { 200, 12, 400 };
			int[] submissionScoresA = new int[] { 0, 12, 390 };

			for (int i = 0; i < assignmentWeightsA.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsA[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));

				int submissionScore = submissionScoresA[i];
				if (submissionScore != 0) {
					Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid,
					"Planted fingerprint on the wedding ring."));


					Utils.AssertExecutedSuccessfully(
						professorController.GradeSubmission("CS", 1286, "Spring", 2019,
						categoryName, categoryName + i, uid, submissionScore));
				}

				Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid2,
					"Planted fingerprint on the wedding ring."));


				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid2, assignmentMaxPoints));
			}

			categoryName = categoryNames[1];
			int[] assignmentWeightsB = new int[] { 20, 120, 400, 10 };
			int[] submissionScoresB = new int[] { 0, 0, 0, 0 };

			for (int i = 0; i < assignmentWeightsB.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsB[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));
				int submissionScore = submissionScoresB[i];
				if (submissionScore != 0) {
					Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid,
					"Planted fingerprint on the wedding ring."));


					Utils.AssertExecutedSuccessfully(
						professorController.GradeSubmission("CS", 1286, "Spring", 2019,
						categoryName, categoryName + i, uid, submissionScore));
				}

				Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid2,
					"Planted fingerprint on the wedding ring."));


				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid2, assignmentMaxPoints));
			}
			categoryName = categoryNames[2];
			int[] assignmentWeightsC = new int[] { 2, 1 };
			int[] submissionScoresC = new int[] { 2, 1 };

			for (int i = 0; i < assignmentWeightsC.Length; i++) {
				int assignmentMaxPoints = assignmentWeightsC[i];
				Utils.AssertExecutedSuccessfully(
					professorController.CreateAssignment("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, assignmentMaxPoints, DateTime.Now,
					"Do whatever it takes to win the case."));

				int submissionScore = submissionScoresC[i];
				if (submissionScore != 0) {
					Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid,
					"Planted fingerprint on the wedding ring."));


					Utils.AssertExecutedSuccessfully(
						professorController.GradeSubmission("CS", 1286, "Spring", 2019,
						categoryName, categoryName + i, uid, submissionScore));
				}

				Utils.AssertExecutedSuccessfully(
					studentController.SubmitAssignmentText("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid2,
					"Planted fingerprint on the wedding ring."));


				Utils.AssertExecutedSuccessfully(
					professorController.GradeSubmission("CS", 1286, "Spring", 2019,
					categoryName, categoryName + i, uid2, assignmentMaxPoints));

			}

			// grade score should be:
			// earnedPercentageForCategoryA = submissionScores (for each Z, Y, and X) / assignmentMaxPoints (for each Z, Y, and X)
			// pointsEarnedInCategoryA = categoryWeightA * earnedPercentageForCategoryA
			// possiblePoints = categoryWeightA + categoryWeightB + categoryWeightC
			// score = (pointsEarnedInCategoryA + pointsEarnedInCategoryB + ... )/ possiblePoints * 100

			double totalCategoryWeights = 0;
			double pointsEarnedInCategoryA = 0;
			double pointsEarnedInCategoryB = 0;
			double pointsEarnedInCategoryC = 0;

			{
				double earnedAssignmentPointsA = 0;
				double possibleAssignmentPointsA = 0;

				for (int i = 0; i < assignmentWeightsA.Length; i++) {
					earnedAssignmentPointsA += submissionScoresA[i];
					possibleAssignmentPointsA += assignmentWeightsA[i];
				}

				pointsEarnedInCategoryA =
					earnedAssignmentPointsA / possibleAssignmentPointsA * categoryWeights[0];

				totalCategoryWeights += categoryWeights[0];
			}
			{
				double earnedAssignmentPointsB = 0;
				double possibleAssignmentPointsB = 0;

				for (int i = 0; i < assignmentWeightsB.Length; i++) {
					earnedAssignmentPointsB += submissionScoresB[i];
					possibleAssignmentPointsB += assignmentWeightsB[i];
				}

				pointsEarnedInCategoryB =
					earnedAssignmentPointsB / possibleAssignmentPointsB * categoryWeights[1];

				totalCategoryWeights += categoryWeights[1];
			}
			{
				double earnedAssignmentPointsC = 0;
				double possibleAssignmentPointsC = 0;

				for (int i = 0; i < assignmentWeightsC.Length; i++) {
					earnedAssignmentPointsC += submissionScoresC[i];
					possibleAssignmentPointsC += assignmentWeightsC[i];
				}

				pointsEarnedInCategoryC =
					earnedAssignmentPointsC / possibleAssignmentPointsC * categoryWeights[2];

				totalCategoryWeights += categoryWeights[2];
			}

			double score = (pointsEarnedInCategoryA + pointsEarnedInCategoryB + pointsEarnedInCategoryC)
				/ totalCategoryWeights * 100f;

			string letterGrade = professorController.GetLetterGrade(score);

			AssertGradeEquals(uid, letterGrade, db);
			AssertGradeEquals(uid2, "A", db);
		}

		[Fact]
		public void GetStudentsInClassOneStudent() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = 101;

			Courses newCourse = new Courses {
				CatalogId = "12345",
				Listing = "CS",
				Name = "Algorithms",
				Number = "3550"
			};
			db.Courses.Add(newCourse);



			Classes newClass = new Classes {
				CId = cid,
				CatalogId = "12345",
				SemesterYear = 2019,
				SemesterSeason = "Spring",
				Location = "On the moon",
				StartTime = new TimeSpan(),
				EndTime = new TimeSpan()
			};
			db.Classes.Add(newClass);

			Enroll(uid, cid, db);
			
			db.SaveChanges();

			var jsonResult = controller.GetStudentsInClass("CS", 3550, "Spring", 2019 ) as JsonResult;
			dynamic resultValue = jsonResult.Value as dynamic;
			Assert.Single(resultValue);
			var oneStudent = resultValue[0] as dynamic;
			Assert.True(Utils.HasField(oneStudent, "fname"));
			Assert.True(Utils.HasField(oneStudent, "lname"));
			Assert.True(Utils.HasField(oneStudent, "uid"));
			Assert.True(Utils.HasField(oneStudent, "dob"));
			Assert.True(Utils.HasField(oneStudent, "grade"));
			Assert.False(Utils.HasField(oneStudent, "two heads"));

			Assert.Equal("Brian", Utils.GetValue<string>(oneStudent, "fname"));
			Assert.Equal("Landes", Utils.GetValue<string>(oneStudent, "lname"));
			Assert.Equal("--", Utils.GetValue<string>(oneStudent, "grade"));
		}


		[Fact]
		public void GetStudentsInClassMoreThanOneStudent() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			uint cid = 101;
			Courses newCourse = new Courses {
				CatalogId = "12345",
				Listing = "CS",
				Name = "Algorithms",
				Number = "3550"
			};
			db.Courses.Add(newCourse);
			
			Classes newClass = new Classes {
				CId = cid,
				CatalogId = "12345",
				SemesterYear = 2019,
				SemesterSeason = "Spring",
				Location = "On the moon",
				StartTime = new TimeSpan(),
				EndTime = new TimeSpan()
			};
			db.Classes.Add(newClass);
			
			int numberOfStudents = 10;
			
			for (int i =0; i < numberOfStudents; i ++ ) {
				string uid = "u000000" + i;
				Students newStudent = new Students {
					UId = uid,
					Major = "CS",
					FirstName = "Brian" + i,
					LastName = "Landes" + i,
					Dob = DateTime.Now,

				};
				db.Students.Add(newStudent);

				Enroll(uid, cid, db);
			}
			
			db.SaveChanges();

			var jsonResult = controller.GetStudentsInClass("CS", 3550, "Spring", 2019) as JsonResult;

			Assert.Equal(numberOfStudents, (jsonResult.Value as dynamic).Length);

			for( int i = 0; i < numberOfStudents; i ++ ) {
				var oneStudent = Utils.JsonResultValueAtIndex(jsonResult, i);
				Assert.True(Utils.HasField(oneStudent, "fname"));
				Assert.True(Utils.HasField(oneStudent, "lname"));
				Assert.True(Utils.HasField(oneStudent, "uid"));
				Assert.True(Utils.HasField(oneStudent, "dob"));
				Assert.True(Utils.HasField(oneStudent, "grade"));
				Assert.False(Utils.HasField(oneStudent, "two heads"));

				Assert.Equal("Brian" + i, Utils.GetValue<string>(oneStudent, "fname"));
				Assert.Equal("Landes" + i, Utils.GetValue<string>(oneStudent, "lname"));
				Assert.Equal("--", Utils.GetValue<string>(oneStudent, "grade"));
			}
			
		}


		[Fact]
		public void GetStudentsInClassMoreThanOneStudentAndClasses() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			Courses newCourse = new Courses {
				CatalogId = "12345",
				Listing = "CS",
				Name = "Algorithms",
				Number = "3550"
			};
			db.Courses.Add(newCourse);

			int numberOfClasses = 6;

			Dictionary<int, int> classesCount = new Dictionary<int, int>();

			for( int i =0; i < numberOfClasses; i ++ ) {
				uint cid = 101 + (uint)i;
				int year = 2000 + i;
				Classes newClass = new Classes {
					CId = cid,
					CatalogId = "12345",
					SemesterYear = (ushort)year,
					SemesterSeason = "Spring",
					Location = "On the moon",
					StartTime = new TimeSpan(),
					EndTime = new TimeSpan()
				};
				db.Classes.Add(newClass);

				

				int numberOfStudents = 10 + i;
				classesCount.Add(year, numberOfStudents);
				for (int j = 0; j < numberOfStudents; j++) {
					string uid = "u00000" + i + j;
					Students newStudent = new Students {
						UId = uid,
						Major = "CS",
						FirstName = "Brian" + year + numberOfStudents,
						LastName = "Landes" + year + numberOfStudents,
						Dob = DateTime.Now,

					};
					db.Students.Add(newStudent);

					Enroll(uid, cid, db);
				}
			}
			
			db.SaveChanges();

			foreach( var pair in classesCount ) {

				int year = pair.Key;
				int numberOfStudents = pair.Value;

				var jsonResult = controller.GetStudentsInClass("CS", 3550, "Spring", year) as JsonResult;

				Utils.AssertJsonResultIsArrayOfLength(jsonResult, numberOfStudents);

				for (int i = 0; i < numberOfStudents; i++) {
					var oneStudent = Utils.JsonResultValueAtIndex(jsonResult, i);
					Assert.True(Utils.HasField(oneStudent, "fname"));
					Assert.True(Utils.HasField(oneStudent, "lname"));
					Assert.True(Utils.HasField(oneStudent, "uid"));
					Assert.True(Utils.HasField(oneStudent, "dob"));
					Assert.True(Utils.HasField(oneStudent, "grade"));
					Assert.False(Utils.HasField(oneStudent, "two heads"));

					Assert.Equal("Brian" + year + numberOfStudents, Utils.GetValue<string>(oneStudent, "fname"));
					Assert.Equal("Landes" + year + numberOfStudents, Utils.GetValue<string>(oneStudent, "lname"));
					Assert.Equal("--", Utils.GetValue<string>(oneStudent, "grade"));
				}
			}
		}

		[Fact]
		public void GetAssignmentsInCategory() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AddAssignmentAndCategory(cid, db);

			Submissions newSubmission = new Submissions {
				UId = uid,
				AId = 999,
				Content = "This is some serious content",
				Time = DateTime.Now,
				Score = 78,
			};
			db.Submissions.Add(newSubmission);

			db.SaveChanges();

			var jsonResult = controller.GetAssignmentsInCategory("CS", 3550, "Spring", 2019,
				"Quizzes" ) as JsonResult;
			Utils.AssertJsonResultIsArrayOfLength(jsonResult, 1);
			var oneAssignment = Utils.JsonResultValueAtIndex(jsonResult, 0);
			Assert.True(Utils.HasField(oneAssignment, "aname"));
			Assert.True(Utils.HasField(oneAssignment, "cname"));
			Assert.True(Utils.HasField(oneAssignment, "due"));
			Assert.True(Utils.HasField(oneAssignment, "submissions"));

			var numSubmissions = Utils.GetValue<int>(oneAssignment, "submissions");
			Assert.Equal(1, numSubmissions);
		}

		[Fact]
		public void GetAssignmentsInCategoryMultipleCategories() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);
			{
				AssignmentCategories assCat = new AssignmentCategories {
					AcId = 1002,
					Name = "Quizzes",
					GradingWeight = 50,
					CId = cid
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
			}
			int numAssCats = 10;

			for (int i = 0; i < numAssCats; i++) {
				AssignmentCategories assCat = new AssignmentCategories {
					AcId = 11002 + (uint)i,
					Name = "Quizzes" + i,
					GradingWeight = 50,
					CId = cid
				};
				db.AssignmentCategories.Add(assCat);

				Assignments newAssignment = new Assignments {
					AId = 1999 + (uint)i,
					AcId = 11002 + (uint)i,
					Name = "Bonus Quiz",
					MaxPointValue = 102,
					DueDate = DateTime.Now,
					Content = "oanklaks fl lja",
				};
				db.Assignments.Add(newAssignment);

				Submissions newSubmission = new Submissions {
					UId = uid,
					AId = 1999 + (uint)i,
					Content = "This is some serious content",
					Time = DateTime.Now,
					Score = 78,
				};
				db.Submissions.Add(newSubmission);
			}

			db.SaveChanges();

			var jsonResult = controller.GetAssignmentsInCategory("CS", 3550, "Spring", 2019,
				"Quizzes") as JsonResult;

			Utils.AssertJsonResultIsArrayOfLength(jsonResult, 1);
			var oneAssignment = Utils.JsonResultValueAtIndex(jsonResult, 0);
			Assert.True(Utils.HasField(oneAssignment, "aname"));
			Assert.True(Utils.HasField(oneAssignment, "cname"));
			Assert.True(Utils.HasField(oneAssignment, "due"));
			Assert.True(Utils.HasField(oneAssignment, "submissions"));

			var numSubmissions = Utils.GetValue<int>(oneAssignment, "submissions");
			Assert.Equal(1, numSubmissions);
		}

		[Fact]
		public void GetAssignmentsInCategoryMultipleAssignments() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			int numAssignments = 10;

			for (int i = 0; i < numAssignments; i++) {

				Assignments newAssignment = new Assignments {
					AId = 1999 + (uint)i,
					AcId = 1002,
					Name = "Bonus Quiz",
					MaxPointValue = 102,
					DueDate = DateTime.Now,
					Content = "oanklaks fl lja",
				};
				db.Assignments.Add(newAssignment);

				Submissions newSubmission = new Submissions {
					UId = uid,
					AId = 1999 + (uint)i,
					Content = "This is some serious content",
					Time = DateTime.Now,
					Score = 78,
				};
				db.Submissions.Add(newSubmission);
			}

			db.SaveChanges();

			var jsonResult = controller.GetAssignmentsInCategory("CS", 3550, "Spring", 2019,
				"Quizzes") as JsonResult;

			Utils.AssertJsonResultIsArrayOfLength(jsonResult, numAssignments);
			for (int i = 0; i < numAssignments; i++) {
				var oneAssignment = Utils.JsonResultValueAtIndex(jsonResult, 0);
				Assert.True(Utils.HasField(oneAssignment, "aname"));
				Assert.True(Utils.HasField(oneAssignment, "cname"));
				Assert.True(Utils.HasField(oneAssignment, "due"));
				Assert.True(Utils.HasField(oneAssignment, "submissions"));

				var numSubmissions = Utils.GetValue<int>(oneAssignment, "submissions");
				Assert.Equal(1, numSubmissions);
			}
		}

		[Fact]
		public void GetAssignmentsInCategoryMultipleSubmissions() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			uint cid = AddCourseAndClass(db);

			int numSubmissions = 10;

			for (int i = 0; i < numSubmissions; i++) {
				string uid = "" + i;
				Students newStudent = new Students {
					UId = uid,
					Major = "CS",
					FirstName = "Brian" + i,
					LastName = "Landes" + i,
					Dob = DateTime.Now,

				};
				db.Students.Add(newStudent);

				Enroll(uid, cid, db);
			}

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			Assignments newAssignment = new Assignments {
				AId = 1999,
				AcId = 1002,
				Name = "Bonus Quiz",
				MaxPointValue = 102,
				DueDate = DateTime.Now,
				Content = "oanklaks fl lja",
			};
			db.Assignments.Add(newAssignment);

			for (int i = 0; i < numSubmissions; i++) {
				
				Submissions newSubmission = new Submissions {
					UId = ""+i,
					AId = 1999,
					Content = "This is some serious content",
					Time = DateTime.Now,
					Score = 78,
				};
				db.Submissions.Add(newSubmission);
			}

			db.SaveChanges();

			var jsonResult = controller.GetAssignmentsInCategory("CS", 3550, "Spring", 2019,
				"Quizzes") as JsonResult;

			Utils.AssertJsonResultIsArrayOfLength(jsonResult, 1);
			var oneAssignment = Utils.JsonResultValueAtIndex(jsonResult, 0);
			Assert.True(Utils.HasField(oneAssignment, "aname"));
			Assert.True(Utils.HasField(oneAssignment, "cname"));
			Assert.True(Utils.HasField(oneAssignment, "due"));
			Assert.True(Utils.HasField(oneAssignment, "submissions"));

			Assert.Equal(numSubmissions, Utils.GetValue<int>(oneAssignment, "submissions"));
		}

		[Fact]
		public void GetAssignmentsNoCategory() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			AddAssignmentAndCategory(cid, db);

			Submissions newSubmission = new Submissions {
				UId = uid,
				AId = 999,
				Content = "This is some serious content",
				Time = DateTime.Now,
				Score = 78,
			};
			db.Submissions.Add(newSubmission);

			db.SaveChanges();

			var jsonResult = controller.GetAssignmentsInCategory("CS", 3550, "Spring", 2019,
				null ) as JsonResult;

			Utils.AssertJsonResultIsArrayOfLength(jsonResult, 1);
			var oneAssignment = Utils.JsonResultValueAtIndex(jsonResult, 0);
			Assert.True(Utils.HasField(oneAssignment, "aname"));
			Assert.True(Utils.HasField(oneAssignment, "cname"));
			Assert.True(Utils.HasField(oneAssignment, "due"));
			Assert.True(Utils.HasField(oneAssignment, "submissions"));

			var numSubmissions = Utils.GetValue<int>(oneAssignment, "submissions");
			Assert.Equal(1, numSubmissions);
		}


		[Fact]
		public void GetAssignmentsNoCategory2() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			string uid = StudentControllerTests.AddOneStudent(db);

			uint cid = AddCourseAndClass(db);

			Enroll(uid, cid, db);

			int numAssCats = 10;

			for (int i = 0; i < numAssCats; i++) {
				AssignmentCategories assCat = new AssignmentCategories {
					AcId = 1002 +(uint)i,
					Name = "Quizzes" + i,
					GradingWeight = 50,
					CId = cid
				};
				db.AssignmentCategories.Add(assCat);

				Assignments newAssignment = new Assignments {
					AId = 999 + (uint)i,
					AcId = 1002 + (uint)i,
					Name = "Bonus Quiz",
					MaxPointValue = 102,
					DueDate = DateTime.Now,
					Content = "oanklaks fl lja",
				};
				db.Assignments.Add(newAssignment);

				Submissions newSubmission = new Submissions {
					UId = uid,
					AId = 999 + (uint)i,
					Content = "This is some serious content",
					Time = DateTime.Now,
					Score = 78,
				};
				db.Submissions.Add(newSubmission);
			}

			db.SaveChanges();

			var jsonResult = controller.GetAssignmentsInCategory("CS", 3550, "Spring", 2019,
				null) as JsonResult;

			Utils.AssertJsonResultIsArrayOfLength(jsonResult, numAssCats);
			for (int i = 0; i < numAssCats; i++) {
				var oneAssignment = Utils.JsonResultValueAtIndex(jsonResult, i);
				Assert.True(Utils.HasField(oneAssignment, "aname"));
				Assert.True(Utils.HasField(oneAssignment, "cname"));
				Assert.True(Utils.HasField(oneAssignment, "due"));
				Assert.True(Utils.HasField(oneAssignment, "submissions"));

				var numSubmissions = Utils.GetValue<int>(oneAssignment, "submissions");
				Assert.Equal(1, numSubmissions);
			}
		}


		[Fact]
		public void CreateAssignmentTrivialSuccess() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);
			
			uint cid = AddCourseAndClass(db);
			
			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			db.SaveChanges();
			string content = "This is a very serious assignment about the collapse" +
				"of the Soviet Union and the fall of communism and the dawn of the cold war.";
			var jsonResult = controller.CreateAssignment("CS", 3550, "Spring",
				2019, "Quizzes", "Making Pizzas", 1000,
				DateTime.Now, content) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));
			var oneAssignment = (new List<Assignments>(db.Assignments))[0];
			Assert.Equal(content, oneAssignment.Content);
			Assert.Equal((uint)1000, oneAssignment.MaxPointValue);
			Assert.Equal((uint)1002, oneAssignment.AcId);
		}

		[Fact]
		public void CreateAssignmentTrivialFailure() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			uint cid = AddCourseAndClass(db);

			AssignmentCategories assCat = new AssignmentCategories {
				AcId = 1002,
				Name = "Quizzes",
				GradingWeight = 50,
				CId = cid
			};
			db.AssignmentCategories.Add(assCat);

			db.SaveChanges();

			var jsonResult = controller.CreateAssignment("CS", 13550, "Spring",
				2019, "Quizzes", "Making Pizzas", 1000,
				DateTime.Now, "This is a very serious assignment about the collapse" +
				"of the Soviet Union and the fall of communism and the dawn of the cold war.") as JsonResult;
			Assert.False(Utils.ResultSuccessValue(jsonResult));
		}


		[Fact]
		public void CreateAssignmentCategoryTrivialSuccess() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			uint cid = AddCourseAndClass(db);
			
			db.SaveChanges();
			var jsonResult = controller.CreateAssignmentCategory("CS", 3550, "Spring",
				2019, "Quizzes", 100 ) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));
			var oneAssCat = (new List<AssignmentCategories>(db.AssignmentCategories))[0];
			Assert.Equal("Quizzes", oneAssCat.Name);
			Assert.Equal((uint)100, oneAssCat.GradingWeight);
		}

		[Fact]
		public void CreateAssignmentCategoryTrivialFailure() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			uint cid = AddCourseAndClass(db);

			db.SaveChanges();
			var jsonResult = controller.CreateAssignmentCategory("CS", 13550, "Spring",
				2019, "Quizzes", 100) as JsonResult;
			Assert.False(Utils.ResultSuccessValue(jsonResult));
		}

		[Fact]
		public void CreateAssignmentCategoryDuplicate() {
			var db = Utils.MakeMockDatabase();

			var controller = MakeController(db);

			uint cid = AddCourseAndClass(db);

			db.SaveChanges();
			var jsonResult = controller.CreateAssignmentCategory("CS", 3550, "Spring",
				2019, "Quizzes", 100) as JsonResult;
			Assert.True(Utils.ResultSuccessValue(jsonResult));
			var oneAssCat = (new List<AssignmentCategories>(db.AssignmentCategories))[0];
			Assert.Equal("Quizzes", oneAssCat.Name);
			Assert.Equal((uint)100, oneAssCat.GradingWeight);

			jsonResult = controller.CreateAssignmentCategory("CS", 3550, "Spring",
				2019, "Quizzes", 100) as JsonResult;
			Assert.False(Utils.ResultSuccessValue(jsonResult));
		}
	}
}
