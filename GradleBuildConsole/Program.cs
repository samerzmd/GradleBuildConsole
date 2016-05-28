using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace GradleBuildConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            StartWaitingForTask();
        }
        private static void StartWaitingForTask()
        {
            using (NamedPipeClientStream namedPipeClient = new NamedPipeClientStream("test-pipe"))
            {
                namedPipeClient.Connect();
                Console.WriteLine(namedPipeClient.ReadByte());

                string jsonObjectText;
                var fileStream = new FileStream(@"c:\GradleServerConfig.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    jsonObjectText = streamReader.ReadToEnd();
                }

                var configuration=JsonConvert.DeserializeObject<Configuration>(jsonObjectText);

                switch (configuration.MainOprationName)
                {
                    case "Gradle":
                        var gradleProcess = new GradleProcess(configuration);

                        gradleProcess.Start();

                        gradleProcess.Exited += (sender, e) =>
                        {
                            namedPipeClient?.WriteByte(2);
                        };
                        break ;
                    case "Git":
                        var gitProcess = new GitProcess(configuration);

                        gitProcess.Start();

                        gitProcess.Exited += (sender, e) =>
                        {
                            namedPipeClient?.WriteByte(2);
                        };
                        break;
                    case "TFS":
                        break;
                    default:
                        break;
                }

                

                StartWaitingForTask();
            }
        }
    }
}