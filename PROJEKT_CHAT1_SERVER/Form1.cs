using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace PROJEKT_CHAT1_SERVER
{
    public partial class Form1 : Form
    {
        
        public Form1(EventHandler buttonStartClick, EventHandler buttonSendClick)
        {
            InitializeComponent();
            this.buttonStart.Click += buttonStartClick;
            this.buttonSend.Click += buttonSendClick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public string GetIPString()
        {
            return this.textAdresIP.Text;
        }

        public int GetPortNumber()
        {
            return (int)this.numericPort.Value;
        }

        public string GetMessageString()
        {
            return this.textMessage.Text.Trim();
            // Trim() usuwa wszystki białe znaki na początku i na końcu
        }

        public void ClearMessageText()
        {
            this.textMessage.Clear();
        }

        delegate void VoidString(string str);
        
        public void Println(string str)
        {
            if (this.textMessage.InvokeRequired)
            {
                VoidString println = Println;
                this.textMessage.Invoke(println, str);
            }
            else
            {
                this.textMessage.AppendText(str + Environment.NewLine);
            }
        }

        public void UsersListAddItem(string str)
        {
            if (this.listBoxUsers.InvokeRequired)
            {
                VoidString listAddItem = UsersListAddItem;
                this.textMessage.Invoke(listAddItem, str);
            }
            else
            {
                this.listBoxUsers.Items.Add(str);
            }
        }

        public void UserListRemoveItem(string str)
        {
            if (this.listBoxUsers.InvokeRequired)
            {
                VoidString listRemoveItem = UserListRemoveItem;
                this.textMessage.Invoke(listRemoveItem, str);
            }
            else
            {
                this.listBoxUsers.Items.Remove(str);
            }
        }
    }
}
