using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class RegisterCmd : CommandHandler
    {
        private Identifier filePath = null;

        [Rule(@"<RegisterCmd> ::= ~'#' ~register FilePathIdentifier")]
        public RegisterCmd(Identifier filePath)
        {
            this.filePath = filePath;
        }

        public override void Execute()
        {
            string fp = (string)filePath.Value;
            fp = fp.Substring(1, fp.Length - 2);

            if (!File.Exists(fp))
            {
                fp = Path.Combine(ScriptEngine.DefaultBlocksPath, fp);

                if (!File.Exists(fp))
                {
                    throw new Exception("Cannot find assembly file:" + fp);
                }
            }

            ExecutionContext.Current.RegisterBrokerAssemblyFile(fp);
        }
    }
}
