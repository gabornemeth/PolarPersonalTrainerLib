using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolarPersonalTrainerLib
{
    /// <summary>
    /// Heart rate data
    /// </summary>
    public class HeartRate
    {
        /// <summary>
        /// Resting heart rate [bpm]
        /// </summary>
        public int Resting { get; set; }
        /// <summary>
        /// Average heart rate [bpm]
        /// </summary>
        public int Average { get; set; }
        /// <summary>
        /// Maximum heart rate [bpm]
        /// </summary>
        public int Maximum { get; set; }
        /// <summary>
        /// VO2 max
        /// </summary>
        public int Vo2Max { get; set; }
        /// <summary>
        /// Heart rate values [bpm]
        /// </summary>
        public List<byte> Values { get; set; }

        public Boolean HasData
        {
            get
            {
                if (Resting <= 0 && Average <= 0 && Maximum <= 0 && Vo2Max <= 0)
                    return false;

                return true;
            }
        }

        public HeartRate()
        {
            Values = new List<byte>();
        }
    }

    /// <summary>
    /// Exercise available through PolarPersonalTrainer.com
    /// </summary>
    public class PPTExercise
    {
        /// <summary>
        /// Start time
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Duration of exercise
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Sport
        /// </summary>
        public string Sport { get; set; }
        /// <summary>
        /// Burned calories (kcal)
        /// </summary>
        public int Calories { get; set; }
        /// <summary>
        /// Distance in meters
        /// </summary>
        public float Distance { get; set; }
        /// <summary>
        /// Heart rate info
        /// </summary>
        public HeartRate HeartRate { get; set; }
        /// <summary>
        /// Cadence values [rpm]
        /// </summary>
        public List<byte> CadenceValues { get; set; }
        /// <summary>
        /// Speed values [kph] ???? or mph
        /// </summary>
        public List<float> SpeedValues { get; set; }
        /// <summary>
        /// Sampling interval [seconds]
        /// </summary>
        public int RecordingRate { get; set; }
        /// <summary>
        /// Total ascent [meters]
        /// </summary>
        public float Ascent { get; set; }
        /// <summary>
        /// Total descent [meters]
        /// </summary>
        public float Descent { get; set; }
    }
}
