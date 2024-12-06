using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien.Classes
{
    internal class CommonControls
    {
        public static void ResetInputFields(Control parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            foreach (Control control in parent.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).Value = DateTime.Now;
                }
                else if (control.HasChildren)
                {
                    // Recursively check child controls
                    ResetInputFields(control);
                }
            }
        }

    }
}
