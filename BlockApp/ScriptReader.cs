using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlockApp
{
    public class ScriptReader : TextReader
    {
        private string filePath = null;
        private StringReader scriptReader = null;
        private StringWriter scriptWriter = null;
        private StringBuilder builder = new StringBuilder();

        private Dictionary<string, string> tokens = new Dictionary<string, string>();
        private List<string> tokenKeys = new List<string>();

        //#include<aa.xda> read file contents
        //#token CZ qq  -> currently is not supported
        public ScriptReader(string filePath)
        {
            this.filePath = filePath;
            
            scriptWriter = new StringWriter(builder);
            readScript(filePath);

            scriptReader = new StringReader(builder.ToString());
            scriptWriter.Close();
        }

        private void readScript(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string currentLine = sr.ReadLine();

            while (currentLine != null)
            {
                currentLine = currentLine.Trim();

                foreach (string tk in tokenKeys)
                {
                    string key = "[" + tk + "]";
                    int idx = currentLine.IndexOf(key);

                    if ( idx != -1 )
                    {
                        if (idx == 0 || currentLine[idx - 1] != '\\')
                        {
                            currentLine = currentLine.Replace(key, tokens[tk]);
                        }
                    }

                    idx = currentLine.IndexOf("\\[");

                    if ( idx != -1 )
                    {
                        //remove escape character
                        currentLine = currentLine.Remove(idx, 1);
                    }

                }

                if (currentLine.StartsWith("#include"))
                {
                    int idx1 = currentLine.IndexOf("<");
                    int idx2 = currentLine.IndexOf(">");

                    string includeFile = currentLine.Substring(idx1 + 1, idx2 - idx1 - 1);

                    readScript(includeFile);
                }
                else if (currentLine.StartsWith("#token"))
                {
                    int idx1 = currentLine.IndexOf(" ");
                    int idx2 = currentLine.IndexOf(" ", idx1+1);

                    if ( idx1 == -1 || idx2 == -1 ) throw new Exception("Invalid token definition: "+currentLine);

                    string token = currentLine.Substring(idx1 + 1, idx2 - idx1 - 1);
                    string value = currentLine.Substring(idx2 + 1);

                    tokens[token] = value;
                    tokenKeys.Add(token);
                }
                else
                {
                    scriptWriter.WriteLine(currentLine);
                }

                currentLine = sr.ReadLine();
            }

            sr.Close();
        }

        public override void Close()
        {
            base.Close();
            scriptReader.Close();
        }

        public override int Peek()
        {
            return scriptReader.Peek();
        }

        public override int Read()
        {
            return scriptReader.Read();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return scriptReader.Read(buffer, index, count);
        }


        public override int ReadBlock(char[] buffer, int index, int count)
        {
            return scriptReader.ReadBlock(buffer, index, count);
        }

        public override string ReadLine()
        {
            return scriptReader.ReadLine();
        }

        public override string ReadToEnd()
        {
            return scriptReader.ReadToEnd();
        }


    }
}
