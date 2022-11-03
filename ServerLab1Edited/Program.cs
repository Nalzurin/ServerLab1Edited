using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ServerLab1Edited
{
    internal class Program
    {
        public class ServerProgram
        {
            const int SIO_UDP_CONNRESET = -1744830452;
            const int port = 1984;

            public static int SizeCheck(Int16 x, Int16 y, Int16 Width, Int16 Height)
            {
                if (x > Width || y > Height || x < -Width || y < -Height)
                {
                    return 1;

                }
                else
                {
                    return 0;
                }


            }

            public MessageData DecodeMessage(byte[] RecievedData)
            {
                byte command;
                string errortext = "Recieved Data Error: Wrong Size";
                if (RecievedData.Length > 0)
                {
                    command = RecievedData[0];
                    switch (command)
                    {
                        case 1:
                            if(RecievedData.Length == 4)
                            {
                                Console.WriteLine("command:Clear Display");
                                return ClearDisplay(RecievedData);
                            }
                            else
                            {
                                Console.WriteLine(errortext);
                                return null;
                            }


                        case 2:
                            if(RecievedData.Length == 8)
                            {
                                Console.WriteLine("command:Draw Pixel");
                                return ThreeVarsDecode(RecievedData);
                            }
                            else
                            {
                                Console.WriteLine(errortext);
                                return null;
                            }
                        case 3:
                            if (RecievedData.Length == 12)
                            {
                                Console.WriteLine("command:Draw Line");
                                return FiveVarsDecode(RecievedData);
                            }
                            else
                            {
                                Console.WriteLine(errortext);
                                return null;
                            }

                        case 4:
                            if (RecievedData.Length == 12)
                            {
                                Console.WriteLine("command:Draw Rectangle");
                                return FiveVarsDecode(RecievedData);
                            }
                            else
                            {
                                Console.WriteLine(errortext);
                                return null;
                            }

                        case 5:
                            if (RecievedData.Length == 12)
                            {
                                Console.WriteLine("command:Fill Rectangle");

                                return FiveVarsDecode(RecievedData);
                            }
                            else
                            {
                                Console.WriteLine(errortext);
                                return null;
                            }

                        case 6:
                            if (RecievedData.Length == 12)
                            {
                                Console.WriteLine("command:Draw Ellipse");

                                return FiveVarsDecode(RecievedData);
                            }
                            else
                            {
                                Console.WriteLine(errortext);
                                return null;
                            }

                        case 7:
                            if (RecievedData.Length == 12)
                            {
                                Console.WriteLine("command:Fill Ellipse");

                                return FiveVarsDecode(RecievedData);
                            }
                            else
                            {
                                Console.WriteLine(errortext);
                                return null;
                            }



                        default:
                            Console.WriteLine("Recieved Data Error: Command doesnt exist!");
                            return null;
                    }
                }
                else
                {
                    Console.WriteLine("Recieved Data Error: Empty Message!");
                    return null;
                }


            }
            public static void SetScreenSize(byte[] RecievedData, out Int16 width, out Int16 height)
            {
                int val1place = 1;
                byte[] transfer = new byte[2];
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                width = BitConverter.ToInt16(transfer, 0);
                val1place += 2;
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                height = BitConverter.ToInt16(transfer, 0);
            }
            public static MessageData ClearDisplay(byte[] RecievedData)
            {

                int val1place = 1;
                byte[] RGB = new byte[3];
                Array.Copy(RecievedData, val1place, RGB, 0, RGB.Length);
                return new MessageData(RGB);
                
            }
            public static MessageData ThreeVarsDecode(byte[] RecievedData)
            {
                byte[] transfer;
                int val1place = 1;
                transfer = new byte[2];
                byte[] RGB = new byte[3];
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                Int16 val1 = BitConverter.ToInt16(transfer, 0);
                
                val1place += 2;
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                Int16 val2 = BitConverter.ToInt16(transfer, 0);
                val1place += 2;
                Array.Copy(RecievedData, val1place, RGB, 0, RGB.Length);
                return new MessageData(val1, val2, RGB);
            }
            public static MessageData FiveVarsDecode(byte[] RecievedData)
            {
                byte[] transfer;
                int val1place = 1;
                transfer = new byte[2];
                byte[] RGB = new byte[3];
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                Int16 val1 = BitConverter.ToInt16(transfer, 0);
                val1place += 2;
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                Int16 val2 = BitConverter.ToInt16(transfer, 0);
                val1place += 2;
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                Int16 val3 = BitConverter.ToInt16(transfer, 0);
                val1place += 2;
                Array.Copy(RecievedData, val1place, transfer, 0, transfer.Length);
                Int16 val4 = BitConverter.ToInt16(transfer, 0);
                val1place += 2;
                Array.Copy(RecievedData, val1place, RGB, 0, RGB.Length);
                return new MessageData(val1, val2, val3, val4, RGB);
            }
        }
        public class MessageData
        {
            public Int16 x0, x1, y0, y1, width = 0, height = 0; public byte[] RGB;
            public MessageData(Byte[] _RGB)
            {
                this.RGB = _RGB;
            }
            public MessageData(Int16 _x0, Int16 _y0, Byte[] _RGB)
            {
                this.RGB = _RGB;
                this.x0 = _x0;
                this.y0 = _y0;
            }
            public MessageData(Int16 _x0, Int16 _y0, Int16 _x1, Int16 _y1, Byte[] _RGB)
            {
                this.RGB = _RGB;
                this.x0 = _x0;
                this.y0 = _y0;
                this.x1 = _x1;
                this.y1 = _y1;
            }
        }
        static void Main(string[] args)
        {
            ServerProgram sp = new ServerProgram();
            byte[] Message1 = { 1, 255, 255, 255 };
            byte[] Message2 = { 2, 10, 0, 250, 0, 255, 255, 255 };
            byte[] Message3 = { 4, 10, 0, 250, 0, 50, 0, 150, 0, 255, 255, 255};
            byte[] Message4 = { };
            byte[] Message5 = { 4, 10, 0, 250, 0, 50, 0, 150, 0, 255, 255, 255, 10 };
            MessageData MessageData = sp.DecodeMessage(Message1);
            if(MessageData != null)
            {
                Console.WriteLine($"RGB Values: {MessageData.RGB[0]} {MessageData.RGB[1]} {MessageData.RGB[2]}");
            }
            
            MessageData MessageData1 = sp.DecodeMessage(Message2);
            if(MessageData1 != null)
            {
                Console.WriteLine($"X1: {MessageData1.x0}, Y1: {MessageData1.y0} RGB Values: {MessageData1.RGB[0]} {MessageData1.RGB[1]} {MessageData1.RGB[2]}");
            }
            
            MessageData MessageData2 = sp.DecodeMessage(Message3);
            if(MessageData2 != null)
            {
                Console.WriteLine($"Coordinates: x1 = {MessageData2.x0}, y1 = {MessageData2.y0}, x2 = {MessageData2.x1}, y2 = {MessageData2.y1}, color: Red = {MessageData2.RGB[0]}, Green = {MessageData2.RGB[1]}, Blue = {MessageData2.RGB[2]}");
            }

            MessageData MessageData4 = sp.DecodeMessage(Message5);
            if(MessageData4 != null)
            {
                Console.WriteLine($"RGB Values: {MessageData4.RGB[0]} {MessageData4.RGB[1]} {MessageData4.RGB[2]}");
            }
            MessageData MessageData3 = sp.DecodeMessage(Message4);
            if(MessageData3 != null)
            {
                Console.WriteLine($"RGB Values: {MessageData3.RGB[0]} {MessageData3.RGB[1]} {MessageData3.RGB[2]}");
            }
            
        }
    }
}

