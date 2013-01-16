﻿/*
 * Copyright 2009 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using vApus.SolutionTree.Properties;
using vApus.Util;

namespace vApus.SolutionTree
{
    /// <summary>
    ///     The solution where everything is stored, this class also keeps its explorer and which solution that is active.
    ///     Saving, Loading, making a solution is done here, this also keeps it's own explorer.
    /// </summary>
    public class Solution
    {
        #region Manage

        /// <summary>
        ///     Used if a new solution has been opened/created or if something has changed in the solution
        /// </summary>
        public static event EventHandler<ActiveSolutionChangedEventArgs> ActiveSolutionChanged;

        #region Fields

        /// <summary>
        ///     Just the type is registered of a project group (Do this in the Link.Linker (Solution.RegisterProjectType(typeof(...))).
        /// </summary>
        private static readonly List<Type> _projectTypes = new List<Type>();

        private static readonly StringCollection _recentSolutions;
        private static Solution _activeSolution;

        private static readonly StresstestingSolutionExplorer _stresstestingSolutionExplorer =
            new StresstestingSolutionExplorer();

        private static readonly OpenFileDialog _ofd = new OpenFileDialog();
        private static readonly SaveFileDialog _sfd = new SaveFileDialog();

        private static DockPanel _dockPanel;

        private static HashSet<Form> _registeredForCancelFormClosing = new HashSet<Form>();

        #endregion

        #region Properties

        /// <summary>
        ///     When set, will enforce loading in the treeview.
        ///     (should only be used in load and new, the rest should be handeled with solution component changed)
        /// </summary>
        public static Solution ActiveSolution
        {
            get { return _activeSolution; }
            private set
            {
                _activeSolution = value;
                if (_activeSolution.FileName != null)
                    RegisterActiveSolutionAsRecent();
                if (ActiveSolutionChanged != null)
                    ActiveSolutionChanged.Invoke(null, new ActiveSolutionChangedEventArgs(false, true));
            }
        }

        internal static DockPanel DockPanel
        {
            get { return _dockPanel; }
        }

        public static Form[] RegisteredForCancelFormClosing {
            get {
                //Cleanup first
                CleanupRegisteredForCancelFormClosing();
                var mdiChilds = new Form[_registeredForCancelFormClosing.Count];
                _registeredForCancelFormClosing.CopyTo(mdiChilds);
                return mdiChilds;
            }
        }

        /// <summary>
        ///     Do not forget to set this to false when cancelling
        /// </summary>
        public static bool ExplicitCancelFormClosing { get; set; }

        #endregion

        #region Functions

        /// <summary>
        ///     Just the type is registered of a project group (Do this in the Link.Linker (Solution.RegisterProjectType(typeof(...))).
        ///     This is needed to be able to make a new solution avoiding circular dependencies.
        /// </summary>
        /// <param name="t"></param>
        public static void RegisterProjectType(Type projectType)
        {
            if (_projectTypes.Contains(projectType))
                throw new ArgumentException("A project type may be added only once.");
            _projectTypes.Add(projectType);
        }

        /// <summary>
        ///     To be able to show the views for the solution components and the panel for the solution explorer.
        /// </summary>
        /// <param name="dockPanel"></param>
        public static void RegisterDockPanel(DockPanel dockPanel)
        {
            if (dockPanel == null)
                throw new ArgumentNullException("dockpanel");
            _dockPanel = dockPanel;
        }

        /// <summary>
        ///     If the explorer is not visible it will be shown, docking left.
        ///     Note: call 'RegisterDockPanel(DockPanel dockPanel)' prior to this.
        /// </summary>
        /// <returns>True on success</returns>
        public static bool ShowStresstestingSolutionExplorer()
        {
            try
            {
                int dockState = Settings.Default.StresstestingSolutionExplorerDockState;
                if (dockState == -1)
                    dockState = 8; //DockLeft

                _stresstestingSolutionExplorer.Show(_dockPanel, (DockState) dockState);
            }
            catch
            {
                //Could fail for slaves
                return false;
            }
            return true;
        }

        public static void HideStresstestingSolutionExplorer()
        {
            _stresstestingSolutionExplorer.Hide();
        }

        /// <summary>
        ///     Tooltips will be provide for the items.
        /// </summary>
        /// <returns></returns>
        public static List<ToolStripMenuItem> GetRecentSolutionsMenuItems()
        {
            var recentSolutionsMenuItems = new List<ToolStripMenuItem>();
            foreach (string filename in _recentSolutions)
            {
                var item = new ToolStripMenuItem(filename);
                item.Click += item_Click;
                recentSolutionsMenuItems.Add(item);
            }
            return recentSolutionsMenuItems;
        }

        private static void item_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (File.Exists(item.Text))
            {
                LoadNewActiveSolution(item.Text);
            }
            else
            {
                _recentSolutions.Remove(item.Text);
                Settings.Default.Save();
                MessageBox.Show(string.Format("'{0}' does not exist and could therefore not be opened!", item.Text),
                                string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);
            }
        }

        public static void ClearRecentSolutions()
        {
            _recentSolutions.Clear();
            Settings.Default.RecentSolutions.Clear();
            Settings.Default.Save();
        }

        private static void RegisterActiveSolutionAsRecent()
        {
            if (!_recentSolutions.Contains(_activeSolution.FileName))
            {
                if (_recentSolutions.Count == 10)
                    _recentSolutions.RemoveAt(9);
                _recentSolutions.Insert(0, _activeSolution.FileName);
                Settings.Default.RecentSolutions = _recentSolutions;
                Settings.Default.Save();
            }
        }

        /// <summary>
        ///     The will be closed before the main window is closed, they can cancel this setting ExplicitCancelFormClosing to true.
        ///     Unregistering is not needed.
        /// </summary>
        /// <param name="mdiChild"></param>
        public static void RegisterForCancelFormClosing(Form mdiChild) {
            CleanupRegisteredForCancelFormClosing();
            _registeredForCancelFormClosing.Add(mdiChild);
        }

        private static void CleanupRegisteredForCancelFormClosing() {
            var newRegisteredForCancelFormClosing = new HashSet<Form>();
            foreach (Form mdiChild in _registeredForCancelFormClosing)
                if (mdiChild != null && !mdiChild.IsDisposed)
                    newRegisteredForCancelFormClosing.Add(mdiChild);
            _registeredForCancelFormClosing = newRegisteredForCancelFormClosing;
        }

        #region File Management

        public static void CreateNew()
        {
            if (_activeSolution != null && (!_activeSolution.IsSaved || _activeSolution.FileName == null))
            {
                DialogResult result =
                    MessageBox.Show(
                        string.Format("Do you want to save '{0}' before creating a new solution?", _activeSolution.Name),
                        string.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes && SaveActiveSolution())
                    ActiveSolution = new Solution();
                else if (result == DialogResult.No)
                    ActiveSolution = new Solution();
                //Do nothing on cancel.
            }
            else
            {
                ActiveSolution = new Solution();
            }
        }

        /// <summary>
        ///     Returns true if it has been saved.
        /// </summary>
        /// <returns></returns>
        public static bool SaveActiveSolution()
        {
            if (_activeSolution.FileName == null)
                return SaveActiveSolutionAs();
            else
            {
                _activeSolution.Save();
                _activeSolution.IsSaved = true;
                if (ActiveSolutionChanged != null)
                    ActiveSolutionChanged.Invoke(null, new ActiveSolutionChangedEventArgs(false, false));

                RegisterActiveSolutionAsRecent();
                return true;
            }
        }

        /// <summary>
        ///     Returns true if it has been saved.
        /// </summary>
        /// <returns></returns>
        public static bool SaveActiveSolutionAs()
        {
            if (_sfd.ShowDialog() == DialogResult.OK)
            {
                _activeSolution.FileName = _sfd.FileName;
                return SaveActiveSolution();
            }
            return false;
        }

        /// <summary>
        ///     Loads a new solution as the active one. Returns true if it has been loaded.
        /// </summary>
        public static bool LoadNewActiveSolution()
        {
            return LoadNewActiveSolution(null);
        }

        public static bool LoadNewActiveSolution(string fileName)
        {
            if (_activeSolution != null && (!_activeSolution.IsSaved || _activeSolution.FileName == null))
            {
                DialogResult result =
                    MessageBox.Show(
                        string.Format("Do you want to save '{0}' before opening another solution?", _activeSolution.Name),
                        string.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes && SaveActiveSolution())
                    return LoadSolution(fileName);
                else if (result == DialogResult.No)
                    return LoadSolution(fileName);
                //Do nothing on cancel.
            }
            else
            {
                return LoadSolution(fileName);
            }
            return false;
        }

        private static bool LoadSolution(string fileName)
        {
            string errorMessage = string.Empty;
            try
            {
                if (fileName != null)
                {
                    var sln = new Solution();
                    sln.FileName = fileName;
                    sln.Load(out errorMessage);
                    ActiveSolution = sln;
                    ActiveSolution.ResolveBranchedIndices();
                    _activeSolution.IsSaved = true;
                    return true;
                }
                else if (_ofd.ShowDialog() == DialogResult.OK)
                {
                    var sln = new Solution();
                    sln.FileName = _ofd.FileName;
                    sln.Load(out errorMessage);
                    ActiveSolution = sln;
                    ActiveSolution.ResolveBranchedIndices();
                    _activeSolution.IsSaved = true;
                    return true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (errorMessage.Length > 0)
                    MessageBox.Show(@"Failed loading one or more items/properties.

This is usally not a problem: Changes in functionality for this version of vApus that are not in the opened .vass file.
Take a copy of the file to be sure and test if stresstesting works.

See 'Tools >> Options... >> Application Logging' for details. (Log Level >= Warning)", string.Empty,
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return false;
        }

        public static bool CreateNewFromTemplate()
        {
            return CreateNewFromTemplate(null);
        }

        private static bool CreateNewFromTemplate(string fileName)
        {
            if (_activeSolution != null && (!_activeSolution.IsSaved || _activeSolution.FileName == null))
            {
                DialogResult result =
                    MessageBox.Show(
                        string.Format("Do you want to save '{0}' before opening another solution?", _activeSolution.Name),
                        string.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes && SaveActiveSolution())
                    return LoadSolutionFromTemplate(fileName);
                else if (result == DialogResult.No)
                    return LoadSolutionFromTemplate(fileName);
                //Do nothing on cancel.
            }
            else
            {
                return LoadSolutionFromTemplate(fileName);
            }
            return false;
        }

        private static bool LoadSolutionFromTemplate(string fileName)
        {
            string errorMessage = string.Empty;
            try
            {
                if (fileName != null)
                {
                    var sln = new Solution();
                    sln.FileName = fileName;
                    sln.Load(out errorMessage);
                    sln.FileName = null;
                    ActiveSolution = sln;
                    ActiveSolution.ResolveBranchedIndices();
                    return true;
                }
                else
                {
                    if (_ofd.ShowDialog() == DialogResult.OK)
                    {
                        var sln = new Solution();
                        sln.FileName = _ofd.FileName;
                        sln.Load(out errorMessage);
                        sln.FileName = null;
                        ActiveSolution = sln;
                        ActiveSolution.ResolveBranchedIndices();
                        return true;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (errorMessage.Length > 0)
                    MessageBox.Show(@"Failed loading one or more items/properties.

This is usally not a problem: Changes in functionality for this version of vApus that are not in the opened .vass file.
Take a copy of the file to be sure and test if stresstesting works.

See 'Tools >> Options... >> Application Logging' for details. (Log Level >= Warning)", string.Empty,
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return false;
        }

        #endregion

        #endregion

        #endregion

        #region Solution itself

        #region Fields

        private readonly List<BaseProject> _projects = new List<BaseProject>();
        private string _fileName;

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                if (_fileName != null)
                {
                    int index = 0;
                    for (int i = 0; i < _fileName.Length; i++)
                        if (_fileName[i] == '\\') index = i;
                    string name = _fileName.Substring(index + 1);
                    return name.Substring(0, name.Length - 5);
                }
                return "<New>";
            }
        }

        public string FileName
        {
            get { return _fileName; }
            private set { _fileName = value; }
        }

        public bool IsSaved { get; internal set; }

        public List<BaseProject> Projects
        {
            get { return _projects; }
        }

        #endregion

        #region Constructors

        static Solution()
        {
            if (Settings.Default.RecentSolutions == null)
                Settings.Default.RecentSolutions = new StringCollection();
            _recentSolutions = Settings.Default.RecentSolutions;

            _ofd.Filter = "vApus Stresstesting Solutions (*.vass) | *.vass";
            _ofd.Multiselect = false;
            _sfd.Filter = _ofd.Filter;

            SolutionComponent.SolutionComponentChanged += SolutionComponent_SolutionComponentChanged;


            _stresstestingSolutionExplorer.DockStateChanged += _stresstestingSolutionExplorer_DockStateChanged;
        }

        private Solution()
        {
            _activeSolution = null;
            ObjectExtension.ClearCache();
            _projects = new List<BaseProject>();
            foreach (Type projectType in _projectTypes)
            {
                var project = Activator.CreateInstance(projectType) as BaseProject;
                project.Parent = this;
                _projects.Add(project);
            }
        }

        private Solution(string fileName)
            : this()
        {
            FileName = fileName;
        }

        private static void _stresstestingSolutionExplorer_DockStateChanged(object sender, EventArgs e)
        {
            if (_stresstestingSolutionExplorer.DockState == DockState.Hidden)
            {
                Settings.Default.StresstestingSolutionExplorerDockState = (int) DockState.DockLeftAutoHide;
                Settings.Default.Save();
            }
            else if (_stresstestingSolutionExplorer.DockState != DockState.Unknown)
            {
                Settings.Default.StresstestingSolutionExplorerDockState = (int) _stresstestingSolutionExplorer.DockState;
                Settings.Default.Save();
            }
        }

        #endregion

        #region Functions

        private static void SolutionComponent_SolutionComponentChanged(object sender,
                                                                       SolutionComponentChangedEventArgs e)
        {
            if (_activeSolution.FileName != null && ActiveSolutionChanged != null)
            {
                _activeSolution.IsSaved = false;
                ActiveSolutionChanged.Invoke(null, new ActiveSolutionChangedEventArgs(true, false));
            }
        }

        /// <summary>
        ///     To do stuff that is not done by 'this', like building main menu items.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public BaseProject GetProject(string typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException(typeName);
            foreach (BaseProject project in _projects)
                if (project.GetType().Name == typeName)
                    return project;
            return null;
        }

        /// <summary>
        ///     Serves mainly for loading.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseProject GetProject(int index)
        {
            return _projects[index];
        }

        /// <summary>
        ///     Gets a solution component by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SolutionComponent GetSolutionComponent(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            foreach (BaseProject project in _projects)
            {
                SolutionComponent solutionComponent = project.GetSolutionComponent(type);
                if (solutionComponent != null)
                    return solutionComponent;
            }
            return null;
        }

        /// <summary>
        ///     Gets a solution component by type and name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public SolutionComponent GetSolutionComponent(Type type, string name)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (name == null)
                throw new ArgumentNullException("name");

            foreach (BaseProject project in _projects)
            {
                SolutionComponent solutionComponent = project.GetSolutionComponent(type, name);
                if (solutionComponent != null)
                    return solutionComponent;
            }
            return null;
        }

        /// <summary>
        ///     Get a labeled base item by name, index and label.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public LabeledBaseItem GetLabeledBaseItem(string name, int index, string label)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (label == null)
                throw new ArgumentNullException("label");

            foreach (BaseProject project in _projects)
            {
                LabeledBaseItem labeledBaseItem = project.GetLabeledBaseItem(name, index, label);
                if (labeledBaseItem != null)
                    return labeledBaseItem;
            }
            return null;
        }

        /// <summary>
        ///     Gets all project items as treenodes for visualization in a treeview.
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetTreeNodes()
        {
            var treeNodes = new List<TreeNode>(_projects.Count);
            foreach (BaseProject project in _projects)
                if (project.ShowInGui)
                    treeNodes.Add(project.GetTreeNode());
            return treeNodes;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected void Save()
        {
            Package package = Package.Open(_activeSolution.FileName, FileMode.Create, FileAccess.ReadWrite);
            foreach (BaseProject project in _projects)
            {
                var uri = new Uri("/" + project.GetType().Name, UriKind.Relative);
                PackagePart part = package.CreatePart(uri, string.Empty, CompressionOption.Maximum);
                var sw = new StreamWriter(part.GetStream(FileMode.Create, FileAccess.Write));
                project.GetXmlToSave().Save(sw);
                sw.Close();
                sw.Dispose();
            }
            package.Flush();
            package.Close();
        }

        protected void Load(out string errorMessage)
        {
            //Error reporting.
            var sb = new StringBuilder();
            try
            {
                Package package = Package.Open(FileName, FileMode.Open, FileAccess.Read);
                foreach (PackagePart part in package.GetParts())
                {
                    foreach (BaseProject project in _projects)
                        if (part.Uri.ToString().EndsWith(project.GetType().Name))
                        {
                            var xmlDocument = new XmlDocument();
                            xmlDocument.Load(part.GetStream());
                            string projectErrorMessage;
                            project.LoadFromXml(xmlDocument, out projectErrorMessage);
                            sb.Append(projectErrorMessage);
                        }
                }
                package.Close();
            }
            catch (Exception ex)
            {
                sb.Append(ex);
            }
            errorMessage = sb.ToString();
        }

        protected void ResolveBranchedIndices()
        {
            foreach (BaseProject project in _projects)
                project.ResolveBranchedIndices();
        }

        #endregion

        #endregion
    }

    public class ActiveSolutionChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     To determine of the treeview should be reloaded or not.
        /// </summary>
        public readonly bool ToBeLoaded;

        /// <summary>
        ///     Not for newly opened/created solutions.
        /// </summary>
        public readonly bool ToBeSaved;

        /// <summary>
        /// </summary>
        /// <param name="toBeSaved">Not for newly opened/created solutions.</param>
        /// <param name="toBeLoaded">To determine of the treeview should be reloaded or not.</param>
        public ActiveSolutionChangedEventArgs(bool toBeSaved, bool toBeLoaded)
        {
            ToBeSaved = toBeSaved;
            ToBeLoaded = toBeLoaded;
        }
    }
}