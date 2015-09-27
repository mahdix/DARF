using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Attributes;
using DCRF.Core;
using DCRF.Interface;

namespace GeneralBlocks
{
    [BlockHandle("DummyBlock", "DummyProduct", 1, 0, 1, 11)]
    [BlockType(DCRF.Definition.BlockType.SingleInstance)]
    [BlockComments("Dummy Comments")]
    [BlockCompany("DummyCompany")]
    [BlockFriendlyName("DummyFriendlyName")]
    [BlockPlatform(DCRF.Definition.PlatformType.Neutral)]
    [BlockTag("DumyTag1")]
    [BlockTag("DummyTag2")]
    [BlockReleaseDate("2013-Jun-18")]
    public class DummyBlock: BlockBase
    {
        public DummyBlock(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        [BlockService]
        public string DummyService(string txt)
        {
            return "Dummy" + txt;
        }

    }
}
