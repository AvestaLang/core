using Core.Lexer;

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

            var lexer = Lexer.Tokenize(source);
        }
    }
}
