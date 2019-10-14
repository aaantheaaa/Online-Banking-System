using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace onlineBankingSystem
{
    public enum Border
    {
        TOP,BOT,MID,TEXT
    }

    public class Bordering
    {
        public static int[,] pos = new int[10,2];
        private readonly static int length = 60;
        public static void Draw(Border borderType, string text = "",bool isMiddle = false, int index = -1)
        {
            char[] edge = new char[2];
            switch (borderType)
            {
                case Border.TOP:
                    edge[0] = '╔';
                    edge[1] = '╗';
                    break;
                case Border.BOT:
                    edge[0] = '╚';
                    edge[1] = '╝';
                    break;
                case Border.MID:
                    edge[0] = '╠';
                    edge[1] = '╣';
                    break;
                case Border.TEXT:
                    edge[0] = '║';
                    edge[1] = '║';
                    break;
                default:
                    break;
            }
            drawLine(edge,text,isMiddle,index);
        }

        private static void drawLine(char[] edge,String text,bool isMiddle,int index)
        {
            string padding = "";
            //int space = length - padding.Length - text.Length;
            if (text == "")
            {
                Console.Write(edge[0]);
                for (int i = 0; i < length; i++)
                    Console.Write('═');
                Console.WriteLine(edge[1]);
                if (index >= 0)
                    savePos(index);
            }
            else
            {
                if (isMiddle)
                {
                    for (int i =0; i<(length - text.Length)/2; i++)
                        padding += " ";
                }
                else {
                    padding += "       ";
                }
                
                Console.Write(edge[0]);
                Console.Write(padding + text);
                if(index >= 0)
                savePos(index);
                for (int i = 0; i < length - padding.Length - text.Length ; i++)
                    Console.Write(' ');
                Console.WriteLine(edge[1]);
            }
            
        }

        public static void savePos(int index)
        {
            pos[index, 0] = Console.CursorLeft;
            pos[index, 1] = Console.CursorTop;
                   
        }

        public static void setPos(int index)
        {
            Console.SetCursorPosition(pos[index, 0],pos[index,1]);
        }
        public static int[] getPos(int index)
        {
            int[] list = new int[2];
            list[0] = pos[index, 0];
            list[1] = pos[index, 1];
            return list;
        }
        //public static void ErrorMsg
    }
}
