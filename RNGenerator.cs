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


        //------------------------------- �ڸ��� �и� -------------------------------
        public static string SplitKorean(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length != 1)
                return "";

            char c = input[0];

            string[] ChoSung = new string[]
            {
                "��","��","��","��","��","��","��","��","��","��",
                "��","��","��","��","��","��","��","��","��"
            };

            string[] JungSung = new string[]
            {
                "��","��","��","��","��","��","��","��","��","��",
                "��","��","��","��","��","��","��","��","��","��","��"
            };

            string[] JongSung = new string[]
            {
                "", "��","��","��","��","��","��","��","��","��",
                "��","��","��","��","��","��","��","��","��","��",
                "��","��","��","��","��","��","��","��"
            };

            if (c < 0xAC00 || c > 0xD7A3)
                return "";

            int unicode = c - 0xAC00;
            int choIndex = unicode / (21 * 28);
            int jungIndex = (unicode % (21 * 28)) / 28;
            int jongIndex = unicode % 28;

            // �ʼ� + �߼� + ������ ���ڿ��� ���� ��ȯ
            return ChoSung[choIndex] + JungSung[jungIndex] + JongSung[jongIndex];
        }
        //---------------------------------------------------------------------------



        //------------------------------- ����, ������, ������ �и� -------------------------------
        private string DetachedElement(string _inputValue)
        {
            return _inputValue switch
            {
                "��" => "�Ǥ�",     // ����
                "��" => "�Ǥ�",
                "��" => "�Ǥ�",
                "��" => "�̤�",
                "��" => "�̤�",    //�̤ä�
                "��" => "�̤�",
                "��" => "�Ѥ�",
                "��" => "����",
                "��" => "�ä�",
                "��" => "����",
                "��" => "�Ť�",
                "��" => "����",    // ������
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",    // ������
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                "��" => "����",
                _ => _inputValue,  // �� �ܴ� �״�� ��ȯ
            };
        }
        //---------------------------------------------------------------------------------------------



        //-------------------------------------------------------- A��Ʈ���� �м� --------------------------------------------------------
        //-------------------------------- 2���� ���� ���� --------------------------------
        private string getNum(string _menu, string _contentAddress)
        {
            string[] locationInfo = _contentAddress.Split("to");       // MetrixA1to1 ������ �����͸� MetrixA1, 1 �������� ����

            string xNum = locationInfo[0].Substring(7);     // X�� �ش��ϴ� ��ġ��
            string yNum = locationInfo[1];                  // Y�� �ش��ϴ� ��ġ��

            string RNum = FindControl(this, $"Metrix{_menu}XKey{xNum}").Text + FindControl(this, $"Metrix{_menu}YKey{yNum}").Text;

            return RNum;
        }
        //----------------------------------------------------------------------------------


        //-------------------------------- ��ġ�� ġȯ --------------------------------
        private Dictionary<string, string> GetLocationValue(string _inputValue, string _menu, Dictionary<string, string> _matchedData)      // �Է°�, ������ ��Ʈ����, ����� �����͸� ������ Dictionary�� �Ű������� ����
        {
            for (int row = 1; row <= 10; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (_inputValue.Contains(ctrl.Text) && !_matchedData.ContainsKey(ctrl.Text))         // �Է°��� ����ġ�� �����ϰ� �ִٸ� Key ���� ����ġ�� Value ���� ����� ������ �Ҵ�
                    {
                        _matchedData.Add(ctrl.Text, getNum(_menu, controlName));
                    }
                }
            }

            return _matchedData;
        }
        //-----------------------------------------------------------------------------


        //-------------------------------- 10������ ���ΰ��� --------------------------------
        public static string InsertNewLine(string input, int groupSize = 10)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < parts.Length; i++)
            {
                result.Append(parts[i]);

                if (i != parts.Length - 1)
                    result.Append(' ');

                // groupSize������ �ٹٲ�
                if ((i + 1) % groupSize == 0 && i != parts.Length - 1)
                    result.Append(Environment.NewLine);
            }

            return result.ToString();
        }
        //-----------------------------------------------------------------------------------


        private void DecipherAMetrix(string _menu)      // ���� 2������ ��츸 �ۼ�
        {
            string inputValue = FindControl(this, $"Input_{_menu}").Text;           // �Է°�
            string removedSameValue = inputValue;               // ���忡�� �̹� ���� �����̵� ���� �����ϱ� ���� ����� ����
            Dictionary<string, string> matchedData = new();     // Key ���� �Է°�����, Value ���� ������ ������ Dictionary



            matchedData = GetLocationValue(inputValue, _menu, matchedData); // �Է°��� �ش��ϴ� ����ġ�� ���� ��Ī



            if (matchedData.Count > 0)          // ���� �Ǵ� �ܾ�, �� ���ڷ� ��Ī�� �����Ͱ� �����ϴ� ��� �ش� �ܾ�� ����
            {
                foreach (KeyValuePair<string, string> item in matchedData)
                {
                    try
                    {
                        removedSameValue = removedSameValue.Replace(item.Key, "");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


            if (removedSameValue.Length > 0) // ����, �ܾ�, �� ���� ������ ���� �� ��ȯ���� ���� �ܾ ���� ���� ���
            {
                string[] restWord = new string[removedSameValue.Length];


                for (int i = 0; i < removedSameValue.Length; i++)       // �����ִ� ������ �� ���� ������ ������
                {
                    restWord[i] = removedSameValue[i].ToString();
                }

                foreach (var eachWord in restWord)      // �� ���ھ� �ݺ����� ���� ������ ����
                {
                    string detachedWord = SplitKorean(eachWord);      // �ڸ��� �и� >> �ϳ��� string���� ��µ� ex) �� -> ������
                    inputValue = inputValue.Replace(eachWord, detachedWord);      // �Է°� �� ������ ġȯ���� ���� �ܾ �ڸ������� �и��Ͽ� ����

                    for (int cnt = 0; cnt < detachedWord.Length; cnt++)
                    {
                        GetLocationValue(detachedWord[cnt].ToString(), _menu, matchedData); // �ڸ��� �и��� �ܾ �ٽ� ������ ġȯ�ϱ� ���� GetLocationValue ȣ��
                    }
                }
            }


            string tempInput = inputValue;            // inputValue�� �ݺ��ϴµ��� inputValue�� ���� ������ �ӽ� ����

            for (int dtc = 0; dtc < inputValue.Length; dtc++)   // ������ ���� ������ ���������� �����Ͽ��� ��� �и� ��Ű�� ����
            {
                tempInput = tempInput.Replace(inputValue[dtc].ToString(), DetachedElement(inputValue[dtc].ToString())); // �Է°� �� ����, ������, �������� �и��Ͽ� ����
            }

            inputValue = tempInput;        // �и��� ����, ������, �������� ������ �Է°����� ����



            var sortedKeys = matchedData.Keys.OrderByDescending(k => k.Length).ToList();   // Key���� ���� �������� �������� ����
            string result = "";         // ����� ������ ����

            while (inputValue.Length > 0)
            {
                string buffer = ""; // �߶� ���ڵ� ����
                bool matched = false;

                // inputValue���� �ڿ������� �� ���ھ� �߶󰡸� ��Ī �õ�
                for (int len = inputValue.Length; len > 0; len--)
                {
                    string current = inputValue.Substring(0, len); // �տ��� len��ŭ �ڸ� �κ�

                    foreach (var key in sortedKeys)
                    {
                        if (current == key)
                        {
                            result += matchedData[key] + " ";
                            inputValue = inputValue.Substring(len); // ��Ī�� �κ� ����
                            inputValue = buffer + inputValue;       // �ڸ� �κ� ����
                            matched = true;
                            break;
                        }
                    }

                    if (matched)
                        break;

                    // ��Ī ���� ��, �� �� ���ڸ� buffer�� �̵�
                    buffer = inputValue[inputValue.Length - 1] + buffer;
                    inputValue = inputValue.Substring(0, inputValue.Length - 1);
                }

                // ��Ī�� �� �ϳ��� ���� inputValue�� �پ�� ���·� ���� ������ ���� ���� ����
                if (!matched)
                {
                    Console.WriteLine($"[���] ��Ī ����: '{inputValue}'�� � Ű���� �ش����� �ʽ��ϴ�.");
                    break;
                }
            }


            FindControl(this, $"Output_{_menu}").Text = InsertNewLine(result); // ���������� ������ ġȯ�� ���ڿ��� ���â�� ǥ��, 10������ ���� ó��
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
                string controlName1 = $"Metrix{_menu}XKey{keyNum}";
                string controlName2 = $"Metrix{_menu}YKey{keyNum}";

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
                string controlName1 = $"Metrix{_menu}XKey{keyNum}";
                string controlName2 = $"Metrix{_menu}YKey{keyNum}";

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

        private void DelKeyValues(string _menu)      // Key�� ����
        {
            for (int row = 1; row <= 10; row++)
            {
                string controlName1 = $"Metrix{_menu}XKey{row}";
                string controlName2 = $"Metrix{_menu}YKey{row}";

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


        //Key�� ���� ----------------------------------------------
        private void KeyValDel1_Click(object sender, EventArgs e)
        {
            DelKeyValues("A");
        }

        private void KeyValDel2_Click(object sender, EventArgs e)
        {
            DelKeyValues("B");
        }

        private void KeyValDel3_Click(object sender, EventArgs e)
        {
            DelKeyValues("C");
        }
        //--------------------------------------------------------


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
