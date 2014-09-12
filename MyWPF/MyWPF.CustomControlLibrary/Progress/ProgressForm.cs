using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MyWPF.CustomControlLibrary
{
    [Description("Progress Form class.")]
    public class ProgressForm : Form
    {
        [Description("Constructor.")]
        public ProgressForm(Image[] images)
        {
            this.images = new Image[4];
            this.images = images;
            this.InitializeComponent();
            this.pictureBox.Image = this.images[this.imageIndex++];
            this.imageIndex = this.imageIndex % this.images.Length;
            this.TopMost = true;
        }

        [Description("Closes the progress form.")]
        public void DoClose()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new MethodInvoker(this.Close));
            }
            else
            {
                base.Close();
            }
        }

        [Description("Shows the progress form.")]
        public void DoShow()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new MethodInvoker(this.Show));
            }
            else
            {
                base.Show();
            }
        }

        [Description("Updates the title, step and animation image.")]
        public void DoUpdate(string title, string step)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new DoubleStringMethodInvoker(this.DoUpdate));
            }
            else
            {
                this.titleLabel.Text = title;
                this.stepLabel.Text = step;
                this.pictureBox.Image = this.images[this.imageIndex++];
                this.imageIndex = this.imageIndex % this.images.Length;
                base.Update();
            }
        }

        [Description("Required method for Designer support.")]
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.stepLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Controls.Add(this.stepLabel);
            this.panel1.Controls.Add(this.titleLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 70);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(6, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(64, 64);
            this.pictureBox.TabIndex = 11;
            this.pictureBox.TabStop = false;
            // 
            // stepLabel
            // 
            this.stepLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stepLabel.ForeColor = System.Drawing.Color.Black;
            this.stepLabel.Location = new System.Drawing.Point(98, 32);
            this.stepLabel.Name = "stepLabel";
            this.stepLabel.Size = new System.Drawing.Size(248, 32);
            this.stepLabel.TabIndex = 10;
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point(82, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(272, 16);
            this.titleLabel.TabIndex = 9;
            // 
            // ProgressForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(362, 70);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "«Î…‘∫Ú...";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        [Description("The next image index for the animation picture box.")]
        private int imageIndex;
        private Panel panel1;
        private PictureBox pictureBox;
        private Label stepLabel;
        private Label titleLabel;
        [Description("The images for the animation picture box.")]
        private Image[] images;
    }
}