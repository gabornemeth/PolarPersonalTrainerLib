using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolarPersonalTrainerLib
{
    public struct PPTColumns
    {
        public const string Time = "Time";
        public const string Duration = "Duration";
        public const string Calories = "Calories";
        public const string Sport = "Sport";
        public const string AverageHR = "Average HR";
        public const string MaximumHR = "Maximum HR";
        public const string RestingHR = "Resting HR";
        public const string VO2Max = "VO2 Max";
    }

    public class PPTConvert
    {
        static String[] columnNames =
                { PPTColumns.Time,
                  PPTColumns.Duration,
                  PPTColumns.Calories,
                  PPTColumns.Sport,
                  PPTColumns.AverageHR,
                  PPTColumns.MaximumHR,
                  PPTColumns.RestingHR,
                  PPTColumns.VO2Max };

        public static DataRow convertExerciseToDataRow(PPTExercise exercise, DataTable dt)
        {
            addMissingColumns(ref dt);

            DataRow dr = dt.NewRow();

            dr[PPTColumns.Time] = exercise.StartTime;
            dr[PPTColumns.Sport] = exercise.Sport;
            dr[PPTColumns.Calories] = exercise.Calories;
            dr[PPTColumns.Duration] = exercise.Duration;

            HeartRate hr = exercise.HeartRate;

            if (hr != null && hr.HasData)
            {
                dr[PPTColumns.RestingHR] = exercise.HeartRate.Resting;
                dr[PPTColumns.AverageHR] = exercise.HeartRate.Average;
                dr[PPTColumns.MaximumHR] = exercise.HeartRate.Maximum;
                dr[PPTColumns.VO2Max] = exercise.HeartRate.Vo2Max;
            }

            return dr;
        }

        public static PPTExercise convertDataRowToExercise(DataRow dr)
        {
            PPTExercise exercise = new PPTExercise();

            exercise.StartTime = Convert.ToDateTime(dr[PPTColumns.Time]);
            exercise.Sport = dr[PPTColumns.Sport].ToString();
            exercise.Calories = Convert.ToInt32(dr[PPTColumns.Calories]);
            exercise.Duration = TimeSpan.Parse(dr[PPTColumns.Duration].ToString());

            exercise.HeartRate = new HeartRate();

            exercise.HeartRate.Resting = Convert.ToInt32(dr[PPTColumns.RestingHR]);
            exercise.HeartRate.Average = Convert.ToInt32(dr[PPTColumns.AverageHR]);
            exercise.HeartRate.Maximum = Convert.ToInt32(dr[PPTColumns.MaximumHR]);
            exercise.HeartRate.Vo2Max = Convert.ToInt32(dr[PPTColumns.VO2Max]);

            return exercise;
        }

        private static void addTypedColumn(ref DataTable dt, String columnName, Type columnType)
        {
            if (!dt.Columns.Contains(columnName))
                dt.Columns.Add(columnName, columnType);
        }

        private static void addMissingColumns(ref DataTable dt)
        {
            addTypedColumn(ref dt, PPTColumns.Time, typeof(DateTime));
            addTypedColumn(ref dt, PPTColumns.Duration, typeof(TimeSpan));
            addTypedColumn(ref dt, PPTColumns.Calories, typeof(int));
            addTypedColumn(ref dt, PPTColumns.Sport, typeof(string));
            addTypedColumn(ref dt, PPTColumns.AverageHR, typeof(int));
            addTypedColumn(ref dt, PPTColumns.MaximumHR, typeof(int));
            addTypedColumn(ref dt, PPTColumns.RestingHR, typeof(int));
            addTypedColumn(ref dt, PPTColumns.VO2Max, typeof(int));
        }
    }
}
