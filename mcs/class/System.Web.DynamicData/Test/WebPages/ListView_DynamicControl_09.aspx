﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListView_DynamicControl_09.aspx.cs" Inherits="ListView_DynamicControl_09" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html >
<head id="Head1" runat="server">
  <title>DynamicControl Sample</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
      <asp:ListView ID="ListView1" runat="server" DataSourceID="DynamicDataSource1">
        <LayoutTemplate>
          <div runat="server" id="itemPlaceholder" />
        </LayoutTemplate>
        <ItemTemplate>
	<div>
		<test:PokerDynamicControl runat="server" DataField="PrimaryKeyColumn1" id="PrimaryKeyColumn1"/> 
		<test:PokerDynamicControl runat="server" DataField="PrimaryKeyColumn2" id="PrimaryKeyColumn2"/>
		<test:PokerDynamicControl runat="server" DataField="Column1" ID="Column1" CssClass="activeCssClass" />
        </div>
        </ItemTemplate>
      </asp:ListView>

	<test:DynamicDataSource runat="server" id="DynamicDataSource1" />
    </div>
    </form>
</body>
</html>
