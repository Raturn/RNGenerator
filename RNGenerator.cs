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



        //------------------------------- 컨트롤 탐색 -------------------------------
        private Control FindControl(Control _parent, string _name)     // 재귀 탐색 >> 선택한 항목의 자식들을 탐색하여 내가 원하는 요소를 매칭
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



        //-------------------------------------------------------- A매트릭스 분석 --------------------------------------------------------
        //-------------------------------- 난수 추출 --------------------------------
        private string getNum(string _menu, string _contentAddress)
        {
            string[] locationInfo = _contentAddress.Split("to");       // MetrixA1to1 형식의 데이터를 MetrixA1, 1 형식으로 나눔

            string xNum = locationInfo[0].Substring(7);     // X에 해당하는 위치값
            string yNum = locationInfo[1];                  // Y에 해당하는 위치값

            string RNum = " "+FindControl(this, $"Metrix{_menu}XKey{xNum}").Text + FindControl(this, $"Metrix{_menu}YKey{yNum}").Text+" ";
            
            return RNum;
        }
        //---------------------------------------------------------------------------


        private void DecipherAMetrix(string _menu)      // 현재 2수조의 경우만 작성
        {
            string inputValue = FindControl(this, $"Input_{_menu}").Text;           // 입력값
            string removedSameValue = inputValue;               // 문장에서 이미 난수 추출이된 값을 제외하기 위해 사용한 변수
            Dictionary<string, string> matchedData = new();


            for (int row = 1; row <= 10; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    string controlName = $"Metrix{_menu}{row}to{col}";
                    Control ctrl = FindControl(this, controlName);

                    if (inputValue.Contains(ctrl.Text) && !matchedData.ContainsKey(ctrl.Text))         // 입력값이 내부치를 포함하고 있다면 Key 값을 내부치로 Value 값을 추출된 난수로 할당
                    {
                        matchedData.Add(ctrl.Text, getNum(_menu, controlName));
                    }
                }
            }

            //if (matchedData.Count > 0)          // 문장 또는 단어, 한 글자로 매칭된 데이터가 존재하는 경우 해당 단어들 제외
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



        //-------------------------------------------------------- B매트릭스 분석 --------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------



        //-------------------------------------------------------- C매트릭스 분석 --------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------------



        //---------------------------------------------------------- 버튼 이벤트 기능 ----------------------------------------------------------
        //----------------------- 복사, 붙여넣기 옵션 -----------------------

        private void CopyKeyValue(string _menu)             // Key값 복사
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
                    strArr_XKey[index] = tb1.Text;        // XKey 값을 글로벌에서 선언된 배열에 할당
                }
                else
                {
                    strArr_XKey[index] = string.Empty;
                }

                if (ctrl2 is System.Windows.Forms.TextBox tb2)
                {
                    strArr_YKey[index] = tb2.Text;        // YKey 값을 글로벌에서 선언된 배열에 할당
                }
                else
                {
                    strArr_YKey[index] = string.Empty;
                }

                index++;
            }
        }

        private void PasteKeyValue(string _menu)            // Key값 붙여넣기
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
                    tb1.Text = strArr_XKey[index];        // XKey 배열에 저장된 값을 할당
                }

                if (ctrl2 is System.Windows.Forms.TextBox tb2)
                {
                    tb2.Text = strArr_YKey[index];        // YKey 배열에 저장된 값을 할당
                }

                index++;
            }

        }


        private void CopyContent(string _menu)             // 내부치 복사
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
                        strArr_TempContent[index] = tb.Text;        // 내부치 값을 글로벌에서 선언된 배열에 할당
                    }
                    else
                    {
                        strArr_TempContent[index] = string.Empty;
                    }

                    index++;
                }
            }
        }

        private void PasteContent(string _menu)            // 내부치 붙여넣기
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
                        tb.Text = strArr_TempContent[index];        // 내부치 배열에 저장된 값을 할당
                    }

                    index++;
                }
            }
        }
        //-----------------------------------------------------------


        //----------------------- 삭제 옵션 -----------------------
        private void DelCols(string _menu)      // 지시부 삭제
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


        private void DelContent(string _menu)      // 내부치 삭제
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






        //---------------------------------------------------------- 버튼 이벤트 사용 ----------------------------------------------------------
        //Key값 복사 ----------------------------------------------
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


        //Key값 붙여넣기 -------------------------------------------
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


        //내부치 복사 ----------------------------------------------
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


        //내부치 붙여넣기 ------------------------------------------
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


        //지시부 삭제 ----------------------------------------------
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


        //내부치 삭제 ----------------------------------------------
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
