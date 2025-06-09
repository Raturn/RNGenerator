using System.Text;

namespace RNGenerator
{
    public partial class RNGenerator : Form
    {
        String[] strArr_TempContent = new String[100];

        public RNGenerator()
        {
            InitializeComponent();
            //txtInput.TextChanged += txtInput_TextChanged; // textInput�� Value�� ����� ������ ����
        }

        //----------------------- ����ġ ����, �ٿ��ֱ� -----------------------
        private Control FindControlRecursive(Control _parent, string _name)     // ��� Ž�� >> ������ �׸��� �ڽĵ��� Ž���Ͽ� ���� ���ϴ� ��Ҹ� ��Ī
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


        private void LoadMetrixTextBoxes(String _Menu)      // TextBox ���� �����Ͽ� �۷ι����� ����� �迭�� �Ҵ�
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

        private void SetTextBoxesFromArray(String _Menu)        // �۷ι� �迭�� ����� ���� �ٿ��ֱ�
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



        //----------------------------- ��ư �̺�Ʈ -----------------------------

        //����ġ ���� ----------------------------------------------
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

        //����ġ �ٿ��ֱ� ------------------------------------------
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
