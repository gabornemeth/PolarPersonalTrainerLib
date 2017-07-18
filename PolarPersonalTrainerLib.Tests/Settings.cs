using System;

namespace PolarPersonalTrainerLib.Tests
{
    public static class Settings
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        public static string UserName { get; } = Environment.GetEnvironmentVariable("PPTLIB_USER");
        /// <summary>
        /// Password
        /// </summary>
        public static string Password { get; } = Environment.GetEnvironmentVariable("PPTLIB_PASSWORD");
    }
}
