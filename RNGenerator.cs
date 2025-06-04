using System.Text;

namespace RNGenerator
{
    public partial class RNGenerator : Form
    {
        public RNGenerator()
        {
            InitializeComponent();
            txtInput.TextChanged += txtInput_TextChanged; //textInput의 Value가 변경될 때마다 동작
        }

        //--------------- 디자이너 파일에 존재하던 로직 ---------------
        //textbox의 name을 이용하여 위치 값 반환
        private String MatchingValue(TextBox tb, char value)
        {
            if (tb.Text.Length == 0)
                return null;

            if (value == tb.Text[0])
                return tb.Name.Substring(3, 2);

            return null;
        }

        // textInput의 값 변경이 감지될 때마다 초성, 중성, 종성을 구분하고 해당하는 값의 위치 값을 반환
        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            StringBuilder sbOutput = new StringBuilder();
            string machingValue = null;

            char[] disassebleString = HangulDisassembler.DisassembleHangul(txtInput.Text);
            foreach (char c in disassebleString)
            {
                if (c == ' ')
                    continue;

                if (c == '-')
                {
                    sbOutput.Append($" ");
                    continue;
                }

                machingValue = MatchingValue(val11, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val12, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val13, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val14, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val15, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val16, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val17, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val18, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val19, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val10, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val21, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val22, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val23, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val24, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val25, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val26, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val27, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val28, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val29, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val20, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val31, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val32, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val33, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val34, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val35, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val36, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val37, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val38, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val39, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val30, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val41, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val42, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val43, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val44, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val45, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val46, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val47, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val48, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val49, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val40, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val51, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val52, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val53, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val54, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val55, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val56, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val57, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val58, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val59, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val50, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }


                machingValue = MatchingValue(val61, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val62, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val63, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val64, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val65, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val66, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val67, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val68, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val69, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val60, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val71, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val72, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val73, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val74, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val75, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val76, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val77, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val78, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val79, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val70, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val81, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val82, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val83, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val84, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val85, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val86, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val87, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val88, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val89, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val80, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val91, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val92, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val93, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val94, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val95, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val96, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val97, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val98, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val99, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val90, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                machingValue = MatchingValue(val01, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val02, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val03, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val04, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val05, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val06, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val07, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val08, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val09, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }
                machingValue = MatchingValue(val00, c); if (machingValue != null) { sbOutput.Append(machingValue); continue; }

                //if(c != null)
                sbOutput.Append(c); //못찾음
            }

            txtOutput.Text = sbOutput.ToString();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox140_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox545_TextChanged(object sender, EventArgs e)
        {

        }
        //-------------------------------------------------------------
    }
}
