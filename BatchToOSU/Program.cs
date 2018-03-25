/*
 * User: Mario
 * Date: 14.02.2018
 * Time: 22:33
 */
using System;
using System.IO;
using System.Diagnostics;

namespace BatchToOSU
{
	class Program
	{
		public static void Main(string[] args)
		{
			int counter = 0;
			string raindrop = "F:\\rdb\\release\\convertOM.bat";
			string sevenZip = "C:\\Program Files\\7-Zip\\7z.exe";
			string targetPath = "F:\\osuSongs";
			Console.Write("Please enter the folder to be batch-converted:");
			string sourcePath = Console.ReadLine();
			if(sourcePath.Length == 0)
			{return;}
			
			string targetFolderPart = sourcePath.Substring(sourcePath.LastIndexOf("\\")+1);
			string targetFolder = targetPath+"\\"+targetFolderPart;
			Directory.CreateDirectory(targetFolder);
			
			string[] subPaths = Directory.GetDirectories(sourcePath);
			foreach(string subPath in subPaths)
			{
				string subPathStripped = subPath.Substring(subPath.LastIndexOf("\\")+1);
				string[] subPathContent = Directory.GetFiles(subPath);
				string smFile = null;
				
				foreach(string subPathFile in subPathContent)
				{
					if(subPathFile.IndexOf(".sm") != -1 && subPathFile.IndexOf(".old") == -1)
					{
						smFile = subPathFile;
					}
				}

				if(smFile == null)
				{
					Console.WriteLine(subPath+ " has no *.sm file, skipping...");
					continue;
				}
				
				var raindropProcess = Process.Start(raindrop,'"'+smFile+'"');
				raindropProcess.WaitForExit();
				var sevenZipProcess = Process.Start(sevenZip,"a "+'"'+targetFolder+"\\"+subPathStripped+".zip"+'"'+" "+'"'+subPath+"\\*.osu"+'"'+" "+'"'+subPath+"\\*.mp3"+'"'+" "+'"'+subPath+"\\*.ogg"+'"'+" "+'"'+subPath+"\\*.jpg"+'"'+" "+'"'+subPath+"\\*.png"+'"');
				sevenZipProcess.WaitForExit();
				File.Move(targetFolder+"\\"+subPathStripped+".zip",targetFolder+"\\"+subPathStripped+".osz");
				Console.WriteLine("Created: "+targetFolder+"\\"+subPathStripped+".osz");
				counter++;
				
				//cleanup
				foreach(string subPathFile in subPathContent)
				{
					if(subPathFile.IndexOf(".osu") != -1)
					{
						File.Delete(subPathFile);
					}
				}
			}
			
			Console.WriteLine("Created "+counter+" *.osz files, press any key to exit...");
			Console.ReadKey(true);
		}
	}
}