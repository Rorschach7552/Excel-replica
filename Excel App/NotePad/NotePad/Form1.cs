using System.IO;
namespace NotePad
{
    public partial class Form1 : Form
{
    private OpenFileDialog openFileDialog;
    private SaveFileDialog saveFileDialog;

    public Form1()
    {
        InitializeComponent();
    }

    private void SaveFile()
    {
        try
        {
            this.saveFileDialog = new SaveFileDialog();
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(this.saveFileDialog.FileName, this.richTextBox1.Text);
            }
        }
        catch (Exception)
        {
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }

    private void LoadText(TextReader sr)
    {
        this.richTextBox1.Text = sr.ReadToEnd();
    }

    private void LoadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.openFileDialog = new OpenFileDialog();
        if (this.openFileDialog.ShowDialog() == DialogResult.OK)
        {
            StreamReader sr = new(this.openFileDialog.FileName);
            this.richTextBox1.Text = sr.ReadToEnd().ToString();

            // LoadText(sr);
            sr.Close();
        }
    }

    private void LoadFibonacciNumbersFirst50ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FibonacciTextReader fib = new(50);
        this.LoadText(fib);
    }

    private void LoadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FibonacciTextReader fib = new(100);
        this.LoadText(fib);
    }

    private void SaveToFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.SaveFile();
    }
}
}