//
// DynamicDataRouteTest.cs
//
// Author:
//      Marek Habersack <mhabersack@novell.com>
//
// Copyright (C) 2009 Novell Inc. http://novell.com
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.DynamicData;
using NUnit.Framework;

namespace MonoTests.System.Web.DynamicData
{
	[TestFixture]
	public class ContextConfigurationTests
	{
		[Test]
		public void DefaultValues()
		{
			var ctx = new ContextConfiguration();
			var func = ctx.MetadataProviderFactory;

			Assert.IsTrue(func != null, "#A1");
			Assert.AreEqual(typeof (Func <Type, TypeDescriptionProvider>), func.GetType (), "#A2");
			Assert.AreEqual(false, ctx.ScaffoldAllTables, "#A3");
			Assert.AreEqual(typeof(AssociatedMetadataTypeTypeDescriptionProvider), func(typeof(string)).GetType (), "#A4");
		}
	}
}
