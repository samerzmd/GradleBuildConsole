using System;
using System.Diagnostics;

namespace GradleBuildConsole
{
    public class GradleProcess : Process, IEquatable<GradleProcess>
    {
        public GradleProcess(Configuration configuration)
        {
            EnableRaisingEvents = true;
            StartInfo = new ProcessStartInfo
            {
                FileName = configuration.Path+@"\gradlew.bat",
                WorkingDirectory = configuration.Path,
                UseShellExecute = false,
                RedirectStandardInput = true,
                Arguments = configuration.BuildCommand
            };

        }

        //for next releases 
        public bool Equals(GradleProcess other)
        {
            return StartInfo.WorkingDirectory.Equals(other.StartInfo.WorkingDirectory);
        }
    }
}
