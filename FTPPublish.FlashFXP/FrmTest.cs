﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace FTPPublish.FlashFXP {
    public partial class FrmTest : Form {
        public FrmTest() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            Utilitys.Shell.ExeShell(textBox1.Text, textBox2.Text);
        }
    }
}
