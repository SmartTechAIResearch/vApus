﻿/*
 * Copyright 2012 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace vApus.Stresstest {
    public class ParameterTokenTextStyle {
        private readonly MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));

        private readonly TextStyle _customListParameterStyle = new TextStyle(Brushes.Black, Brushes.LightPink, FontStyle.Bold);

        private readonly IEnumerable<string> _customListParameters;

        private readonly TextStyle _customRandomParameterStyle = new TextStyle(Brushes.Black, Brushes.Yellow, FontStyle.Bold);

        private readonly IEnumerable<string> _customRandomParameters;

        //styles
        private readonly TextStyle _delimiterStyle = new TextStyle(Brushes.Black, null, FontStyle.Bold);
        private readonly IEnumerable<string> _delimiters;
        private readonly FastColoredTextBox _fastColoredTextBox;

        private readonly TextStyle _numericParameterStyle = new TextStyle(Brushes.Black, Brushes.LightGreen, FontStyle.Bold);

        private readonly IEnumerable<string> _numericParameters;

        private readonly TextStyle _textParameterStyle = new TextStyle(Brushes.Black, Brushes.LightBlue, FontStyle.Bold);
        private readonly IEnumerable<string> _textParameters;

        private readonly TextStyle _whiteSpaceStyle = new TextStyle(Brushes.Black, new SolidBrush(Color.FromArgb(255, 240, 240, 240)), FontStyle.Regular);
        private bool _visualizeWhiteSpace;

        public ParameterTokenTextStyle(FastColoredTextBox fastColoredTextBox,
                                       IEnumerable<string> delimiters,
                                       IEnumerable<string> customListParameters,
                                       IEnumerable<string> numericParameters,
                                       IEnumerable<string> textParameters,
                                       IEnumerable<string> customRandomParameters,
                                       bool visualizeWhiteSpace) {
            if (delimiters == null || fastColoredTextBox == null || customListParameters == null ||
                numericParameters == null || textParameters == null || customRandomParameters == null)
                throw new ArgumentNullException();

            _delimiters = delimiters;
            _fastColoredTextBox = fastColoredTextBox;
            _customListParameters = customListParameters;
            _numericParameters = numericParameters;
            _textParameters = textParameters;
            _customRandomParameters = customRandomParameters;

            _visualizeWhiteSpace = visualizeWhiteSpace;

            _fastColoredTextBox.ClearStylesBuffer();
            //add this style explicitly for drawing under other styles
            _fastColoredTextBox.AddStyle(SameWordsStyle);

            SetStyle(_fastColoredTextBox.Range);

            _fastColoredTextBox.TextChanged += _fastColoredTextBox_TextChanged;
        }

        public bool VisualizeWhiteSpace {
            get { return _visualizeWhiteSpace; }
            set {
                if (_visualizeWhiteSpace != value) {
                    _visualizeWhiteSpace = value;
                    SetStyle(_fastColoredTextBox.Range);
                }
            }
        }

        private void _fastColoredTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            SetStyle(e.ChangedRange);
        }

        private void SetStyle(Range range) {
            _fastColoredTextBox.LeftBracket = '\x0';
            _fastColoredTextBox.RightBracket = '\x0';
            _fastColoredTextBox.LeftBracket2 = '\x0';
            _fastColoredTextBox.RightBracket2 = '\x0';

            //clear style of changed range
            range.ClearStyle(FastColoredTextBoxNS.StyleIndex.All);
            string regex = ExtractRegex(_delimiters);
            if (regex != null)
                range.SetStyle(_delimiterStyle, regex);

            regex = ExtractRegex(_customListParameters);
            if (regex != null)
                range.SetStyle(_customListParameterStyle, regex);

            regex = ExtractRegex(_numericParameters);
            if (regex != null)
                range.SetStyle(_numericParameterStyle, regex);

            regex = ExtractRegex(_textParameters);
            if (regex != null)
                range.SetStyle(_textParameterStyle, regex);

            regex = ExtractRegex(_customRandomParameters);
            if (regex != null)
                range.SetStyle(_customRandomParameterStyle, regex);

            if (_visualizeWhiteSpace)
                range.SetStyle(_whiteSpaceStyle, @"\s");
        }

        private string ExtractRegex(IEnumerable<string> col) {
            var sb = new StringBuilder();
            foreach (string item in col) {
                sb.Append(Regex.Escape(item));
                sb.Append("|");
            }

            string s = sb.ToString();
            if (s.Length == 0)
                return null;
            return s.Substring(0, s.Length - 1);
        }

        public void Dispose() {
            _fastColoredTextBox.TextChanged -= _fastColoredTextBox_TextChanged;
        }
    }
}