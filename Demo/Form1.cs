using Core.Errors;
using Core.Lexer;
using Core.Parser;
using Core.Semantic;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var lexer = new Lexer(richTextBox1.Text);

                var parser = new Parser(lexer.Tokenize());

                var analyzer = new SemanticAnalyzer();

                analyzer.Analyze(parser.Parse());
            }
            catch (AvestaException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var source = """
            صفحه {
                عنوان {
                    متن: «سلام دنیا»
                    اندازه: 40
                }

                دکمه {
                    متن: «شروع»
                }
            }
            """;

            richTextBox1.Text = source;
        }
    }
}
