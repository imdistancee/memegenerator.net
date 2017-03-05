using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meme_Generator.Forms
{
    public partial class CreatorForm : Form
    {
        private Image image;
        private Tuple<string, string, string> item;
        private Helpers.Create create;
        private Image currentMeme;

        public CreatorForm(Image image, Tuple<string, string, string> item)
        {
            InitializeComponent();

            this.image = image;
            this.item = item;
            create = new Helpers.Create();
        }

        private void CreatorForm_Load(object sender, EventArgs e)
        {
            pictureBox.Image = image;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            currentMeme = create.createMeme(item, txtTop.Text, txtBottom.Text);
            pictureBox.Image = currentMeme;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == currentMeme)
            {
                pictureBox.Image = image;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == currentMeme)
            {
                var sfd = new SaveFileDialog
                {
                    FileName = "Meme",
                    Filter = "JPEG Files (*.jpg) | *.jpg"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap imageCopy = new Bitmap(currentMeme))
                    {
                        //this method fixes a gdi+ error by creating a copy of the image instead of saving the image directly
                        imageCopy.Save(sfd.FileName, ImageFormat.Jpeg);
                        MessageBox.Show("Meme has been exported successfully", "memegenerator.net", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
