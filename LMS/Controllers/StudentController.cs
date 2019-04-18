using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers {
	[Authorize(Roles = "Student")]
	public class StudentController : CommonController {

		public IActionResult Index() {
			return View();
		}

		public IActionResult Catalog() {
			return View();
		}

		public IActionResult Class(string subject, string num, string season, string year) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			return View();
		}

		public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			ViewData["cat"] = cat;
			ViewData["aname"] = aname;
			return View();
		}


		public IActionResult ClassListings(string subject, string num) {
			System.Diagnostics.Debug.WriteLine(subject + num);
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			return View();
		}


		/*******Begin code to modify********/

		/// <summary>
		/// Returns a JSON array of the classes the given student is enrolled in.
		/// Each object in the array should have the following fields:
		/// "subject" - The subject abbreviation of the class (such as "CS")
		/// "number" - The course number (such as 5530)
		/// "name" - The course name
		/// "season" - The season part of the semester
		/// "year" - The year part of the semester
		/// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
		/// </summary>
		/// <param name="uid">The uid of the student</param>
		/// <returns>The JSON array</returns>
		public IActionResult GetMyClasses(string uid) {

			var query =
				from enrollment in db.Enrolled
				where enrollment.UId == uid
				from c in db.Classes
				where c.CId == enrollment.CId
				from course in db.Courses
				where c.CatalogId == course.CatalogId
				select new {
					subject = course.Listing,
					number = course.Number,
					name = course.Name,
					season = c.SemesterSeason,
					year = c.SemesterYear,
					grade = enrollment.Grade
				};

			return Json(query.ToArray());
		}

		/// <summary>
		/// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
		/// Each object in the array should have the following fields:
		/// "aname" - The assignment name
		/// "cname" - The category name that the assignment belongs to
		/// "due" - The due Date/Time
		/// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="uid"></param>
		/// <returns>The JSON array</returns>
		public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid) {

			// TODO: test it against a student with one assignment where other students and other assignments exist
			// TODO: test it against a student with multiple assignments
			// TODO: test it against a student with multiple assignments where other students and other assignments exist
			var query = from course in db.Courses
					join classOffering in db.Classes
					on course.CatalogId equals classOffering.CatalogId
					join assCat in db.AssignmentCategories
					on classOffering.CId equals assCat.CId
					join assignment in db.Assignments
					on assCat.AcId equals assignment.AcId
					where course.Listing == subject
					&& course.Number == num.ToString()
					&& classOffering.SemesterSeason == season
					&& classOffering.SemesterYear == year

					select new {
						aname = assignment.Name,
						cname = assCat.Name,
						due = assignment.DueDate,
						//score = sub == null ? null : (uint?)sub.Score
						score = (from sub in db.Submissions
								where assignment.AId == sub.AId
								&& sub.UId == uid
								select (uint ? )sub.Score)
								.FirstOrDefault<uint?>()
					};
			
			return Json(query.ToArray());
		}



		/// <summary>
		/// Adds a submission to the given assignment for the given student
		/// The submission should use the current time as its DateTime
		/// You can get the current time with DateTime.Now
		/// The score of the submission should start as 0 until a Professor grades it
		/// If a Student submits to an assignment again, it should replace the submission contents
		/// and the submission time (the score should remain the same).
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class</param>
		/// <param name="asgname">The new assignment name</param>
		/// <param name="uid">The student submitting the assignment</param>
		/// <param name="contents">The text contents of the student's submission</param>
		/// <returns>A JSON object containing {success = true/false}</returns>
		public IActionResult SubmitAssignmentText(string subject, int num, string season, int year,
		  string category, string asgname, string uid, string contents) {

			// TODO: test it at all
			// TODO: test it against a typical, successful case
			// TODO: test it against a resubmission

			// course subject <-> course CatalogId

			// get the assignment id
			var query =
				from enrollment in db.Enrolled
				where enrollment.UId == uid
				from c in db.Classes
				where c.CId == enrollment.CId
				&& c.CatalogId == subject
				&& Int32.Parse(c.Catalog.Number) == num
				&& c.SemesterSeason == season
				&& c.SemesterYear == year
				from assCat in db.AssignmentCategories
				where assCat.CId == c.CId
				&& assCat.Name == category
				from assignment in db.Assignments
				where assignment.AcId == assCat.AcId
				&& assignment.Name == asgname
				select assignment;

			var assignmentInfo = query.FirstOrDefault();
			if ( assignmentInfo==null ) {
				return Json(new { success = false });
			}

			// check to see if this is a re-submission
			var resubmissionQuery =
				from submission in db.Submissions
				where submission.UId == uid
				&& submission.AId == assignmentInfo.AId
				select submission;

			var resubmission = resubmissionQuery.FirstOrDefault();
			if (resubmission != null) {
				// there is already a submission for this assignment -> update the existing one
				resubmission.Content = contents;
				resubmission.Time = DateTime.Now;
				int rowsAffected = db.SaveChanges();
				return Json(new { success = rowsAffected > 0 });
			} else {
				// there is no previous submission -> create a new one

				var newSubmission = new Submissions {
					UId = uid,
					AId = assignmentInfo.AId,
					Time = DateTime.Now,
					Content = contents,
					Score = 0
				};
				db.Submissions.Add(newSubmission);
				int rowsAffected = db.SaveChanges();

				return Json(new { success = rowsAffected>0 });
			}
		}


		/// <summary>
		/// Enrolls a student in a class.
		/// </summary>
		/// <param name="subject">The department subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester</param>
		/// <param name="year">The year part of the semester</param>
		/// <param name="uid">The uid of the student</param>
		/// <returns>A JSON object containing {success = {true/false}. 
		/// false if the student is already enrolled in the class, true otherwise.</returns>
		public IActionResult Enroll(string subject, int num, string season, int year, string uid) {

			// TODO: test it at all
			// TODO: test it against a successful, typical case
			// TODO: test it when the student is already enrolled in the class
			// TODO: test it against a class that doesn't exist

			// get the class
			var classQuery =
				from c in db.Classes
				where c.CatalogId == subject
				&& Int32.Parse(c.Catalog.Number) == num
				&& c.SemesterSeason == season
				&& c.SemesterYear == year
				select c;

			// check if the student is already enrolled
			{
				var query =
					from enrollment in db.Enrolled
					where enrollment.UId == uid
					from c in classQuery
					where c.CId == enrollment.CId
					select enrollment;

				var enrollmentInfo = query.FirstOrDefault();
				if (enrollmentInfo != null) {
					// already enrolled
					return Json(new { success = false });
				}
			}// end of check if the student is already enrolled

			var theClass = classQuery.FirstOrDefault();
			if ( theClass == null ) {
				// the class doesn't exist
				return Json(new { success = false });
			}

			var newEnrollment = new Enrolled {
				UId = uid,
				CId = theClass.CId,
				Grade = "--"
			};

			db.Enrolled.Add(newEnrollment);
			int rowsAffected = db.SaveChanges();

			return Json(new { success = rowsAffected > 0 });
		}



		/// <summary>
		/// Calculates a student's GPA
		/// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
		/// Assume all classes are 4 credit hours.
		/// If a student does not have a grade in a class ("--"), that class is not counted in the average.
		/// If a student is not enrolled in any classes, they have a GPA of 0.0.
		/// Otherwise, the point-value of a letter grade is determined by the table on this page:
		/// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
		/// </summary>
		/// <param name="uid">The uid of the student</param>
		/// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
		public IActionResult GetGPA(string uid) {

			// TODO: test it at all
			// TODO: test against a student that is not enrolled in any classes
			// TODO: test against a student that is enrolled in classes, but has no grades
			// TODO: test against a student that is enrolled in a class, has a grade, and should get a 4.0
			// TODO: test against a student that is enrolled in multiple classes, has grades, and should get a 4.0
			// TODO: test against a student that is enrolled in multiple classes, has grades, and should get a 3.3 etc

			var query =
				from enrollment in db.Enrolled
				where enrollment.UId == uid
				select enrollment;

			if ( query.FirstOrDefault() == null ) {
				// this student is not enrolled in any classes
				return Json(new { gpa = 0.0f });
			}

			float creditCount = 0;
			float totalCredits = 0;

			foreach( var enrollment in query ) {
				if ( enrollment.Grade == "--" || string.IsNullOrEmpty(enrollment.Grade) ) {
					// ignore
					continue;
				}
				totalCredits += 4;
				creditCount += GradeToGradePoint(enrollment.Grade);
			}

			return Json(new { gpa = creditCount/ totalCredits });
		}

		private float GradeToGradePoint(string grade) {
			switch( grade ) {
				case "A": return 4f;
				case "A-": return 3.7f;
				case "B+": return 3.3f;
				case "B": return 3.0f;
				case "B-": return 2.7f;
				case "C+": return 2.3f;
				case "C": return 2.0f;
				case "C-": return 1.7f;
				case "D+": return 1.3f;
				case "D": return 1.0f;
				case "D-": return 0.7f;
				case "E": return 0.0f;
				default: return 0.0f;
			}
		}

		/*******End code to modify********/

	}
}