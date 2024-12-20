using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien.Classes
{
    internal class CommonControls
    {
        static ContextMenuStrip contextMenu;

        static TabControl tabControl;
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

        public static void InitializeTabControl(TabControl tabCtrl)
        {
            tabControl = tabCtrl;
            contextMenu = new ContextMenuStrip();
            ToolStripMenuItem closeTabMenuItem = new ToolStripMenuItem("Close Tab");
            closeTabMenuItem.Click += CloseTabMenuItem_Click;
            contextMenu.Items.Add(closeTabMenuItem);
            tabControl.MouseUp += TabControl_MouseUp;
        }

        public static void AddFormToTab(Form childForm, string tabName)
        {
            foreach (TabPage existingTab in tabControl.TabPages)
            {
                if (existingTab.Text == tabName)
                {
                    tabControl.SelectedTab = existingTab;
                    return;
                }
            }
            TabPage tabPage = new TabPage(tabName);
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            tabPage.Controls.Add(childForm);
            tabControl.TabPages.Add(tabPage);
            childForm.Size = tabControl.Size;
            childForm.Show();
            tabControl.SelectedTab = tabPage;
        }

        public static void TabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Identify the tab under the mouse pointer
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(e.Location))
                    {
                        tabControl.SelectedIndex = i; // Select the tab
                        contextMenu.Show(tabControl, e.Location); // Show the context menu
                        break;
                    }
                }
            }
        }

        public static void CloseTabMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab); // Close the selected tab
            }
        }


    }
}
