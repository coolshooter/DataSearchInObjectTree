function test()
{
	var a =
	{
		Id: 1,
		Name: "Peter",
		Age: 25
	};

	var b =
	{
		Id: 1,
		Name: "Peter",
		// это свойство нужно, чтобы индекс следующего был уже другим,
		// чтобы протестировать лучше алгоритм
		Something: 10,
		Age: 26,
	};

	findAndAlertObjectDifference(a, b);
}

function findAndAlertObjectDifference(a, b)
{
	var state1 = getStateDictionary(a);
	var state2 = getStateDictionary(b);

	findAndAlertStateDifference(state1, state2);
}

function findAndAlertStateDifference(state1, state2)
{
	var res = getStateDifference(state1, state2);
	var text = "";

	if (res.length > 0)
	{
		if (res.length > 5)
			text += "Первые 5 отличий:\n";
		else
			text += "Отличия:\n"

		for (var i = 0; i < res.length && i < 5; i++)
		{
			var r = res[i];
			var res1 = r.State1Index;
			var res2 = r.State2Index;

			text += "\nСвойство: \"" + state1[res1].key + "\":\n" +
					" значение в первом объекте: \"" + state1[res1].value + "\",\n" +
					" значение во втором: \"" + state2[res2].value +
					"\"\n";

		}

		alert(text);
	}
	else
	{
		alert("Разницы в свойствах нет");
	}
}

function getStateDifference(state1, state2)
{
	var found = false;
	var results = new Array();

	for (var i = 0; i < state1.length; i++)
	{
		for (var j = 0; j < state2.length; j++)
		{
			if (state1[i].key == state2[j].key)
			{
				if (state1[i].value != state2[j].value)
				{
					res1 = i;
					res2 = j;
					results.push(
						{
							State1Index: i,
							State2Index: j
						});
					found = true;
				}

				break;
			}
		}
	}

	return results;
}

function getStateDictionary(target)
{
	var dict = [];

	for (p in target)
	{
		dict.push({
			key: p,
			value: target[p] != null ? target[p].toString() : ""
		});
	}

	return dict;
}

//---------------------------------------------

function findValue(what, where, isExactComparison, maxDepth)
{
	var result = new Array();
	findValueInner(what, where, result, isExactComparison, maxDepth, 0, "Root");
	return result;
}

function findValueInner(what, where, result, isExactComparison, maxDepth, depth, path)
{
	if (where)
	{
		var found = false;

		if (where instanceof Array)
		{
			for (var i = 0; i < where.length; i++)
			{
				var newPath = path + "[" + i + "]";
				var value = where[i];

				found = found ||
					compareAndPushToResult(value, what, result, isExactComparison, newPath);
			}
		}
		else if (typeof where !== 'string' && !(where instanceof String))
		{
			for (var x in where)
			{
				var newPath = path + "." + x.toString();
				var value = where[x];

				found = found ||
					compareAndPushToResult(value, what, result, isExactComparison, newPath);
			}
		}

		depth++;

		if (!found && depth < maxDepth)
		{
			if (where instanceof Array)
			{
				for (var i = 0; i < where.length; i++)
				{
					var newPath = path + "[" + i + "]";
					var value = where[i];

					findValueInner(what, value, result, isExactComparison, maxDepth, depth, newPath);
				}
			}
			else if (typeof where !== 'string' && !(where instanceof String))
			{
				for (var x in where)
				{
					var newPath = path + "." + x.toString();
					var value = where[x];

					findValueInner(what, value, result, isExactComparison, maxDepth, depth, newPath);
				}
			}
		}
	}
}

function compareAndPushToResult(value, searchValue, result, isExactComparison, newPath)
{
	var found = false;

	if (value)
	{
		if (isExactComparison)
		{
			if (value === searchValue)
			{
				result.push({ value: value, path: newPath });
				found = true;
			}
		}
		else
		{
			if (value.toString().indexOf(searchValue.toString()) >= 0)
			{
				result.push({ value: value, path: newPath });
				found = true;
			}
		}
	}

	return found;
}



