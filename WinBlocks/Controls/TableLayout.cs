using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Contract.Impl;
using DCRF.Attributes;
using DCRF.Interface;
using WinUI = System.Windows.Forms;
using DCRF.Core;
using DCRF.Definition;
using WinBlocks.Base;
using DCRF.Primitive;

namespace WinBlocks.Controls
{
    [BlockHandle("TableLayout")]
    public class TableLayout: WinControlBase<WinUI.TableLayoutPanel>
    {
        public TableLayout(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        public override void InitBlock()
        {
            base.InitBlock();

            innerWeb[SysEventHelper.CoordinatorBlockID].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector,
                SysEventCode.Join(SysEventTiming.After, SysEventCode.AddBlock), null);
            innerWeb[SysEventHelper.CoordinatorBlockID][SysEventCode.Join(SysEventTiming.After, SysEventCode.AddBlock)].AttachEndPoint(Id, "BlockAdded");
        }

        public void OnAfterLoad()
        {
            int rows = this["Rows"].GetValue<int>(2);
            int cols = this["Columns"].GetValue<int>(2);
            string[] colStyles = this["ColumnStyles"].GetValue<string[]>();

            for (int i = 0; i < rows; i++)
            {
                ctl.RowStyles.Add(new WinUI.RowStyle(WinUI.SizeType.Percent,10f));
                ctl.RowCount++;
            }

            for (int i = 0; i < cols; i++)
            {
                if (colStyles != null)
                {
                    if (colStyles[i].EndsWith("%"))
                    {
                        int width = int.Parse(colStyles[i].Replace("%", ""));
                        ctl.ColumnStyles.Add(new WinUI.ColumnStyle(WinUI.SizeType.Percent, width));
                    }
                    else
                    {
                        int width = int.Parse(colStyles[i]);
                        ctl.ColumnStyles.Add(new WinUI.ColumnStyle(WinUI.SizeType.Absolute, width));
                    }
                }
                else
                {
                    ctl.ColumnStyles.Add(new WinUI.ColumnStyle(WinUI.SizeType.AutoSize));
                }

                ctl.ColumnCount++;
            }

            ctl.CellBorderStyle = WinUI.TableLayoutPanelCellBorderStyle.Single;
        }

        [BlockService]
        public void BlockAdded(BlockWebSysEventArgs args)
        {
            innerWeb[args.BlockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, "Row");
            innerWeb[args.BlockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, "Column");
            innerWeb[args.BlockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, "ColumnSpan");
            innerWeb[args.BlockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, "RowSpan");

            innerWeb[args.BlockId]["ColumnSpan"].AttachEndPoint(1);
            innerWeb[args.BlockId]["RowSpan"].AttachEndPoint(1);

            
        }

        public override void InitConnectors()
        {
            createConnectors("Rows", "ColumnStyles", "Columns", "AutoLayout");
            base.InitConnectors();
        }

        public override object GetUIElement()
        {
            OnAfterLoad();

            int currentRow = 0; //used for auto layout
            int currentCol = 0; //used for auto layout


            foreach (string id in innerWeb.BlockIds)
            {
                if (innerWeb[id]["Row"] != null)
                {
                    WinUI.Control child = null;

                    List<string> blockServices = innerWeb[id].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.Services, null, null) as List<string>;
                    if (blockServices.Contains("GetUIElement"))
                    {
                        child = innerWeb[id].ProcessRequest("GetUIElement") as WinUI.Control;
                    }

                    if (child != null)
                    {
                        int r = innerWeb[id]["Row"].GetValue<int>(0);
                        int c = innerWeb[id]["Column"].GetValue<int>(0);

                        if (this["AutoLayout"].GetValue<bool>())
                        {
                            if (currentCol == this["Columns"].GetValue<int>())
                            {
                                currentCol = 0;
                                currentRow++;
                            }

                            r = currentRow;
                            c = currentCol;
                            currentCol++;
                        }

                        ctl.SetRowSpan(child, innerWeb[id]["RowSpan"].GetValue<int>());
                        ctl.SetColumnSpan(child, innerWeb[id]["ColumnSpan"].GetValue<int>());

                        ctl.Controls.Add(child, c, r);
                        ctl.Controls.SetChildIndex(child, 0);
                    }
                }
            }

            return base.GetUIElement();
        }
    }
}
