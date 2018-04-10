using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;

namespace ExploScripts.Data
{
    public class ContextMenuHandler
    {
        ObservableCollection<ExploScript> scripts;

        public ContextMenuHandler(ObservableCollection<ExploScript> scripts)
        {
            this.scripts = scripts;
        }

        public void UpdateRegistry()
        {
            Clear();
            CreateBaseEntry();
            CreateScriptEntries();
        }

        public void Clear()
        {
            // 1. Clear Base Entry
            string regPath = @"Software\Classes\Directory\Background\shell\";

            RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            RegistryKey sk = rk.OpenSubKey(regPath, true);

            if (sk != null && sk.GetSubKeyNames().Contains("ExploScripts"))
            {
                sk.DeleteSubKeyTree("ExploScripts");
            }

            // 2. Clear Commands Entries
            regPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\";
            rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            sk = rk.OpenSubKey(regPath, true);
            foreach (var key in sk.GetSubKeyNames()) 
            {
                if (key.Contains("explo"))
                {
                    sk.DeleteSubKeyTree(key);
                }
            }
        }

        private void CreateBaseEntry()
        {
            string regPath = @"Software\Classes\Directory\Background\shell\ExploScripts\";

            RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            RegistryKey sk = rk.OpenSubKey(regPath, true);

            if (sk == null)
            {
                string cmds = "";
                foreach (var script in scripts)
                {
                    if (script.Active)
                        cmds += "explo." + script.Name + ";";
                }
                cmds += "explo";

                sk = rk.CreateSubKey(regPath);
                sk.SetValue("MUIVerb", "ExploScripts");
                sk.SetValue("SeparatorBefore", "");
                sk.SetValue("SubCommands", cmds);
            }
        }

        private void CreateScriptEntries()
        {
            string curDir = Directory.GetCurrentDirectory();
            string regPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\";
            RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

            RegistryKey sk, skCmd;
            foreach (var script in scripts)
            {
                if (script.Active)
                {
                    sk = rk.CreateSubKey(regPath + "explo." + script.Name);
                    sk.SetValue(null, script.Caption);

                    skCmd = sk.CreateSubKey("command");
                    skCmd.SetValue(null, "cmd /c \"python ^\"" + curDir + "\\Scripts\\" + script.Name + "\\" + script.Name + ".py^\" ^\"%w^\"\"");
                }
            }

            sk = rk.CreateSubKey(regPath + "explo");
            sk.SetValue(null, "ExploScripts");
            sk.SetValue("CommandFlags", 48);

            skCmd = sk.CreateSubKey("command");
            skCmd.SetValue(null, "\"" + curDir + "\\ExploScripts.exe\" \"" + curDir + "\"");
        }
    }
}
