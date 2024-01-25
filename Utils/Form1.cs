namespace Utils
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void e1_TextChanged(object sender, EventArgs e)
        {
            string newText = e1.Text;
            try
            {
                if (!string.IsNullOrEmpty(newText))
                {
                    string encryptedText = EncryptionHelper.Encrypt(newText);

                    // Check if the encrypted text is different from the current text to avoid unnecessary updates
                    if (e2.Text != encryptedText)
                    {
                        e2.Text = encryptedText;
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Invalid Base64 string: " + ex.Message);
            }

        }

        private void d1_TextChanged(object sender, EventArgs e)
        {
            string newText = d1.Text;
            try
            {
                if (!string.IsNullOrEmpty(newText))
                {
                    string decryptedText = EncryptionHelper.Decrypt(newText);

                    // Check if the encrypted text is different from the current text to avoid unnecessary updates
                    if (d2.Text != decryptedText)
                    {
                        d2.Text = decryptedText;
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Invalid Base64 string: " + ex.Message);
            }
        }

        private void dpaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                d1.Text = Clipboard.GetText();
            }
        }

        private void epaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                e1.Text = Clipboard.GetText();
            }
        }

        private void ecopy_Click(object sender, EventArgs e)
        {
            string textToCopy = e2.Text;

            if (!string.IsNullOrEmpty(textToCopy))
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => Clipboard.SetText(textToCopy)));
                }
                else
                {
                    Clipboard.SetText(textToCopy);
                }
            }
        }

        private void dcopy_Click(object sender, EventArgs e)
        {
            string textToCopy = d2.Text;

            if (!string.IsNullOrEmpty(textToCopy))
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => Clipboard.SetText(textToCopy)));
                }
                else
                {
                    Clipboard.SetText(textToCopy);
                }
            }
        }
    }
}