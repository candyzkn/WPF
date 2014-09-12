// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;
using System.Diagnostics;
using System.Linq;

namespace MyWPF.Utils
{
    /// <summary>
    /// Runs a process that sends output to standard output and to
    /// standard error.
    /// </summary>
    public class ProcessRunner : IDisposable
    {
        private Process _process;
        string _workingDirectory = String.Empty;
        private OutputReader _standardOutputReader;
        private OutputReader _standardErrorReader;
        private bool _redirectStandardOutput = true;
        private bool _redirectStandardError = true;

        /// <summary>
        /// Triggered when the process has exited.
        /// </summary>
        public event EventHandler ProcessExited;
		
        /// <summary>
        /// Triggered when a line of text is read from the standard output.
        /// </summary>
        public event LineReceivedEventHandler OutputLineReceived;
		
        /// <summary>
        /// Triggered when a line of text is read from the standard error.
        /// </summary>
        public event LineReceivedEventHandler ErrorLineReceived;
		
        /// <summary>
        /// Gets or sets the process's working directory.
        /// </summary>
        public string WorkingDirectory {
            get {
                return _workingDirectory;
            }
			
            set {
                _workingDirectory = value;
            }
        }

        /// <summary>
        /// Gets the standard output returned from the process.
        /// </summary>
        public string StandardOutput {
            get {
                string output = String.Empty;
                if (_standardOutputReader != null) {
                    output = _standardOutputReader.Output;
                }
                return output;
            }
        }
		
        /// <summary>
        /// Gets the standard error output returned from the process.
        /// </summary>
        public string StandardError {
            get {
                string output = String.Empty;
                if (_standardErrorReader != null) {
                    output = _standardErrorReader.Output;
                }
                return output;
            }
        }
		
        /// <summary>
        /// Releases resources held by the <see cref="ProcessRunner"/>
        /// </summary>
        public void Dispose()
        {
        }
		
        /// <summary>
        /// Gets the process exit code.
        /// </summary>
        public int ExitCode {
            get {	
                int exitCode = 0;
                if (_process != null) {
                    exitCode = _process.ExitCode;
                }
                return exitCode;
            }
        }
		
        /// <summary>
        /// Waits for the process to exit.
        /// </summary>
        public void WaitForExit()
        {
            WaitForExit(Int32.MaxValue);
        }
		
        /// <summary>
        /// Waits for the process to exit.
        /// </summary>
        /// <param name="timeout">A timeout in milliseconds.</param>
        /// <returns><see langword="true"/> if the associated process has 
        /// exited; otherwise, <see langword="false"/></returns>
        public bool WaitForExit(int timeout)
        {
            if (_process == null) {
                throw new ProcessRunnerException("NoProcessRunningErrorText");
            }
			
            bool exited = _process.WaitForExit(timeout);
			
            if (exited) 
            {
                if (this.RedirectStandardOutput)
                    _standardOutputReader.WaitForFinish();
                if (this.RedirectStandardError)
                    _standardErrorReader.WaitForFinish();
            }
			
            return exited;
        }
		
        public bool IsRunning {
            get {
                bool isRunning = false;
				
                if (_process != null) {
                    isRunning = !_process.HasExited;
                }
				
                return isRunning;
            }
        }
		
        public bool RedirectStandardOutput
        {
            get
            {
                return _redirectStandardOutput;
            }
            set
            {
                _redirectStandardOutput = value;
            }
        }

        public bool RedirectStandardError
        {
            get
            {
                return _redirectStandardError;
            }
            set
            {
                _redirectStandardError = value;
            }
        }

        public bool UseShellExecute { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        public void Start(string command,params string[] arguments)
        {
            _process = new Process
            {
                StartInfo =
                    {
                        CreateNoWindow = true,
                        FileName = command,
                        WorkingDirectory = _workingDirectory,
                        RedirectStandardOutput = RedirectStandardOutput,
                        RedirectStandardError = RedirectStandardError,
                        UseShellExecute = UseShellExecute
                    }
            };
            if (arguments != null)
            {
                var args = arguments.Aggregate("", (current, argument) => current + (" \"" + argument + "\""));
                _process.StartInfo.Arguments = args.Trim();
            }
			
            if (ProcessExited != null) {
                _process.EnableRaisingEvents = true;
                _process.Exited += OnProcessExited;
            }

            bool started = false;
            try {
                _process.Start();
                started = true;
            } finally {
                if (!started) {
                    _process.Exited -= OnProcessExited;			
                    _process = null;
                }
            }

            if (RedirectStandardOutput)
            {
                _standardOutputReader = new OutputReader(_process.StandardOutput);
                if (OutputLineReceived != null)
                {
                    _standardOutputReader.LineReceived += OnOutputLineReceived;
                }
                _standardOutputReader.Start();
            }

            if (RedirectStandardError)
            {
                _standardErrorReader = new OutputReader(_process.StandardError);
                if (ErrorLineReceived != null)
                {
                    _standardErrorReader.LineReceived += OnErrorLineReceived;
                }

                _standardErrorReader.Start();
            }
        }
		
        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <param name="command">The process filename.</param>
        public void Start(string command)
        {
            Start(command, String.Empty);
        }
		
        /// <summary>
        /// Kills the running process.
        /// </summary>
        public void Kill()
        {
            if (_process != null) {
                if (!_process.HasExited) {
                    _process.Kill();
                    _process.Close();
                    _process.Dispose();
                    _process = null;
                    _standardOutputReader.WaitForFinish();
                    _standardErrorReader.WaitForFinish();
                } else {
                    _process = null;
                }
            }
            // Control-C does not seem to work.
            //GenerateConsoleCtrlEvent((int)ConsoleEvent.ControlC, 0);
        }		
		
        /// <summary>
        /// Raises the <see cref="ProcessExited"/> event.
        /// </summary>
        protected void OnProcessExited(object sender, EventArgs e)
        {
            if (ProcessExited != null) {
				
                _standardOutputReader.WaitForFinish();
                _standardErrorReader.WaitForFinish();
				
                ProcessExited(this, e);
            }
        }
		
        /// <summary>
        /// Raises the <see cref="OutputLineReceived"/> event.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The line received event arguments.</param>
        protected void OnOutputLineReceived(object sender, LineReceivedEventArgs e)
        {
            if (OutputLineReceived != null) {
                OutputLineReceived(this, e);
            }
        }
		
        /// <summary>
        /// Raises the <see cref="ErrorLineReceived"/> event.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The line received event arguments.</param>
        protected void OnErrorLineReceived(object sender, LineReceivedEventArgs e)
        {
            if (ErrorLineReceived != null) {
                ErrorLineReceived(this, e);
            }
        }		
    }
}