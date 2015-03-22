using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileMatcher
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 3)
			{
				Console.WriteLine("Not enough arguments supplied.");
				return;
			}
			else if (!Directory.Exists(args[0]))
			{
				Console.WriteLine("Target directory doesn't exist.");
				return;
			}
			int i = 0;
			if (args.Length == 4 && (args[3] == "and" || args[3] == "not" || args[3] == "notconv"))
			{
				FindMatchingFiles(args[0], args[1], args[2], ref i, args[3]);
			}
			else
			{
				FindMatchingFiles(args[0], args[1], args[2], ref i);
			}
			Console.WriteLine("Found a total of " + i + " matches.");
		}

		static void FindMatchingFiles(string basedir, string ext1, string ext2, ref int count, string op = "and")
		{
			string[] f;
			try
			{
				f = Directory.GetFiles(basedir);
			}
			catch (UnauthorizedAccessException)
			{
				Console.WriteLine("Permission denied in " + basedir);
				return;
			}

			foreach (string file in f)
			{
				string match = basedir + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file) + ext2;
				if (op == "and")
				{
					if (Path.GetExtension(file) == ext1 && f.Contains(match))
					{
						Console.WriteLine(String.Format("Found {0} and {1}", Path.GetFullPath(file), Path.GetFullPath(match)));
						count++;
					}
				}
				else if (op == "not")
				{
					if (Path.GetExtension(file) == ext1 && !f.Contains(match))
					{
						Console.WriteLine(String.Format("Found {0} without {1}", Path.GetFullPath(file), Path.GetFullPath(match)));
						count++;
					}
				}
				else if (op == "notconv")
				{
					if (Path.GetExtension(file) == ext1 && !f.Contains(match))
					{
						ConvertFile(file);
					}
				}
			}
			string[] d = Directory.GetDirectories(basedir);
			foreach (string dir in d)
			{
				FindMatchingFiles(dir, ext1, ext2, ref count, op);
			}
		}

		static void FindMatchingFiles(string basedir, string ext1, string ext2, string op = "and")
		{
			int i = 0;
			FindMatchingFiles(basedir, ext1, ext2, ref i, op);
		}

		static void ConvertFile(string file)
		{
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo.FileName = @"C:\Users\Jonathan\Documents\My Dropbox\VSprojects\Visual Studio 2010\Projects\any2mp3\any2mp3\bin\x64\Debug\any2mp3.exe";
			p.StartInfo.Arguments = file;
			p.StartInfo.UseShellExecute = false;
			p.Start();
			p.WaitForExit();
		}
	}
}
