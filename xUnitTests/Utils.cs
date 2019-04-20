using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Xunit;

namespace xUnitTests {
	public static class Utils {

		public static Team41LMSContext MakeMockDatabase() {
			var options = new DbContextOptionsBuilder<Team41LMSContext>();
			// had to run:
			// Install-Package Microsoft.EntityFrameworkCore.InMemory
			// in the Package Manager Console to get this command
			options.UseInMemoryDatabase(Guid.NewGuid().ToString());
			// there was also something in the lecture about a service provider but he
			// didn't show the actual code for that

			Team41LMSContext db = new Team41LMSContext(options.Options);

			// Add a department
			Departments newDepartment = new Departments() {
				Name = "Computer Science",
				Subject = "CS",
			};

			db.Departments.Add(newDepartment);
			db.SaveChanges();

			return db;

		}

		/// <summary>
		/// Takes the JsonResult from a call and returns the bool success value,
		/// assuming that the call returned an anonymous type with a bool property
		/// called 'success'
		/// </summary>
		public static bool ResultSuccessValue(JsonResult jsonResult ) {
			dynamic resultValue = jsonResult.Value;

			return resultValue.GetType().GetProperty("success").GetValue(resultValue);
		}

		/// <summary>
		/// Have to use this reflection detour because the controllers return
		/// anonymous types, which the tests don't know the internals of, I guess?
		/// </summary>
		public static T GetValue<T>(dynamic resultValue, string propertyName) {
			return (T)(resultValue.GetType().GetProperty(propertyName).GetValue(resultValue));
		}

		public static bool HasField(dynamic resultValue, string propertyName) {
			try {
				PropertyInfo myPropInfo = resultValue.GetType().GetProperty(propertyName);
				return myPropInfo!=null;
			} catch (NullReferenceException e) {
				return false;
			}
		}

		public static dynamic JsonResultValueAtIndex(JsonResult jsonResult, int index) {
			dynamic resultValue = jsonResult.Value as dynamic;
			return resultValue[index] as dynamic;
		}

		public static void AssertJsonResultIsArrayOfLength(JsonResult jsonResult, int length) {
			Assert.Equal(length, (jsonResult.Value as dynamic).Length);
		}
		
	}
}
