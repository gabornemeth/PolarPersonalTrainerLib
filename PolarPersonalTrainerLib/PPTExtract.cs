using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PolarPersonalTrainerLib
{
    /// <summary>
    /// XML to PPTExercise converter
    /// </summary>
    public class PPTExtract
    {
        /// <summary>
        /// Parsing sample/values node
        /// </summary>
        /// <param name="sampleElement"></param>
        /// <param name="action"></param>
        private static void ParseValues(XElement sampleElement, Action<string> action)
        {
            var values = sampleElement.GetFirstDescendantValue<string>("values");
            if (values != null)
            {
                foreach (var value in values.Split(','))
                {
                    action(value);
                }
            }
        }

        public static List<PPTExercise> ConvertXmlToExercises(XElement xml, bool requireSport = false)
        {
            var workouts = xml.GetDescendants("exercise");
            if (workouts == null)
                throw new InvalidDataException("No Polar exercises found");

            List<PPTExercise> exercises = new List<PPTExercise>();

            foreach (var element in workouts)
            {
                PPTExercise exercise = new PPTExercise();

                exercise.Distance = element.GetFirstDescendantValue<float>("distance", CultureInfo.InvariantCulture);
                exercise.StartTime = element.GetFirstDescendantValue<DateTime>("time");
                exercise.Sport = element.GetFirstDescendantValue<string>("sport");
                exercise.RecordingRate = element.GetFirstDescendantValue<int>("recording-rate");
                exercise.CadenceValues = new List<byte>();
                exercise.SpeedValues = new List<float>();

                var timeNode = element.GetFirstDescendant("time");
                var sportNode = element.GetFirstDescendant("sport");
                var resultNode = element.GetFirstDescendant("result");

                if (timeNode == null || resultNode == null)
                    continue;

                if (requireSport && sportNode == null)
                    continue;

                if (sportNode == null && requireSport)
                    continue;

                var hrNode = resultNode.GetFirstElement("heart-rate");
                var userNode = resultNode.GetFirstDescendant("user-settings");
                var hrUserNode = userNode.GetFirstDescendant("heart-rate");
                var vo2MaxNode = userNode.GetFirstDescendant("vo2max");

                exercise.Calories = resultNode.GetFirstDescendantValue<int>("calories");
                var duration = resultNode.GetFirstDescendantValue<string>("duration");
                if (!string.IsNullOrEmpty(duration))
                {
                    exercise.Duration = TimeSpan.Parse(duration);
                }

                var hrData = new HeartRate();

                if (hrNode != null)
                {
                    hrData.Maximum = hrNode.GetFirstDescendantValue<int>("maximum");
                    hrData.Average = hrNode.GetFirstDescendantValue<int>("average");
                }

                if (hrUserNode != null)
                {
                    hrData.Resting = hrUserNode.GetFirstDescendantValue<int>("resting");
                }
                hrData.Vo2Max = userNode.GetFirstDescendantValue<int>("vo2max");

                exercise.HeartRate = hrData;

                foreach (var sample in element.GetDescendants("sample"))
                {
                    var type = sample.GetFirstDescendantValue<string>("type");
                    if (type == "HEARTRATE")
                        ParseValues(sample, hr => { exercise.HeartRate.Values.Add(Convert.ToByte(hr)); });
                    else if (type == "SPEED")
                        ParseValues(sample, speed => { exercise.SpeedValues.Add(Convert.ToSingle(speed, CultureInfo.InvariantCulture)); });
                    else if (type == "CADENCE")
                        ParseValues(sample, cadence => { exercise.CadenceValues.Add(Convert.ToByte(cadence)); });
                }

                exercises.Add(exercise);
            }

            return exercises;
        }
    }
}
