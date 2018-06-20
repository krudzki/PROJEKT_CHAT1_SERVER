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

        public Form1(EventHandler buttonStartClick, EventHandler buttonSendClick, EventHandler buttonStopClick)
        {
            InitializeComponent();
            this.buttonStart.Click += buttonStartClick;
            this.buttonSend.Click += buttonSendClick;
            this.buttonStop.Click += buttonStopClick;
        }

        public Form1() { }

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
            if (this.textLog.InvokeRequired)
            {
                VoidString println = Println;
                this.textLog.Invoke(println, str);
            }
            else
            {
                this.textLog.AppendText(str + Environment.NewLine);
            }
        }

        public void UsersListAddItem(string str)
        {
            if (this.listBoxUsers.InvokeRequired)
            {
                VoidString listAddItem = UsersListAddItem;
                this.textLog.Invoke(listAddItem, str);
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
                this.textLog.Invoke(listRemoveItem, str);
            }
            else
            {
                this.listBoxUsers.Items.Remove(str);
            }
        }

        delegate void VoidBool(bool bo);

        public void SetStartEnabled(bool enabled)
        {
            if (this.buttonStart.InvokeRequired)
            {
                VoidBool sse = SetStartEnabled;
                this.textLog.Invoke(sse, enabled);
            }
            else
            {
                this.buttonStart.Enabled = enabled;
            }
        }

        public void SetStopEnabled(bool enabled)
        {
            if (this.buttonStop.InvokeRequired)
            {
                VoidBool sse = SetStopEnabled;
                this.textLog.Invoke(sse, enabled);
            }
            else
            {
                this.buttonStop.Enabled = enabled;
            }
        }
    }
}
