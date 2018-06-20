using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PROJEKT_CHAT1_SERVER.Logs;
using PROJEKT_CHAT1_SERVER.Cryptography;

namespace PROJEKT_CHAT1_SERVER
{
    static class Program
    {
        static Socket socketServer = null;
        static IPAddress addressIP = null;
        static IPEndPoint iPEndPoint = null;

        static Dictionary<string, Socket> clientSockets = null;

        static Form1 mainForm = null;
        static SaveLogs saveLogs = new SaveLogs();
        static Crypto crypto = new Crypto();

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            clientSockets = new Dictionary<string, Socket>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Form1(buttonStartClick, buttonSendClieck, buttonStopClick);
            Application.Run(mainForm);
        }

        static EventHandler buttonStartClick = Listenning;
        static EventHandler buttonSendClieck = SendMessage;
        static EventHandler buttonStopClick = StopWork;

        static void Listenning(object sender, EventArgs e)
        {
            addressIP = IPAddress.Parse(mainForm.GetIPString());
            iPEndPoint = new IPEndPoint(addressIP, mainForm.GetPortNumber());

            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socketServer.Bind(iPEndPoint);
                socketServer.Listen(100);
                saveLogs.WriteLine("----------------------------");
                PrintlnAndSave($"Włączono serwer pod adresem: {iPEndPoint}");
                mainForm.SetStartEnabled(false);
                mainForm.SetStopEnabled(true);

                Thread thread = new Thread(Listen);
                thread.IsBackground = true;
                thread.Start(socketServer);
            }
            catch (Exception ex)
            {
                PrintlnAndSave($"Błąd: {ex.Message}");
            }
        }

        static void StopWork(object sender, EventArgs e)
        {
            try
            {
                socketServer.Close();
                mainForm.SetStartEnabled(true);
                mainForm.SetStopEnabled(false);
                PrintlnAndSave($"Serwer wyłączono");
            }
            catch (Exception ex)
            {
                PrintlnAndSave($"Błąd: {ex.Message}");
            }
        }

        static void Listen(object obj)
        {
            Socket socketServer = obj as Socket;
            while (true)
            {
                try
                {
                    // Czekanie na połączenie
                    Socket socketClient = socketServer.Accept();

                    // Pozyskiwanie adresu IP
                    string pointClient = socketClient.RemoteEndPoint.ToString();
                    PrintlnAndSave($"{pointClient} klient połączony");

                    clientSockets.Add(pointClient, socketClient);
                    mainForm.UsersListAddItem(pointClient);

                    // Otwarcie nowego wątku i pozyskanie wiaodmości
                    Thread thread = new Thread(Receive);
                    thread.IsBackground = true;
                    thread.Start(socketClient);
                }
                catch (Exception ex)
                {
                    PrintlnAndSave($"Błąd: {ex.Message}");
                    break;
                }
            }
        }

        static void Receive(object obj)
        {
            Socket socketClient = obj as Socket;
            string pointClient = socketClient.RemoteEndPoint.ToString();
            while (true)
            {
                try
                {
                    // Uzyskanie wysłanego kontenera wiaodmosci
                    byte[] buffor = new byte[1024 * 1024 * 2];
                    int len = socketClient.Receive(buffor);

                    // pomijanie bajtów pustych
                    if (len == 0)
                    {
                        break;
                    }

                    string encodedStr = Encoding.UTF8.GetString(buffor, 0, len);
                    string str = crypto.Base64Decode(encodedStr);

                    PrintlnAndSave($"{pointClient}: {str}");

                    foreach (Socket s in clientSockets.Values)
                    {
                        byte[] sendBytes = Encoding.UTF8.GetBytes(crypto.Base64Encode($"{pointClient}: {str}"));
                        s.Send(sendBytes);
                    }
                }
                catch (SocketException ex)
                {
                    clientSockets.Remove(pointClient);
                    mainForm.UserListRemoveItem(pointClient);

                    PrintlnAndSave($"Klient {socketClient.RemoteEndPoint} przerwał połączenie: {ex.Message}");
                    socketClient.Close();
                    break;
                }
                catch (Exception ex)
                {                  
                    PrintlnAndSave($"Błąd: {ex.Message}");
                }
            }
        }

        static void SendMessage(object sender, EventArgs e)
        {
            string message = mainForm.GetMessageString();
            if (message == "")
            {
                return;
            }
            byte[] sendBytes = Encoding.UTF8.GetBytes(crypto.Base64Encode($"Serwer: {message}"));
            foreach (Socket s in clientSockets.Values)
            {
                s.Send(sendBytes);
            }
            PrintlnAndSave($"Server: " + message);
            mainForm.ClearMessageText();
        }

        static void PrintlnAndSave(string str)
        {
            mainForm.Println(str);
            saveLogs.WriteLine(str);
        }

        
    }
}
