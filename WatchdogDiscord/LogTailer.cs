using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogDiscord
{
    class LogTailer
    {
        private const string logPath = @"/var/log/";

        private DiscordSocketClient client;
        private FileSystemWatcher watcher;

        public LogTailer(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task Run()
        {
            watcher = new FileSystemWatcher(logPath);
            watcher.Filter = "ufw.log";

            watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Size |
                NotifyFilters.Security;

            watcher.EnableRaisingEvents = true;
            watcher.Changed += Watcher_Changed;

            Log.Info("LOGFILE", "OnChanged event registered");
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Log.Warn("LOGFILE", "OnChanged");

            FileStream fs = new FileStream("/var/log/ufw.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                var s = "";
                while (true)
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        Log.Warn("UFW", s);
                        //client.Guilds.FirstOrDefault().DefaultChannel.SendMessageAsync(s);
                    }
                }
            }
        }
    }
}
