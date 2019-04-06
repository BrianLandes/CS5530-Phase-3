using LMS.Models.LMSModels;
using Microsoft.EntityFrameworkCore;

namespace xUnitTests {
	public static class Utils {

		public static Team41LMSContext MakeMockDatabase() {
			var options = new DbContextOptionsBuilder<Team41LMSContext>();
			// had to run:
			// Install-Package Microsoft.EntityFrameworkCore.InMemory
			// in the Package Manager Console to get this command
			options.UseInMemoryDatabase();

			Team41LMSContext db = new Team41LMSContext(options.Options);
			return db;

		}
	}
}
