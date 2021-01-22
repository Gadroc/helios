//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace GadrocsWorkshop.Helios.Interfaces.Profile
{
    using System;
    using System.IO;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using GadrocsWorkshop.Helios.ComponentModel;

    [HeliosInterface("Helios.Base.ProfileInterface", "Profile", null, typeof(UniqueHeliosInterfaceFactory), AutoAdd = true)]
    public class ProfileInterface : HeliosInterface
    {
        private HeliosTrigger _profileStartedTrigger;
        private HeliosTrigger _profileResetTrigger;
        private HeliosTrigger _profileStoppedTrigger;

        public ProfileInterface()
            : base("Profile")
        {
            HeliosAction resetAction = new HeliosAction(this, "", "", "reset", "Resets the profile to default state.");
            resetAction.Execute += new HeliosActionHandler(ResetAction_Execute);
            Actions.Add(resetAction);

            HeliosAction stopAction = new HeliosAction(this, "", "", "stop", "Stops the profile from running.");
            stopAction.Execute += new HeliosActionHandler(StopAction_Execute);
            Actions.Add(stopAction);

            HeliosAction showControlCenter = new HeliosAction(this, "", "", "show control center", "Shows the control center.");
            showControlCenter.Execute += new HeliosActionHandler(ShowAction_Execute);
            Actions.Add(showControlCenter);

            HeliosAction hideControlCenter = new HeliosAction(this, "", "", "hide control center", "Shows the control center.");
            hideControlCenter.Execute += new HeliosActionHandler(HideAction_Execute);
            Actions.Add(hideControlCenter);

            HeliosAction launchApplication = new HeliosAction(this, "", "", "launch application", "Launches an external application", "Full path to application or document you want to launch or URL to a web page.  If the path contains space characters, then these must be enclosed in double quote characters \".", BindingValueUnits.Text);
            launchApplication.Execute += LaunchApplication_Execute;
            Actions.Add(launchApplication);

            HeliosAction killApplication = new HeliosAction(this, "", "", "kill application", "Kills an external process", "Process Image name of the process to be killed.", BindingValueUnits.Text);
            killApplication.Execute += KillApplication_Execute;
            Actions.Add(killApplication);

            _profileStartedTrigger = new HeliosTrigger(this, "", "", "Started", "Fired when a profile is started.");
            Triggers.Add(_profileStartedTrigger);

            _profileResetTrigger = new HeliosTrigger(this, "", "", "Reset", "Fired when a profile has been reset.");
            Triggers.Add(_profileResetTrigger);

            _profileStoppedTrigger = new HeliosTrigger(this, "", "", "Stopped", "Fired when a profile is stopped.");
            Triggers.Add(_profileStoppedTrigger);
        }

        void LaunchApplication_Execute(object action, HeliosActionEventArgs e)
        {
            // Arguments need to be split off from the incoming string, and both the
            // application full path and the arguments could contain spaces.  
            // The first un-double-quoted space is assumed to be the delimiter between 
            // executable and argument.  
            // 
            // Once arguments have been split off, both the executable and arguments are 
            // processed to expand environment variables which might introduce spaces.
            //
            // This process might break backward compatibility because existing launch actions
            // could contain blanks and not be enclosed in double quotes
            //
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
            MatchCollection _matches;
            // Find out if we're launching a common executable for this machine
            _matches = Regex.Matches(e.Value.StringValue, "(" + Environment.GetEnvironmentVariable("PATHEXT").Replace(";", "|").Replace(".", "\\.") + ")",options);
            bool _exe = _matches.Count > 0 ? true : false;

            string _expandedPath = e.Value.StringValue;
            string _expandedArgs = "";
            if (_exe)
            {
                // For executables launches, we resolve environment variables
                ConfigManager.LogManager.LogDebug("Launch type determined to be an executable based on %PATHEXT% \"" + e.Value.StringValue + "\")");

                _matches = Regex.Matches(e.Value.StringValue, "(?:\\\")([^\"]*)(?:\\\")");  // Extract anything which is enclosed in escaped double quotes
                int _blank = e.Value.StringValue.IndexOf(" ");
                if (_matches.Count == 0)
                {
                    //  There is nothing enclosed in double-quotes, so we assume the executable is before the first space and any arguments follow the first space

                    if (_blank > 0)
                    {
                        _expandedPath = Environment.ExpandEnvironmentVariables(e.Value.StringValue.Substring(0, _blank));
                        _expandedArgs = Environment.ExpandEnvironmentVariables(e.Value.StringValue.Substring(_blank + 1));
                    }
                    else
                    {
                        _expandedPath = Environment.ExpandEnvironmentVariables(e.Value.StringValue);
                        _expandedArgs = "";
                    }
                }
                else
                {
                    int _matchCursor = 0;
                    foreach (Match _matchItem in _matches)
                    {
                        if (_matchItem.Equals(_matches[0]))
                        {
                            if (_matchItem.Index == 0)
                            {
                                _expandedPath = Environment.ExpandEnvironmentVariables(_matchItem.Groups[1].ToString());
                                _matchCursor = _matchItem.Length;
                            }
                            else
                            {
                                _expandedPath = Environment.ExpandEnvironmentVariables(e.Value.StringValue.Substring(0, _blank));
                                _expandedArgs = Environment.ExpandEnvironmentVariables(e.Value.StringValue.Substring(_blank, _matchItem.Index - _blank));
                                _expandedArgs += " " + Environment.ExpandEnvironmentVariables(_matchItem.ToString());
                                _matchCursor = _matchItem.Index + _matchItem.Length;
                            }
                        }
                        else
                        {
                            if (_matchItem.Index == _matchCursor)
                            {
                                _expandedArgs += " " + Environment.ExpandEnvironmentVariables(_matchItem.ToString());
                            }
                            else
                            {
                                _expandedArgs += Environment.ExpandEnvironmentVariables(e.Value.StringValue.Substring(_matchCursor, _matchItem.Index - _matchCursor));
                                _matchCursor = _matchItem.Index + _matchItem.Length;
                                _expandedArgs += " " + Environment.ExpandEnvironmentVariables(_matchItem.ToString());
                            }
                        }
                        ConfigManager.LogManager.LogDebug("Double Quoted item found \"" + _matchItem.ToString() + "\"");
                    }
                }
            }
            try
            {
                if (_exe)
                {
                    ProcessStartInfo _psi = new ProcessStartInfo();
                    _psi.FileName = Path.GetFileName(_expandedPath);
                    _psi.WorkingDirectory = Path.GetDirectoryName(_expandedPath);
                    _psi.Arguments = _expandedArgs.Trim();
                    _psi.UseShellExecute = true;
                    _psi.RedirectStandardOutput = false;
                    Process.Start(_psi);
                }
                else
                {
                    ConfigManager.LogManager.LogDebug("Launch type determined to be non-executable based on %PATHEXT% \"" + e.Value.StringValue + "\")");
                    Process.Start(e.Value.StringValue);
                }
             }
            catch (Exception ex)
            {
                ConfigManager.LogManager.LogError("Error caught launching external application (path=\"" + _expandedPath + "\") Exception " + ex.Message);
            }
        }

        void KillApplication_Execute(object action, HeliosActionEventArgs e)
        {
            try
            {
                Process[] _localProcessesByName = Process.GetProcessesByName(e.Value.StringValue);
                foreach (Process _proc in _localProcessesByName) {
                    ConfigManager.LogManager.LogInfo("Killing process image name \"" + e.Value.StringValue + "\"");
                    _proc.Kill();
                }
            }
            catch (Exception ex)
            {
                ConfigManager.LogManager.LogError("Error caught killing process image name \"" + e.Value.StringValue + "\")", ex);
            }
        }

        void HideAction_Execute(object action, HeliosActionEventArgs e)
        {
            if (Profile != null)
            {
                Profile.HideControlCenter();
            }
        }

        void ShowAction_Execute(object action, HeliosActionEventArgs e)
        {
            if (Profile != null)
            {
                Profile.ShowControlCenter();
            }
        }

        void StopAction_Execute(object action, HeliosActionEventArgs e)
        {
            if (Profile != null)
            {
                Profile.Stop();
            }
        }

        void ResetAction_Execute(object action, HeliosActionEventArgs e)
        {
            if (Profile != null)
            {
                Profile.Reset();
            }
        }
        public void Start()
        {
            _profileStartedTrigger.FireTrigger(BindingValue.Empty);
        }

        public override void Reset()
        {
            _profileResetTrigger.FireTrigger(BindingValue.Empty);
            base.Reset();
        }
        public void Stop()
        {
            _profileStoppedTrigger.FireTrigger(BindingValue.Empty);
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            // No-Op
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            // No-Op
        }
    }
}
