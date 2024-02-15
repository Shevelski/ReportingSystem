using Microsoft.VisualBasic.ApplicationServices;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace Utils
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("test");
        }

        private void e1_TextChanged(object sender, EventArgs e)
        {
            string newText = e1.Text;
            try
            {
                if (!string.IsNullOrEmpty(newText))
                {
                    string encryptedText = EncryptionHelper.Encrypt(newText);

                    if (e2.Text != encryptedText)
                    {
                        e2.Text = encryptedText;
                    }
                }
            }
            catch (Exception)
            {
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

                    if (d2.Text != decryptedText)
                    {
                        d2.Text = decryptedText;
                    }
                }
            }
            catch (Exception)
            {
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

        private void EUDeveloper_mail_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUDeveloper_mail.Text;

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

        private void EUDeveloper_password_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUDeveloper_password.Text;

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

        private void EUDevAdministrator_mail_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUDevAdministrator_mail.Text;

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

        private void EUDevAdministrator_password_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUDevAdministrator_mail.Text;

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

        private void EUCustomer_mail_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUCustomer_mail.Text;

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

        private void EUCustomer_password_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUCustomer_mail.Text;

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

        private void EUCEO_mail_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUCEO_mail.Text;

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

        private void EUCEO_password_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUCEO_mail.Text;

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

        private void EUAdministrator_mail_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUAdministrator_mail.Text;

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

        private void EUAdministrator_password_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUAdministrator_mail.Text;

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

        private void EUProjectManager_mail_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUProjectManager_mail.Text;

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

        private void EUProjectManager_password_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUProjectManager_mail.Text;

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

        private void EUUser_mail_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUUser_mail.Text;

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

        private void EUUser_password_copy_Click(object sender, EventArgs e)
        {
            string textToCopy = EUUser_mail.Text;

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


        private async void Get1_Click(object sender, EventArgs e)
        {

            List<string?>? data = await new SQLRead().GetData();

            EUDeveloper_mail.Text = data[0];
            EUDeveloper_password.Text = data[1];

            EUDevAdministrator_mail.Text = data[2];
            EUDevAdministrator_password.Text = data[3];

            EUCustomer_mail.Text = data[4];
            EUCustomer_password.Text = data[5];

            EUCEO_mail.Text = data[6];
            EUCEO_password.Text = data[7];

            EUAdministrator_mail.Text = data[8];
            EUAdministrator_password.Text = data[9];

            EUProjectManager_mail.Text = data[10];
            EUProjectManager_password.Text = data[11];

            EUUser_mail.Text = data[12];
            EUUser_password.Text = data[13];

        }
    }
}