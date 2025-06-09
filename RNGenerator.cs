using System.Text;

namespace RNGenerator
{
    public partial class RNGenerator : Form
    {
        String[] strArr_TempContent = new String[100];

        public RNGenerator()
        {
            InitializeComponent();
            //txtInput.TextChanged += txtInput_TextChanged; // textInput의 Value가 변경될 때마다 동작
        }

        //----------------------- 내부치 복사, 붙여넣기 -----------------------
        private Control FindControlRecursive(Control _parent, string _name)     // 재귀 탐색 >> 선택한 항목의 자식들을 탐색하여 내가 원하는 요소를 매칭
        {
            foreach (Control control in _parent.Controls)
            {
                if (control.Name == _name)
                    return control;

                Control found = FindControlRecursive(control, _name);
                if (found != null)
                    return found;
            }
            return null;
        }


        private void LoadMetrixTextBoxes(String _Menu)      // TextBox 값을 복사하여 글로벌에서 선언된 배열에 할당
        {
            int index = 0;

            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_Menu}{row}to{col}";
                    Control ctrl = FindControlRecursive(this, controlName);

                    if (ctrl is TextBox tb)
                    {
                        strArr_TempContent[index] = tb.Text;
                    }
                    else
                    {
                        strArr_TempContent[index] = string.Empty;
                    }

                    index++;
                }
            }
        }

        private void SetTextBoxesFromArray(String _Menu)        // 글로벌 배열에 저장된 값을 붙여넣기
        {
            int index = 0;

            for (int row = 1; row <= 10; row++) // X
            {
                for (int col = 1; col <= 10; col++) // Y
                {
                    string controlName = $"Metrix{_Menu}{row}to{col}";
                    Control ctrl = FindControlRecursive(this, controlName);

                    if (ctrl is TextBox tb)
                    {
                        tb.Text = strArr_TempContent[index];
                    }

                    index++;
                }
            }
        }
        //-----------------------------------------------------------



        //----------------------------- 버튼 이벤트 -----------------------------

        //내부치 복사 ----------------------------------------------
        private void ContentCopy1_Click(object sender, EventArgs e)
        {
            LoadMetrixTextBoxes("A");
        }

        private void ContentCopy2_Click(object sender, EventArgs e)
        {
            LoadMetrixTextBoxes("B");
        }

        private void ContentCopy3_Click(object sender, EventArgs e)
        {
            LoadMetrixTextBoxes("C");
        }

        //내부치 붙여넣기 ------------------------------------------
        private void ContentPaste1_Click(object sender, EventArgs e)
        {
            SetTextBoxesFromArray("A");
        }

        private void ContentPaste2_Click(object sender, EventArgs e)
        {
            SetTextBoxesFromArray("B");
        }

        private void ContentPaste3_Click(object sender, EventArgs e)
        {
            SetTextBoxesFromArray("C");
        }
        //---------------------------------------------------------------------

    }
}
