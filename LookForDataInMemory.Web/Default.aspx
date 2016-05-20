<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LookForDataInMemory.Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<script type="text/javascript">
		function f()
		{
			var target = this.document;
			var keys = new Array();
			var values = new Array();
			
			for (p in target)
			{
				keys.push(p);
				values.push(target[p]);
			}

			//alert(keys.length);
			//alert(values.length);
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<button onclick="f();">click me</button>
    </div>
    </form>
</body>
</html>
