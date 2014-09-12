using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MyWPF.CustomControlLibrary
{
    [Description("Progress Thread class.")]
    public class ProgressThread
    {
        [Description("Constructor.")]
        public ProgressThread(): this(null, 200)
        {

        }

        [Description("Constructor.")]
        public ProgressThread(int progressWidth) : this(null,progressWidth)
        {
            
        }

        [Description("Constructor.")]
        public ProgressThread(Form parentForm, int progressWidth): this(parentForm,progressWidth,DefaultAnimationImages.SimpleAnimation)
        {
        }

        [Description("Constructor.")]
        public ProgressThread(Form parentForm, int progressWidth, Image[] images)
            : this(parentForm, progressWidth, images, 0xbb8)
        {
        }

        [Description("Constructor.")]
        public ProgressThread(Form parentForm, int progressWidth, Image[] images, int timeoutInMilliseconds)
        {
            this.timeoutInMilliseconds = 0xbb8;
            this.endEvent = new ManualResetEvent(false);
            this.title = "";
            this.step = "";
            this.parentForm = parentForm;
            this.progressForm = new ProgressForm(images);
            this.progressForm.Enabled = false;
            this.progressForm.Size = new Size(progressWidth + 80, 70);
            if (parentForm != null)
                this.progressForm.Location = new Point(parentForm.Left + ((parentForm.Width - this.progressForm.Width) / 2), parentForm.Top + ((parentForm.Height - this.progressForm.Height) / 2));
            else
                this.progressForm.StartPosition = FormStartPosition.CenterScreen;
            this.timeoutInMilliseconds = timeoutInMilliseconds;
        }

        [Description("Ends the progress thread and waits for the thread to join.")]
        public void End()
        {
            this.endEvent.Set();
            this.progressThread.Join();
        }

        [Description("Starts the progress thread running.")]
        public void Start()
        {
            this.progressThread = new Thread(new ThreadStart(this.ThreadProc));
            this.progressThread.Start();
        }

        [Description("Starts the progress thread running.")]
        public void Start(string atitle, string astep)
        {
            this.progressThread = new Thread(new ThreadStart(this.ThreadProc));
            this.Title = atitle;
            this.Step = astep;
            this.progressThread.Start();
        }

        [Description("The thread procedure for the thread.")]
        private void ThreadProc()
        {
            {
                do
                {
                    {
                        progressFormVisible = true;
                        this.progressForm.DoShow();
                        bool flag1 = false;
                        while (true)
                        {
                            flag1 = this.endEvent.WaitOne(80, true);
                            if (flag1)
                            {
                                break;
                            }
                            //if (this.progressForm != Form.ActiveForm)
                            //{
                            //    this.progressForm.Visible = false;
                            //    progressFormVisible = false;
                            //    break;
                            //}
                            this.progressForm.DoUpdate(this.title, this.step);
                        }
                        if (flag1)
                        {
                            this.progressForm.DoClose();
                            progressFormVisible = false;
                        }
                    }
                }
                while (!this.endEvent.WaitOne(this.timeoutInMilliseconds, false));
            }
        }

        [Description("The global visibility flag for the progress form.")]
        public static bool ProgressFormVisible
        {
            get
            {
                return progressFormVisible;
            }
        }

        [Description("The step for the progress.")]
        public string Step
        {
            get
            {
                return this.step;
            }
            set
            {
                this.step = value;
            }
        }

        [Description("The title for the progress.")]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        [Description("The end event to stop this thread.")]
        private ManualResetEvent endEvent;
        [Description("The parent form for this thread.")]
        private Form parentForm;
        [Description("The progress form for this thread.")]
        private ProgressForm progressForm;
        [Description("The global visibility flag for the progress form.")]
        private static bool progressFormVisible;
        [Description("The progress thread itself.")]
        private Thread progressThread;
        [Description("The current progress step.")]
        private string step;
        [Description("The timeout before the progress UI is shown.")]
        private int timeoutInMilliseconds;
        [Description("The current progress title.")]
        private string title;
    }
}