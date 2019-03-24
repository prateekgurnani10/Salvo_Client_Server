using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace pa8_C00124478_C00012875
{
    public partial class Form1 : Form
    {
        GameGrid grid;
        TcpClient client;
        //StreamReader and Streamwriter read and send guesses from player to player
        private StreamReader reader;
        private StreamWriter writer;
        private String guessSent;
        private String opponentGuess;

        int nextX;  //coordinates to be determined by opponent
        int nextY;  // same

        List<PictureBox> selfposition;
        List<GameGrid.Outcomes> gridList;
        List<PictureBox> opponentposition;

        int totalselfscore = 0;
        int totalopponentscore = 0;

        public Form1()
        {
            InitializeComponent();
            loadbuttons();
            attack.Enabled = true;
            opponentLocationlist.Text = null;
            grid = new GameGrid(5); //creates grid size 5
            
        }

        private void pickposition()
        {
            grid.GenerateShip();
            updateMyGrid();
        }

        //This logic could be useful later

       /* 
        private void attackopponentposition(object sender, EventArgs e)
        {
            if (opponentLocationlist.Text != "")
            {
                
                if(opponentposition[index].Enabled)
                {
                    if(grid.CheckGridSpace(nextX,nextY)==GameGrid.Outcomes.Hit)
                    {
                        opponentposition[index].Enabled = false;
                        opponentposition[index].BackColor = System.Drawing.Color.DarkBlue;
                        totalselfscore++;
                        selfscore.Text = "" + totalselfscore;
                        opponentplaytimer.Start();
                    }
                    else
                    {
                        opponentposition[index].Enabled = false;
                        opponentposition[index].BackColor = System.Drawing.Color.DarkBlue;
                        opponentplaytimer.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose a location from the drop down list");
            }
        }
        
        
        private void opponentpicksposition(object sender, EventArgs e)
        {
            int index = r.Next(opponentposition.Count);

            if(opponentposition[index].Enabled == true && opponentposition[index].Tag == null)
            {
                opponentposition[index].Tag = "opponentship";
                totalopponent--;

                Debug.WriteLine("Opponent position " + opponentposition[index].Text);

           
            }
            else
            {
                index = r.Next(opponentposition.Count);
            }

            if(totalopponent < 1)
            {
                opponentpositionpicker.Stop();
            }
        }

        private void opponentattackself(object sender, EventArgs e)
        {
            if(selfposition.Count > 0)
            {
                int index = r.Next(selfposition.Count);

                if(selfposition[index].Tag == "SelfShip")
                {
                    opponentmoves.Text = "" + selfposition[index].Text;
                    selfposition[index].Enabled = false;
                    selfposition[index].BackColor = System.Drawing.Color.DarkBlue;
                    selfposition.RemoveAt(index);
                    totalopponentscore++;
                    opponentscore.Text = " " + totalopponentscore;
                    opponentplaytimer.Stop();
                     
                }
                else
                {
                    opponentmoves.Text = " " + selfposition[index].Text;
                    selfposition[index].Enabled = false;
                    selfposition[index].BackColor = System.Drawing.Color.DarkBlue;
                    selfposition.RemoveAt(index);
                    opponentplaytimer.Stop();

                }
            }

            if(totalselfscore > 2 || totalopponentscore > 2)
            {
                if(totalselfscore > totalopponentscore)
                {
                    MessageBox.Show("You win", "winning");
                }
                if(totalselfscore == totalopponentscore)
                {
                    MessageBox.Show("its a draw.");
               
                }

                if(totalopponentscore > totalselfscore)
                {
                    MessageBox.Show("you lost boy!");
                }
            }
        }
        */

        private void loadbuttons()
        {
            selfposition = new List<PictureBox> { myA1, myA2, myA3, myA4, myA5, myB1, myB2, myB3, myB4, myB5, myC1, myC2, myC3, myC4, myC5, myD1, myD2, myD3, myD4, myD5, myE1, myE2, myE3, myE4, myE5 }; //Changed buttons to images
            opponentposition = new List<PictureBox> { opA1, opA2, opA3, opA4, opA5, opB1, opB2, opB3, opB4, opB5, opC1, opC2, opC3, opC4, opC5, opD1, opD2, opD3, opD4, opD5, opE1, opE2, opE3, opE4, opE5 }; //Changed buttons to images

            for (int i =0;i < opponentposition.Count; i++)
            {
                opponentLocationlist.Items.Add(opponentposition[i].Name.Substring(2));  //This just takes opA1 and cuts off 'op' for displaying
            }
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void StartServerButton_Click(object sender, EventArgs e)    //Hosts a server at port 6969
        {                                                                   //Based on Frank's example
            
            TcpListener listener = new TcpListener(IPAddress.Any, 6969);
            listener.Start();
            client = listener.AcceptTcpClient();
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync();
        }

        private void ConnectButton_Click(object sender, EventArgs e)    //Starts a client at port 6969
                                                                        //Based on Frank's example
        {

            String ip = UserIP.Text;
            int port;
            int.TryParse(UserPort.Text, out port);

            client = new TcpClient();
            IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(UserIP.Text), 6969);

            try
            {
                client.Connect(IpEnd);
                if (client.Connected)
                {
                    writer = new StreamWriter(client.GetStream());
                    reader = new StreamReader(client.GetStream());
                    writer.AutoFlush = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Converts string input (received from stream) into usable grid coordinates. 
        //nextX and nextY act as static references 
        private void ConvertInputToCoordinates(String input)
        {
            char[] charInput = input.ToCharArray();
            try
            {
                char x = charInput[0];
                nextY = charInput[1]-1;
            }
            catch (Exception e)
            {
                label27.Text = "Input Error";
            }

            switch(charInput[0])
            {
                case 'A':
                    nextX = 0;
                    break;

                case 'B':
                    nextX = 1;
                    break;

                case 'C':
                    nextX = 2;
                    break;
                case 'D':
                    nextX = 3;
                    break;
                case 'E':
                    nextX = 4;
                    break;
                default:
                    nextX = 0;
                    break;
            }
            
        }

        //Background worker component on form, based on Frank's "improved server/client GUI" on moodle. WIP
        private void backgroundWorker1_dowork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    opponentGuess = reader.ReadLine();
                    //TODO: Send guess to other program
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //When attack, Send Data to the writer, later to be received by client
        private void attack_Click(object sender, EventArgs e)
        {
            if (opponentLocationlist.Text != "")
            {
                guessSent = opponentLocationlist.Text;
                if (guessSent != null)
                {
                    SendData(guessSent);
                }
            }
        }

        //Send data to Stream whenever it is availible
        private async void SendData(String data)
        {
            await writer.WriteLineAsync(data);  //Code breaks here
        }

        //Generates a ship. Left for demonstration/debug (This will later be done when you click start server/connect to client button)
        private void button1_Click(object sender, EventArgs e)
        {
            pickposition();
        }

        //Updates the grid colors on request
        private void updateMyGrid()
        {
            gridList = grid.toList();
            for (int i = 0; i < selfposition.Count; i++)    //Depends on order of input.
            {
                if (gridList[i] == GameGrid.Outcomes.Occupied)
                    selfposition[i].BackColor = Color.Blue;

                if (gridList[i] == GameGrid.Outcomes.Hit)
                    selfposition[i].BackColor = Color.Red;

                if (gridList[i] == GameGrid.Outcomes.Miss)
                    selfposition[i].BackColor = Color.White;

                if (gridList[i] == GameGrid.Outcomes.Empty)
                    selfposition[i].BackColor = Color.Gray;

            }
        }
        private void attackSelf()
        {

            //TODO: get guess from client or server
            grid.TakeShot(nextX, nextY);
            updateMyGrid();
        }
    }
}
