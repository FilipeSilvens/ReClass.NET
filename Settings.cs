﻿using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ReClassNET
{
	public class Settings
	{
		public static Settings Load(string filename)
		{
			Contract.Requires(!string.IsNullOrEmpty(filename));

			try
			{
				using (var sr = new StreamReader(filename))
				{
					return (Settings)new XmlSerializer(typeof(Settings)).Deserialize(sr);
				}
			}
			catch
			{
				return new Settings();
			}
		}

		public static void Save(Settings settings, string filename)
		{
			Contract.Requires(settings != null);
			Contract.Requires(!string.IsNullOrEmpty(filename));

			using (var sr = new StreamWriter(filename))
			{
				new XmlSerializer(typeof(Settings)).Serialize(sr, settings);
			}
		}

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color BackgroundColor { get; set; } = Color.FromArgb(255, 255, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color SelectedColor { get; set; } = Color.FromArgb(240, 240, 240);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color HiddenColor { get; set; } = Color.FromArgb(240, 240, 240);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color OffsetColor { get; set; } = Color.FromArgb(255, 0, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color AddressColor { get; set; } = Color.FromArgb(0, 200, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color HexColor { get; set; } = Color.FromArgb(0, 0, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color TypeColor { get; set; } = Color.FromArgb(0, 0, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color NameColor { get; set; } = Color.FromArgb(32, 32, 128);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color ValueColor { get; set; } = Color.FromArgb(255, 128, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color IndexColor { get; set; } = Color.FromArgb(32, 200, 200);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color CommentColor { get; set; } = Color.FromArgb(0, 200, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color TextColor { get; set; } = Color.FromArgb(0, 0, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color VTableColor { get; set; } = Color.FromArgb(0, 255, 0);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color FunctionColor { get; set; } = Color.FromArgb(255, 0, 255);

		[Category("Colors")]
		[XmlElement(Type = typeof(XmlColorWrapper))]
		public Color CustomColor { get; set; } = Color.FromArgb(64, 128, 64);

		private static Random random = new Random();
		private static Color[] highlightColors = new Color[]
		{
			Color.Aqua, Color.Aquamarine, Color.Blue, Color.BlueViolet, Color.Chartreuse, Color.Crimson, Color.LawnGreen, Color.Magenta
		};
		[XmlIgnore]
		[Browsable(false)]
		public Color HighlightColor => highlightColors[random.Next(highlightColors.Length)];

		[Category("Display")]
		public bool ShowAddress { get; set; } = true;

		[Category("Display")]
		public bool ShowOffset { get; set; } = true;

		[Category("Display")]
		public bool ShowText { get; set; } = true;

		[Category("Display")]
		public bool HighlightChangedValues { get; set; } = true;

		[Category("Comment")]
		public bool ShowFloat { get; set; } = true;

		[Category("Comment")]
		public bool ShowInteger { get; set; } = true;

		[Category("Comment")]
		public bool ShowPointer { get; set; } = true;

		[Category("Comment")]
		public bool ShowRTTI { get; set; } = false;

		[Category("Comment")]
		public bool ShowSymbols { get; set; } = false;

		[Category("Comment")]
		public bool ShowStrings { get; set; } = true;

		[Browsable(false)]
		public string LastProcess { get; set; } = string.Empty;
	}

	public class XmlColorWrapper : IXmlSerializable
	{
		private Color color;

		public XmlColorWrapper()
			: this(Color.Empty)
		{

		}

		public XmlColorWrapper(Color color)
		{
			this.color = color;
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			color = Color.FromArgb((int)(0xFF000000 | reader.ReadElementContentAsInt()));
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteString(color.ToRgb().ToString());
		}


		public static implicit operator XmlColorWrapper(Color color)
		{
			if (color != Color.Empty)
			{
				return new XmlColorWrapper(color);
			}

			return null;
		}

		public static implicit operator Color(XmlColorWrapper wrapper)
		{
			if (wrapper != null)
			{
				return wrapper.color;
			}

			return Color.Empty;
		}
	}
}
