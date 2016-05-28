using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradleBuildConsole
{
    public class GitProcess:Process, IEquatable<GitProcess>
    {
        public GitProcess(Configuration configuration)
        {
            EnableRaisingEvents = true;
            StartInfo = new ProcessStartInfo
            {
                FileName = @"cmd.exe",
                WorkingDirectory = configuration.Path,
                UseShellExecute = false,
                RedirectStandardInput = true,
            };

        }

        public new bool Start()
        {
            var result = base.Start();
            StandardInput.WriteLine(@"git pull origin master");
            StandardInput.WriteLine(@"exit");
            return result;
        }

        //for next releases 
        public bool Equals(GitProcess other)
        {
            return StartInfo.WorkingDirectory.Equals(other.StartInfo.WorkingDirectory);
        }
    }
}
