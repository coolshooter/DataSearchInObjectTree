<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestJSCode.aspx.cs" Inherits="LookForDataInMemory.Web.TestJSCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Поиск разницы в двух состояниях объекта или двух разных объектах.</title>
	<script type="text/javascript" src="ValueSearch.js"></script>
	<script type="text/javascript">
		function test2()
		{
			var a = document.getElementById('btn1');
			var b = document.getElementById('btn2');

			findAndAlertObjectDifference(a, b);
		}

		function test3()
		{
			var what = 'Paris';
			var where = [
				{
					Id: 1,
					Name: '[Moscow]'
				},
				{
					Id: 2,
					Name: '[Paris]'
				}];

			var result = findValue(what, where, false, 3);
			
			if (result.length > 0)
				alert("Первый путь из найденных: " + result[0].path);
			else
				alert("Ничего не нашли");
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		Поиск разницы между двумя объектами или двумя состояниями одного объекта
		<br />
		<br />
		<button id="btn1" onclick="test();">Найти разницу между 2мя объектами</button>
		<br />
		<br />
		<button id="btn2" onclick="test2();">Найти разницу между этими кнопками</button>
		<br />
		<button id="btn3" onclick="test3();">Найти значение</button>
		<br />
		<br />
		Пользовательский скрипт для сравнения двух объектов может иметь такой вид:
		<br />
		<br />

		function test()
		<br />
		{
		<br />
			var a = document.getElementById('btn1');
		<br />
			var b = document.getElementById('btn2');
		<br />
		<br />
			findAndAlertObjectDifference(a, b);
		<br />
		}
		<br />

    </div>
    </form>
</body>
</html>
