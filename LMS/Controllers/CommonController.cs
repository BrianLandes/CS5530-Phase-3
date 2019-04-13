using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers {

	public class CommonController : Controller {

		/*******Begin code to modify********/

		protected Team41LMSContext db;

		public CommonController() {
			db = new Team41LMSContext();
		}
		/*
		 * WARNING: This is the quick and easy way to make the controller
		 *          use a different LibraryContext - good enough for our purposes.
		 *          The "right" way is through Dependency Injection via the constructor 
		 *          (look this up if interested).
		*/

		public void UseLMSContext(Team41LMSContext ctx) {
			db = ctx;
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}



		/// <summary>
		/// Retreive a JSON array of all departments from the database.
		/// Each object in the array should have a field called "name" and "subject",
		/// where "name" is the department name and "subject" is the subject abbreviation.
		/// </summary>
		/// <returns>The JSON array</returns>
		public IActionResult GetDepartments() {
			var query =
				from d in db.Departments
				select new {
					subject = d.Subject,
					name = d.Name
				};
			return Json(query.ToArray());
		}



		/// <summary>
		/// Returns a JSON array representing the course catalog.
		/// Each object in the array should have the following fields:
		/// "subject": The subject abbreviation, (e.g. "CS")
		/// "dname": The department name, as in "Computer Science"
		/// "courses": An array of JSON objects representing the courses in the department.
		///            Each field in this inner-array should have the following fields:
		///            "number": The course number (e.g. 5530)
		///            "cname": The course name (e.g. "Database Systems")
		/// </summary>
		/// <returns>The JSON array</returns>
		public IActionResult GetCatalog() {
			var query =
				from d in db.Departments
				join c in db.Courses
				on d.Subject equals c.Listing
				select new {
					subject = d.Subject,
					dname = d.Name,
					courses = from c2 in db.Courses
							  select new {
								  number = c2.Number,
								  cname = c2.Name
							  }
				};
			return Json(query.ToArray());
		}

		/// <summary>
		/// Returns a JSON array of all class offerings of a specific course.
		/// Each object in the array should have the following fields:
		/// "season": the season part of the semester, such as "Fall"
		/// "year": the year part of the semester
		/// "location": the location of the class
		/// "start": the start time in format "hh:mm:ss"
		/// "end": the end time in format "hh:mm:ss"
		/// "fname": the first name of the professor
		/// "lname": the last name of the professor
		/// </summary>
		/// <param name="subject">The subject abbreviation, as in "CS"</param>
		/// <param name="number">The course number, as in 5530</param>
		/// <returns>The JSON array</returns>
		public IActionResult GetClassOfferings(string subject, int number) {
			var query =
				from c in db.Classes
				join c2 in db.Courses
				on c.CatalogId equals c2.CatalogId
				join p in db.Professors
				on c.Teacher equals p.UId
				where c2.Listing == subject
				&& c2.Number == number.ToString()
				select new {
					season = c.SemesterSeason,
					year = c.SemesterYear,
					location = c.Location,
					start = c.StartTime,
					end = c.EndTime,
					fname = p.FirstName,
					lname = p.LastName
				};
			return Json(query.ToArray());
		}

		/// <summary>
		/// This method does NOT return JSON. It returns plain text (containing html).
		/// Use "return Content(...)" to return plain text.
		/// Returns the contents of an assignment.
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class</param>
		/// <param name="asgname">The name of the assignment in the category</param>
		/// <returns>The assignment contents</returns>
		public IActionResult GetAssignmentContents(string subject, int num, string season, int year, string category, string asgname) {

			return Content("");
		}


		/// <summary>
		/// This method does NOT return JSON. It returns plain text (containing html).
		/// Use "return Content(...)" to return plain text.
		/// Returns the contents of an assignment submission.
		/// Returns the empty string ("") if there is no submission.
		/// </summary>
		/// <param name="subject">The course subject abbreviation</param>
		/// <param name="num">The course number</param>
		/// <param name="season">The season part of the semester for the class the assignment belongs to</param>
		/// <param name="year">The year part of the semester for the class the assignment belongs to</param>
		/// <param name="category">The name of the assignment category in the class</param>
		/// <param name="asgname">The name of the assignment in the category</param>
		/// <param name="uid">The uid of the student who submitted it</param>
		/// <returns>The submission text</returns>
		public IActionResult GetSubmissionText(string subject, int num, string season, int year, string category, string asgname, string uid) {

			return Content("");
		}


		/// <summary>
		/// Gets information about a user as a single JSON object.
		/// The object should have the following fields:
		/// "fname": the user's first name
		/// "lname": the user's last name
		/// "uid": the user's uid
		/// "department": (professors and students only) the name (such as "Computer Science") of the department for the user. 
		///               If the user is a Professor, this is the department they work in.
		///               If the user is a Student, this is the department they major in.    
		///               If the user is an Administrator, this field is not present in the returned JSON
		/// </summary>
		/// <param name="uid">The ID of the user</param>
		/// <returns>
		/// The user JSON object 
		/// or an object containing {success: false} if the user doesn't exist
		/// </returns>
		public IActionResult GetUser(string uid) {

			// TODO: test it all
			// TODO: test it against a userid that doesn't exist
			// TODO: test it against an administrator
			// TODO: test it against a professor
			// TODO: test it against a student

			// we're only looking for one object
			// lets just check one at a time whether they are in admins, students, or professors
			// and return the first one we find

			// Administrators
			{
				var query = 
					from administrator in db.Administrators
					where administrator.UId == uid
					select new {
						fname = administrator.FirstName,
						lname = administrator.LastName,
						uid = administrator.UId,
						// leave off department
					};
				var theAdministrator = query.FirstOrDefault();
				if ( theAdministrator!=null ) {
					// found one
					return Json(theAdministrator);
				}
			}

			// Professor
			{
				var query =
					from professor in db.Professors
					where professor.UId == uid
					select new {
						fname = professor.FirstName,
						lname = professor.LastName,
						uid = professor.UId,
						department = professor.WorksInNavigation.Name
					};
				var theProfessor = query.FirstOrDefault();
				if (theProfessor != null) {
					// found one
					return Json(theProfessor);
				}
			}

			// Student
			{
				var query =
					from student in db.Students
					where student.UId == uid
					select new {
						fname = student.FirstName,
						lname = student.LastName,
						uid = student.UId,
						department = student.MajorNavigation.Name
					};
				var theStudent = query.FirstOrDefault();
				if (theStudent != null) {
					// found one
					return Json(theStudent);
				}
			}

			// didn't find one
			return Json(new { success = false });
		}


		/*******End code to modify********/

	}
}