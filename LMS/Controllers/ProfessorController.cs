using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LMS.Models.LMSModels;

namespace LMS.Controllers {
	[Authorize(Roles = "Professor")]
	public class ProfessorController : CommonController {
		public IActionResult Index() {
			return View();
		}

		public IActionResult Students(string subject, string num, string season, string year) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			return View();
		}

		public IActionResult Class(string subject, string num, string season, string year) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			return View();
		}

		public IActionResult Categories(string subject, string num, string season, string year) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			return View();
		}

		public IActionResult CatAssignments(string subject, string num, string season, string year, string cat) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			ViewData["cat"] = cat;
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

		public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			ViewData["cat"] = cat;
			ViewData["aname"] = aname;
			return View();
		}

		public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			ViewData["season"] = season;
			ViewData["year"] = year;
			ViewData["cat"] = cat;
			ViewData["aname"] = aname;
			ViewData["uid"] = uid;
			return View();
		}

		/*******Begin code to modify********/


		/// <summary>
		/// Returns a JSON array of all the students in a class.
		/// Each object in the array should have the following fields:
		/// "fname" - first name
		/// "lname" - last name
		/// "uid" - user ID
		/// "dob" - date of birth
		/// "grade" - the student's grade in this class
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <returns>The JSON array</returns>
		public IActionResult GetStudentsInClass(string subject, int num, string season, int year) {
			var query =
				from c in db.Courses
				join c2 in db.Classes
				on c.CatalogId equals c2.CatalogId
				join e in db.Enrolled
				on c2.CId equals e.CId
				join s in db.Students
				on e.UId equals s.UId
				where c.Listing == subject
				&& c.Number == num.ToString()
				&& c2.SemesterSeason == season
				&& c2.SemesterYear == year
				select new {
					fname = s.FirstName,
					lname = s.LastName,
					uid = s.UId,
					dob = s.Dob.Date,
					grade = e.Grade
				};
			return Json(query.ToArray());
		}



		/// <summary>
		/// Returns a JSON array with all the assignments in an assignment category for a class.
		/// If the "category" parameter is null, return all assignments in the class.
		/// Each object in the array should have the following fields:
		/// "aname" - The assignment name
		/// "cname" - The assignment category name.
		/// "due" - The due DateTime
		/// "submissions" - The number of submissions to the assignment
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class, 
		/// or null to return assignments from all categories</param>
		/// <returns>The JSON array</returns>
		public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category) {
			//better way to do this??
			if (category == null) {
				var query = from c in db.Courses
							join c2 in db.Classes
							on c.CatalogId equals c2.CatalogId
							join ac in db.AssignmentCategories
							on c2.CId equals ac.CId
							join a in db.Assignments
							on ac.AcId equals a.AcId
							where c.Listing == subject
							&& c.Number == num.ToString()
							&& c2.SemesterSeason == season
							&& c2.SemesterYear == year
							//&& ac.Name == category
							select new {
								aname = a.Name,
								cname = ac.Name,
								due = a.DueDate,
								// fix this
								Submissions = 1
							};
				return Json(query.ToArray());
			}
			else {
				var query = from c in db.Courses
							join c2 in db.Classes
							on c.CatalogId equals c2.CatalogId
							join ac in db.AssignmentCategories
							on c2.CId equals ac.CId
							join a in db.Assignments
							on ac.AcId equals a.AcId
							where c.Listing == subject
							&& c.Number == num.ToString()
							&& c2.SemesterSeason == season
							&& c2.SemesterYear == year
							&& ac.Name == category
							select new {
								aname = a.Name,
								cname = ac.Name,
								due = a.DueDate,
								// fix this
								Submissions = 1
							};
				return Json(query.ToArray());
			}
		}


		/// <summary>
		/// Returns a JSON array of the assignment categories for a certain class.
		/// Each object in the array should have the folling fields:
		/// "name" - The category name
		/// "weight" - The category weight
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class</param>
		/// <returns>The JSON array</returns>
		public IActionResult GetAssignmentCategories(string subject, int num, string season, int year) {
			var query = from c in db.Courses
						join c2 in db.Classes
						on c.CatalogId equals c2.CatalogId
						join ac in db.AssignmentCategories
						on c2.CId equals ac.CId
						where c.Listing == subject
						&& c.Number == num.ToString()
						&& c2.SemesterSeason == season
						&& c2.SemesterYear == year
						select new {
							name = ac.Name,
							weight = ac.GradingWeight
						};

			return Json(query.ToArray());
		}

		/// <summary>
		/// Creates a new assignment category for the specified class.
		/// If a category of the given class with the given name already exists, return success = false.
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The new category name</param>
		/// <param name="catweight">The new category weight</param>
		/// <returns>A JSON object containing {success = true/false} </returns>
		public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight) {

			var query = from c in db.Courses
						join c2 in db.Classes
						on c.CatalogId equals c2.CatalogId
						where c.Listing == subject
						&& c.Number == num.ToString()
						&& c2.SemesterSeason == season
						&& c2.SemesterYear == year
						select c2.CId;
			AssignmentCategories newAC = new AssignmentCategories() {
				Name = category,
				GradingWeight = (uint)catweight,
				CId = query.FirstOrDefault()
			};

			db.AssignmentCategories.Add(newAC);
			int rowsAffected = db.SaveChanges();

			return Json(new { success = rowsAffected > 0 });
		}

		/// <summary>
		/// Creates a new assignment for the given class and category.
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class</param>
		/// <param name="asgname">The new assignment name</param>
		/// <param name="asgpoints">The max point value for the new assignment</param>
		/// <param name="asgdue">The due DateTime for the new assignment</param>
		/// <param name="asgcontents">The contents of the new assignment</param>
		/// <returns>A JSON object containing success = true/false</returns>
		public IActionResult CreateAssignment(string subject, int num, string season,
				int year, string category, string asgname, int asgpoints,
				DateTime asgdue, string asgcontents) {

			var query = from c in db.Courses
						join c2 in db.Classes
						on c.CatalogId equals c2.CatalogId
						join a in db.AssignmentCategories
						on c2.CId equals a.CId
						where c.Listing == subject
						&& c.Number == num.ToString()
						&& c2.SemesterSeason == season
						&& c2.SemesterYear == year
						&& a.Name == category
						select a.AcId;
			Assignments newAssignment = new Assignments() {
				AcId = query.FirstOrDefault(),
				Name = asgname,
				MaxPointValue = (uint)asgpoints,
				DueDate = asgdue,
				Content = asgcontents
			};

			db.Assignments.Add(newAssignment);
			int rowsAffected = db.SaveChanges();

			if (rowsAffected > 0) {
				UpdateGrades(subject, num, season, year);
			}

			return Json(new { success = rowsAffected > 0 });
		}


		/// <summary>
		/// Gets a JSON array of all the submissions to a certain assignment.
		/// Each object in the array should have the following fields:
		/// "fname" - first name
		/// "lname" - last name
		/// "uid" - user ID
		/// "time" - DateTime of the submission
		/// "score" - The score given to the submission
		/// 
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class</param>
		/// <param name="asgname">The name of the assignment</param>
		/// <returns>The JSON array</returns>
		public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname) {
			var query = from c in db.Courses
						join c2 in db.Classes
						on c.CatalogId equals c2.CatalogId
						join ac in db.AssignmentCategories
						on c2.CId equals ac.CId
						join a in db.Assignments
						on ac.AcId equals a.AcId
						join s in db.Submissions
						on a.AId equals s.AId
						join s2 in db.Students
						on s.UId equals s2.UId

						where c.Listing == subject
						&& c.Number == num.ToString()
						&& c2.SemesterSeason == season
						&& c2.SemesterYear == year
						&& ac.Name == category
						&& a.Name == asgname
						select new {
							fname = s2.FirstName,
							lname = s2.LastName,
							uid = s2.UId,
							time = s.Time,
							score = s.Score
						};

			return Json(query.ToArray());
		}


		/// <summary>
		/// Set the score of an assignment submission
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class</param>
		/// <param name="asgname">The name of the assignment</param>
		/// <param name="uid">The uid of the student who's submission is being graded</param>
		/// <param name="score">The new score for the submission</param>
		/// <returns>A JSON object containing success = true/false</returns>
		public IActionResult GradeSubmission(string subject, int num, string season,
				int year, string category, string asgname, string uid, int score) {

			var submissionQuery =
				from course in db.Courses
				join classOffering in db.Classes
				on course.CatalogId equals classOffering.CatalogId
				join assCat in db.AssignmentCategories
				on classOffering.CId equals assCat.CId
				join assignment in db.Assignments
				on assCat.AcId equals assignment.AcId
				where assCat.Name == category
				where assignment.Name == asgname
				where course.Listing == subject
				&& course.Number == num.ToString()
				&& classOffering.SemesterSeason == season
				&& classOffering.SemesterYear == year
				from sub in db.Submissions
				where assignment.AId == sub.AId
				&& sub.UId == uid
				select sub;

			var theSubmission = submissionQuery.FirstOrDefault();
			if (theSubmission == null) {
				return Json(new { success = false });
			}

			theSubmission.Score = (uint)score;
			int rowsAffected = db.SaveChanges();

			UpdateGrade(subject, num, season, year, uid);

			return Json(new { success = rowsAffected > 0 });
		}


		/// <summary>
		/// Returns a JSON array of the classes taught by the specified professor
		/// Each object in the array should have the following fields:
		/// "subject" - The subject abbreviation of the class (such as "CS")
		/// "number" - The course number (such as 5530)
		/// "name" - The course name
		/// "season" - The season part of the semester in which the class is taught
		/// "year" - The year part of the semester in which the class is taught
		/// </summary>
		/// <param name="uid">The professor's uid</param>
		/// <returns>The JSON array</returns>
		public IActionResult GetMyClasses(string uid) {
			var query =
				from c in db.Classes
				join course in db.Courses
				on c.CatalogId equals course.CatalogId
				where c.Teacher == uid
				select new {
					subject = course.Listing,
					number = course.Number,
					name = course.Name,
					season = c.SemesterSeason,
					year = c.SemesterYear

				};
			return Json(query.ToArray());
		}

		private void UpdateGrades(string subject, int num, string season,
			int year) {
			var query = from course in db.Courses
						join classes in db.Classes
						on course.CatalogId equals classes.CatalogId
						join enroll in db.Enrolled
						on classes.CId equals enroll.CId
						where course.Listing == subject
						&& course.Number == num.ToString()
						&& classes.SemesterSeason == season
						&& classes.SemesterYear == year
						select new {
							uid = enroll.UId
						};
			foreach (var uid in query) {
				UpdateGrade(subject, num, season, year, uid.ToString());
			}
		}

		private IActionResult UpdateGrade(string subject, int num, string season,
				int year, string uid) {

			var query =
				from course in db.Courses
				join classOffering in db.Classes
				on course.CatalogId equals classOffering.CatalogId
				join assCat in db.AssignmentCategories
				on classOffering.CId equals assCat.CId
				where course.Listing == subject
				&& Int32.Parse(course.Number) == num
				&& classOffering.SemesterSeason == season
				&& classOffering.SemesterYear == year
				select new {
					weight = assCat.GradingWeight,
					assignments = from a in db.Assignments
								  where a.AcId == assCat.AcId
								  join s in db.Submissions
								  on a.AId equals s.AId
								  select new {
									  maxpoint = a.MaxPointValue,
									  earned = s.Score
								  }
				};


			double totalWeight = 0;
			double totalPercentage = 0;

			foreach (var category in query) {
				if (category.assignments.Count() != 0) {
					double totalEarned = 0;
					double totalMax = 0;
					totalWeight += category.weight;
					foreach (var assignment in category.assignments) {
						totalEarned += assignment.earned;
						totalMax += assignment.maxpoint;
					}

					totalPercentage += category.weight * (totalEarned / totalMax);
				}
			}
			double scalingFactor = 100 / totalWeight;
			double percent = totalPercentage * scalingFactor;
			string grade = GetLetterGrade(percent);

			//update the enrolled table
			var updateQuery = from enroll in db.Enrolled
							  where enroll.UId == uid
							  select enroll;
			var update = updateQuery.FirstOrDefault();
			update.Grade = grade;
			int rowsAffected = db.SaveChanges();

			return Json(new { success = rowsAffected > 0 });
		}

		private string GetLetterGrade(double percentage) {
			if (percentage >= 93) {
				return "A";
			}
			else if (percentage >= 90) {
				return "A-";
			}
			else if (percentage >= 87) {
				return "B+";
			}
			else if (percentage >= 83) {
				return "B";
			}
			else if (percentage >= 80) {
				return "B-";
			}
			else if (percentage >= 77) {
				return "C+";
			}
			else if (percentage >= 73) {
				return "C";
			}
			else if (percentage >= 70) {
				return "C-";
			}
			else if (percentage >= 67) {
				return "D+";
			}
			else if (percentage >= 63) {
				return "D";
			}
			else if (percentage >= 60) {
				return "D-";
			}
			else {
				return "E";
			}
		}

		/*******End code to modify********/

	}
}