﻿using RandomUtils.Log;
/*
 * 2009 Sizing Servers Lab, affiliated with IT bachelor degree NMCT
 * University College of West-Flanders, Department GKG (www.sizingservers.be, www.nmct.be, www.howest.be/en)
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using vApus.Util;
using WeifenLuo.WinFormsUI.Docking;

namespace vApus.SolutionTree {
    /// <summary>
    ///     Use this to display a "BaseSolutionComponentView" in the dockpanel of the GUI, this will ensure that no multiple instances can exist.
    ///     When having a name that equals SolutionComponent.ToString + "View", you must not even specify the view type you want to show.
    /// </summary>
    public class SolutionComponentViewManager {

        #region Fields
        /// <summary>
        ///     Used to determine if the views should be cleared or not.
        /// </summary>
        private static Solution _activeSolution;
        private static readonly List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> _solutionComponentViews = new List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>>();
        #endregion

        #region Constructors
        static SolutionComponentViewManager() {
            Init();
            _activeSolution = Solution.ActiveSolution;
        }
        private SolutionComponentViewManager() { }
        #endregion

        #region Functions
        private static void Init() {
            Solution.ActiveSolutionChanged += Solution_ActiveSolutionChanged;
            SolutionComponent.SolutionComponentChanged += SolutionComponent_SolutionComponentChanged;
        }
        private static void Solution_ActiveSolutionChanged(object sender, ActiveSolutionChangedEventArgs e) {
            if (_activeSolution != Solution.ActiveSolution) {
                _activeSolution = null;

                _activeSolution = Solution.ActiveSolution;
                DisposeViews();
            }
        }

        /// <summary>
        ///     Dispose all views.
        /// </summary>
        public static void DisposeViews() {
            foreach (BaseSolutionComponentView view in _solutionComponentViews.GetValues())
                if (view != null && !view.IsDisposed && !view.Disposing) {
                    view.Close();
                    view.Dispose();
                }
            _solutionComponentViews.Clear();

        }

        public static IEnumerable<BaseSolutionComponentView> GetAllViews() {
            foreach (var view in _solutionComponentViews.GetValues())
                yield return view;
        }

        private static void SolutionComponent_SolutionComponentChanged(object sender, SolutionComponentChangedEventArgs e) {
            SolutionComponent solutionComponent;
            switch (e.__DoneAction) {
                case SolutionComponentChangedEventArgs.DoneAction.Added:
                    //When having controls for child items, refresh the value in the gui.
                    foreach (BaseSolutionComponentView view in _solutionComponentViews.GetValues())
                        view.Refresh();
                    break;
                case SolutionComponentChangedEventArgs.DoneAction.Cleared:
                    //Remove views
                    solutionComponent = sender as SolutionComponent;
                    var toRemove = new List<SolutionComponent>();
                    foreach (SolutionComponent component in _solutionComponentViews.GetKeys())
                        if (component is BaseItem) {
                            var item = component as BaseItem;
                            if (item.Parent == solutionComponent)
                                toRemove.Add(component);
                        }
                    foreach (SolutionComponent component in toRemove)
                        RemoveWithChilds(component);
                    foreach (BaseSolutionComponentView view in _solutionComponentViews.GetValues())
                        view.Refresh();
                    break;
                case SolutionComponentChangedEventArgs.DoneAction.Edited:
                    foreach (BaseSolutionComponentView view in _solutionComponentViews.GetValues())
                        view.Refresh();
                    break;
                case SolutionComponentChangedEventArgs.DoneAction.Removed:
                    //Remove property view
                    RemoveWithChilds(e.Arg as SolutionComponent);
                    foreach (BaseSolutionComponentView view in _solutionComponentViews.GetValues())
                        view.Refresh();
                    break;
                default:
                    foreach (BaseSolutionComponentView view in _solutionComponentViews.GetValues())
                        view.Refresh();
                    break;
            }
        }

        private static void RemoveWithChilds(SolutionComponent solutionComponent) {
            if (_solutionComponentViews.ContainsKey(solutionComponent)) {
                List<BaseSolutionComponentView> views = _solutionComponentViews.GetValues(solutionComponent);
                foreach (BaseSolutionComponentView view in views) {
                    if (view != null && !view.IsDisposed) {
                        view.Close();
                        view.Dispose();
                    }
                    _solutionComponentViews.RemoveKVP(solutionComponent, view);
                }
            }
            foreach (BaseItem item in solutionComponent)
                RemoveWithChilds(item);
        }

        /// <summary>
        ///     Searches and displays a "BaseSolutionComponentView" with the name SolutionComponent.ToString() + "View".
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="args"></param>
        public static BaseSolutionComponentView Show(SolutionComponent owner) {
            return Show(owner, DockState.Document);
        }

        /// <summary>
        ///     Displays a "BaseSolutionComponentView" of the given type.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="viewType"></param>
        /// <param name="args"></param>
        public static BaseSolutionComponentView Show(SolutionComponent owner, Type viewType) {
            return Show(owner, viewType, DockState.Document);
        }

        /// <summary>
        ///     Searches and displays a "BaseSolutionComponentView" with the name SolutionComponent.ToString + "View".
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="dockState"></param>
        /// <param name="args"></param>
        public static BaseSolutionComponentView Show(SolutionComponent owner, DockState dockState) {
            return Show(owner, owner.GetType().Assembly.GetTypeByName(owner.GetType().Name + "View"), dockState);
        }

        /// <summary>
        ///     Displays a "BaseSolutionComponentView" of the given type.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="viewType"></param>
        /// <param name="dockState"></param>
        /// <param name="args"></param>
        public static BaseSolutionComponentView Show(SolutionComponent owner, Type viewType, DockState dockState) {
            BaseSolutionComponentView view = null;
            if (_solutionComponentViews.ContainsKey(owner)) {
                bool containsView = false;
                foreach (BaseSolutionComponentView v in _solutionComponentViews.GetValues(owner))
                    if (v.GetType() == viewType) {
                        if (v == null || v.IsDisposed) {
                            view = CreateView(owner, viewType);
                            _solutionComponentViews.ReplaceKVP(owner, v, owner, view);
                        } else {
                            view = v;
                        }
                        containsView = true;
                        break;
                    }
                if (!containsView) {
                    view = CreateView(owner, viewType);
                    _solutionComponentViews.AddKVP(owner, view);
                }
            } else {
                view = CreateView(owner, viewType);
                _solutionComponentViews.AddKVP(owner, view);
            }
            view.Show(Solution.DockPanel, dockState);

            return view;
        }

        private static BaseSolutionComponentView CreateView(SolutionComponent owner, Type viewType) {
            var view = Activator.CreateInstance(viewType, owner) as BaseSolutionComponentView;
            Image image = owner.GetImage();
            if (image != null)
                view.Icon = Icon.FromHandle(new Bitmap(image).GetHicon());
            view.Text = owner.ToString();
            view.Disposed += view_Disposed;
            return view;
        }

        private static void view_Disposed(object sender, EventArgs e) {
            var view = sender as BaseSolutionComponentView;
            foreach (BaseSolutionComponentView v in _solutionComponentViews.GetValues())
                if (v == view) {
                    SolutionComponent solutionComponent = _solutionComponentViews.GetKey(view);
                    if (solutionComponent != null)
                        _solutionComponentViews.RemoveKVP(solutionComponent, view);
                    break;
                }

            //Do manual de-referencing, otherwise we have a mem leak. This is expensive but effective.
            try {
                foreach (FieldInfo info in view.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    if (!info.FieldType.IsValueType)
                        try {
                            info.SetValue(view, null);
                        } catch (Exception ex) {
                            Loggers.Log(Level.Error, "Failed dereferencing field.", ex, new object[] { sender, e });
                        }
            } catch(Exception exc) {
                Loggers.Log(Level.Error, "Failed dereferencing fields.", exc, new object[] { sender, e });
            }
        }

        #endregion
    }

    /// <summary>
    /// I want multiple keys in a collection, that is why I use a List. To keep stuff managable, some extension methods where put in place. 
    /// </summary>
    static class ListExtension {
        public static void AddKVP(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l, SolutionComponent key, BaseSolutionComponentView value) { l.Add(new KeyValuePair<SolutionComponent, BaseSolutionComponentView>(key, value)); }
        public static bool RemoveKVP(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l, SolutionComponent key, BaseSolutionComponentView value) {
            foreach (var kvp in l)
                if (kvp.Key == key && kvp.Value == value)
                    return l.Remove(kvp);
            return false;
        }
        public static void ReplaceKVP(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l, SolutionComponent oldKey, BaseSolutionComponentView oldValue, SolutionComponent newKey, BaseSolutionComponentView newValue) {
            foreach (var kvp in l)
                if (kvp.Key == oldKey && kvp.Value == oldValue) {
                    l.Remove(kvp);
                    l.Add(new KeyValuePair<SolutionComponent, BaseSolutionComponentView>(newKey, newValue));
                    break;
                }
        }
        public static bool ContainsKey(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l, SolutionComponent key) {
            foreach (var kvp in l)
                if (kvp.Key == key)
                    return true;
            return false;
        }
        public static List<SolutionComponent> GetKeys(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l) {
            var keys = new List<SolutionComponent>(l.Count);
            foreach (var kvp in l)
                keys.Add(kvp.Key);
            return keys;
        }
        public static SolutionComponent GetKey(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l, BaseSolutionComponentView view) {
            foreach (var kvp in l)
                if (kvp.Value == view)
                    return kvp.Key;
            return null;
        }
        public static List<BaseSolutionComponentView> GetValues(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l, SolutionComponent key) {
            var values = new List<BaseSolutionComponentView>(l.Count);
            foreach (var kvp in l)
                if (kvp.Key == key)
                    values.Add(kvp.Value);
            return values;
        }
        public static List<BaseSolutionComponentView> GetValues(this List<KeyValuePair<SolutionComponent, BaseSolutionComponentView>> l) {
            var values = new List<BaseSolutionComponentView>(l.Count);
            foreach (var kvp in l)
                values.Add(kvp.Value);
            return values;
        }
    }
}