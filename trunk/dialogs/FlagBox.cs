using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class FlagBox : UserControl
    {
        public FlagBox()
        {
            InitializeComponent();
        }

        private Type innerFlagBaseType;
        private Type innerFlagType;
        private string[] innerFlagNames;
        private Array innerFlagValues;

        public event FlagChangingEventHandler FlagChanging;

        public void FillBox(object flagValue)
        {
            
            checkedListBoxInner.Items.Clear();

            innerFlagType = flagValue.GetType();
            innerFlagBaseType = Enum.GetUnderlyingType(innerFlagType);
            
            innerFlagNames = Enum.GetNames(innerFlagType);
            innerFlagValues = Enum.GetValues(innerFlagType);

            for (int i = 0; i < innerFlagNames.Length; i++)
            {
                bool ch = false;

                UInt32 i1 = Convert.ToUInt32(flagValue);
                uint i2 = Convert.ToUInt32(innerFlagValues.GetValue(i));
                uint i3 = Convert.ToUInt32(innerFlagValues.GetValue(i));

                ch = 
                    (Convert.ToUInt32(flagValue) & Convert.ToUInt32(innerFlagValues.GetValue(i))) ==
                    Convert.ToUInt32(innerFlagValues.GetValue(i));
                
                checkedListBoxInner.Items.Add(innerFlagValues.GetValue(i), ch);

            }
        }

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object FlagValue 
        {
            get
            {
                object retEnum = Enum.ToObject(innerFlagType, 0);

                
                for (int i = 0; i < checkedListBoxInner.CheckedItems.Count; i++)
                {
                    retEnum = Convert.ToUInt32(retEnum) | Convert.ToUInt32(Enum.ToObject(innerFlagType,checkedListBoxInner.CheckedItems[i]));
                }

                return retEnum;
            }
            set
            {
                FillBox(value);
            }
        }

        private void checkedListBoxInner_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            object enumVal = Enum.ToObject(innerFlagType, checkedListBoxInner.Items[e.Index]);

            FlagChangingEventArgs args = new FlagChangingEventArgs
                (enumVal,
                e.NewValue == CheckState.Checked);
            OnFlagChanging(args);
            if (args.CancelChange)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        protected void OnFlagChanging(FlagChangingEventArgs e)
        {
            if (FlagChanging != null)
            {
                FlagChanging(this, e);
            }
        }
    }

    public class FlagChangingEventArgs : EventArgs
    {
        public object FlagValue { get; private set; }
        public bool FlagSet { get; private set; }
        public bool CancelChange { get; set; }

        public FlagChangingEventArgs(object flagValue,bool flagSet)
        {
            FlagValue = flagValue;
            FlagSet = flagSet;
            CancelChange = false;
        }
    }

    public delegate void FlagChangingEventHandler(object sender,FlagChangingEventArgs e);
}
