using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FirebaseAngular.Models
{
    public class CommandExecuter
    {
        /// <summary>
        /// Returns the high preferenced mx server using command line execution
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        MXServer mxServer = new MXServer();
     

        /// <summary>
        /// Returns list of MXServer
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public IList<MXServer> GetMXServers(string domainName)
        {
            string command = "nslookup -q=mx " + domainName;
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;

            procStartInfo.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();

            if (!string.IsNullOrEmpty(result))
                result = result.Replace("\r\n", Environment.NewLine);

            IList<MXServer> list = new List<MXServer>();
            foreach (string line in result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains("MX preference =") && line.Contains("mail exchanger ="))
                {
                    mxServer.Prefrence = Int(GetStringBetween(line, "MX preference = ", ","));
                    mxServer.Email = GetStringFrom(line, "mail exchanger = ");

                    list.Add(mxServer);
                }
            }

            return list.OrderBy(m => m.Prefrence).ToList();
        }

        private string GetStringFrom(string Data, string StartKey)
        {
            if (String.IsNullOrEmpty(Data) || String.IsNullOrEmpty(StartKey))
                return String.Empty;

            if (!Data.Contains(StartKey))
                return String.Empty;

            int ix_start = Data.IndexOf(StartKey);
            int ix_end = Data.Length;

            int ValueStart = ix_start + StartKey.Length;

            string ret = Data.Substring(ValueStart, ix_end - ix_start - StartKey.Length);

            return ret;
        }

        public int Int(object value)
        {
            if (value == null || value.ToString() == String.Empty)
                return 0;

            try
            {
                return Int32.Parse(value.ToString());
            }
            catch
            {
                return 0;
            }
        }


        private string GetStringBetween(string Data, string StartKey, string EndKey)
        {
            if (String.IsNullOrEmpty(Data) || String.IsNullOrEmpty(StartKey) || String.IsNullOrEmpty(EndKey))
                return String.Empty;

            if (!Data.Contains(StartKey) || !Data.Contains(EndKey))
                return String.Empty;

            int ix_start = Data.IndexOf(StartKey);
            int ix_end = Data.IndexOf(EndKey, ix_start + StartKey.Length);

            int ValueStart = ix_start + StartKey.Length;

            string ret = Data.Substring(ValueStart, ix_end - ix_start - StartKey.Length);

            return ret;
        }

    }
}