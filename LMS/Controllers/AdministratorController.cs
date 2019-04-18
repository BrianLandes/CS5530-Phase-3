using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers {
	
	[Authorize(Roles = "Administrator")]
	public class AdministratorController : CommonController {
		public IActionResult Index() {
			return View();
		}

		public IActionResult Department(string subject) {
			ViewData["subject"] = subject;
			return View();
		}

		public IActionResult Course(string subject, string num) {
			ViewData["subject"] = subject;
			ViewData["num"] = num;
			return View();
		}

		/*******Begin code to modify********/

		/// <summary>
		/// Returns a JSON array of all the courses in the given department.
		/// Each object in the array should have the following fields:
		/// "number" - The course number (as in 5530)
		/// "name" - The course name (as in "Database Systems")
		/// </summary>
		/// <param name="subject">The department subject abbreviation (as in "CS")</param>
		/// <returns>The JSON result</returns>
		public IActionResult GetCourses(string subject) {
			var query =
				from c in db.Courses
				where c.Listing == subject
				select new {
					number = c.Number,
					name = c.Name
				};
			return Json(query.ToArray());
		}

		/// <summary>
		/// Returns a JSON array of all the professors working in a given department.
		/// Each object in the array should have the following fields:
		/// "lname" - The professor's last name
		/// "fname" - The professor's first name
		/// "uid" - The professor's uid
		/// </summary>
		/// <param name="subject">The department subject abbreviation</param>
		/// <returns>The JSON result</returns>
		public IActionResult GetProfessors(string subject) {
			var query =
				from p in db.Professors
				where p.WorksIn == subject
				select new {
					lname = p.LastName,
					fname = p.FirstName,
					uid = p.UId
				};
			return Json(query.ToArray());
		}

		/// <summary>
		/// Creates a course.
		/// A course is uniquely identified by its number + the subject to which it belongs
		/// </summary>
		/// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
		/// <param name="number">The course number</param>
		/// <param name="name">The course name</param>
		/// <returns>A JSON object containing {success = true/false}.
		/// false if the course already exists, true otherwise.</returns>
		public IActionResult CreateCourse(string subject, int number, string name) {

			var query = from course in db.Courses
						where course.Listing == subject &&
						course.Number == "" + number
						select course;

			// there already exists a course with that listing and number
			if ( query.Any() ) {
				return Json(new { success = false });
			}

			Courses newCourse = new Courses() {
				Listing = subject,
				Number = "" + number,
				Name = name
			};
			db.Courses.Add(newCourse);
			int rowsAffected = db.SaveChanges();
			
			return Json(new { success = rowsAffected>0 });
		}



		/// <summary>
		/// Creates a class offering of a given course.
		/// </summary>
		/// <param name="subject">The department subject abbreviation</param>
		/// <param name="number">The course number</param>
		/// <param name="season">The season part of the semester</param>
		/// <param name="year">The year part of the semester</param>
		/// <param name="start">The start time</param>
		/// <param name="end">The end time</param>
		/// <param name="location">The location</param>
		/// <param name="instructor">The uid of the professor</param>
		/// <returns>A JSON object containing {success = true/false}. 
		/// false if another class occupies the same location during any time 
		/// within the start-end range in the same semester, or if there is already
		/// a Class offering of the same Course in the same Semester,
		/// true otherwise.</returns>
		public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end, string location, string instructor) {

			var courseQuery = from course in db.Courses
						where course.Listing == subject
						&& Int32.Parse(course.Number) == number
						select course;

			// check for class offerings of the same course in the same semester
			{
				var classQuery = from course in courseQuery
								 join classOffering in db.Classes
								 on course.CatalogId equals classOffering.CatalogId
								 where classOffering.SemesterSeason == season
								 && classOffering.SemesterYear == year
								 select course;

				if (classQuery.Any()) {
					return Json(new { success = false });
				}
			}

			// check for class offerings that overlap the time and location
			{
				var classQuery = from classOffering in db.Classes
								 where classOffering.Location == location
								 where classOffering.SemesterSeason == season
								 && classOffering.SemesterYear == year
								 && classOffering.StartTime < end.TimeOfDay
								 && classOffering.EndTime > start.TimeOfDay
								 select classOffering;

				if (classQuery.Any()) {
					return Json(new { success = false });
				}
			}

			var theCourse = courseQuery.FirstOrDefault();

			Classes newClass = new Classes() {
				CatalogId = theCourse.CatalogId,
				Catalog = theCourse,
				SemesterSeason = season,
				SemesterYear = (ushort) year,
				StartTime = start.TimeOfDay,
				EndTime = end.TimeOfDay,
				Location = location,
				Teacher = instructor

			};
			db.Classes.Add(newClass);
			int rowsAffected = db.SaveChanges();
			
			return Json(new { success = rowsAffected > 0 });
		}


		/*******End code to modify********/

	}
}