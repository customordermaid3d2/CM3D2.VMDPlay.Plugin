using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;
using System.IO;

namespace COM3D2.Lilly.Plugin
{
    public class MyLog 
    {

        static ManualLogSource log = BepInEx.Logging.Logger.CreateLogSource("VMDPlay");

        public MyLog()
        {

        }

        public static void LogTest()
        {
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.ForegroundColor = color;
                //Console.Clear();// 로그 지워짐
                Console.WriteLine("Foreground color set to {0}", color);
            }
            Console.WriteLine("=====================================");
            Console.ForegroundColor = ConsoleColor.White;
            // Let's go through all Console colors and set them as background  
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.BackgroundColor = color;
                Console.WriteLine("Background color set to {0}", color);
            }
            Console.WriteLine("=====================================");
            Console.ResetColor();
        }

        public static void LogLine()
        {
            Console.ForegroundColor = ConsoleColor.White;
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.BackgroundColor = color;
                Console.Write("=== {0} ===", color);
            }
            Console.WriteLine();
            Console.ResetColor();
        }


        private static void LogOut(object[] args, Action<string> action)
        {
            //if (!Lilly.isLogOn)
            //    return;
            action(MyUtill.Join(" , ", args));
        }

        private static void ConsoleOut(object[] args, ConsoleColor consoleColor)
        {

            Console.BackgroundColor = consoleColor;
            Console.WriteLine(MyUtill.Join(" , ", args));
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.ResetColor();            
        }
        

        private static void ConsoleOut(object[] args)
        {

            Console.WriteLine(MyUtill.Join(" , ", args));
            //Console.ResetColor();
        }

        internal static void LogDebug(params object[] args)
        {
            LogOut(args, log.LogDebug);
           // ConsoleOut(args);
        }

        internal static void LogInfo(params object[] args)
        {
            LogOut(args, log.LogInfo);
            //ConsoleOut(args, ConsoleColor.DarkGray);
        }

        internal static void LogMessage(params object[] args)
        {
            LogOut(args, log.LogMessage);
           // ConsoleOut(args, ConsoleColor.DarkBlue);
        }
        
        internal static void LogDarkMagenta(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkMagenta);
        }        
        internal static void LogDarkYellow(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkYellow);
        }
                
        internal static void LogBlue(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.Blue);
        }
                                        
        internal static void LogDarkRed(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkRed);
        }
                                   
        internal static void LogDarkBlue(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkBlue);
        }
                        
        internal static void Log(ConsoleColor consoleColor ,params object[] args)
        {
            ConsoleOut(args, consoleColor);
        }                        

        internal static void Log( params object[] args)
        {
            ConsoleOut(args);
        }

        internal static void LogWarning(params object[] args)
        {
            LogOut(args, log.LogWarning);
            //ConsoleOut(args, ConsoleColor.DarkYellow);
        }

        internal static void LogFatal(params object[] args)
        {
            LogOut(args, log.LogFatal);
            //ConsoleOut(args, ConsoleColor.Red);
        }

        internal static void LogError(params object[] args)
        {
            LogOut(args, log.LogError);
            //ConsoleOut(args, ConsoleColor.DarkRed);
        }

    }



}
