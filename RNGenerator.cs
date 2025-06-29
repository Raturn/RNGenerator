using System;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RNGenerator
{
    public partial class RNGenerator : Form
    {
        public static RNGenerator Instance;  // �ٸ� Ŭ�������� ������ �� �ֵ��� static ����


        public RNGenerator()
        {
            InitializeComponent();
            Instance = this;

            this.KeyPreview = true;
        }


        string[] tableClassArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" }; // ���̺� Ŭ���� �̸� �迭


        private float currentZoom = 1.0f;
        private readonly float minZoom = 0.5f;
        private readonly float maxZoom = 2.0f;


        private List<System.Windows.Forms.TextBox> matchedTextBoxes = new List<System.Windows.Forms.TextBox>();



        //----------------------------- �˻� ��� -----------------------------
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                SearchBox searchForm = new SearchBox(this);  // ���� �� ����
                searchForm.Show(this);  // �˻� �˾� ǥ��
            }
        }


        public void SearchAndFocus(string searchText, int matchIndex)
        {
            // �˻� ��� ���� ����
            matchedTextBoxes = this.Controls.OfType<System.Windows.Forms.TextBox>()
                .Where(tb => tb.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

            if (matchedTextBoxes.Count == 0)
            {
                MessageBox.Show("��ġ�ϴ� �׸��� �����ϴ�.");
                return;
            }

            if (matchIndex >= matchedTextBoxes.Count)
                matchIndex = 0;  // ��ȯ ����

            var tb = matchedTextBoxes[matchIndex];
            tb.Focus();
            tb.Select(tb.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase), searchText.Length);
            this.ScrollControlIntoView(tb);
        }



        public int GetMatchCount(string keyword)
        {
            return this.Controls
                .OfType<System.Windows.Forms.TextBox>()
                .Count(tb => tb.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
        //---------------------------------------------------------------------



        //------------------------------- ��Ʈ�� Ž�� -------------------------------
        public Control FindControl(Control _parent, string _name)     // ��� Ž�� >> ������ �׸��� �ڽĵ��� Ž���Ͽ� ���� ���ϴ� ��Ҹ� ��Ī
        {
            foreach (Control control in _parent.Controls)
            {
                if (control.Name == _name)
                    return control;

                Control found = FindControl(control, _name);
                if (found != null)
                    return found;
            }


            //MessageBox.Show($"[����] '{_name}' ��Ʈ���� ã�� �� �����ϴ�.", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);


            return null;
        }
        //---------------------------------------------------------------------------



        //-------------------- ��ġ�� ���� --------------------
        // �켱 2������ ����
        private string positionValue(string tableClass, string xVal, string yVal)
        {
            string position = FindControl(this, $"{tableClass}keyX{xVal}").Text + FindControl(this, $"{tableClass}keyY{yVal}").Text;

            return position;
        }
        //-----------------------------------------------------

        //-------------------- ����ġ ��Ī --------------------
        public Dictionary<string, string> MatchedData(Dictionary<string, string> _dic)
        {
            bool isMatched = false; // �ش� key�� ���� ��Ī ����
            List<string> keys = _dic.Keys.ToList(); // Ű ��� ����


            foreach (string key in keys)
            {
                isMatched = false;

                for (int i = 0; i < tableClassArray.Length && !isMatched; i++) // ���� ���� ����
                {
                    for (int x = 1; x <= 10 && !isMatched; x++)
                    {
                        for (int y = 1; y <= 10 && !isMatched; y++)
                        {
                            string ctrlKey = $"{tableClassArray[i]}var{x}_{y}"; // ��Ʈ�� �̸�
                            Control ctrl = FindControl(this, ctrlKey);

                            if (ctrl is System.Windows.Forms.TextBox textBox)
                            {
                                // ���⼭ �� ������ �����̳Ŀ� ���� �޶���
                                // Text ���� ���� key�� ���ٸ�? �Ǵ� �ܼ��� ������� �ʴٸ�?
                                if (textBox.Text == key) // ��Ȯ�� ��Ī �ø� ���
                                {
                                    _dic[key] = positionValue(tableClassArray[i], x.ToString(), y.ToString());
                                    isMatched = true; // �� �̻� �ݺ����� �ʰ�
                                }
                            }
                        }
                    }
                }
            }

            // �� Ȯ�ο� ���
            //string aaa = string.Join(Environment.NewLine, _dic.Select(kv => $"{kv.Key}: {kv.Value}"));
            //MessageBox.Show(aaa);

            return _dic;
        }
        //-----------------------------------------------------


        //---------------------------------------------------------- ��ư �̺�Ʈ ���� ----------------------------------------------------------
        private void Controller_Btn_Click(object sender, EventArgs e)
        {
            Controller controller = new Controller(); // �� �� ����
            controller.Name = "Controller";           // �� �̸� ����

            Form existingForm = Application.OpenForms["Controller"];
            if (existingForm != null)
            {
                existingForm.BringToFront();  // ȭ�� ������ ������
                existingForm.Activate();      // ��Ŀ�� ��
            }
            else
            {

                controller.Show();                // ���޷� ����
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scenario scenario = new Scenario();
            scenario.Name = "Scenario";

            Form existingForm = Application.OpenForms["Scenario"];
            if (existingForm != null)
            {
                existingForm.BringToFront();
                existingForm.Activate();
            }
            else
            {
                scenario.Show();
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------
    }
}

