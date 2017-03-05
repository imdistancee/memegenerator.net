using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meme_Generator.Forms
{
    public partial class MainForm : Form
    {
        private List<Tuple<Image, string, string, string>> items;
        private Helpers.Search search;
        private int currentIndex = 0;

        public MainForm()
        {
            InitializeComponent();

            items = new List<Tuple<Image, string, string, string>>();
            search = new Helpers.Search();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            items = search.getImagesFromSearch();
            currentImage.Image = items[currentIndex].Item1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex != items.Count - 1)
            {
                currentIndex = currentIndex + 1;
                currentImage.Image = items[currentIndex].Item1;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentIndex != 0)
            {
                currentIndex = currentIndex - 1;
                currentImage.Image = items[currentIndex].Item1;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                try
                {
                    currentIndex = 0;
                    items = search.getImagesFromSearch(txtSearch.Text);
                    currentImage.Image = items[currentIndex].Item1;
                }
                catch
                {
                    MessageBox.Show("No results found", "memegenerator.net", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    currentIndex = 0;
                    items = search.getImagesFromSearch();
                    currentImage.Image = items[currentIndex].Item1;
                }
            }
        }

        private void currentImage_Click(object sender, EventArgs e)
        {
            Tuple<string, string, string> item = new Tuple<string, string, string>
                (items[currentIndex].Item2, items[currentIndex].Item3, items[currentIndex].Item4);

            CreatorForm form = new CreatorForm(items[currentIndex].Item1, item);
            form.Show();
        }
    }
}
