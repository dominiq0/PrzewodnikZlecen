using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PrzewodnikZlecen
{
    internal class DaneZBloob
    {
    
        public BitmapImage GetPictB(System.Data.SQLite.SQLiteDataReader reader, int ordinal)
            {
            byte[] result = null;
            
            reader.Read();
            long size = reader.GetBytes(ordinal, 0, null, 0, 0); //get the length of data 
            result = new byte[size];
            int bufferSize = size > int.MaxValue ? int.MaxValue : (int)size;
            // int bufferSize = 1024;
            long bytesRead = 0;
            int curPos = 0;
            while (bytesRead < size)
            {
                bytesRead += reader.GetBytes(ordinal, curPos, result, curPos, bufferSize);
                curPos += bufferSize;
            }
            BitmapImage _bitmapImage = new BitmapImage();
           
            _bitmapImage.BeginInit();
            _bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            _bitmapImage.StreamSource = new MemoryStream(result);
            _bitmapImage.EndInit();

            return _bitmapImage;

        }

        public byte[] GetRawdata(System.Data.SQLite.SQLiteDataReader reader)
        {
            
            FileStream fs;                          // Writes the BLOB to a file (*.bmp).
            BinaryWriter bw;                        // Streams the BLOB to the FileStream object.
            int bufferSize = 100;                   // Size of the BLOB buffer.
            byte[] outbyte = new byte[bufferSize];  // The BLOB byte[] buffer to be filled by GetBytes.

            outbyte = null;

            long retval;                            // The bytes returned from GetBytes.
            long startIndex = 0;                    // The starting position in the BLOB output.
            string emp_id = "";


            while (reader.Read())
            {
               // retval = null;

                fs = new FileStream("employee" + emp_id + ".bmp",
                                    FileMode.OpenOrCreate, FileAccess.Write);
                bw = new BinaryWriter(fs);
                startIndex = 0;
                retval = reader.GetBytes(0, startIndex, outbyte, 0, bufferSize);

                // Continue reading and writing while there are bytes beyond the size of the buffer.
                while (retval == bufferSize)
                {
                    bw.Write(outbyte);
                    bw.Flush();

                    // Reposition the start index to the end of the last buffer and fill the buffer.
                    startIndex += bufferSize;
                    retval = reader.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                }

                // Write the remaining buffer.
                bw.Write(outbyte, 0, (int)retval);
                bw.Flush();

                // Close the output file.
                bw.Close();
                fs.Close();

                

            }
            return outbyte;
        }


    }
}
