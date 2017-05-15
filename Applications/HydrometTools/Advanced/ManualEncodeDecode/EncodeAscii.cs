using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HydrometTools.Advanced.ManualEncodeDecode
{
    public partial class EncodeAscii : UserControl
    {
        public EncodeAscii()
        {
            InitializeComponent();
        }

        private void buttonDecimalToBinary_Click(object sender, EventArgs e)
        {
            int d = Convert.ToInt32(textBoxDecimal.Text);
            string b = Convert.ToString(d, 2).PadLeft(18, '0');
            if (b.Length > 18)
            {
                b = b.Substring(b.Length - 18, 18);
                if (b.Length != 18)
                {
                    throw new Exception("bad equation by karl");
                }
            }

            this.textBoxBinary.Text = b;

            this.textBoxGroup1.Text = ConvertBinaryToCharacter(b.Substring(12, 6));
            this.textBoxGroup2.Text = ConvertBinaryToCharacter(b.Substring(6, 6));
            this.textBoxGroup3.Text = ConvertBinaryToCharacter(b.Substring(0, 6));

            this.textBoxResult.Text =
                ConvertBinaryToCharacter(b.Substring(0, 6))
            + ConvertBinaryToCharacter(b.Substring(6, 6))
            + ConvertBinaryToCharacter(b.Substring(12, 6));

        }
        private string ConvertBinaryToCharacter(string inputBinary)
        {
            int i = Convert.ToInt32(inputBinary, 2);
            if (i == 63)
                return "?";

            string s = Char.ConvertFromUtf32(i + 64);
            return s;
        }

    }
}
