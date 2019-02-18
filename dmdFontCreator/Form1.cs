using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;

namespace dmdFontCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        int column = 16;
        int row = 16;
        int isLeft = 2;
        double temp = 0;
        int picNumber = 0;
        PictureBox[,] imgArray;
        int[,] matrix = new int[16,16];
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "16";
            textBox2.Text = "16";
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
            textBox4.Enabled = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            //pictureBox1.Controls.SetChildIndex(pictureBox1, 1);
            //panel1.Controls.SetChildIndex(panel1, 2);
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void RefreshFontSize(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            if (textBox1.Text != "")
            {
                int.TryParse(textBox2.Text, out row);
                int.TryParse(textBox1.Text, out column);
                if (column > 32)
                    column = 32;
                if (row > 32)
                    row = 32;
                if (row * 20 + 120 > 430)
                {
                    this.Height = row * 20 + 120;
                    panel1.Height = row * 20;
                }
                else
                {
                    this.Height = 430;
                    panel1.Height = row * 20;
                }
                if (column * 20 + 240 > 580)
                {
                    this.Width = column * 20 + 240;
                    panel1.Width = column * 20;
                }
                else
                {
                    this.Width = 584;
                    panel1.Width = column * 20;
                }
                AddPictureBox(row, column);
            }
            else
                AddPictureBox(0, 0);


        }

        int id = 0;
        private void AddPictureBox(int row, int column)
        {
            imgArray = new System.Windows.Forms.PictureBox[row, column];
            matrix = new int[row, column];
            for (int i = 0; i < row; i++)
            {

                for (int j = 0; j < column; j++)
                {
                    imgArray[i, j] = new System.Windows.Forms.PictureBox();
                    imgArray[i, j].BorderStyle = BorderStyle.FixedSingle;
                    imgArray[i, j].Click += new System.EventHandler(ClickImage);
                    imgArray[i, j].MouseDown += new MouseEventHandler(MouseDwn);
                    imgArray[i, j].MouseUp += new MouseEventHandler(MouseUP);
                    imgArray[i, j].MouseMove += new MouseEventHandler(MouseMve);
                    imgArray[i, j].BackColor = Color.Gray;
                    imgArray[i, j].Name = (id).ToString();
                    id++;
                    imgArray[i, j].Height = 20;
                    imgArray[i, j].Width = 20;
                    imgArray[i, j].Location = new Point(j*20, i*20);
                    imgArray[i, j].Visible = true;
                    panel1.Controls.Add(imgArray[i,j]);
                    
                }
            }
            id = 0;
        }
        


        private void MouseDwn(object sender, MouseEventArgs e)
        {
            
            switch(MouseButtons)
            {
                case MouseButtons.Left:
                    isLeft = 0;
                    break;
                case MouseButtons.Right:
                    isLeft = 1;
                    break;
                default:
                    isLeft = 2;
                    break;

            }
            //label5.Text = isLeft.ToString();
        }

        private void MouseUP(object sender, MouseEventArgs e)
        {
            isLeft = 2;
            //label5.Text = isLeft.ToString();
        }
        int mouseX=-1, mouseY=-1;
        private void MouseMve(object sender, MouseEventArgs e)
        {
            mouseX = panel1.Location.X;
            mouseY = panel1.Location.Y;
            Point cursorPoint = new Point(mouseX, mouseY);
            Point point = panel1.PointToClient(Cursor.Position);
            //label5.Text = point.ToString();
            Control crp = panel1.GetChildAtPoint(point);

            try
            {
                if (crp != null)
                {
                    int.TryParse(crp.Name, out picNumber);
                    int sutun = (int)(double)picNumber % column;
                    int satır = (int)(double)picNumber / column;
                    //label8.Text = crp.Name;
                    if (isLeft == 1)
                    {
                        crp.BackColor = Color.Red;
                        matrix[satır, sutun] = 1;

                    }
                    if (isLeft == 0)
                    {
                        crp.BackColor = Color.Gray;
                        int.TryParse(crp.Name, out picNumber);
                        matrix[satır, sutun] = 0;
                    }
                    //MessageBox.Show(crp.Name);
                }
            }
            catch (Exception)
            {
                moveError++;
                //label7.Text = moveError.ToString();
            }
            
        }
        int clickError = 0, moveError = 0;
        private void ClickImage(Object sender, System.EventArgs e)
        {
            Point cursorPoint = new Point(mouseX, mouseY);
            Point point = panel1.PointToClient(Cursor.Position);
            Control crp = panel1.GetChildAtPoint(point);
            int sutun = -1, satır=-1;
            if (crp != null)
            {
                int.TryParse(crp.Name, out picNumber);
                sutun = (int)(double)picNumber % column;
                satır = (int)(double)picNumber / column;
            }
                try
            {
                if (isLeft == 1)
                {
                    ((System.Windows.Forms.PictureBox)sender).BackColor = Color.Red;
                    matrix[satır, sutun] = 1;
                }
                if (isLeft == 0)
                {
                   
                    ((System.Windows.Forms.PictureBox)sender).BackColor = Color.Gray;
                    matrix[satır, sutun] = 0;
                }
            }
            catch (Exception)
            {
                clickError++;
                //label6.Text = clickError.ToString();
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        int type = 0,typeRef;
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "")
            {
                int.TryParse(textBox2.Text, out row);
                int.TryParse(textBox1.Text, out column);
                if (row > column)
                    typeRef = row;
                else if (column > row)
                    typeRef = column;
                else if (row == column)
                    typeRef = row;

                if (typeRef <= 32)
                    type = 32;
                if (typeRef <= 16)
                    type = 16;
                if (typeRef <= 8)
                    type = 8;


                richTextBox1.Text = "const uint" + type + "_t" + " " + textBox3.Text + "[] =" + Environment.NewLine + "{" + Environment.NewLine;

                if (comboBox1.SelectedIndex == 0)
                {
                    BitArray BitArray = new BitArray(column);
                    int Number = 0;
                    BitArray bit = new BitArray(1);
                    string comment = "";
                    for (int i = 0; i < row; i++)
                    {
                        
                        for (int j = 0; j < column; j++)
                        {
                            BitArray[j] = GetBit(matrix[i, j], 0);
                            Number = getIntFromBitArray(BitArray);
                        }
                        string hexValue;
                        if (comboBox2.SelectedIndex == 1)
                            hexValue = LittleEndian(Number);
                        else
                            hexValue = BigEndian(Number);

                        if (Comment.Checked)
                            comment = " //" + textBox4.Text;
                        else
                            comment = "";
                        richTextBox1.AppendText("0x" + hexValue);

                        if (i < row - 1)
                            richTextBox1.AppendText(", ");

                        Number = 0;

                    }
                    richTextBox1.AppendText(comment + Environment.NewLine + "};");
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    BitArray BitArray = new BitArray(row);
                    int Number = 0;
                    BitArray bit = new BitArray(1);
                    string comment = "";
                    for (int i = 0; i < column; i++)
                    {

                        for (int j = 0; j < row; j++)
                        {
                            BitArray[j] = GetBit(matrix[j, i], 0);
                            Number = getIntFromBitArray(BitArray);
                        }
                        string hexValue;
                        if (comboBox2.SelectedIndex == 1)
                            hexValue = LittleEndian(Number);
                        else
                            hexValue = BigEndian(Number);

                        if (Comment.Checked)
                            comment = " //" + textBox4.Text;
                        else
                            comment = "";
                        richTextBox1.AppendText("0x" + hexValue);
                        if(i<column-1)
                            richTextBox1.AppendText(", ");

                        Number = 0;

                    }
                    richTextBox1.AppendText(comment + Environment.NewLine + "};");
                }
                
            }
            else
                MessageBox.Show("Check the rules!", "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


        }
        public static int ReverseBytes(int value)
        {

            return (int)((value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24);
        }
        private string BigEndian(int num)
        {
            int revNum = 0;
            revNum = ReverseBytes(num);
            int.TryParse(textBox2.Text, out row);
            int.TryParse(textBox1.Text, out column);
            int delete = 0;
            if(comboBox1.SelectedIndex == 1)
            {
                if (row <= 32)
                    delete = 0;
                if (row <= 24)
                    delete = 2;
                if (row <= 16)
                    delete = 4;
                if (row <= 8)
                    delete = 6;
            } 
            else
            {
                if (column <= 32)
                    delete = 0;
                if (column <= 24)
                    delete = 2;
                if (column <= 16)
                    delete = 4;
                if (column <= 8)
                    delete = 6;



            }
            

            byte[] bytes = BitConverter.GetBytes(revNum);
            string retval = "";
            foreach (byte b in bytes)
                retval += b.ToString("X2");

            if (delete != 0)
                retval = retval.Remove(0, delete);
            return retval;
        }

        private string LittleEndian(int num)
        {

            int delete = 0;
            if (comboBox1.SelectedIndex == 1)
            {
                if (row <= 32)
                    delete = 0;
                if (row <= 24)
                    delete = 2;
                if (row <= 16)
                    delete = 4;
                if (row <= 8)
                    delete = 6;
            }
            else
            {
                if (column <= 32)
                    delete = 0;
                if (column <= 24)
                    delete = 2;
                if (column <= 16)
                    delete = 4;
                if (column <= 8)
                    delete = 6;
            }

                byte[] bytes = BitConverter.GetBytes(num);
            string retval = "";
            foreach (byte b in bytes)
                retval += b.ToString("X2");
            if(delete!=0)
            retval = retval.Remove(retval.Length - delete);
            return retval;
        }

        private bool GetBit(int b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        private void Comment_CheckedChanged(object sender, EventArgs e)
        {
            if (!Comment.Checked)
                textBox4.Enabled = false;
            else
                textBox4.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void AddNewChar_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "")
            {
                int.TryParse(textBox2.Text, out row);
                int.TryParse(textBox1.Text, out column);
                if (row > column)
                    typeRef = row;
                else if (column > row)
                    typeRef = column;
                else if (row == column)
                    typeRef = row;

                if (typeRef <= 32)
                    type = 32;
                if (typeRef <= 16)
                    type = 16;
                if (typeRef <= 8)
                    type = 8;

                int index = richTextBox1.Text.IndexOf("}");
                if (index > 0)
                    richTextBox1.Text = richTextBox1.Text.Substring(0, index);
                index = richTextBox1.Text.LastIndexOf("\n");
                if (index > 0)
                    richTextBox1.Text = richTextBox1.Text.Substring(0, index);
                index = richTextBox1.Text.LastIndexOf("//");
                string tempt = "";
                if (index > 0)
                {
                    tempt = richTextBox1.Text.Substring(index, richTextBox1.Text.Length - index);
                    richTextBox1.Text = richTextBox1.Text.Substring(0, index);
                }

                    richTextBox1.AppendText(", "+tempt+"\n");



                BitArray BitArray = new BitArray(row);
                int Number = 0;
                BitArray bit = new BitArray(1);
                string comment = "";
                for (int i = 0; i < column; i++)
                {

                    for (int j = 0; j < row; j++)
                    {
                        BitArray[j] = GetBit(matrix[j, i], 0);
                        Number = getIntFromBitArray(BitArray);
                    }
                    string hexValue;
                    if (comboBox2.SelectedIndex == 1)
                        hexValue = LittleEndian(Number);
                    else
                        hexValue = BigEndian(Number);

                    if (Comment.Checked)
                        comment = "//" + textBox4.Text;
                    else
                        comment = "";
                    richTextBox1.AppendText("0x" + hexValue);
                    Number = 0;
                    if (i < column - 1)
                        richTextBox1.AppendText(", ");

                }

                richTextBox1.AppendText(comment + Environment.NewLine + "};");
            }
            else
                MessageBox.Show("Check the rules!", "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


        }

        private void ClearLastChar_Click(object sender, EventArgs e)
        {
            int index = richTextBox1.Text.LastIndexOf("\n");
            if (index > 0)
                richTextBox1.Text = richTextBox1.Text.Substring(0, index);
            index = richTextBox1.Text.LastIndexOf("\n");
            if (index > 0)
                richTextBox1.Text = richTextBox1.Text.Substring(0, index);
            index = richTextBox1.Text.LastIndexOf(",");
            if (index > 0)
                richTextBox1.Text = richTextBox1.Text.Substring(0, index);

            richTextBox1.AppendText("\n};");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveMyFile();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                int width = panel1.Size.Width;
                int height = panel1.Size.Height;

                Bitmap bm = new Bitmap(width, height);
                panel1.DrawToBitmap(bm, new Rectangle(0, 0, width, height));
                if (Directory.Exists(Application.StartupPath + "/Fonts"))
                {
                    bm.Save(Application.StartupPath + "/Fonts/" + textBox4.Text + ".bmp", ImageFormat.Bmp);

                }
                else
                {
                    Directory.CreateDirectory(Application.StartupPath + "/Fonts");
                    bm.Save(Application.StartupPath + "/Fonts/" + textBox4.Text + ".bmp", ImageFormat.Bmp);
                }

            }
            else
            {
                MessageBox.Show("Comment can not be null !");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox2.Text, out row);
            int.TryParse(textBox1.Text, out column);
            for (int i = 0; i < row; i++)
            {

                for (int j = 0; j < column; j++)
                {
                    imgArray[i, j].BackColor = Color.Gray;
                    matrix[i, j] = 0;
                }
            }
        }

        public void SaveMyFile()
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            SaveFileDialog saveFile1 = new SaveFileDialog();

            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.c";
            saveFile1.Filter = "Text file |*.c";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            }
        }
        private int getIntFromBitArray(BitArray bitArray)
        {
            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
