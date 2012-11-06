﻿//#define EXCEPTION
//#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnablePluginFF;
using System.Runtime.InteropServices;



namespace EnablePluginTest
{

    class Program
    {

       
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(" The Program should be run as program Name Extension name logfilepath");
                
            }
            else
            {

                string extension = args[0];
                string logfile = args[1];
                //To Test this, set function getFFExepath() in EnablePlugin.cs to public
                //testFFinstallation();

                testGetHandle(extension, logfile);
            }

        }



        //Function to test the function which finds the FF installation folder
        private static void testFFinstallation()
        {
            EnablePlugin obj = new EnablePlugin();

            //To Test this, set function getFFExepath() in EnablePlugin.cs to public and uncomment the below line
            //obj.getFFExepath();

            Console.ReadLine();
        }

        //This functions has the Actual Algorithm 
        private static void testGetHandle(string extension, string logfile)
        {

            EnablePlugin obj = new EnablePlugin();

            IntPtr handle;

            /*The actual algorithm is here.*/
#if(!TEST)
            {
                ///Get the handle for FF : getHandle() function
                /// 1. If there is no instance of FF running on the system, it runs FF and returns the handle of that instance.
                /// 2. If there are more than 1 instance of FF running on the system, then it returns the handle of the last FF which is iterated by Enumwindows
                /// 3. If there is firefox Browser and Thunderbird both running, then it returns the handle of FF
                /// 4. if there are more than 1 instance of FF browser and Thunderbird running, then it returns the FF of the last FF which is iterated by Enumwindows
                /// 5. if FF is not running and we are not able to run it(i.e. it is not installed), it throws an exception saying that FF not installed
                handle = obj.getHandle();
                

                /*This is to log if we get handle zero even after all the checks are done*/
                //Console.WriteLine("The handle is : " + handle);
                if (handle == IntPtr.Zero)
                {
#if (EXCEPTION)
                {
                    Exception ex = new Exception("The Handle is zero in main program");
                    throw ex;
                }
#else
                    {
                        Console.WriteLine("The Handle is zero in main program");
                    }
#endif
                }
                else
                {
                    Console.WriteLine("Found Handle for Firefox");
                    Console.WriteLine("Done");
                }
                AccessibleCode acobj = new AccessibleCode(handle);

                //SET THE EXTENSION NAME HERE, now we are setting it through the command line
                //const string ext = "XTalk 2.36 (disabled) An Extension for HearSay"
               
                //const string ext = "XTalk";
                acobj.doAccessibleHandle(extension,logfile);
              
                
# if(TEST)
                //For testing whether we got the right handle
                //SetWindowText(handle, "Found it ");
                //obj.closeWindow(handle);
#endif
            }
#endif




#if(TEST)
            //TEST CASES:
            //1. One instance of Firefox Open - so gets 1 handle

            //2. 2 instance open - so we need to get the right thing



            //3. No instance of FF open, so we need to open FF

#endif
            //Close the window
            obj.closeWindow(handle);
        }
    }
}
