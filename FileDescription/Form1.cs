using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using static Iplugin.PetPlug;
using Iplugin;

namespace FileDescription
{
    public partial class Form1 : Form
    {
        private ClosedCallback callback = null;
        private IPetPlug plug = null;
        public Form1(ClosedCallback callback, IPetPlug plug)
        {
            this.callback = callback;
            this.plug = plug;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (callback != null && plug != null)
            {
                ClosedCallback callbackTmp = callback;
                IPetPlug plugTmp = plug;
                plug = null;
                callback = null;
                plug = null;
                callbackTmp(plugTmp);
            }
        }
    }
}
