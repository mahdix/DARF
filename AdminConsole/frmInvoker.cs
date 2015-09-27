using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using System.Threading;
using sliver.Windows.Forms;
using DCRF.Core;
using NetSockets.Peer;
using DCRF.Primitive;

namespace AdminConsole
{
    public partial class frmInvoker : Form
    {
        private sliver.Windows.Forms.StateBrowser browser = new sliver.Windows.Forms.StateBrowser();

        public IBlockWeb myWeb = null;
        public string myId = "";
        public string myService = null;
        public string myConnectorKey = null;

        private Thread invokeThread = null;
        private bool stopInvoke = false;
        public bool IsQuickInvoke = false;  //if true, invoke upol form load

        

        public frmInvoker()
        {
            InitializeComponent();
        }

        private void frmInvokeService_Load(object sender, EventArgs e)
        {
            lblBlockWeb.Text = myWeb.Id + " @ " + myWeb.Address;

            if (myId == "")
            {
                lblBlock.Text = "Global BlockWeb Connector";
            }
            else
            {
                lblBlock.Text = myWeb.GetBlockHandle(myId).ToString();
            }

            if (myService != null)
            {
                lblService.Text = myService;
            }
            else
            {
                lblService.Text = myConnectorKey;
            }

            browser.VerticalScroll.Enabled = true;
            browser.HorizontalScroll.Enabled = true;
            browser.ShowStaticMembers = true;
            browser.ShowNonPublicMembers = true;
            browser.ShowDataTypes = true;
            browser.BoldMemberTypes.Add(typeof(int));
            browser.BoldMemberTypes.Add(typeof(bool));
            browser.BoldMemberTypes.Add(typeof(string));
            browser.BoldMemberTypes.Add(typeof(long));
            browser.BoldMemberTypes.Add(typeof(uint));
            browser.BoldMemberTypes.Add(typeof(ulong));
            browser.BoldMemberTypes.Add(typeof(float));
            browser.BoldMemberTypes.Add(typeof(double));
            browser.BoldMemberTypes.Add(typeof(decimal));
            browser.BoldMemberTypes.Add(typeof(char));
            browser.BoldMemberTypes.Add(typeof(byte));
            browser.BoldMemberTypes.Add(typeof(sbyte));
            browser.BoldMemberTypes.Add(typeof(short));

            pnlObjectView.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            ctlArgPicker1.myWeb = myWeb;
            ctlArgPicker1.myConnectorKey = myConnectorKey;
            ctlArgPicker1.myService = myService;
            ctlArgPicker1.myId = myId;

            try
            {
                ctlArgPicker1.initControl();

                if (IsQuickInvoke)
                {
                    invoke(false);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error Initialization: " + exc.Message);
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void cmdInvoke_Click(object sender, EventArgs e)
        {
            //if autoinvoke was in progress cancel it
            stopAutoInvoke();
            
            invoke(false);           
        }

        private void invoke(bool scheduled)
        {
            if (!scheduled)
            {
                innerInvoke();
            }
            else
            {
                grpResult.Invoke(new MethodInvoker(delegate()
                    {
                        innerInvoke();
                    }));
            }
        }

        private void innerInvoke()
        {
            object result = null;
            object[] parameters = ctlArgPicker1.Args;

            try
            {
                if (myConnectorKey == null)
                {
                    result = myWeb[myId].ProcessRequest(myService, parameters);
                }
                else
                {
                    if (myId != "")
                    {
                        //block connector
                        result = myWeb[myId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.InvokeConnector, myConnectorKey, parameters);
                    }
                    else
                    {
                        //global connector
                        result = null; // myWeb.GetConnector(myConnectorKey).ProcessRequest(parameters);
                    }
                }
            }
            catch (RemoteException rexc)
            {
                result = "Invoke Failed - Remote Exception Type: " + rexc.ExceptionType + " - Message: " + rexc.Message;
            }
            catch (Exception exc)
            {
                result = "Invoke Failed: " + exc.Message;
            }

            if (result == null)
            {
                lblResult.Text = "(null)";
            }
            else
            {
                lblResult.Text = result.ToString();
            }

            browser.ObjectToBrowse = result;
        }

        private void cmdAutoInvoke_Click(object sender, EventArgs e)
        {
            if (invokeThread == null)
            {
                cmdAutoInvoke.Text = "Stop Auto Invoke";
                stopInvoke = false;

                invokeThread = new Thread(new ThreadStart(delegate()
                    {
                        while (!stopInvoke)
                        {
                            invoke(true);

                            int waitSec = 5;
                            for (int i = 0; i < waitSec; i++)
                            {
                                grpResult.Invoke(new MethodInvoker(delegate()
                                    {
                                        int remaining = waitSec - i;
                                        grpResult.Text = "Result - Refresh in " + remaining.ToString() + " seconds";
                                    }));

                                if (stopInvoke) break;
                                Thread.Sleep(1000);
                                if (stopInvoke) break;
                            }
                        }
                    }));

                invokeThread.Start();
            }
            else
            {
                stopAutoInvoke();
            }
        }

        void threadBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopAutoInvoke();
        }

        private void stopAutoInvoke()
        {
            if (invokeThread != null)
            {
                stopInvoke = true;
                
                cmdAutoInvoke.Text = "Auto Invoke";
                Thread.Sleep(1000);

                if (invokeThread.ThreadState == ThreadState.Running)
                {
                    invokeThread.Abort();
                }

                grpResult.Text = "Result";
            }

            invokeThread = null;
        }

        private void frmInvokeService_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (invokeThread != null)
            {
                stopAutoInvoke();
            }
        }

        private void cmdCopyResult_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, lblResult.Text);
        }

        private void frmInvokeService_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void frmInvokeService_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                cmdInvoke.PerformClick();
            }
        }

        private void cmdMetaInfo_Click(object sender, EventArgs e)
        {
            object result = null;

            try
            {
                if (myConnectorKey == null)
                {
                    result = myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.ServiceInfo, myService, null);
                }
                else
                {
                    if (myId != "")
                    {
                        //block connector
                        result = myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.ConnectorInfo, myConnectorKey, null);
                    }
                    else
                    {
                        //global connector 
                        result = "GlobalConnectors have no meta-info";
                    }
                }
            }
            catch (RemoteException rexc)
            {
                result = "Error retrieving meta-info: "+rexc.Message;
            }

            MessageBox.Show((string)result, "Service Signature");
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
                
        }
    }
}
