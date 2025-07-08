using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class Error
    {
        /// <summary>
        /// This method is used for write error in file
        /// </summary>
        /// <param name="errorMessage"></param>
        /// Defines the error messages
        public static void GetErroronNotepad(string errorMessage)
        {
            string Messagedata = errorMessage;
            string path = "\\Error\\" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
            // string path = "\\Error\\" + DateTime.Now.Ticks.ToString() + ".txt";
            //  string path = Path.Combine(directory, filename);
            if (!File.Exists(@"C:" + path))
            {
                //this code segment write data to file.
                Directory.CreateDirectory(@"C:" + "//Error");
                FileStream fs = new FileStream(@"C:" + path, FileMode.CreateNew, FileAccess.Write);
                StreamWriter w = new StreamWriter(fs);
                w.WriteLine("{0}", DateTime.Now.ToString());
                string err = "Error log :" + Messagedata;
                w.WriteLine(err);
                w.WriteLine("__________________________");
                w.Flush();
                w.Close();


            }

            else
            {
                FileStream fs = new FileStream(@"C:" + path, FileMode.Append, FileAccess.Write);
                StreamWriter w = new StreamWriter(fs);
                w.WriteLine("{0}", DateTime.Now.ToString());
                string err = "Error log :" + Messagedata;
                w.WriteLine(err);
                w.WriteLine("___________________________");
                w.Flush();
                w.Close();

            }
        }
    }
}