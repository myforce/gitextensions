using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForceReleaser
{
    #region Logging
    public class Logger
    {
        public enum LogType { lt_info, lt_warning, lt_error }
        private static Logger _myInstance = new Logger();
        private StringBuilder _ErrorLog;

        private Logger()
        {
            _ErrorLog = new StringBuilder();
        }

        public static Logger GetLogger()
        {
            return _myInstance;
        }

        public void LogMessage(string strToLog, LogType lt = LogType.lt_info, int nLevel = 0)
        {
            string strType;
            switch (lt)
            {
                case LogType.lt_warning:
                    strType = "WARNING";
                    break;
                case LogType.lt_error:
                    strType = "ERROR";
                    break;
                case LogType.lt_info:
                default:
                    strType = "INFO";
                    break;
            }

            _ErrorLog.AppendFormat("{0,-12}{1,-10}L{2,-2} {3}"
                , DateTime.Now.ToString("hh:mm:ss")
                , strType
                , nLevel
                , strToLog
                );
        }

        public string GetLog()
        {
            return _ErrorLog.ToString();
        }

        public void Clear()
        {
            _ErrorLog.Clear();
        }

    }
    #endregion

    public static class StaticTools
    {
        #region FileManipulations
        public static byte[] GetBinaryFileContents(string strFile)
        {
            byte[] toReturn = null;
            try
            {
                using (FileStream fs = File.Open(strFile, FileMode.Open))
                {
                    toReturn = new BinaryReader(fs).ReadBytes((int)fs.Length);
                }
            }
            catch (Exception)
            {
                toReturn = null;
            }
            return toReturn;
        }
        public static string GetFileContents(string strFile)
        {
            string strResult = "";
            try
            {
                using (StreamReader sr = new StreamReader(strFile, Encoding.Default))
                {                    
                    strResult = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                strResult = "";
            }
            return strResult;
        }
        public static string FindInFile(string strFile, string strRegex, int nGroup)
        {
            string strFileContents = GetFileContents(strFile);
            if (strFileContents.Length > 0)
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(strFileContents, strRegex);
                if (match.Success && match.Groups.Count > nGroup)
                    return match.Groups[nGroup].Value;
            }
            return "";
        }
        public static string GitCommitVersionToNormalVersion(string strFile)
        {
            return strFile.Replace('-', '.');
        }
        public static string RemoveGitCommitVersion(string strFile)
        {
            string strResult = strFile;
            System.Text.RegularExpressions.Match Result = System.Text.RegularExpressions.Regex.Match(strFile, "^(\\d+[\\.])+(\\d+)-\\d+$");
            if (Result.Success)
            {
                strResult = "";
                for (int i = 0; i < Result.Groups[1].Captures.Count; i++)
                    strResult = strResult + Result.Groups[1].Captures[i].Value;
                strResult = strResult + Result.Groups[2].Captures[0].Value;
            }
            return strResult;
        }
        public static List<List<string>> FindAllMatchesInFile(string strFile, string strRegex, List<int> lstGroupsToCapture)
        {
            //Determine the max group number (needed for validation of the result later)
            int nMaxGroupIndex = lstGroupsToCapture.Max(obj => obj);
            List<List<string>> result = new List<List<string>>();
            string strFileContents = GetFileContents(strFile);
            if (strFileContents.Length > 0)
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(strFileContents, strRegex);
                while (match.Success)
                {
                    if (match.Groups.Count > nMaxGroupIndex)
                    {
                        List<string> lstGroupInfo = new List<string>();
                        foreach (var item in lstGroupsToCapture)
                            lstGroupInfo.Add(match.Groups[item].Value);
                        result.Add(lstGroupInfo);
                    }
                    match = match.NextMatch();
                }
            }
            return result;
        }
        public static bool ReplaceInFile(string strFile, string strFindRegex, string strNewValue)
        {
            string strFileContents = GetFileContents(strFile);
            if (strFileContents.Length > 0)
            {
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(strFindRegex);
                if (rgx.Match(strFileContents).Success)
                {
                    strFileContents = rgx.Replace(strFileContents, strNewValue);
                    try
                    {
                        StreamReader sr = new StreamReader(strFile, Encoding.Default);
                        Encoding toWrite = sr.CurrentEncoding;
                        sr.Close();

                        using (StreamWriter wr = new StreamWriter(strFile, false, toWrite))
                        {
                            wr.Write(strFileContents);
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public static bool? IsDirectory(string path)
        {
            if (System.IO.Directory.Exists(path))
                return true;
            // is a directory 
            else if (System.IO.File.Exists(path))
                return false;
            // is a file 
            else
                return null; // is a nothing }
        }
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool bOverwrite = false)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, bOverwrite);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs, bOverwrite);
                }
            }
        }
        public static bool SaveFileContents(string strPath, string strData)
        {
            try
            {
                StreamReader sr = new StreamReader(strPath, Encoding.Default);
                Encoding toWrite = sr.CurrentEncoding;
                sr.Close();

                using (StreamWriter wr = new StreamWriter(strPath, false, toWrite))
                {
                    wr.Write(strData);
                }                
            }
            catch (Exception)
            {
                return false;
            }
            return true;  
        }
        public static string[] TranslateWildCardsToFiles(string strPath)
        {
            if (!System.IO.File.Exists(strPath))
            {
                System.Text.RegularExpressions.Match result = System.Text.RegularExpressions.Regex.Match(strPath, @"((.*\\))+(.*)");
                if (!result.Success)
                    return null;
                int test = result.Captures.Count;
                string strTest = result.Groups[1].Value;
                string strTest2 = result.Groups[result.Groups.Count - 1].Value;
                if (!System.IO.Directory.Exists(strTest))
                    return null;

                return System.IO.Directory.GetFiles(strTest, strTest2);
            }
            else
                return new string[] { strPath };
        }
        //We need to handle wildcards 
                
        #endregion

        #region Version Manipulations
        public static int CompareFileVersions(string strVersionA, string strVersionB)
        {
            string[] arrVersionA = strVersionA.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrVersionB = strVersionB.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (arrVersionA.Length != arrVersionB.Length)
                return -2;

            for (int index = 0; index < arrVersionA.Length; index++)
            {
                if (int.Parse(arrVersionA[index]) < int.Parse(arrVersionB[index]))
                    return -1; // A < B

                if (int.Parse(arrVersionA[index]) > int.Parse(arrVersionB[index]))
                    return 1; // A > B
            }
            return 0; //Equal
        }
        public static bool IsValidVersionNumber(string strVersionNumber, bool bOnlyRelease = false)
        {
            return bOnlyRelease ? System.Text.RegularExpressions.Regex.Match(strVersionNumber, "^(\\d+\\.)+[1-9]\\d*\\.0+$").Success : System.Text.RegularExpressions.Regex.Match(strVersionNumber, "^(\\d+\\.)+\\d+$").Success;
        }
        public static string GetMainTrackVersionNumber(string strVersionNumber, int nDigits = 3)
        {
            string strResult = "";
            System.Text.RegularExpressions.Match Result = System.Text.RegularExpressions.Regex.Match(strVersionNumber, "^(\\d+\\.)+\\d+(-\\d+)*$");
            if (Result.Success && Result.Groups[1].Captures.Count >= nDigits)
            {
                for (int i = 0; i < nDigits; i++)
                    strResult = strResult + Result.Groups[1].Captures[i].Value;
            }
            return strResult.TrimEnd('.');
        }
        public static string RemoveLatestPartFromVersionNumber(string strVersionNumber)
        {
            string strResult = "";
            System.Text.RegularExpressions.Match Result = System.Text.RegularExpressions.Regex.Match(strVersionNumber, "^(\\d+\\.)+\\d+$");
            if (Result.Success)
            {
                for (int i = 0; i < Result.Groups[1].Captures.Count; i++)
                    strResult = strResult + Result.Groups[1].Captures[i].Value;
            }
            return strResult.TrimEnd('.');
        }
        public static string IncrementVersionNumber(string strCurrentVersionNumber)
        {
            string strNewVersion = "999.999.999.999";
            string[] arrVersionDetails = strCurrentVersionNumber.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            int nLatestIndex = arrVersionDetails.Length - 1;
            int nVersionPart = 0;
            if (nLatestIndex >= 0 && int.TryParse(arrVersionDetails[nLatestIndex], out nVersionPart))
            {
                nVersionPart++;
                arrVersionDetails[nLatestIndex] = nVersionPart.ToString();
                strNewVersion = String.Join(".", arrVersionDetails);
            }
            return strNewVersion;
        }
        #endregion

        #region General
        public static void ReplaceConstansInFilePath(Dictionary<string,string> Constants, ref string strPath, bool bExpand=true)
        {
            if (bExpand)
             {
                 //We should expand the constants
                 System.Text.RegularExpressions.MatchCollection reMatch = System.Text.RegularExpressions.Regex.Matches(strPath, @"\$\((.+?)\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                 foreach (System.Text.RegularExpressions.Match item in reMatch)
                 {
                     if (item.Groups.Count != 2)
                         continue;

                     string strKey = item.Groups[1].Value.ToLower();
                     if (Constants.ContainsKey(strKey))
                         strPath = strPath.Replace("$(" + strKey + ")", Constants[strKey]);                   
                 }
             }
            else 
            {
                string strCurrentPath="", strConstant="";
                foreach (var constant in Constants)
                {
                    strPath = strPath.ToLower();
                    strCurrentPath = constant.Value.ToLower();
                    strConstant = constant.Key.ToLower();
                    strPath = strPath.Replace(strCurrentPath, "$(" + strConstant + ")");
                }
            }
        }

        public static string Serialize<T>(T value)
        {

            if (value == null)
            {
                return null;
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (StringWriter textWriter = new StringWriter())
            {
                using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {

            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
            // No settings need modifying here

            using (StringReader textReader = new StringReader(xml))
            {
                using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }
        #endregion

        #region CommandLine
        private static Dictionary<int, StringBuilder> ProcessOutCapture = new Dictionary<int, StringBuilder>();
        private static Dictionary<int, StringBuilder> ProcessErrCapture = new Dictionary<int, StringBuilder>();
        public static int ExecuteCommand(string strProgram, string strArguments, string strWorkingDir, ref string strOutputLog, ref string strErrorOutputLog, bool bOverwriteFile = true, bool bLogIntoOutputAndErrorVariablesInsteadOfToFile = false)
        {
            bool bResult = false;

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = strProgram;
            startInfo.Arguments = strArguments;
            startInfo.RedirectStandardError = bLogIntoOutputAndErrorVariablesInsteadOfToFile || !string.IsNullOrWhiteSpace(strErrorOutputLog);
            startInfo.RedirectStandardOutput = bLogIntoOutputAndErrorVariablesInsteadOfToFile || !string.IsNullOrWhiteSpace(strOutputLog);
            startInfo.WorkingDirectory = strWorkingDir;
            startInfo.UseShellExecute = false;

            //Prepare for capturing the output
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            if (bLogIntoOutputAndErrorVariablesInsteadOfToFile || !string.IsNullOrWhiteSpace(strOutputLog))
            {
                startInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += process_OutputDataReceived;
            }

            if (bLogIntoOutputAndErrorVariablesInsteadOfToFile || !string.IsNullOrWhiteSpace(strErrorOutputLog))
            {
                startInfo.RedirectStandardError = true;
                process.ErrorDataReceived += process_ErrorDataReceived;
            }

            //Start the process with the correct info            
            process.StartInfo = startInfo;
            if (!process.Start())
                return -1;

            //Start capture of the output if needed
            if (bLogIntoOutputAndErrorVariablesInsteadOfToFile|| !string.IsNullOrWhiteSpace(strOutputLog))
                process.BeginOutputReadLine();

            if (bLogIntoOutputAndErrorVariablesInsteadOfToFile || !string.IsNullOrWhiteSpace(strErrorOutputLog))
                process.BeginErrorReadLine();

            //Wait max one hour for the process to complete
            int nMaxExectionWaitRetries = 3600;
            while (--nMaxExectionWaitRetries >= 0)
                bResult = process.WaitForExit(1000);

            //Handle the logging if needed
            if (bLogIntoOutputAndErrorVariablesInsteadOfToFile ||!string.IsNullOrWhiteSpace(strOutputLog))
            {
                if (ProcessOutCapture.ContainsKey(process.Id))
                {
                    if (bLogIntoOutputAndErrorVariablesInsteadOfToFile)
                    {
                        //Write to variables
                        strOutputLog = ProcessOutCapture[process.Id].ToString();
                    }
                    else
                    {
                        //Write to file
                        try
                        {
                            System.IO.FileInfo dirInf = new FileInfo(strOutputLog);
                            if (!dirInf.Directory.Exists)
                                dirInf.Directory.Create();

                            using (StreamWriter wr = new StreamWriter(strOutputLog, !bOverwriteFile))
                            {
                                wr.Write(ProcessOutCapture[process.Id]);
                            }
                        }
                        catch (Exception) { }
                    }
                    ProcessOutCapture.Remove(process.Id);
                }
            }            

            if (bLogIntoOutputAndErrorVariablesInsteadOfToFile || !string.IsNullOrWhiteSpace(strErrorOutputLog))
            {
                if (ProcessErrCapture.ContainsKey(process.Id))
                {
                    if (bLogIntoOutputAndErrorVariablesInsteadOfToFile)
                    {
                        //Write to variables
                        strErrorOutputLog = ProcessOutCapture[process.Id].ToString();
                    }
                    else
                    {
                        //Write to file
                        try
                        {
                            System.IO.FileInfo dirInf = new FileInfo(strErrorOutputLog);
                            if (!dirInf.Directory.Exists)
                                dirInf.Directory.Create();
                            using (StreamWriter wr = new StreamWriter(strErrorOutputLog, !bOverwriteFile))
                            {
                                wr.Write(ProcessErrCapture[process.Id]);
                            }
                        }
                        catch (Exception) { }
                    }
                    ProcessErrCapture.Remove(process.Id);
                }
            }
            return bResult ? process.ExitCode : -1;
        }
        private static void process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            System.Diagnostics.Process senderProcess = sender as System.Diagnostics.Process;
            if (senderProcess == null || string.IsNullOrWhiteSpace(e.Data))
                return;

            if (!ProcessErrCapture.ContainsKey(senderProcess.Id))
                ProcessErrCapture.Add(senderProcess.Id, new StringBuilder());

            ProcessErrCapture[senderProcess.Id].Append(e.Data);
            ProcessErrCapture[senderProcess.Id].Append(Environment.NewLine);
        }
        private static void process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            System.Diagnostics.Process senderProcess = sender as System.Diagnostics.Process;
            if (senderProcess == null || string.IsNullOrWhiteSpace(e.Data))
                return;

            if (!ProcessOutCapture.ContainsKey(senderProcess.Id))
                ProcessOutCapture.Add(senderProcess.Id, new StringBuilder());

            ProcessOutCapture[senderProcess.Id].Append(e.Data);
            ProcessOutCapture[senderProcess.Id].Append(Environment.NewLine);
        }
        #endregion        
    }
}
