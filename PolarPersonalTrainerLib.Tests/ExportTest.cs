﻿using System;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;

namespace PolarPersonalTrainerLib.Tests
{
    [TestFixture]
    public class ExportTest
    {
        [Test]
        public async Task ExportRecent()
        {
            var export = new PPTExport(Settings.UserName, Settings.Password);
            var exercises = await export.GetExercises(DateTime.Now.AddDays(-10), DateTime.Now);
            Assert.IsNotNull(exercises);
            var pptExercises = exercises as PPTExercise[] ?? exercises.ToArray();
            Assert.IsTrue(pptExercises.Any());
            foreach (var exercise in pptExercises)
            {
                Assert.IsNotNull(exercise.HeartRate);
                if (exercise.HeartRate.Maximum > 0)
                    Assert.IsTrue(exercise.HeartRate.Maximum > 0);
                if (exercise.HeartRate.Values.Count > 0)
                {
                    Assert.AreEqual(exercise.HeartRate.Values.Count, exercise.SpeedValues.Count);
                    //Assert.AreEqual(exercise.HeartRate.Values.Count, exercise.CadenceValues.Count);
                }
                // Download GPS data
                try
                {
                    var gpxData = await export.GetGpsData(exercise);
                    Assert.IsTrue(gpxData.Track.Segments.Length > 0);
                    foreach (var segment in gpxData.Track.Segments)
                    {
                        Assert.IsTrue(segment.Trackpoints.Count > 0);
                    }
                }
                catch (PPTException)
                {
                    // no GPS data for this exercise
                }
            }
        }
    }
}
