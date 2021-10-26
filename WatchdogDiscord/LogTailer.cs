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

        public LogTailer(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task Run()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(logPath);
            watcher.Filter = "auth.log";
            watcher.EnableRaisingEvents = true;
            watcher.Changed += Watcher_Changed;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
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
                        client.GroupChannels.FirstOrDefault().SendMessageAsync(s);
                    }
                }
            }
        }
    }
}
