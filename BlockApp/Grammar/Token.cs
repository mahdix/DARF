using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BlockBroker;
using bsn.GoldParser.Grammar;
using bsn.GoldParser.Parser;
using bsn.GoldParser.Semantic;
using DCRF.Contract;
using DCRF.Core;
using DCRF.Interface;

namespace BlockApp.Grammar
{
    [Terminal("(EOF)")]
    [Terminal("(Error)")]
    [Terminal("(Comment)")]
    [Terminal("(NewLine)")]
    [Terminal("(Whitespace)")]
    [Terminal("(*/)")]
    [Terminal("(/*)")]
    [Terminal("#")]
    [Terminal("(")]
    [Terminal(")")]
    [Terminal("=")]
    [Terminal(",")]
    [Terminal(".")]
    [Terminal(";")]
    [Terminal(":")]
    [Terminal("[")]
    [Terminal("{")]
    [Terminal("]")]
    [Terminal("}")]
    [Terminal("!")]
    [Terminal("~")]
    [Terminal("&")]
    [Terminal("if")]
    [Terminal("(//)")]
    [Terminal("var")]
    [Terminal("uid")]
    [Terminal("do")]
    [Terminal("else")]
    [Terminal("while")]
    [Terminal("block")]
    [Terminal("define")]
    [Terminal("return")]
    [Terminal("break")]
    [Terminal("continue")]
    [Terminal("service")]
    [Terminal("blockWeb")]
    [Terminal("register")]
    [Terminal("connector")]
    public class Token : SemanticToken
    {
        private int line = 0;
        private int column = 0;

        protected override void Initialize(Symbol symbol, LineInfo position)
        {
            base.Initialize(symbol, position);
            this.line = position.Line;
            this.column = position.Column;
        }
        public int Line
        {
            get
            {
                return line;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
        }
    }   
}
