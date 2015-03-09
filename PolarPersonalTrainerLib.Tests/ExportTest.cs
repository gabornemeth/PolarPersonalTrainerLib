using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
namespace PolarPersonalTrainerLib.Tests
{
    [TestClass]
    public class ExportTest
    {
        [TestMethod]
        public async Task ExportRecent()
        {
            PPTExport export = new PPTExport(Settings.UserName, Settings.Password);
            var exercises = await export.GetExercises(DateTime.Now.AddDays(-7), DateTime.Now);
            Assert.IsNotNull(exercises);
            Assert.IsTrue(exercises.Count() > 0);
            foreach (var exercise in exercises)
            {
                Assert.IsNotNull(exercise.HeartRate);
                Assert.IsTrue(exercise.HeartRate.Maximum > 0);
            }
        }
    }
}
