using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FakedGameClient.ServerController
{
    public partial class FormWaiting : Form
    {
        bool currentlyAnimating = false;
        Bitmap animatedImage = Properties.Resources.connecting;

        public string Label
        {
            get { return lblNotice.Text; }
            set { lblNotice.Text = value; }
        }

        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public void AnimateImage()
        {
            if (!currentlyAnimating)
            {

                //Begin the animation only once.
                ImageAnimator.Animate(animatedImage, new EventHandler(this.OnFrameChanged));
                currentlyAnimating = true;
            }
        }

        private void OnFrameChanged(object o, EventArgs e)
        {

            //Force a call to the Paint event handler.
            this.Invalidate();
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{

        //    //Begin the animation.
        //    AnimateImage();

        //    //Get the next frame ready for rendering.
        //    ImageAnimator.UpdateFrames();

        //    //Draw the next frame in the animation.
        //    e.Graphics.DrawImage(this.animatedImage, new Point(0, 0));
        //}

        public FormWaiting()
        {
            InitializeComponent();
        }

        private void FormWaiting_Load(object sender, EventArgs e)
        {

        }

        private void picLoading_Paint(object sender, PaintEventArgs e)
        {
            //Begin the animation.
            AnimateImage();

            //Get the next frame ready for rendering.
            ImageAnimator.UpdateFrames();

            //Draw the next frame in the animation.
            //e.Graphics.DrawImage(this.animatedImage, new Point(0, 0));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
