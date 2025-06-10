using System;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RNGenerator
{
    public partial class RNGenerator : Form
    {
        string[] strArr_TempContent = new string[100];
        string[] strArr_XKey = new string[10];
        string[] strArr_YKey = new string[10];

        public RNGenerator()
        {
            InitializeComponent();

            Input_A.TextChanged += (s, e) => DecipherAMetrix("A");
            Input_B.TextChanged += (s, e) => DecipherAMetrix("B");
            Input_C.TextChanged += (s, e) => DecipherAMetrix("C");
        }



        //------------------------------- ��Ʈ�� Ž�� -------------------------------
        private Control FindControl(Control _parent, string _name)     // ��� Ž�� >> ������ �׸��� �ڽĵ��� Ž���Ͽ� ���� ���ϴ� ��Ҹ� ��Ī
        {
            foreach (Control control in _parent.Controls)
            {
                if (control.Name == _name)
                    return control;

                Control found = FindControl(control, _name);
                if (found != null)
                    return found;
            }
            return null;
        }
        //---------------------------------------------------------------------------



        //-------------------------------------------------------- A��Ʈ���� �м� --------------------------------------------------------
        //-------------------------------- ���� ���� --------------------------------
        private string getNum(string _menu, string _contentAddress)
        {
            string[] locationInfo = _contentAddress.Split("to");       // MetrixA1to1 ������ �����͸� MetrixA1, 1 �������� ����

            string xNum = locationInfo[0].Substring(7);     // X�� �ش��ϴ� ��ġ��
            string yNum = locationInfo[1];                  // Y�� �ش��ϴ� ��ġ��

            string RNum = " "+FindControl(this, $"Metrix{_menu}XKey{xNum}").Text + FindControl(this, $"Metrix{_menu}YKey{yNum}").Text+" ";
            
            return RNum;
        }
        //---------------------------------------------------------------------------


        private void DecipherAMetrix(string _menu)      // ���� 2������ ��츸 �ۼ�
        {
            string inputValue = FindControl(this, $"Input_{_menu}").Text;           // �Է°�
            string removedSameValue = inputValue;               // ���忡�� �̹� ���� �����̵� ���� �����ϱ� ���� ����� ����
            Dictionary<string, string> matchedData = new();


            for (int row = 1; row <= 10; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (inputValue.Contains(ctrl.Text) && !matchedData.ContainsKey(ctrl.Text))         // �Է°��� ����ġ�� �����ϰ� �ִٸ� Key ���� ����ġ�� Value ���� ����� ������ �Ҵ�
                    {
                        matchedData.Add(ctrl.Text, getNum(_menu, controlName));
                    }
                }
            }

            //if (matchedData.Count > 0)          // ���� �Ǵ� �ܾ�, �� ���ڷ� ��Ī�� �����Ͱ� �����ϴ� ��� �ش� �ܾ�� ����
            //{
            //    foreach (KeyValuePair<string, string> item in matchedData)
            //    {
            //        removedSameValue = inputValue.Replace(item.Value, "");
            //    }
            //}


            foreach (KeyValuePair<string, string> item in matchedData)
            {
                try
                {
                    removedSameValue = removedSameValue.Replace(item.Key, item.Value);
                }
                catch (Exception ex)
                {

                }
            }
            

            FindControl(this, $"Output_{_menu}").Text = removedSameValue.Trim();
        }
        //--------------------------------------------------------------------------------------------------------------------------------



        //-------------------------------------------------------- B��Ʈ���� �м� --------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------



        //-------------------------------------------------------- C��Ʈ���� �м� --------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------



        //---------------------------------------------------------- ��ư �̺�Ʈ ��� ----------------------------------------------------------
        //----------------------- ����, �ٿ��ֱ� �ɼ� -----------------------

        private void CopyKeyValue(string _menu)             // Key�� ����
        {
            int index = 0;

            for (int keyNum = 1; keyNum <= 10; keyNum++)
            {
                string controlName1 = $"Metrix{_menu}X{keyNum}";
                string controlName2 = $"Metrix{_menu}Y{keyNum}";

                Control ctrl1 = FindControl(this, controlName1);
                Control ctrl2 = FindControl(this, controlName2);

                if (ctrl1 is System.Windows.Forms.TextBox tb1)
                {
                    strArr_XKey[index] = tb1.Text;        // XKey ���� �۷ι����� ����� �迭�� �Ҵ�
                }
                else
                {
                    strArr_XKey[index] = string.Empty;
                }

                if (ctrl2 is System.Windows.Forms.TextBox tb2)
                {
                    strArr_YKey[index] = tb2.Text;        // YKey ���� �۷ι����� ����� �迭�� �Ҵ�
                }
                else
                {
                    strArr_YKey[index] = string.Empty;
                }

                index++;
            }
        }

        private void PasteKeyValue(string _menu)            // Key�� �ٿ��ֱ�
        {
            int index = 0;

            for (int keyNum = 1; keyNum <= 10; keyNum++)
            {
                string controlName1 = $"Metrix{_menu}X{keyNum}";
                string controlName2 = $"Metrix{_menu}Y{keyNum}";

                Control ctrl1 = FindControl(this, controlName1);
                Control ctrl2 = FindControl(this, controlName2);

                if (ctrl1 is System.Windows.Forms.TextBox tb1)
                {
                    tb1.Text = strArr_XKey[index];        // XKey �迭�� ����� ���� �Ҵ�
                }

                if (ctrl2 is System.Windows.Forms.TextBox tb2)
                {
                    tb2.Text = strArr_YKey[index];        // YKey �迭�� ����� ���� �Ҵ�
                }

                index++;
            }

        }


        private void CopyContent(string _menu)             // ����ġ ����
        {
            int index = 0;

            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (ctrl is System.Windows.Forms.TextBox tb)
                    {
                        strArr_TempContent[index] = tb.Text;        // ����ġ ���� �۷ι����� ����� �迭�� �Ҵ�
                    }
                    else
                    {
                        strArr_TempContent[index] = string.Empty;
                    }

                    index++;
                }
            }
        }

        private void PasteContent(string _menu)            // ����ġ �ٿ��ֱ�
        {
            int index = 0;

            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (ctrl is System.Windows.Forms.TextBox tb)
                    {
                        tb.Text = strArr_TempContent[index];        // ����ġ �迭�� ����� ���� �Ҵ�
                    }

                    index++;
                }
            }
        }
        //-----------------------------------------------------------


        //----------------------- ���� �ɼ� -----------------------
        private void DelCols(string _menu)      // ���ú� ����
        {
            for (int row = 1; row <= 2; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    string controlName1 = $"MultiVal{_menu}X{row}to{col}";
                    string controlName2 = $"MultiVal{_menu}Y{row}to{col}";

                    Control ctrl1 = FindControl(this, controlName1);
                    Control ctrl2 = FindControl(this, controlName2);

                    if (ctrl1 is System.Windows.Forms.TextBox tb1)
                    {
                        tb1.Text = "";
                    }

                    if (ctrl2 is System.Windows.Forms.TextBox tb2)
                    {
                        tb2.Text = "";
                    }
                }
            }
        }


        private void DelContent(string _menu)      // ����ġ ����
        {
            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (ctrl is System.Windows.Forms.TextBox tb)
                    {
                        tb.Text = "";
                    }
                }
            }
        }
        //-------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------






        //---------------------------------------------------------- ��ư �̺�Ʈ ��� ----------------------------------------------------------
        //Key�� ���� ----------------------------------------------
        private void KeyValCopy1_Click(object sender, EventArgs e)
        {
            CopyKeyValue("A");
        }

        private void KeyValCopy2_Click(object sender, EventArgs e)
        {
            CopyKeyValue("B");
        }

        private void KeyValCopy3_Click(object sender, EventArgs e)
        {
            CopyKeyValue("C");
        }
        //----------------------------------------------------------


        //Key�� �ٿ��ֱ� -------------------------------------------
        private void KeyValPaste1_Click(object sender, EventArgs e)
        {
            PasteKeyValue("A");
        }

        private void KeyValPaste2_Click(object sender, EventArgs e)
        {
            PasteKeyValue("B");
        }

        private void KeyValPaste3_Click(object sender, EventArgs e)
        {
            PasteKeyValue("C");
        }
        //----------------------------------------------------------


        //����ġ ���� ----------------------------------------------
        private void ContentCopy1_Click(object sender, EventArgs e)
        {
            CopyContent("A");
        }

        private void ContentCopy2_Click(object sender, EventArgs e)
        {
            CopyContent("B");
        }

        private void ContentCopy3_Click(object sender, EventArgs e)
        {
            CopyContent("C");
        }
        //----------------------------------------------------------


        //����ġ �ٿ��ֱ� ------------------------------------------
        private void ContentPaste1_Click(object sender, EventArgs e)
        {
            PasteContent("A");
        }

        private void ContentPaste2_Click(object sender, EventArgs e)
        {
            PasteContent("B");
        }

        private void ContentPaste3_Click(object sender, EventArgs e)
        {
            PasteContent("C");
        }
        //----------------------------------------------------------


        //���ú� ���� ----------------------------------------------
        private void RowDel1_Click(object sender, EventArgs e)
        {
            DelCols("A");
        }

        private void RowDel2_Click(object sender, EventArgs e)
        {
            DelCols("B");
        }
        private void RowDel3_Click(object sender, EventArgs e)
        {
            DelCols("C");
        }
        //----------------------------------------------------------


        //����ġ ���� ----------------------------------------------
        private void ContentDel1_Click(object sender, EventArgs e)
        {
            DelContent("A");
        }

        private void ContentDel2_Click(object sender, EventArgs e)
        {
            DelContent("B");
        }

        private void ContentDel3_Click(object sender, EventArgs e)
        {
            DelContent("C");
        }
        //----------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}
