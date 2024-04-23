using OfficeOpenXml;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SpreadsheetApp
{
    public partial class Form1 : Form
    {
        private List<Class1> rows { get; set; }

        public Form1()
        {
            fill_table();
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var rows = this.rows;
            dataGridView1.DataSource = rows;
        }

        private void fill_table() {

            rows = new List<Class1>();
            for (int i = 0; i < 37; i++)
                rows.Add(new Class1() { Acol = "", Bcol = "", Ccol = "", Dcol = "", Ecol = "", Fcol = "", Gcol = "" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = textBox1.Text;
            LoadExcelFile(data);
        }


        private void SaveExcelFile()
        {
            using var package = new ExcelPackage(new FileInfo("spreadsheetApp.xlsx"));
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Add columns to the worksheet
            for (int c = 0; c < dataGridView1.Columns.Count; c++)
            {
                worksheet.Cells[1, c + 1].Value = dataGridView1.Columns[c].HeaderText;
            }

            // Add rows and populate data from DataGridView
            for (int r = 0; r < dataGridView1.Rows.Count; r++)
            {
                for (int c = 0; c < dataGridView1.Columns.Count; c++)
                {
                    worksheet.Cells[r + 2, c + 1].Value = dataGridView1.Rows[r].Cells[c].Value;
                }
            }

            package.Save();
        }
    private void LoadExcelFile(string fileName)
    {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(new FileInfo(String.Concat(fileName, ".xlsx")));
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.End.Row;
            int colCount = worksheet.Dimension.End.Column;

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

        // Add columns to the DataGridView
        for (int c = 1; c <= colCount; c++)
        {
            dataGridView1.Columns.Add("Column" + c, "Column" + c);
        }

        // Add rows and populate data from Excel
        for (int r = 1; r <= rowCount; r++)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView1);

            for (int c = 1; c <= colCount; c++)
            {
                row.Cells[c - 1].Value = worksheet.Cells[r, c].Value.ToString(); 
                }

            dataGridView1.Rows.Add(row);
        }


        
    }

    private void button2_Click(object sender, EventArgs e)
        {
            SaveExcelFile();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell selectedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                textBoxShow.Text = selectedCell.Value.ToString();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}