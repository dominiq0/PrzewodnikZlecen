
using System;
using Ookii.Dialogs.WinForms;
using System.Data;
using System.Text;
using System.Windows;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.IO.Ports;
using System.Threading;
using System.Data.SQLite;
using System.Collections.Generic;



namespace PrzewodnikZlecen
{
	public class ZakresDanych
	{
		
	//	SQL_mg Qm = new SQL_mg();
		string path="",dataZakr = "5";
		FileInfo pathFile2;
		public static List<string> FileListpath = new List<string> ();

	

		public void  ZakDanych()
		{
			try {
			
				
			//	Qm.queryB = "SELECT*\r\nFROM Settings";
		//		Qm.ExecuteQuery();
				DataTable dt2 = new DataTable();
                //		dt2 = Qm.DT.Copy();

                /////	path =	dt2.Rows[12]["Wartosc"].ToString();//Axel4
                ////	dataZakr = dt2.Rows[13]["Wartosc"].ToString();
                string currentDirName = System.IO.Directory.GetCurrentDirectory();
                path = currentDirName+"\\METY 15.11.2022 dane";

				dataZakr = "10";


                dataZakr =Convert.ToString(Convert.ToDouble(dataZakr)+1);
		//		bool progg = Convert.ToBoolean(Qm.DT.Rows[15]["Wartosc"]);
				
				int mdni = -Math.Abs(Convert.ToInt32(dataZakr));
				string dataC = DateTime.Now.ToString("yyy.MM.dd");
				string dt = DateTime.Now.AddDays(mdni).ToString("yyy.MM.dd");
				
				
				
				if(DateTime.Now.AddDays(mdni)<=DateTime.Now)
					
				{
					int ss = 1;
				}
	
				VistaFolderBrowserDialog OFOLDer2 = new VistaFolderBrowserDialog();
			
				OFOLDer2.SelectedPath = path;
				var dirInfo = new DirectoryInfo(path);
				var file2 = (from f in dirInfo.GetDirectories()
            	orderby f.CreationTime descending
				             select f);

				int countD = 0;
				
                VistaOpenFileDialog OpF = new VistaOpenFileDialog();
				
				foreach (var sd in file2) {
					
					
					if (sd.CreationTime >= DateTime.Now.AddDays(mdni)) {
						
						string[] fileEntries = Directory.GetFiles(sd.FullName.ToString());
						try {
							foreach (string pathFile in fileEntries) {
								FileInfo fin = new FileInfo(pathFile);
								
							if (fin.Extension == ".MET") {


                                    pathFile2 = new FileInfo(pathFile);

                                    FileListpath.Add(pathFile);

                                    string nazwa = pathFile2.Name;
							//	if (progg == true && (!nazwa.Contains("strop") & !nazwa.Contains("progi") & !nazwa.Contains("PRO") & !nazwa.Contains("STROP"))) {
							//		FileListpath.Add(pathFile);
							//	}
							//	if(progg == false){FileListpath.Add(pathFile);}

								
								}
							}
						} catch {
						}
					}
				}
				
				
				countD = 0;
				DirectoryInfo info = new DirectoryInfo(path);
				var files2 = (from f in info.GetFiles()orderby f.CreationTime descending
				              select f);
				foreach (FileInfo filesa in files2) {
					
					if (filesa.CreationTime >= DateTime.Now.AddDays(mdni)) {
						if (filesa.Extension == ".MET") {
						
							string nazwa =filesa.Name;
						//	if (progg == true && (!nazwa.Contains("strop") & !nazwa.Contains("progi") & !nazwa.Contains("PRO") & !nazwa.Contains("STROP"))) {
								
						//	string fileEntries = filesa.FullName.ToString();
						//	FileListpath.Add(fileEntries);
							
						//	}
							
						//	if(progg == false){
								string fileEntries = filesa.FullName.ToString();
							FileListpath.Add(fileEntries);
							
							
						//	}
					
						}
				 
					}
				}

			} catch {
				if(path =="Ścieżka folderu zleceń"){}
				
				else{
					System.Windows.MessageBox.Show("Nieprawidłowy, lub brak wskazanego folderu z planami, wybierz prawidłowy i zresetuj program");
				}
			}	
			
		}
		
		
		public void ZakZlece(string bcode)
		{
			foreach( string p in FileListpath){
			
			
			}
			string hh = bcode;
		
		}
		
		
	}
}
