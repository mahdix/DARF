using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DCRF.Primitive;

namespace AdminConsole
{
    public delegate object ConvertDelegate(string input);

    public partial class ctlValuePicker : UserControl
    {
        private List<Type> availableTypes = new List<Type>();
        private Dictionary<Type, ConvertDelegate> availableConverters = new Dictionary<Type, ConvertDelegate>();

        public ctlValuePicker()
        {
            InitializeComponent();
            initTypeComboBox();
        }

        private void initTypeComboBox()
        {
            //fill types array
            availableTypes.Add(typeof(string));
            availableTypes.Add(typeof(int));
            availableTypes.Add(typeof(BlockHandle));
            availableTypes.Add(typeof(Guid));
            availableTypes.Add(typeof(int[]));
            availableTypes.Add(typeof(string[]));

            availableConverters.Add(typeof(string), new ConvertDelegate(delegate(string input) { return input; }));
            availableConverters.Add(typeof(int), new ConvertDelegate(delegate(string input) { return int.Parse(input); }));
            availableConverters.Add(typeof(BlockHandle), new ConvertDelegate(delegate(string input) { return BlockHandle.Parse(input); }));
            availableConverters.Add(typeof(Guid), new ConvertDelegate(delegate(string input) { return new Guid(input); }));

            //populate combo box
            foreach (Type t in availableTypes)
            {
                cboType.Items.Add(t.Name);
            }
        }

        public bool FixedType
        {
            get
            {
                return !cboType.Enabled;
            }
            set
            {
                cboType.Enabled = !value;
            }
        }

        public void SetValueByType(string typeName)
        {
            Type type = Type.GetType(typeName);

            //by default if an arg type is unknonw, we suppose that it is string
            //user can change type if he wants to
            if (type == null)
            {
                type = typeof(string);
            }

            //we cannot show values for enum so convert them to int
            if (type.IsEnum)
            {
                type = typeof(int);
            }

            if (type.IsArray && type.GetElementType() == typeof(object))
            {
                type = typeof(string[]);
            }

            //string has no defauly constructor
            if (type == typeof(string))
            {
                Value = "";
            }
            else
            {
                if ( type.IsArray )
                {
                    Array.CreateInstance(type.GetElementType(), 1);
                }
                else
                {
                    Value = Activator.CreateInstance(type);
                }
            }
        }

        public object Value
        {
            get
            {
                if (DesignMode) return "";

                Type dataType = availableTypes[cboType.SelectedIndex];

                if (dataType.IsArray)
                {
                    dataType = dataType.GetElementType();
                    ConvertDelegate converter = availableConverters[dataType];
                    string[] parts = txtValue.Text.Split(',');
                    object[] result = new object[parts.Length];

                    for(int i=0;i<parts.Length;i++)
                    {
                        string part = parts[i];

                        if (part == "(null)") result[i] = null;
                        else if (part == MissingValue.Value.ToString()) result[i] = MissingValue.Value;
                        else result[i] = converter(part);
                    }

                    return result;
                }
                else
                {
                    if (txtValue.Text == "(null)") return null;
                    if (txtValue.Text == MissingValue.Value.ToString()) return MissingValue.Value;

                    ConvertDelegate converter = availableConverters[dataType];
                    return converter(txtValue.Text);
                }


                //switch (cboType.SelectedItem.ToString())
                //{
                //    case "string": myValue = txtValue.Text; break;
                //    case "int": myValue = int.Parse(txtValue.Text); break;
                //    case "bool": myValue = bool.Parse(txtValue.Text); break;
                //    case "null": myValue = null; break;
                //    case "Guid": myValue = new Guid(txtValue.Text); break;
                //    case "BlockHandle": myValue = BlockHandle.Parse(txtValue.Text); break;
                //    case "object[]":
                //        {
                //            string[] parts = 
                //            break;
                //        }
                //}

                //return myValue;
            }
            set
            {
                if (DesignMode) return;

                if (value == null)
                {
                    txtValue.Text = "(null)";
                    cboType.SelectedItem = "(null)";
                }
                else
                {
                    Type dataType = value.GetType();

                    if (!availableTypes.Contains(dataType))
                    {
                        dataType = availableTypes[0];
                    }

                    cboType.SelectedIndex = availableTypes.IndexOf(dataType);

                    if (!dataType.IsArray)
                    {
                        txtValue.Text = value.ToString();
                    }
                    else
                    {
                        object[] items = value as object[];
                        StringBuilder sb = new StringBuilder();
                        int counter = 0;

                        foreach (object item in items)
                        {
                            if (item == null) sb.Append("(null)");
                            else sb.Append(item.ToString());

                            if (counter < items.Length - 1) sb.Append(",");
                            counter++;                            
                        }
                    }
                }

                //if (value != null)
                //{
                //    txtValue.Text = value.ToString();

                //    if (value is Int32)
                //    {
                //        cboType.SelectedItem = "Int32";
                //    }
                //    else if (value is string)
                //    {
                //        cboType.SelectedItem = "String";
                //    }
                //    else if (value is bool)
                //    {
                //        cboType.SelectedItem = "Boolean";
                //    }
                //    else if (value is Guid)
                //    {
                //        cboType.SelectedItem = "Guid";
                //    }
                //    else if (value is BlockHandle)
                //    {
                //        cboType.SelectedItem = "BlockHandle";
                //    }

                //}
                //else
                //{
                //    txtValue.Text = "";
                //}
            }
        }

        private void ctlValueHolder_Load(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == -1) cboType.SelectedIndex = 0;
        }
    }
}
