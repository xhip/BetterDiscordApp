﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using BetterDiscordWI.panels;

namespace BetterDiscordWI
{
    public partial class FormMain : Form
    {

        private readonly IPanel[] _panels = { new Panel0(), new Panel1(), new Panel2() };
        private int _index;

        public String DiscordPath;
        public String Sha;

        public XmlNodeList ResourceList;

        public FormMain()
        {
            InitializeComponent();

            Sha = Utils.GetHash();

            if (Sha.Length < 1)
            {
                MessageBox.Show("Failed to get sha", "Error", MessageBoxButtons.OK);
                Environment.Exit(0);
            }
            

          /*  ZipArchive archive = ZipFile.OpenRead(@"C:\Users\Jiiks\AppData\Roaming\BetterDiscord\temp\asar.zip");

            archive.ExtractToDirectory("C:/Users/Jiiks/AppData/Roaming/BetterDiscord/temp/");*/

          

            //Load installer config
            XmlDocument doc = new XmlDocument();

            doc.Load(@"G:\Git\BetterDiscordApp\BetterDiscordApp\WindowsInstaller\config.xml");

            String latestVersion = doc.GetElementsByTagName("latestversion")[0].InnerText;
            ResourceList = doc.GetElementsByTagName("resource");

            foreach (IPanel ipanel in _panels)
            {
                panelContainer.Controls.Add((UserControl)ipanel);
                ((UserControl)ipanel).Dock = DockStyle.Fill;
                ((UserControl)ipanel).Hide();
            }
            ((UserControl)_panels[_index]).Show();
            _panels[_index].SetVisible();



            btnCancel.Click += (sender, args) => Close();
            btnNext.Click += (sender, args) => _panels[_index].BtnNext();
            btnBack.Click += (sender, args) => _panels[_index].BtnPrev();
        }

        public void SwitchPanel(int index)
        {
            ((UserControl)_panels[_index]).Hide();
            _index = index;
            ((UserControl)_panels[_index]).Show();
            _panels[_index].SetVisible();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Setup is not complete. If you exit now, BetterDiscord will not be installed.\n\nExit Setup?", "Exit Setup?", MessageBoxButtons.YesNo);

            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        readonly Pen borderPen = new Pen(Color.FromArgb(160,160,160));
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(SystemBrushes.Window, new Rectangle(0,0, Width, 50) );
            g.DrawLine(borderPen, 0, 50, Width, 50);
            g.DrawLine(SystemPens.Window, 0, 51, Width, 51);

            g.DrawLine(borderPen, 0, 310, Width, 310);
            g.DrawLine(SystemPens.Window, 0, 311, Width, 311);
            

            base.OnPaint(e);
        }
    }
}
